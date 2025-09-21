namespace Jwt.PresentationLayer.Dtos.UserSongHistoryDtos
{
    public class UpdateUserSongHistoryDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int SongId { get; set; }
        public int ListenCount { get; set; }
        public DateTime LastListened { get; set; }
    }
}
