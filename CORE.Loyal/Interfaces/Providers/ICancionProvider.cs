using Core.Loyal.Models.FTMUSIC;
using CORE.Loyal.Models.DTOs;
using CORE.Loyal.Models.FTMUSIC;

namespace CORE.Loyal.Interfaces.Providers
{
    public interface ICancionProvider
    {
        Task<List<CancionModel>> GetListCanciones();
        Task<List<InterpretacionModel>> GetListInterpretaciones();
        Task<long> SaveCancion(CancionModel user);
        Task<long> SaveInterpretacion(InterpretacionModel user);
        Task<long> ModificarInterpretacion(InterpretacionModel interpretacion);
        Task<CancionModel> ConsultarCancion(int id);
        Task<long> DesactivarCancion(int id);
        Task<InterpretacionModel> ConsultarInterpretacion(int id);
        Task<long> ModificarCancion(CancionModel cancion);
        Task<long> DesactivarInterpretacion(int id);
        Task<List<InterpretacionModel>> ConsultarInterpretacionPorUsuario(int idUsuario);
        Task<List<CancionModel>> ConsultarCancionPorUsuario(int idUsuario);

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
        Task<long> ValidarLikeYDislikePorUsuario(int IdUsuario, int IdCancion);
        Task<long> DesactivarComentario(int id);
        Task<long> ModificarComentario(ComentarioModel comentario);
        Task<long> AniadirInterpretacionAFavoritos(int idUsuario, int idCancion);
        Task<long> AniadirCancionAFavoritos(int idUsuario, int idCancion);
        Task<List<CancionModel>> ConsultarCancionesFavoritasPorUsuario(int idUsuario);
        Task<List<CancionModel>> ConsultarInterpretacionesFavoritasPorUsuario(int idUsuario);

    }
}
