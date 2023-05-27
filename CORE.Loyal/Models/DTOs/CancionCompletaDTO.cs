using Core.Loyal.Models.FTMUSIC;
using CORE.Loyal.Models.FTMUSIC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace CORE.Loyal.Models.DTOs
{
    public class CancionCompletaDTO:CancionModel
    {
        public long? NumeroLikes { get; set; }
        public long? NumeroDisLikes { get; set; }
        public List<ComentarioModel> Comentarios { get; set; }

        public void MapearCancion(CancionModel cancion)
        {
            if (cancion != null) { 
            Id = cancion.Id;
            IdUsuario = cancion.IdUsuario;
            IdAlbun= cancion.IdAlbun;
            Nombre = cancion.Nombre;
            Genero = cancion.Genero;
            Letra = cancion.Letra;
            FechaPublicacion = cancion.FechaPublicacion;
            Link = cancion.Link;
            NombreAutor = cancion.NombreAutor;
                }
        }
    }
}
