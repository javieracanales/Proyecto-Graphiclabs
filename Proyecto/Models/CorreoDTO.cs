using System;
using System.Collections.Generic;

namespace Proyecto.Models;

public partial class CorreoDTO
{
   public string? Destinatario { set; get; }

   public string? Asunto { set; get; }

   public string? Mensaje { set; get; }
}
