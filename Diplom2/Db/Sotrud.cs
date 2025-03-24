using System;
using System.Collections.Generic;

namespace Diplom2.Db;

public partial class Sotrud
{
    public int IdSotrud { get; set; }

    public string ParolSotrud { get; set; } = null!;

    public string LoginSotrud { get; set; } = null!;

    public string NameSotrud { get; set; } = null!;

    public virtual ICollection<Raspisanie> Raspisanies { get; set; } = new List<Raspisanie>();
}
