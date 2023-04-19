using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CORE.Loyal.Models.FTMUSIC
{
    public class ComentarioModel
    {
        public long? Id { get; set; }
        public long? IdUsuario { get; set; }
        public long? IdCancion { get; set; }
        public string? Mensaje { get; set; }
        public DateTime? Fecha { get; set; }
        public string? NombreUsuario { get; set; }
    }
}
