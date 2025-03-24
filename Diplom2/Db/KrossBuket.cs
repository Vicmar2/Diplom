using System;
using System.Collections.Generic;

namespace Diplom2.Db;

public partial class KrossBuket
{
    public int IdKrossBuket { get; set; }

    public int IdBuket { get; set; }

    public int IdTovar { get; set; }

    public virtual Buket IdBuketNavigation { get; set; } = null!;

    public virtual Tovar IdTovarNavigation { get; set; } = null!;
}
