using Core.Loyal.Models.FTMUSIC;
using CORE.Loyal.Interfaces.Providers;
using CORE.Loyal.Interfaces.Services;
using Support.Loyal.Util;

namespace Core.Loyal.Services
{
    public class CancionServices : ICancionServices
    {
        private readonly ICancionProvider _provider;
        public CancionServices(ICancionProvider provider)
        {
            _provider = provider;
        }
        public async Task<List<CancionModel>> GetList()
        {
            List<CancionModel> list = new List<CancionModel>(); 
            try
            {
               list = await _provider.GetList();
            }
            catch (Exception ex)
            {
                Plugins.WriteExceptionLog(ex);
            }
            return list;
        }
        public async Task<long> SaveCancion(CancionModel cancion)
        {
            long consecutivo = 0;
            try
            {
                consecutivo = await _provider.SaveCancion(cancion);

            }
            catch (Exception ex)
            {
                Plugins.WriteExceptionLog(ex);
                return -1;
            }
            return consecutivo;
        }




        public async Task<CancionModel> ConsultarCancion(int id)
        {
            CancionModel cancion =new CancionModel();
            try
            {
                cancion = await _provider.ConsultarCancion(id);
            }
            catch (Exception ex)
            {
                Plugins.WriteExceptionLog(ex);
            }
            return cancion;
        }

    }
}
