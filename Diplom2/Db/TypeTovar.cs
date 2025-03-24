using System;
using System.Collections.Generic;

namespace Diplom2.Db;

public partial class TypeTovar
{
    public int IdTypeTovar { get; set; }

    public string? NameType { get; set; }

    public virtual ICollection<Tovar> Tovars { get; set; } = new List<Tovar>();
}
