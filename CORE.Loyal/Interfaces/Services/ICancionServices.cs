using Core.Loyal.Models.FTMUSIC;

namespace CORE.Loyal.Interfaces.Services
{
    public interface ICancionServices
    {
        Task<List<CancionModel>> GetList();
        Task<long> SaveCancion(CancionModel user);
        Task<CancionModel> ConsultarCancion(int id);
        Task<long> DesactivarCancion(int id);
        Task<long> ModificarCancion(CancionModel cancion);

    }
}
