using System;
using System.Collections.Generic;

namespace Diplom2.Db;

public partial class Buket
{
    public int IdBuket { get; set; }

    public byte[]? ImageBuket { get; set; }

    public decimal? PriceBuket { get; set; }

    public DateTime? DeleteAt { get; set; }

    public virtual ICollection<KrossBuket> KrossBukets { get; set; } = new List<KrossBuket>();

    public virtual ICollection<KrossOrder> KrossOrders { get; set; } = new List<KrossOrder>();
}
