using System;
using System.Collections.Generic;

namespace Diplom2.Db;

public partial class KrossOrder
{
    public int IdKrossOrder { get; set; }

    public int? IdOrder { get; set; }

    public int? IdBuket { get; set; }

    public int? IdTovar { get; set; }

    public virtual Buket? IdBuketNavigation { get; set; }

    public virtual Order? IdOrderNavigation { get; set; }

    public virtual Tovar? IdTovarNavigation { get; set; }
}
