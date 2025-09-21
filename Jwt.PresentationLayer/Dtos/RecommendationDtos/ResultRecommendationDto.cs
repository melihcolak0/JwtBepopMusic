namespace Jwt.PresentationLayer.Dtos.RecommendationDtos
{
    public class ResultRecommendationDto
    {
        public int SongId { get; set; }
        public string Title { get; set; }
        public string Artist { get; set; }
        public string Genre { get; set; }
        public DateTime ReleaseDate { get; set; }
        public string DataSourceUrl { get; set; }
        public string CoverImageUrl { get; set; }
        public string Reason { get; set; }
    }
}
