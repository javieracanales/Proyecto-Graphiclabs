using System;
namespace Proyecto.Models
{
	public partial class Ban
	{
		public byte Id { get; set; }
		public string? Descripcion { get; set; }
		public bool Estado { get; set; }
		public byte IdUsuario { get; set; }

        public virtual Usuario? IdUsuarioNavigation { get; set; }
    }
}

