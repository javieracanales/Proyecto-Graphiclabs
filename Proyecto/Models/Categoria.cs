using System;
using System.Collections.Generic;

namespace Proyecto.Models;

public partial class Categoria
{
    public byte Id { get; set; }

    public string? Nombre { get; set; }

    public virtual ICollection<Producto> Productos { get; } = new List<Producto>();
}
