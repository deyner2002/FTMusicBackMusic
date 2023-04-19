using CORE.Loyal.Models.FTMUSIC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CORE.Loyal.Models.DTOs
{
    public class InterpretacionCompletaDTO:CancionCompletaDTO
    {
        public long? IdCancion { get; set; }
        public void MapearInterpretacion(InterpretacionModel interpretacion)
        {
            if (interpretacion != null)
            {
                MapearCancion(interpretacion);
                IdCancion = interpretacion.IdCancion;
            }
        }
    }
}
