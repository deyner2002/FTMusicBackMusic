using Core.Loyal.Models.FTMUSIC;
using CORE.Loyal.Interfaces.Providers;
using CORE.Loyal.Interfaces.Services;
using CORE.Loyal.Models.DTOs;
using CORE.Loyal.Models.FTMUSIC;
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
        public async Task<List<CancionModel>> GetListCanciones()
        {
            List<CancionModel> list = new List<CancionModel>(); 
            try
            {
               list = await _provider.GetListCanciones();
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




        public async Task<long> DesactivarCancion(int id)
        {
            long salida = -1;
            try
            {
                salida = await _provider.DesactivarCancion(id);
            }
            catch (Exception ex)
            {
                Plugins.WriteExceptionLog(ex);
            }
            return salida;
        }

        public async Task<long> ModificarCancion(CancionModel cancion)
        {
            long salida = -1;
            try
            {
                salida = await _provider.ModificarCancion(cancion);
            }
            catch (Exception ex)
            {
                Plugins.WriteExceptionLog(ex);
            }
            return salida;
        }




        public async Task<List<CancionModel>> ConsultarCancionPorUsuario(int idUsuario)
        {
            List<CancionModel> list = new List<CancionModel>();
            try
            {
                list = await _provider.ConsultarCancionPorUsuario(idUsuario);
            }
            catch (Exception ex)
            {
                Plugins.WriteExceptionLog(ex);
            }
            return list;
        }






        public async Task<List<CancionModel>> ConsultarCancionPorNombre(string nombre)
        {
            List<CancionModel> list = new List<CancionModel>();
            try
            {
                list = await _provider.ConsultarCancionPorNombre(nombre);
            }
            catch (Exception ex)
            {
                Plugins.WriteExceptionLog(ex);
            }
            return list;
        }





        public async Task<long> GuardarComentarioCancion(ComentarioModel comentario)
        {
            long consecutivo = 0;
            try
            {
                consecutivo = await _provider.GuardarComentarioCancion(comentario);

            }
            catch (Exception ex)
            {
                Plugins.WriteExceptionLog(ex);
                return -1;
            }
            return consecutivo;
        }





        public async Task<List<ComentarioModel>> ConsultarComentarioPorCancion(int idCancion)
        {
            List<ComentarioModel> list = new List<ComentarioModel>();
            try
            {
                list = await _provider.ConsultarComentarioPorCancion(idCancion);
            }
            catch (Exception ex)
            {
                Plugins.WriteExceptionLog(ex);
            }
            return list;
        }



        public async Task<long> GuardarLikeCancion(LikeModel like)
        {
            long resultado = 0;
            try
            {
                resultado = await _provider.GuardarLikeCancion(like);

            }
            catch (Exception ex)
            {
                Plugins.WriteExceptionLog(ex);
                return -1;
            }
            return resultado;
        }

        public async Task<long> GuardarDisLikeCancion(DisLikeModel disLike)
        {
            long resultado = 0;
            try
            {
                resultado = await _provider.GuardarDisLikeCancion(disLike);

            }
            catch (Exception ex)
            {
                Plugins.WriteExceptionLog(ex);
                return -1;
            }
            return resultado;
        }


        public async Task<long> ConsultarNumeroMegustaPorCancion(int idCancion)
        {
            long resultado = 0;
            try
            {
                resultado = await _provider.ConsultarNumeroMegustaPorCancion(idCancion);

            }
            catch (Exception ex)
            {
                Plugins.WriteExceptionLog(ex);
                return -1;
            }
            return resultado;
        }

        public async Task<long> ConsultarNumeroNoMegustaPorCancion(int idCancion)
        {
            long resultado = 0;
            try
            {
                resultado = await _provider.ConsultarNumeroNoMegustaPorCancion(idCancion);

            }
            catch (Exception ex)
            {
                Plugins.WriteExceptionLog(ex);
                return -1;
            }
            return resultado;
        }

        public async Task<CancionCompletaDTO> ConsultarCancionCompleta(int idCancion)
        {
            CancionCompletaDTO cancionCompletaDTO = new CancionCompletaDTO();
            try
            {
                cancionCompletaDTO = await _provider.ConsultarCancionCompleta(idCancion);
            }
            catch (Exception ex)
            {
                Plugins.WriteExceptionLog(ex);
            }
            return cancionCompletaDTO;
        }



        public async Task<List<InterpretacionModel>> GetListInterpretaciones()
        {
            List<InterpretacionModel> list = new List<InterpretacionModel>();
            try
            {
                list = await _provider.GetListInterpretaciones();
            }
            catch (Exception ex)
            {
                Plugins.WriteExceptionLog(ex);
            }
            return list;
        }

        public async Task<List<InterpretacionModel>> ConsultarInterpretacionPorNombre(string nombre)
        {
            List<InterpretacionModel> list = new List<InterpretacionModel>();
            try
            {
                list = await _provider.ConsultarInterpretacionPorNombre(nombre);
            }
            catch (Exception ex)
            {
                Plugins.WriteExceptionLog(ex);
            }
            return list;
        }



        public async Task<long> SaveInterpretacion(InterpretacionModel interpretacion)
        {
            long consecutivo = 0;
            try
            {
                consecutivo = await _provider.SaveInterpretacion(interpretacion);

            }
            catch (Exception ex)
            {
                Plugins.WriteExceptionLog(ex);
                return -1;
            }
            return consecutivo;
        }




        public async Task<List<InterpretacionModel>> ConsultarInterpretacionPorUsuario(int idUsuario)
        {
            List<InterpretacionModel> list = new List<InterpretacionModel>();
            try
            {
                list = await _provider.ConsultarInterpretacionPorUsuario(idUsuario);
            }
            catch (Exception ex)
            {
                Plugins.WriteExceptionLog(ex);
            }
            return list;
        }




        public async Task<long> GuardarLikeInterpretacion(LikeModel like)
        {
            long resultado = 0;
            try
            {
                resultado = await _provider.GuardarLikeInterpretacion(like);

            }
            catch (Exception ex)
            {
                Plugins.WriteExceptionLog(ex);
                return -1;
            }
            return resultado;
        }

        public async Task<long> GuardarDisLikeInterpretacion(DisLikeModel disLike)
        {
            long resultado = 0;
            try
            {
                resultado = await _provider.GuardarDisLikeInterpretacion(disLike);

            }
            catch (Exception ex)
            {
                Plugins.WriteExceptionLog(ex);
                return -1;
            }
            return resultado;
        }

        public async Task<long> GuardarComentarioInterpretacion(ComentarioModel comentario)
        {
            long consecutivo = 0;
            try
            {
                consecutivo = await _provider.GuardarComentarioInterpretacion(comentario);

            }
            catch (Exception ex)
            {
                Plugins.WriteExceptionLog(ex);
                return -1;
            }
            return consecutivo;
        }



        public async Task<InterpretacionCompletaDTO> ConsultarInterpretacionCompleta(int id)
        {
            InterpretacionCompletaDTO interpretacionCompletaDTO = new InterpretacionCompletaDTO();
            try
            {
                interpretacionCompletaDTO = await _provider.ConsultarInterpretacionCompleta(id);
            }
            catch (Exception ex)
            {
                Plugins.WriteExceptionLog(ex);
            }
            return interpretacionCompletaDTO;
        }





        public async Task<InterpretacionModel> ConsultarInterpretacion(int id)
        {
            InterpretacionModel cancion = new InterpretacionModel();
            try
            {
                cancion = await _provider.ConsultarInterpretacion(id);
            }
            catch (Exception ex)
            {
                Plugins.WriteExceptionLog(ex);
            }
            return cancion;
        }

        public async Task<long> DesactivarInterpretacion(int id)
        {
            long salida = -1;
            try
            {
                salida = await _provider.DesactivarInterpretacion(id);
            }
            catch (Exception ex)
            {
                Plugins.WriteExceptionLog(ex);
            }
            return salida;
        }


        public async Task<long> ModificarInterpretacion(InterpretacionModel interpretacion)
        {
            long salida = -1;
            try
            {
                salida = await _provider.ModificarInterpretacion(interpretacion);
            }
            catch (Exception ex)
            {
                Plugins.WriteExceptionLog(ex);
            }
            return salida;
        }



        public async Task<long> ValidarLikeYDislikePorUsuario(int IdUsuario, int IdCancion)
        {
            long salida = -2;
            try
            {
                salida = await _provider.ValidarLikeYDislikePorUsuario(IdUsuario,IdCancion);
            }
            catch (Exception ex)
            {
                Plugins.WriteExceptionLog(ex);
            }
            return salida;
        }


        public async Task<long> DesactivarComentario(int id)
        {
            long salida = -1;
            try
            {
                salida = await _provider.DesactivarComentario(id);
            }
            catch (Exception ex)
            {
                Plugins.WriteExceptionLog(ex);
            }
            return salida;
        }





        public async Task<long> ModificarComentario(ComentarioModel comentario)
        {
            long salida = -1;
            try
            {
                salida = await _provider.ModificarComentario(comentario);
            }
            catch (Exception ex)
            {
                Plugins.WriteExceptionLog(ex);
            }
            return salida;
        }




    }
}
