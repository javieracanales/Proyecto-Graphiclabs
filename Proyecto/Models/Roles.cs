using System;
namespace Proyecto.Models
{
	public partial class Roles
	{
        public byte Id { get; set; }
        public string? Descripcion { get; set; }
        public virtual ICollection<Usuario> Usuarios { get; } = new List<Usuario>();
    }
}

