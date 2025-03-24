using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Diplom2.DTO
{
    public  class RegistUser
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdUser { get; set; }

        public string NameUser { get; set; } = null!;

        public string EmailUser { get; set; } = null!;

        public string ParolUser { get; set; } = null!;

        public int NumberUser { get; set; }
      public string kod {  get; set; } = null!;
    }
}
