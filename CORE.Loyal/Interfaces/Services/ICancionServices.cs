using Core.Loyal.Models.FTMUSIC;
using CORE.Loyal.Models.DTOs;
using CORE.Loyal.Models.FTMUSIC;

namespace CORE.Loyal.Interfaces.Services
{
    public interface ICancionServices
    {
        Task<List<CancionModel>> GetListCanciones(int limiteInferiorConsulta, int limiteSuperiorConsulta);
        Task<long> SaveCancion(CancionModel user);
        Task<long> SaveInterpretacion(InterpretacionModel user);
        Task<CancionModel> ConsultarCancion(int id);
        Task<InterpretacionModel> ConsultarInterpretacion(int id);
        Task<long> DesactivarCancion(int id);
        Task<long> DesactivarInterpretacion(int id);
        Task<long> ModificarCancion(CancionModel cancion);
        Task<long> ModificarInterpretacion(InterpretacionModel interpretacion);
        Task<List<CancionModel>> ConsultarCancionPorUsuario(int idUsuario);
        Task<List<InterpretacionModel>> ConsultarInterpretacionPorUsuario(int idUsuario);
        Task<List<CancionModel>> ConsultarCancionPorNombre(string nombre);
        Task<List<InterpretacionModel>> ConsultarInterpretacionPorNombre(string nombre);
        Task<long> GuardarComentarioCancion(ComentarioModel comentario);
        Task<long> GuardarComentarioInterpretacion(ComentarioModel comentario);
        Task<List<ComentarioModel>> ConsultarComentarioPorCancion(int idCancion);
        Task<long> GuardarLikeCancion(LikeModel like);
        Task<long> GuardarDisLikeCancion(DisLikeModel disLike);
        Task<long> GuardarLikeInterpretacion(LikeModel like);
        Task<long> GuardarDisLikeInterpretacion(DisLikeModel disLike);
        Task<long> ConsultarNumeroMegustaPorCancion(int idCancion);
        Task<long> ConsultarNumeroNoMegustaPorCancion(int idCancion);
        Task<CancionCompletaDTO> ConsultarCancionCompleta(int idCancion);
        Task<InterpretacionCompletaDTO> ConsultarInterpretacionCompleta(int id);
        Task<List<InterpretacionModel>> GetListInterpretaciones(int limiteInferiorConsulta, int limiteSuperiorConsulta);
        Task<long> ValidarLikeYDislikePorUsuario(int IdUsuario, int IdCancion);
        Task<long> DesactivarComentario(int id);
        Task<long> ModificarComentario(ComentarioModel comentario);
        Task<long> AniadirInterpretacionAFavoritos(int idUsuario, int idCancion);
        Task<long> AniadirCancionAFavoritos(int idUsuario, int idCancion);
        Task<List<CancionModel>> ConsultarCancionesFavoritasPorUsuario(int idUsuario);
        Task<List<CancionModel>> ConsultarInterpretacionesFavoritasPorUsuario(int idUsuario);

        Task<long> EliminarDeFavoritos(int idUsuario, int idCancion);
        Task<long> ValidarCancionEnFavoritos(int idUsuario, int idCancion);

    }
}
