using System;
using System.Collections.Generic;

namespace Diplom2.Db;

public partial class Status
{
    public int IdStatusOrder { get; set; }

    public string? NameStatus { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
