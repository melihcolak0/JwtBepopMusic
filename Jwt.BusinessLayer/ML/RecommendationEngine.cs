using Jwt.BusinessLayer.ML.Models;
using Microsoft.ML;
using Microsoft.ML.Trainers;

namespace Jwt.BusinessLayer.ML
{
    public class RecommendationEngine
    {
        private readonly MLContext _mlContext;

        public RecommendationEngine()
        {
            _mlContext = new MLContext();
        }       

        // Pipeline tanýmý
        private IEstimator<ITransformer> BuildPipeline()
        {
            return _mlContext.Transforms.Conversion
                .MapValueToKey(outputColumnName: "UserIdEncoded", inputColumnName: nameof(UserSongRating.UserId))
                .Append(_mlContext.Transforms.Conversion
                    .MapValueToKey(outputColumnName: "SongIdEncoded", inputColumnName: nameof(UserSongRating.SongId)))
                .Append(_mlContext.Recommendation().Trainers.MatrixFactorization(
                    labelColumnName: nameof(UserSongRating.Rating),
                    matrixColumnIndexColumnName: "SongIdEncoded",
                    matrixRowIndexColumnName: "UserIdEncoded",
                    numberOfIterations: 20,
                    approximationRank: 100
                ));
        }

        // Normal Train
        public ITransformer Train(IEnumerable<UserSongRating> data)
        {
            var trainingData = _mlContext.Data.LoadFromEnumerable(data);
            var pipeline = BuildPipeline();
            return pipeline.Fit(trainingData);
        }

        // Train + Save (offline eðitim)
        public void TrainAndSave(IEnumerable<UserSongRating> data, string modelPath)
        {
            var trainingData = _mlContext.Data.LoadFromEnumerable(data);
            var pipeline = BuildPipeline();
            var model = pipeline.Fit(trainingData);

            using var fs = File.Create(modelPath);
            _mlContext.Model.Save(model, trainingData.Schema, fs);
        }

        // Load saved model
        public ITransformer LoadModel(string modelPath)
        {
            using var fs = File.OpenRead(modelPath);
            return _mlContext.Model.Load(fs, out _);
        }

        // Prediction
        public List<int> PredictTopNSongs(ITransformer model, int userId, IEnumerable<int> allSongIds, int n = 5)
        {
            var predictionEngine = _mlContext.Model.CreatePredictionEngine<UserSongRating, SongRatingPrediction>(model);

            var scores = new List<(int SongId, float Score)>();

            foreach (var songId in allSongIds)
            {
                var prediction = predictionEngine.Predict(new UserSongRating
                {
                    UserId = userId,
                    SongId = songId
                });

                scores.Add((songId, prediction.Score));
            }

            return scores.OrderByDescending(x => x.Score).Take(n).Select(x => x.SongId).ToList();
        }
    }

    public class SongRatingPrediction
    {
        public float Score { get; set; }  // ML.NET tahmin skoru
    }
}
