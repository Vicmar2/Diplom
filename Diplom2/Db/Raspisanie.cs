using System;
using System.Collections.Generic;

namespace Diplom2.Db;

public partial class Raspisanie
{
    public int IdRaspisanie { get; set; }

    public int? IdSotrud { get; set; }

    public int? IdSmena { get; set; }

    public virtual Smena? IdSmenaNavigation { get; set; }

    public virtual Sotrud? IdSotrudNavigation { get; set; }
}
