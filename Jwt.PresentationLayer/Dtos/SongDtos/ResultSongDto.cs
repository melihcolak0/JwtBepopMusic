namespace Jwt.PresentationLayer.Dtos.SongDtos
{
    public class ResultSongDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Artist { get; set; }
        public string Album { get; set; }
        public string Genre { get; set; }
        public string DataSourceUrl { get; set; }
        public string CoverImageUrl { get; set; }
        public string ArtistImageUrl { get; set; }

        public TimeSpan Duration { get; set; }
        public DateTime ReleaseDate { get; set; }

        public int PackageId { get; set; }
        public string PackageName { get; set; }
    }
}
