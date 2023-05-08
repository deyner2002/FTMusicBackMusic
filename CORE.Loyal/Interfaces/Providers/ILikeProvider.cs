using Core.Loyal.Models.FTMUSIC;
using CORE.Loyal.Models.DTOs;
using CORE.Loyal.Models.FTMUSIC;

namespace CORE.Loyal.Interfaces.Providers
{
    public interface ILikeProvider
    {
        
        Task<long> GuardarLikeCancion(LikeModel like);
        Task<long> GuardarDisLikeCancion(DisLikeModel disLike);
        Task<long> GuardarLikeInterpretacion(LikeModel like);
        Task<long> GuardarDisLikeInterpretacion(DisLikeModel disLike);
        Task<long> ConsultarNumeroMegustaPorCancion(int idCancion);
        Task<long> ConsultarNumeroNoMegustaPorCancion(int idCancion);
        Task<long> ValidarLikeYDislikePorUsuario(int IdUsuario, int IdCancion);

    }
}
