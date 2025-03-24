using Diplom2.Db;

namespace Diplom2.DTO
{
    public class OrderDTO
    {
        public int IdOrder { get; set; }

        public int IdUser { get; set; }

        public DateTime CreatedAt { get; set; }

        public int StatusOrder { get; set; } 

        public decimal PriceOrder { get; set; }

        public int? IdSkidka { get; set; }
        public DateTime? DeleteAt { get; set; }

        public virtual SkidkaDTO? IdSkidkaNavigation { get; set; }

        public virtual UserDTO IdUserNavigation { get; set; } = null!;
        public virtual StatusDTO StatusOrderNavigation { get; set; } = null!;
    }
}
