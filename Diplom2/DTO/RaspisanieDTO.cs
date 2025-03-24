using Diplom2.Db;

namespace Diplom2.DTO
{
    public class RaspisanieDTO
    {
        public int IdRaspisanie { get; set; }

        public int? IdSotrud { get; set; }

        public int? IdSmena { get; set; }

        public virtual SmenaDTO? IdSmenaNavigation { get; set; }

        public virtual SotrudDTO? IdSotrudNavigation { get; set; }

    }
}
