using Jwt.BusinessLayer.Abstract;
using Jwt.BusinessLayer.ML;
using Jwt.BusinessLayer.ML.Models;
using Jwt.DataAccessLayer.Abstract;
using Jwt.DataAccessLayer.Concrete;
using Jwt.DtoLayer.RecommendationDtos;
using Jwt.EntityLayer.Concrete;
using Microsoft.EntityFrameworkCore;
using Microsoft.ML;

namespace Jwt.BusinessLayer.Concrete
{
    public class RecommendationManager : GenericManager<Recommendation>, IRecommendationService
    {
        private readonly IUserDal _userDal;
        private readonly ISongDal _songDal;
        private readonly IRecommendationDal _recommendationDal;
        private readonly RecommendationEngine _mlEngine;
        private readonly JwtContext _context;
        private readonly MLContext _mlContext;

        public RecommendationManager(
            IRecommendationDal recommendationDal,
            IUserDal userDal,
            ISongDal songDal,
            JwtContext context
        ) : base(recommendationDal)
        {
            _userDal = userDal;
            _songDal = songDal;
            _context = context;
            _recommendationDal = recommendationDal;
            _mlEngine = new RecommendationEngine(); // ML katmanı
            _mlContext = new MLContext();
        }

        private const string ModelPath = "recommendationModel.zip";

        public async Task TrainModelAsync()
        {
            var allHistory = await _context.UserSongHistories.ToListAsync();
            if (!allHistory.Any()) return;

            var trainingData = allHistory.Select(h => new UserSongRating
            {
                UserId = h.UserId,
                SongId = h.SongId,
                Rating = (float)h.ListenCount
            }).ToList();

            _mlEngine.TrainAndSave(trainingData, ModelPath);
        }

        public async Task<IEnumerable<ResultRecommendationDto>> GetRecommendationsAsync(int userId, int count = 5)
        {
            if (!File.Exists(ModelPath))
            {
                var allHistoryInit = await _context.UserSongHistories
                    .Include(h => h.Song)
                    .ToListAsync();

                if (allHistoryInit.Any())
                {
                    var listenCountsInit = allHistoryInit.Select(h => h.ListenCount).ToList();
                    int minCountInit = listenCountsInit.Min();
                    int maxCountInit = listenCountsInit.Max();

                    var trainingDataInit = allHistoryInit.Select(h => new UserSongRating
                    {
                        UserId = h.UserId,
                        SongId = h.SongId,
                        Rating = 1f + 4f * (h.ListenCount - minCountInit) / (float)(maxCountInit - minCountInit + 1)

                    }).ToList();

                    _mlEngine.TrainAndSave(trainingDataInit, ModelPath);
                }
            }

            var model = _mlEngine.LoadModel(ModelPath);

            var allHistory = await _context.UserSongHistories
                .Include(h => h.Song)
                .ToListAsync();

            if (!allHistory.Any())
                return new List<ResultRecommendationDto>();

            var userHistory = allHistory.Where(h => h.UserId == userId).ToList();

            if (!userHistory.Any())
            {
                var topSongs = await _context.Songs
                    .Include(s => s.ListeningHistory)
                    .OrderByDescending(s => s.ListeningHistory.Sum(h => h.ListenCount))
                    .Take(count)
                    .ToListAsync();

                return topSongs.Select(s => new ResultRecommendationDto
                {
                    SongId = s.Id,
                    Title = s.Title,
                    Artist = s.Artist,
                    Reason = "Popular Song"
                }).ToList();
            }

            var favoriteGenres = userHistory
                .GroupBy(h => h.Song.Genre)
                .OrderByDescending(g => g.Sum(h => h.ListenCount))
                .Select(g => g.Key)
                .Take(3)
                .ToList();

            var favoriteArtists = userHistory
                .GroupBy(h => h.Song.Artist)
                .OrderByDescending(g => g.Sum(h => h.ListenCount))
                .Select(g => g.Key)
                .Take(3)
                .ToList();

            var allSongs = await _context.Songs.ToListAsync();
            var unlistenedSongs = allSongs
                .Where(s => !userHistory.Any(h => h.SongId == s.Id))
                .ToList();

            if (!unlistenedSongs.Any())
                return new List<ResultRecommendationDto>();

            var predictionData = _mlContext.Data.LoadFromEnumerable(
                unlistenedSongs.Select(s => new UserSongRating { UserId = userId, SongId = s.Id })
            );

            var predictions = model.Transform(predictionData);

            var mlScores = _mlContext.Data.CreateEnumerable<SongRatingPrediction>(predictions, reuseRowObject: false)
                .Select((p, index) => new
                {
                    Song = unlistenedSongs[index],
                    MLScore = p.Score
                })
                .ToList();
                       
            var recommendedSongs = mlScores
                .Select(x =>
                {
                    int genreScore = favoriteGenres.Contains(x.Song.Genre) ? 1 : 0;
                    int artistScore = favoriteArtists.Contains(x.Song.Artist) ? 1 : 0;

                    float finalScore = x.MLScore * 0.7f + (genreScore + artistScore) * 0.3f;

                    return new
                    {
                        Song = x.Song,
                        Score = finalScore
                    };
                })
                .OrderByDescending(x => x.Score)
                .Take(count)
                .Select(x => new ResultRecommendationDto
                {
                    SongId = x.Song.Id,
                    Title = x.Song.Title,
                    Artist = x.Song.Artist,
                    Genre = x.Song.Genre,                
                    ReleaseDate = x.Song.ReleaseDate,
                    DataSourceUrl = x.Song.DataSourceUrl,
                    CoverImageUrl = x.Song.CoverImageUrl,
                    Reason = "Recommended by ML + Similarity"
                })
                .ToList();

            return recommendedSongs;
        }       
    }
}
