namespace Jwt.PresentationLayer.Dtos.UserSongHistoryDtos
{
    public class CreateUserSongHistoryDto
    {
        public int UserId { get; set; }
        public int SongId { get; set; }
        public int ListenCount { get; set; }
        public DateTime LastListened { get; set; }
    }
}
