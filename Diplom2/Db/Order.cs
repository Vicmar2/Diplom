using System;
using System.Collections.Generic;

namespace Diplom2.Db;

public partial class Order
{
    public int IdOrder { get; set; }

    public int IdUser { get; set; }

    public DateTime CreatedAt { get; set; }

    public int StatusOrder { get; set; }

    public decimal PriceOrder { get; set; }

    public int? IdSkidka { get; set; }

    public DateTime? DeleteAt { get; set; }

    public virtual Skidka? IdSkidkaNavigation { get; set; }

    public virtual User IdUserNavigation { get; set; } = null!;

    public virtual ICollection<KrossOrder> KrossOrders { get; set; } = new List<KrossOrder>();

    public virtual Status StatusOrderNavigation { get; set; } = null!;
}
