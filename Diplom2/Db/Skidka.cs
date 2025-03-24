using System;
using System.Collections.Generic;

namespace Diplom2.Db;

public partial class Skidka
{
    public int IdSkidka { get; set; }

    public string? NameSkidka { get; set; }

    public double PriceOrder { get; set; }

    public DateTime? DeleteAt { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
