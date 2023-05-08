using Core.Loyal.Models.FTMUSIC;
using CORE.Loyal.Models.DTOs;
using CORE.Loyal.Models.FTMUSIC;

namespace CORE.Loyal.Interfaces.Providers
{
    public interface IComentarioProvider
    {
        
        Task<long> GuardarComentarioCancion(ComentarioModel comentario);
        Task<long> GuardarComentarioInterpretacion(ComentarioModel comentario);
        Task<List<ComentarioModel>> ConsultarComentarioPorCancion(int idCancion);
        Task<long> DesactivarComentario(int id);
        Task<long> ModificarComentario(ComentarioModel comentario);


    }
}
