using System;
using System.Collections.Generic;

namespace Diplom2.Db;

public partial class User
{
    public int IdUser { get; set; }

    public string NameUser { get; set; } = null!;

    public string EmailUser { get; set; } = null!;

    public string ParolUser { get; set; } = null!;

    public int NumberUser { get; set; }

    public DateTime? DeleteAt { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
