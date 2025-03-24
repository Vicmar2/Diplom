using System;
using System.Collections.Generic;

namespace Diplom2.Db;

public partial class Tovar
{
    public int IdTovar { get; set; }

    public string NameTovar { get; set; } = null!;

    public decimal PriceTovar { get; set; }

    public byte[]? ImageTovar { get; set; }

    public int StockTovar { get; set; }

    public int IdTypeTovar { get; set; }

    public DateTime? DeleteAt { get; set; }

    public virtual TypeTovar IdTypeTovarNavigation { get; set; } = null!;

    public virtual ICollection<KrossBuket> KrossBukets { get; set; } = new List<KrossBuket>();

    public virtual ICollection<KrossOrder> KrossOrders { get; set; } = new List<KrossOrder>();
}
