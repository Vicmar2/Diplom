using Diplom2.Db;

namespace Diplom2.DTO
{
    public class KrossOrderDTO
    {
        public int IdKrossOrder { get; set; }

        public int? IdOrder { get; set; }

        public int? IdBuket { get; set; }

        public int? IdTovar { get; set; }

        public virtual BuketDTO? IdBuketNavigation { get; set; }

        public virtual OrderDTO? IdOrderNavigation { get; set; }

        public virtual TovarDTO? IdTovarNavigation { get; set; }
    }
}
