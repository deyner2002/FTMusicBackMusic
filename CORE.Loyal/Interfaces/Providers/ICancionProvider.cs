using Core.Loyal.Models.FTMUSIC;

namespace CORE.Loyal.Interfaces.Providers
{
    public interface ICancionProvider
    {
        Task<List<CancionModel>> GetList();
        Task<long> SaveCancion(CancionModel user);
        Task<CancionModel> ConsultarCancion(int id);
    }
}
