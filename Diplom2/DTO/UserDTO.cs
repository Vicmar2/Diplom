namespace Diplom2.DTO
{
    public class UserDTO
    {
        public int IdUser { get; set; }

        public string NameUser { get; set; } = null!;

        public string EmailUser { get; set; } = null!;

        public string ParolUser { get; set; } = null!;

        public int NumberUser { get; set; }
        public DateTime? DeleteAt { get; set; }

    }
}
