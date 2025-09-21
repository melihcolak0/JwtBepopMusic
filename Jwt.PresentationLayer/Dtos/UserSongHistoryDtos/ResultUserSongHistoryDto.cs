namespace Jwt.PresentationLayer.Dtos.UserSongHistoryDtos
{
    public class ResultUserSongHistoryDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string UserNameSurname { get; set; }
        public int SongId { get; set; }
        public string SongName { get; set; }
        public string SongArtist { get; set; }
        public int ListenCount { get; set; }
        public DateTime LastListened { get; set; }
    }
}
