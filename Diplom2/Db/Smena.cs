using System;
using System.Collections.Generic;

namespace Diplom2.Db;

public partial class Smena
{
    public int IdSmena { get; set; }

    public DateTime? StartSmena { get; set; }

    public DateTime? EndSmena { get; set; }

    public virtual ICollection<Raspisanie> Raspisanies { get; set; } = new List<Raspisanie>();
}
