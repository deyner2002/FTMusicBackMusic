using Core.Loyal.Models.FTMUSIC;
using CORE.Loyal.Interfaces.Services;
using CORE.Loyal.Models.DTOs;
using CORE.Loyal.Models.FTMUSIC;
using Microsoft.AspNetCore.Mvc;
using Support.Loyal.DTOs;
using Support.Loyal.Util;
using System.Diagnostics;
using System.Net;

namespace Api.Loyal.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CancionController : ControllerBase
    {
        private readonly ICancionServices _provider;

        public CancionController(ICancionServices provider)
        {
            _provider = provider;
        }

        [HttpGet]
        [Route("GetList")]
        public async Task<ResponseModels> GetListCanciones()
        {
            ResponseModels response = new ResponseModels();
            int intentosRestantes = 3;
            while (intentosRestantes > 0)
            {
                try
            {
                response.Datos = _provider.GetListCanciones().Result;
                if (response.Datos != null)
                {
                    intentosRestantes = 0;
                    response.IsError = false;
                    response.Mensaje = "Ok";
                }
                else
                {
                    intentosRestantes--;
                    response.IsError = true;
                    response.Mensaje = "Error en obtener datos";
                }

            }
            catch (Exception ex)
            {
                intentosRestantes--;
                Plugins.WriteExceptionLog(ex);
                response.IsError = true;
                response.Mensaje = "Error en obtener datos";
            }
        }
            return response;
        }

        [HttpPost]
        [Route("Save")]
        public async Task<ResponseModels> SaveCancion(CancionModel cancion)
        {
            ResponseModels response = new ResponseModels();
            int intentosRestantes = 3;
            while (intentosRestantes > 0)
            {
                try
            {
                response.Datos = _provider.SaveCancion(cancion).Result;
                long codigoRespuesta = long.Parse(response.Datos.ToString());
                if (codigoRespuesta == -2)
                {
                    intentosRestantes = 0;
                    response.IsError = true;
                    response.Mensaje = "Error: hay campos nulos que son obligatorios";
                }
                else
                {
                    if (codigoRespuesta == -1)
                    {
                        await Task.Delay(1000);
                        intentosRestantes--;
                        response.IsError = true;
                        response.Mensaje = "Error del sistema";
                    }
                    else
                    {
                        intentosRestantes = 0;
                        response.IsError = false;
                        response.Mensaje = "La cancion llamada: " + cancion.Nombre + " ha sido guardada";
                    }
                }

            }
            catch (Exception ex)
            {
                await Task.Delay(1000);
                intentosRestantes--;
                Plugins.WriteExceptionLog(ex);
                response.IsError = true;
                response.Mensaje = "Error del sistema";
            }
        }
            return response;
        }





        [HttpPost]
        [Route("ConsultarCancion")]
        public async Task<ResponseModels> ConsultarCancion(int id)
        {
            ResponseModels response = new ResponseModels();
            int intentosRestantes = 3;
            while (intentosRestantes > 0)
            {
                try
            {
                response.Datos = _provider.ConsultarCancion(id).Result;
                if (response.Datos != null)
                {
                    intentosRestantes = 0;
                    response.IsError = false;
                    response.Mensaje = "Ok";
                }
                else
                {
                    intentosRestantes = 0;
                    response.IsError = true;
                    response.Mensaje = "La cancion consultada no existe";
                }

            }
            catch (Exception ex)
            {
                intentosRestantes--;
                Plugins.WriteExceptionLog(ex);
                response.IsError = true;
                response.Mensaje = "Error en obtener datos";
            }
        }
            return response;
        }




        [HttpPost]
        [Route("DesactivarCancion")]
        public async Task<ResponseModels> DesactivarCancion(int id)
        {
            ResponseModels response = new ResponseModels();
            int intentosRestantes = 3;
            while (intentosRestantes > 0)
            {
                try
            {
                response.Datos = _provider.DesactivarCancion(id).Result;
                long codigoRespuesta = long.Parse(response.Datos.ToString());
                if (codigoRespuesta == 1)
                {
                    intentosRestantes = 0;
                    response.IsError = false;
                    response.Mensaje = "Cancion Desactivada";
                }
                if (codigoRespuesta == 0)
                {
                    intentosRestantes = 0;
                    response.IsError = true;
                    response.Mensaje = "La cancion a desactivar no existe";
                }
                if (codigoRespuesta == -1)
                {
                    intentosRestantes--;
                    response.IsError = true;
                    response.Mensaje = "Error";
                }

            }
            catch (Exception ex)
            {
                intentosRestantes--;
                Plugins.WriteExceptionLog(ex);
                response.IsError = true;
                response.Mensaje = "Error en obtener datos";
            }
        }
            return response;
        }





        [HttpPost]
        [Route("ModificarCancion")]
        public async Task<ResponseModels> ModificarCancion(CancionModel cancion)
        {
            ResponseModels response = new ResponseModels();
            int intentosRestantes = 3;
            while (intentosRestantes > 0)
            {
                try
            {
                response.Datos = _provider.ModificarCancion(cancion).Result;
                long codigoRespuesta = long.Parse(response.Datos.ToString());
                if (codigoRespuesta == 1)
                {
                    intentosRestantes = 0;
                    response.IsError = false;
                    response.Mensaje = "Cancion Modificada";
                }
                if (codigoRespuesta == 0)
                {
                    intentosRestantes = 0;
                    response.IsError = true;
                    response.Mensaje = "La cancion a Modificar no existe";
                }
                if (codigoRespuesta == -1)
                {
                    intentosRestantes--;
                    response.IsError = true;
                    response.Mensaje = "Error";
                }
                if (codigoRespuesta == -2)
                {
                    intentosRestantes = 0;
                    response.IsError = true;
                    response.Mensaje = "Campos vacios";
                }
                }
            catch (Exception ex)
            {
                intentosRestantes--;
                Plugins.WriteExceptionLog(ex);
                response.IsError = true;
                response.Mensaje = "Error en obtener datos";
            }
        }
            return response;
        }







        [HttpPost]
        [Route("ConsultarCancionPorUsuario")]
        public async Task<ResponseModels> ConsutlarCancionPorUsuario(int idUsuario)
        {
            ResponseModels response = new ResponseModels();
            int intentosRestantes = 3;
            while (intentosRestantes > 0)
            {
                try
            {
                response.Datos = _provider.ConsultarCancionPorUsuario(idUsuario).Result;
                if (response.Datos != null)
                {
                    intentosRestantes = 0;
                    response.IsError = false;
                    response.Mensaje = "Ok";
                }
                else
                {
                    intentosRestantes = 0;
                    response.IsError = true;
                    response.Mensaje = "No se encontraron canciones de este usuario";
                }

            }
            catch (Exception ex)
            {
                intentosRestantes--;
                Plugins.WriteExceptionLog(ex);
                response.IsError = true;
                response.Mensaje = "Error en obtener datos";
            }
        }
            return response;
        }




        [HttpPost]
        [Route("ConsultarCancionPorNombre")]
        public async Task<ResponseModels> ConsultarCancionPorNombre(string nombre)
        {
            ResponseModels response = new ResponseModels();
            int intentosRestantes = 3;
            while (intentosRestantes > 0)
            {
                try
            {
                response.Datos = _provider.ConsultarCancionPorNombre(nombre).Result;
                if (response.Datos != null)
                {
                    intentosRestantes = 0;
                    response.IsError = false;
                    response.Mensaje = "Ok";
                }
                else
                {
                    intentosRestantes--;
                    response.IsError = true;
                    response.Mensaje = "Error en obtener datos";
                }

            }
            catch (Exception ex)
            {
                intentosRestantes--;
                Plugins.WriteExceptionLog(ex);
                response.IsError = true;
                response.Mensaje = "Error en obtener datos";
            }
        }
            return response;
        }








        [HttpPost]
        [Route("GuardarComentarioCancion")]
        public async Task<ResponseModels> GuardarComentarioCancion(ComentarioModel comentario)
        {
            ResponseModels response = new ResponseModels();
            int intentosRestantes = 3;
            while (intentosRestantes > 0)
            {
                try
            {
                response.Datos = _provider.GuardarComentarioCancion(comentario).Result;
                long codigoRespuesta = long.Parse(response.Datos.ToString());
                if (codigoRespuesta == -2)
                {
                    intentosRestantes = 0;
                    response.IsError = true;
                    response.Mensaje = "Error: hay campos nulos que son obligatorios";
                }
                if (codigoRespuesta == -1)
                {
                    intentosRestantes--;
                    response.IsError = true;
                    response.Mensaje = "Error del sistema";
                }

                if (codigoRespuesta == -3)
                {
                    intentosRestantes = 0;
                    response.IsError = true;
                    response.Mensaje = "La cancion a la que se quiere registrar el comentario, no existe";
                }

                if (codigoRespuesta > 0)
                {
                    intentosRestantes = 0;
                    response.IsError = false;
                    response.Mensaje = "El comentario " + codigoRespuesta + " Ha sido guardado";
                }

            }
            catch (Exception ex)
            {
                intentosRestantes--;
                Plugins.WriteExceptionLog(ex);
                response.IsError = true;
                response.Mensaje = "Error del sistema";
            }
        }
            return response;
        }







        [HttpPost]
        [Route("ConsultarComentarioPorCancion")]
        public async Task<ResponseModels> ConsultarComentarioPorCancion(int idCancion)
        {
            ResponseModels response = new ResponseModels();
            int intentosRestantes = 3;
            while (intentosRestantes > 0)
            {
                try
            {
                response.Datos = _provider.ConsultarComentarioPorCancion(idCancion).Result;
                if (response.Datos != null)
                {
                    intentosRestantes = 0;
                    response.IsError = false;
                    response.Mensaje = "Ok";
                }
                else
                {
                    intentosRestantes = 0;
                    response.IsError = true;
                    response.Mensaje = "No se encontraron comentarios de esta cancion";
                }

            }
            catch (Exception ex)
            {
                intentosRestantes--;
                Plugins.WriteExceptionLog(ex);
                response.IsError = true;
                response.Mensaje = "Error en obtener datos";
            }
        }
            return response;
        }





        [HttpPost]
        [Route("GuardarLikeCancion")]
        public async Task<ResponseModels> GuardarLikeCancion(LikeModel like)
        {
            ResponseModels response = new ResponseModels();
            int intentosRestantes = 3;
            while (intentosRestantes > 0)
            {
                try
            {
                response.Datos = _provider.GuardarLikeCancion(like).Result;
                long codigoRespuesta = long.Parse(response.Datos.ToString());
                if (codigoRespuesta == -1)
                {
                    intentosRestantes--;
                    response.IsError = true;
                    response.Mensaje = "Error del sistema";
                }

                if (codigoRespuesta == -2)
                {
                    intentosRestantes = 0;
                    response.IsError = true;
                    response.Mensaje = "Error: hay campos nulos que son obligatorios";
                }

                if (codigoRespuesta == -3)
                {
                    intentosRestantes = 0;
                    response.IsError = true;
                    response.Mensaje = "La cancion a la que se quiere registrar el like, no existe";
                }

                if (codigoRespuesta == -4)
                {
                    intentosRestantes = 0;
                    response.IsError = false;
                    response.Mensaje = "Like eliminado de la cancion";
                }

                if (codigoRespuesta > 0)
                {
                    intentosRestantes = 0;
                    response.IsError = false;
                    response.Mensaje = "El like " + codigoRespuesta + " Ha sido guardado";
                }

            }
            catch (Exception ex)
            {
                intentosRestantes--;
                Plugins.WriteExceptionLog(ex);
                response.IsError = true;
                response.Mensaje = "Error del sistema";
            }
        }
            return response;
        }

        [HttpPost]
        [Route("GuardarDisLikeCancion")]
        public async Task<ResponseModels> GuardarDisLikeCancion(DisLikeModel disLike)
        {
            ResponseModels response = new ResponseModels();
            int intentosRestantes = 3;
            while (intentosRestantes > 0)
            {
                try
            {
                response.Datos = _provider.GuardarDisLikeCancion(disLike).Result;
                long codigoRespuesta = long.Parse(response.Datos.ToString());
                if (codigoRespuesta == -1)
                {
                    intentosRestantes--;
                    response.IsError = true;
                    response.Mensaje = "Error del sistema";
                }

                if (codigoRespuesta == -2)
                {
                    intentosRestantes = 0;
                    response.IsError = true;
                    response.Mensaje = "Error: hay campos nulos que son obligatorios";
                }

                if (codigoRespuesta == -3)
                {
                    intentosRestantes = 0;
                    response.IsError = true;
                    response.Mensaje = "La cancion a la que se quiere registrar el dislike, no existe";
                }

                if (codigoRespuesta == -4)
                {
                    intentosRestantes = 0;
                    response.IsError = false;
                    response.Mensaje = "DisLike eliminado de la cancion";
                }

                if (codigoRespuesta > 0)
                {
                    intentosRestantes = 0;
                    response.IsError = false;
                    response.Mensaje = "El DisLike " + codigoRespuesta + " Ha sido guardado";
                }

            }
            catch (Exception ex)
            {
                intentosRestantes--;
                Plugins.WriteExceptionLog(ex);
                response.IsError = true;
                response.Mensaje = "Error del sistema";
            }
        }
            return response;
        }




        [HttpPost]
        [Route("ConsultarNumeroMegustaPorCancion")]
        public async Task<ResponseModels> ConsultarNumeroMegustaPorCancion(int idCancion)
        {
            ResponseModels response = new ResponseModels();
            int intentosRestantes = 3;
            while (intentosRestantes > 0)
            {
                try
            {
                response.Datos = _provider.ConsultarNumeroMegustaPorCancion(idCancion).Result;
                long codigoRespuesta = long.Parse(response.Datos.ToString());
                if (codigoRespuesta == -2)
                {
                    intentosRestantes = 0;
                    response.IsError = true;
                    response.Mensaje = "Error del sistema";
                }

                if (codigoRespuesta == -1)
                {
                    intentosRestantes = 0;
                    response.IsError = true;
                    response.Mensaje = "La cancion en cuestion no tiene likes asignados";
                }

                if (codigoRespuesta > 0)
                {
                    intentosRestantes = 0;
                    response.IsError = false;
                    response.Mensaje = "El numero de Likes es de: " + codigoRespuesta;
                }

            }
            catch (Exception ex)
            {
                intentosRestantes--;
                Plugins.WriteExceptionLog(ex);
                response.IsError = true;
                response.Mensaje = "Error del sistema";
            }
        }
            return response;
        }

        [HttpPost]
        [Route("ConsultarNumeroNoMegustaPorCancion")]
        public async Task<ResponseModels> ConsultarNumeroNoMegustaPorCancion(int idCancion)
        {
            ResponseModels response = new ResponseModels();
            int intentosRestantes = 3;
            while (intentosRestantes > 0)
            {
                try
            {
                response.Datos = _provider.ConsultarNumeroNoMegustaPorCancion(idCancion).Result;
                long codigoRespuesta = long.Parse(response.Datos.ToString());
                if (codigoRespuesta == -2)
                {
                    intentosRestantes--;
                    response.IsError = true;
                    response.Mensaje = "Error del sistema";
                }

                if (codigoRespuesta == -1)
                {
                    intentosRestantes = 0;
                    response.IsError = true;
                    response.Mensaje = "La cancion en cuestion no tiene Dislikes asignados";
                }

                if (codigoRespuesta > 0)
                {
                    intentosRestantes = 0;
                    response.IsError = false;
                    response.Mensaje = "El numero de DisLikes es de: " + codigoRespuesta;
                }

            }
            catch (Exception ex)
            {
                intentosRestantes--;
                Plugins.WriteExceptionLog(ex);
                response.IsError = true;
                response.Mensaje = "Error del sistema";
            }
        }
            return response;
        }



        [HttpPost]
        [Route("ConsultarCancionCompleta")]
        public async Task<ResponseModels> ConsultarCancionCompleta(int idCancion)
        {
            ResponseModels response = new ResponseModels();
            int intentosRestantes = 3;
            while (intentosRestantes > 0)
            {
                try
            {
                response.Datos = _provider.ConsultarCancionCompleta(idCancion).Result;
                if (response.Datos != null)
                {
                    intentosRestantes = 0;
                    response.IsError = false;
                    response.Mensaje = "la cancion se ha consultado correctamente";
                }
                else
                {
                    intentosRestantes = 0;
                    response.IsError = true;
                    response.Mensaje = "no se han encontrado registros pertenecientes a la cancion identificada como: " + idCancion;
                }

            }
            catch (Exception ex)
            {
                intentosRestantes--;
                Plugins.WriteExceptionLog(ex);
                response.IsError = true;
                response.Mensaje = "Error del sistema";
            }
        }
            return response;
        }





        [HttpGet]
        [Route("GetListInterpretaciones")]
        public async Task<ResponseModels> GetListInterpretaciones()
        {
            ResponseModels response = new ResponseModels();
            int intentosRestantes = 3;
            while (intentosRestantes > 0)
            {
                try
            {
                response.Datos = _provider.GetListInterpretaciones().Result;
                if (response.Datos != null)
                {
                    intentosRestantes = 0;
                    response.IsError = false;
                    response.Mensaje = "Ok";
                }
                else
                {
                    intentosRestantes = 0;
                    response.IsError = true;
                    response.Mensaje = "Error en obtener datos";
                }

            }
            catch (Exception ex)
            {
                intentosRestantes--;
                Plugins.WriteExceptionLog(ex);
                response.IsError = true;
                response.Mensaje = "Error en obtener datos";
            }
        }
            return response;
        }


        [HttpPost]
        [Route("ConsultarInterpretacionPorNombre")]
        public async Task<ResponseModels> ConsultarInterpretacionPorNombre(string nombre)
        {
            ResponseModels response = new ResponseModels();
            int intentosRestantes = 3;
            while (intentosRestantes > 0)
            {
                try
            {
                response.Datos = _provider.ConsultarInterpretacionPorNombre(nombre).Result;
                if (response.Datos != null)
                {
                    intentosRestantes = 0;
                    response.IsError = false;
                    response.Mensaje = "Ok";
                }
                else
                {
                    intentosRestantes--;
                    response.IsError = true;
                    response.Mensaje = "Error en obtener datos";
                }

            }
            catch (Exception ex)
            {
                intentosRestantes--;
                Plugins.WriteExceptionLog(ex);
                response.IsError = true;
                response.Mensaje = "Error en obtener datos";
            }
        }
            return response;
        }





        [HttpPost]
        [Route("ConsultarInterpretacionPorUsuario")]
        public async Task<ResponseModels> ConsutlarInterpretacionPorUsuario(int idUsuario)
        {
            ResponseModels response = new ResponseModels();
            int intentosRestantes = 3;
            while (intentosRestantes > 0)
            {
                try
            {
                response.Datos = _provider.ConsultarInterpretacionPorUsuario(idUsuario).Result;
                if (response.Datos != null)
                {
                    intentosRestantes = 0;
                    response.IsError = false;
                    response.Mensaje = "Ok";
                }
                else
                {
                    intentosRestantes = 0;
                    response.IsError = true;
                    response.Mensaje = "No se encontraron canciones de este usuario";
                }

            }
            catch (Exception ex)
            {
                intentosRestantes--;
                Plugins.WriteExceptionLog(ex);
                response.IsError = true;
                response.Mensaje = "Error en obtener datos";
            }
        }
            return response;
        }



        [HttpPost]
        [Route("SaveInterpretacion")]
        public async Task<ResponseModels> SaveInterpretacion(InterpretacionModel user)
        {
            ResponseModels response = new ResponseModels();
            int intentosRestantes = 3;
            while (intentosRestantes > 0)
            {
                try
            {
                response.Datos = _provider.SaveInterpretacion(user).Result;
                long codigoRespuesta = long.Parse(response.Datos.ToString());
                if (codigoRespuesta == -2)
                {
                    intentosRestantes = 0;
                    response.IsError = true;
                    response.Mensaje = "Error: hay campos nulos que son obligatorios";
                }
                else
                {
                    if (codigoRespuesta == -1)
                    {
                        intentosRestantes--;
                        response.IsError = true;
                        response.Mensaje = "Error del sistema";
                    }
                    if (codigoRespuesta >= 0)
                    {
                        intentosRestantes = 0;
                        response.IsError = false;
                        response.Mensaje = "La Interpretacion llamada: " + user.Nombre + " ha sido guardada";
                    }
                    if (codigoRespuesta == -3)
                    {
                        intentosRestantes = 0;
                        response.IsError = true;
                        response.Mensaje = "La cancion a la que se hace referencia en la interpretacion no existe";
                    }
                }

            }
            catch (Exception ex)
            {
                intentosRestantes--;
                Plugins.WriteExceptionLog(ex);
                response.IsError = true;
                response.Mensaje = "Error del sistema";
            }
        }
            return response;
        }




        [HttpPost]
        [Route("GuardarLikeInterpretacion")]
        public async Task<ResponseModels> GuardarLikeInterpretacion(LikeModel like)
        {
            ResponseModels response = new ResponseModels();
            int intentosRestantes = 3;
            while (intentosRestantes > 0)
            {
                try
            {
                response.Datos = _provider.GuardarLikeInterpretacion(like).Result;
                long codigoRespuesta = long.Parse(response.Datos.ToString());
                if (codigoRespuesta == -1)
                {
                    intentosRestantes--;
                    response.IsError = true;
                    response.Mensaje = "Error del sistema";
                }

                if (codigoRespuesta == -2)
                {
                    intentosRestantes = 0;
                    response.IsError = true;
                    response.Mensaje = "Error: hay campos nulos que son obligatorios";
                }

                if (codigoRespuesta == -3)
                {
                    intentosRestantes = 0;
                    response.IsError = true;
                    response.Mensaje = "La interpretacion a la que se quiere registrar el like, no existe";
                }

                if (codigoRespuesta == -4)
                {
                    intentosRestantes = 0;
                    response.IsError = false;
                    response.Mensaje = "Like eliminado de la interpretacion";
                }

                if (codigoRespuesta > 0)
                {
                    intentosRestantes = 0;
                    response.IsError = false;
                    response.Mensaje = "El like " + codigoRespuesta + " Ha sido guardado";
                }

            }
            catch (Exception ex)
            {
                intentosRestantes--;
                Plugins.WriteExceptionLog(ex);
                response.IsError = true;
                response.Mensaje = "Error del sistema";
            }
        }
            return response;
        }

        [HttpPost]
        [Route("GuardarDisLikeInterpretacion")]
        public async Task<ResponseModels> GuardarDisLikeInterpretacion(DisLikeModel disLike)
        {
            ResponseModels response = new ResponseModels();
            int intentosRestantes = 3;
            while (intentosRestantes > 0)
            {
                try
            {
                response.Datos = _provider.GuardarDisLikeInterpretacion(disLike).Result;
                long codigoRespuesta = long.Parse(response.Datos.ToString());
                if (codigoRespuesta == -1)
                {
                    intentosRestantes--;
                    response.IsError = true;
                    response.Mensaje = "Error del sistema";
                }

                if (codigoRespuesta == -2)
                {
                    intentosRestantes = 0;
                    response.IsError = true;
                    response.Mensaje = "Error: hay campos nulos que son obligatorios";
                }

                if (codigoRespuesta == -3)
                {
                    intentosRestantes = 0;
                    response.IsError = true;
                    response.Mensaje = "La interpretacion a la que se quiere registrar el dislike, no existe";
                }

                if (codigoRespuesta == -4)
                {
                    intentosRestantes = 0;
                    response.IsError = false;
                    response.Mensaje = "DisLike eliminado de la interpretacion";
                }

                if (codigoRespuesta > 0)
                {
                    intentosRestantes = 0;
                    response.IsError = false;
                    response.Mensaje = "El DisLike " + codigoRespuesta + " Ha sido guardado";
                }

            }
            catch (Exception ex)
            {
                intentosRestantes--;
                Plugins.WriteExceptionLog(ex);
                response.IsError = true;
                response.Mensaje = "Error del sistema";
            }
        }
            return response;
        }

        [HttpPost]
        [Route("GuardarComentarioInterpretacion")]
        public async Task<ResponseModels> GuardarComentarioInterpretacion(ComentarioModel comentario)
        {
            ResponseModels response = new ResponseModels();
            int intentosRestantes = 3;
            while (intentosRestantes > 0)
            {
                try
            {
                response.Datos = _provider.GuardarComentarioInterpretacion(comentario).Result;
                long codigoRespuesta = long.Parse(response.Datos.ToString());
                if (codigoRespuesta == -2)
                {
                    intentosRestantes = 0;
                    response.IsError = true;
                    response.Mensaje = "Error: hay campos nulos que son obligatorios";
                }
                if (codigoRespuesta == -1)
                {
                    intentosRestantes--;
                    response.IsError = true;
                    response.Mensaje = "Error del sistema";
                }

                if (codigoRespuesta == -3)
                {
                    intentosRestantes = 0;
                    response.IsError = true;
                    response.Mensaje = "La Interpretacion a la que se quiere registrar el comentario, no existe";
                }

                if (codigoRespuesta > 0)
                {
                    intentosRestantes = 0;
                    response.IsError = false;
                    response.Mensaje = "El comentario " + codigoRespuesta + " Ha sido guardado";
                }

            }
            catch (Exception ex)
            {
                intentosRestantes--;
                Plugins.WriteExceptionLog(ex);
                response.IsError = true;
                response.Mensaje = "Error del sistema";
            }
        }
            return response;
        }




        [HttpPost]
        [Route("ConsultarInterpretacionCompleta")]
        public async Task<ResponseModels> ConsultarInterpretacionCompleta(int id)
        {
            ResponseModels response = new ResponseModels();
            int intentosRestantes = 3;
            while (intentosRestantes > 0)
            {
                try
            {
                response.Datos = _provider.ConsultarInterpretacionCompleta(id).Result;
                if (response.Datos != null)
                {
                    intentosRestantes = 0;
                    response.IsError = false;
                    response.Mensaje = "la cancion se ha consultado correctamente";
                }
                else
                {
                    intentosRestantes = 0;
                    response.IsError = true;
                    response.Mensaje = "no se han encontrado registros pertenecientes a la Interpretacion identificada como: " + id;
                }

            }
            catch (Exception ex)
            {
                intentosRestantes--;
                Plugins.WriteExceptionLog(ex);
                response.IsError = true;
                response.Mensaje = "Error del sistema";
            }
        }
            return response;
        }


        [HttpPost]
        [Route("DesactivarInterpretacion")]
        public async Task<ResponseModels> DesactivarInterpretacion(int id)
        {
            ResponseModels response = new ResponseModels();
            int intentosRestantes = 3;
            while (intentosRestantes > 0)
            {
                try
            {
                response.Datos = _provider.DesactivarInterpretacion(id).Result;
                long codigoRespuesta = long.Parse(response.Datos.ToString());
                if (codigoRespuesta == 1)
                {
                    intentosRestantes = 0;
                    response.IsError = false;
                    response.Mensaje = "Interpretacion Desactivada";
                }
                if (codigoRespuesta == 0)
                {
                    intentosRestantes = 0;
                    response.IsError = true;
                    response.Mensaje = "La interpretacion a desactivar no existe";
                }
                if (codigoRespuesta == -1)
                {
                    intentosRestantes--;
                    response.IsError = true;
                    response.Mensaje = "Error";
                }

            }
            catch (Exception ex)
            {
                intentosRestantes--;
                Plugins.WriteExceptionLog(ex);
                response.IsError = true;
                response.Mensaje = "Error en obtener datos";
            }
        }
            return response;
        }



        [HttpPost]
        [Route("ModificarInterpretacion")]
        public async Task<ResponseModels> ModificarInterpretacion(InterpretacionModel interpretacion)
        {
            ResponseModels response = new ResponseModels();
            int intentosRestantes = 3;
            while (intentosRestantes > 0)
            {
                try
            {
                response.Datos = _provider.ModificarInterpretacion(interpretacion).Result;
                long codigoRespuesta = long.Parse(response.Datos.ToString());
                if (codigoRespuesta == 1)
                {
                    intentosRestantes = 0;
                    response.IsError = false;
                    response.Mensaje = "Interpretacion Modificada";
                }
                if (codigoRespuesta == 0)
                {
                    intentosRestantes = 0;
                    response.IsError = true;
                    response.Mensaje = "La Interpretacion a Modificar no existe";
                }
                if (codigoRespuesta == -1)
                {
                    intentosRestantes--;
                    response.IsError = true;
                    response.Mensaje = "Error";
                }
                if (codigoRespuesta == -2)
                {
                    intentosRestantes = 0;
                    response.IsError = true;
                    response.Mensaje = "Campos vacios";
                }
            }
            catch (Exception ex)
            {
                intentosRestantes--;
                Plugins.WriteExceptionLog(ex);
                response.IsError = true;
                response.Mensaje = "Error en obtener datos";
            }
        }
            return response;
        }

        [HttpPost]
        [Route("ValidarLikeYDislikePorUsuario")]
        public async Task<ResponseModels> ValidarLikeYDislikePorUsuario(int IdUsuario, int IdCancion)
        {
            ResponseModels response = new ResponseModels();
            int intentosRestantes = 3;
            while (intentosRestantes > 0)
            {
                try
            {
                response.Datos = _provider.ValidarLikeYDislikePorUsuario(IdUsuario, IdCancion).Result;
                long codigoRespuesta = long.Parse(response.Datos.ToString());
                switch (codigoRespuesta)
                {
                    case -1:
                        {
                            intentosRestantes = 0;
                            response.IsError = true;
                            response.Mensaje = "Campos vacios";
                            break;
                        }
                    case -2:
                        {
                            intentosRestantes--;
                            response.IsError = true;
                            response.Mensaje = "Error";
                            break;
                        }
                    case 0:
                        {
                            intentosRestantes = 0;
                            response.IsError = false;
                            response.Mensaje = "nada";
                            break;
                        }
                    case 1:
                        {
                            intentosRestantes = 0;
                            response.IsError = false;
                            response.Mensaje = "like";
                            break;
                        }
                    case 2:
                        {
                            intentosRestantes = 0;
                            response.IsError = false;
                            response.Mensaje = "dislike";
                            break;
                        }
                }
            }
            catch (Exception ex)
            {
                intentosRestantes--;
                Plugins.WriteExceptionLog(ex);
                response.IsError = true;
                response.Mensaje = "Error en obtener datos";
            }
        }
            return response;
        }




        [HttpPost]
        [Route("ConsultarInterpretacion")]
        public async Task<ResponseModels> ConsultarInterpretacion(int id)
        {
            ResponseModels response = new ResponseModels();
            int intentosRestantes = 3;
            while (intentosRestantes > 0)
            {
                try
            {
                response.Datos = _provider.ConsultarInterpretacion(id).Result;
                if (response.Datos != null)
                {
                    intentosRestantes = 0;
                    response.IsError = false;
                    response.Mensaje = "Ok";
                }
                else
                {
                    intentosRestantes = 0;
                    response.IsError = true;
                    response.Mensaje = "La Interpretacion consultada no existe";
                }

            }
            catch (Exception ex)
            {
                intentosRestantes--;
                Plugins.WriteExceptionLog(ex);
                response.IsError = true;
                response.Mensaje = "Error en obtener datos";
            }
        }
            return response;
        }





        [HttpPost]
        [Route("DesactivarComentario")]
        public async Task<ResponseModels> DesactivarComentario(int id)
        {
            ResponseModels response = new ResponseModels();
            int intentosRestantes = 3;
            while (intentosRestantes > 0)
            {
                try
            {
                response.Datos = _provider.DesactivarComentario(id).Result;
                long codigoRespuesta = long.Parse(response.Datos.ToString());
                if (codigoRespuesta == 1)
                {
                    intentosRestantes = 0;
                    response.IsError = false;
                    response.Mensaje = "Comentario Desactivado";
                }
                if (codigoRespuesta == 0)
                {
                    intentosRestantes = 0;
                    response.IsError = true;
                    response.Mensaje = "El comentario a desactivar no existe";
                }
                if (codigoRespuesta == -1)
                {
                    intentosRestantes--;
                    response.IsError = true;
                    response.Mensaje = "Error";
                }

            }
            catch (Exception ex)
            {
                intentosRestantes--;
                Plugins.WriteExceptionLog(ex);
                response.IsError = true;
                response.Mensaje = "Error en obtener datos";
            }
        }

            return response;
        }







        [HttpPost]
        [Route("ModificarComentario")]
        public async Task<ResponseModels> ModificarComentario(ComentarioModel comentario)
        {
            ResponseModels response = new ResponseModels();
            int intentosRestantes = 3;
            while (intentosRestantes > 0)
            {
                try
            {
                response.Datos = _provider.ModificarComentario(comentario).Result;
                long codigoRespuesta = long.Parse(response.Datos.ToString());
                if (codigoRespuesta == 1)
                {
                    intentosRestantes=0;
                    response.IsError = false;
                    response.Mensaje = "Comentario Modificado";
                }
                if (codigoRespuesta == 0)
                {
                    intentosRestantes=0;
                    response.IsError = true;
                    response.Mensaje = "El comentario a Modificar no existe";
                }
                if (codigoRespuesta == -1)
                {
                    intentosRestantes--;
                    response.IsError = true;
                    response.Mensaje = "Error";
                }
                if (codigoRespuesta == -2)
                {
                    intentosRestantes=0;
                    response.IsError = true;
                    response.Mensaje = "Campos vacios";
                }
            }
            catch (Exception ex)
            {
                Plugins.WriteExceptionLog(ex);
                response.IsError = true;
                response.Mensaje = "Error en obtener datos";
            }
        }
            return response;
        }




        //-.----------------------------------------------------------------------------------------------------------------------------------





        [HttpPost]
        [Route("DesactivarInterpretacionRetry")]
        public async Task<ResponseModels> DesactivarInterpretacionRetry(int id)
        {
            ResponseModels response = new ResponseModels();
            int intentosRestantes = 3;
            while (intentosRestantes > 0)
            {
                try
                {
                    response.Datos = _provider.DesactivarInterpretacion(id).Result;
                    long codigoRespuesta = long.Parse(response.Datos.ToString());
                    if (codigoRespuesta == 1)
                    {
                        response.IsError = false;
                        response.Mensaje = "Interpretacion Desactivada";
                        intentosRestantes = 0;
                    }
                    if (codigoRespuesta == 0)
                    {
                        response.IsError = true;
                        response.Mensaje = "La interpretacion a desactivar no existe";
                        intentosRestantes = 0;
                    }
                    if (codigoRespuesta == -1)
                    {
                        intentosRestantes--;
                        response.IsError = true;
                        response.Mensaje = "Error";
                        await Task.Delay(1000);
                    }

                }
                catch (Exception ex)
                {
                    intentosRestantes--;
                    Plugins.WriteExceptionLog(ex);
                    response.IsError = true;
                    response.Mensaje = "Error en obtener datos";
                    await Task.Delay(1000);
                }
            }
            return response;
        }





        [HttpPost]
        [Route("AniadirCancionAFavoritos")]
        public async Task<ResponseModels> AniadirCancionAFavoritos(int idUsuario, int idCancion)
        {
            ResponseModels response = new ResponseModels();
            int intentosRestantes = 3;
            while (intentosRestantes > 0)
            {
                try
                {
                    response.Datos = _provider.AniadirCancionAFavoritos(idUsuario, idCancion).Result;
                    long codigoRespuesta = long.Parse(response.Datos.ToString());
                    if (codigoRespuesta > 0)
                    {
                        response.IsError = false;
                        response.Mensaje = "La cancion ha sido añadida correctamente a favoritos y usa el id: "+codigoRespuesta;
                        intentosRestantes = 0;
                    }
                    else
                    {
                        if (codigoRespuesta == -4)
                        {
                            intentosRestantes = 0;
                            response.IsError = true;
                            response.Mensaje = "Esta cancion ya se encuentra en la lista de favoritos correspondiente a este usuario";
                        }
                        if (codigoRespuesta == -3)
                        {
                            response.IsError = true;
                            response.Mensaje = "La cancion que desea aniadir a favoritos no existe";
                            intentosRestantes = 0;
                        }
                        if (codigoRespuesta == -2)
                        {
                            response.IsError = true;
                            response.Mensaje = "campos obligatorios se encuentran vacios";
                            intentosRestantes = 0;
                        }
                        if (codigoRespuesta == -1)
                        {
                            intentosRestantes--;
                            response.IsError = true;
                            response.Mensaje = "Error";
                            await Task.Delay(1000);
                        }
                    }
                    

                }
                catch (Exception ex)
                {
                    intentosRestantes--;
                    Plugins.WriteExceptionLog(ex);
                    response.IsError = true;
                    response.Mensaje = "Error en obtener datos";
                    await Task.Delay(1000);
                }
            }
            return response;
        }

        [HttpPost]
        [Route("AniadirInterpretacionAFavoritos")]
        public async Task<ResponseModels> AniadirInterpretacionAFavoritos(int idUsuario, int idCancion)
        {
            ResponseModels response = new ResponseModels();
            int intentosRestantes = 3;
            while (intentosRestantes > 0)
            {
                try
                {
                    response.Datos = _provider.AniadirInterpretacionAFavoritos(idUsuario, idCancion).Result;
                    long codigoRespuesta = long.Parse(response.Datos.ToString());
                    if (codigoRespuesta > 0)
                    {
                        response.IsError = false;
                        response.Mensaje = "La interpretacion ha sido añadida correctamente a favoritos y usa el id: " + codigoRespuesta;
                        intentosRestantes = 0;
                    }
                    else
                    {
                        if (codigoRespuesta == -4)
                        {
                            intentosRestantes = 0;
                            response.IsError = true;
                            response.Mensaje = "Esta interpretacion ya se encuentra en la lista de favoritos correspondiente a este usuario";
                        }
                        if (codigoRespuesta == -3)
                        {
                            response.IsError = true;
                            response.Mensaje = "La interpretacion que desea aniadir a favoritos no existe";
                            intentosRestantes = 0;
                        }
                        if (codigoRespuesta == -2)
                        {
                            response.IsError = true;
                            response.Mensaje = "campos obligatorios se encuentran vacios";
                            intentosRestantes = 0;
                        }
                        if (codigoRespuesta == -1)
                        {
                            intentosRestantes--;
                            response.IsError = true;
                            response.Mensaje = "Error";
                            await Task.Delay(1000);
                        }
                    }


                }
                catch (Exception ex)
                {
                    intentosRestantes--;
                    Plugins.WriteExceptionLog(ex);
                    response.IsError = true;
                    response.Mensaje = "Error en obtener datos";
                    await Task.Delay(1000);
                }
            }
            return response;
        }





        [HttpPost]
        [Route("ConsultarCancionesFavoritasPorUsuario")]
        public async Task<ResponseModels> ConsultarCancionesFavoritasPorUsuario(int idUsuario)
        {
            ResponseModels response = new ResponseModels();
            int intentosRestantes = 3;
            while (intentosRestantes > 0)
            {
                try
                {
                    response.Datos = _provider.ConsultarCancionesFavoritasPorUsuario(idUsuario).Result;
                    if (response.Datos != null)
                    {
                        intentosRestantes = 0;
                        response.IsError = false;
                        response.Mensaje = "Ok";
                    }
                    else
                    {
                        intentosRestantes--;
                        response.IsError = true;
                        response.Mensaje = "No se encontraron datos";
                    }

                }
                catch (Exception ex)
                {
                    intentosRestantes--;
                    Plugins.WriteExceptionLog(ex);
                    response.IsError = true;
                    response.Mensaje = "Error en obtener datos";
                }
            }
            return response;
        }

        [HttpPost]
        [Route("ConsultarInterpretacionesFavoritasPorUsuario")]
        public async Task<ResponseModels> ConsultarInterpretacionesFavoritasPorUsuario(int idUsuario)
        {
            ResponseModels response = new ResponseModels();
            int intentosRestantes = 3;
            while (intentosRestantes > 0)
            {
                try
                {
                    response.Datos = _provider.ConsultarInterpretacionesFavoritasPorUsuario(idUsuario).Result;
                    if (response.Datos != null)
                    {
                        intentosRestantes = 0;
                        response.IsError = false;
                        response.Mensaje = "Ok";
                    }
                    else
                    {
                        intentosRestantes--;
                        response.IsError = true;
                        response.Mensaje = "No se encontraron datos";
                    }

                }
                catch (Exception ex)
                {
                    intentosRestantes--;
                    Plugins.WriteExceptionLog(ex);
                    response.IsError = true;
                    response.Mensaje = "Error en obtener datos";
                }
            }
            return response;
        }








        [HttpPost]
        [Route("EliminarDeFavoritos")]
        public async Task<ResponseModels> EliminarDeFavoritos(int idUsuario, int idCancion)
        {
            ResponseModels response = new ResponseModels();
            int intentosRestantes = 3;
            while (intentosRestantes > 0)
            {
                try
                {
                    response.Datos = _provider.EliminarDeFavoritos(idUsuario, idCancion).Result;
                    long codigoRespuesta = long.Parse(response.Datos.ToString());
                    if (codigoRespuesta == 1)
                    {
                        response.IsError = false;
                        response.Mensaje = "La cancion ha sido eliminada da favoritos";
                        intentosRestantes = 0;
                    }
                    else
                    {

                        if (codigoRespuesta == -2)
                        {
                            response.IsError = true;
                            response.Mensaje = "La cancion que desea eliminar de favoritos no existe";
                            intentosRestantes = 0;
                        }
                        if (codigoRespuesta == -3)
                        {
                            response.IsError = true;
                            response.Mensaje = "campos obligatorios se encuentran vacios";
                            intentosRestantes = 0;
                        }
                        if (codigoRespuesta == -1)
                        {
                            intentosRestantes--;
                            response.IsError = true;
                            response.Mensaje = "Error";
                            await Task.Delay(1000);
                        }
                    }


                }
                catch (Exception ex)
                {
                    intentosRestantes--;
                    Plugins.WriteExceptionLog(ex);
                    response.IsError = true;
                    response.Mensaje = "Error en obtener datos";
                    await Task.Delay(1000);
                }
            }
            return response;
        }



        [HttpPost]
        [Route("ValidarCancionEnFavoritos")]
        public async Task<ResponseModels> ValidarCancionEnFavoritos(int idUsuario, int idCancion)
        {
            ResponseModels response = new ResponseModels();
            int intentosRestantes = 3;
            while (intentosRestantes > 0)
            {
                try
                {
                    response.Datos = _provider.ValidarCancionEnFavoritos(idUsuario, idCancion).Result;
                    long codigoRespuesta = long.Parse(response.Datos.ToString());
                    if (codigoRespuesta == 1)
                    {
                        response.IsError = false;
                        response.Mensaje = "La cancion consultada no se encuentra en favoritos";
                        intentosRestantes = 0;
                    }
                    else
                    {

                        if (codigoRespuesta == 2)
                        {
                            response.IsError = false;
                            response.Mensaje = "La cancion consultada se encuentra en favoritos";
                            intentosRestantes = 0;
                        }
                        if (codigoRespuesta == -2)
                        {
                            response.IsError = true;
                            response.Mensaje = "campos obligatorios se encuentran vacios";
                            intentosRestantes = 0;
                        }
                        if (codigoRespuesta == -1)
                        {
                            intentosRestantes--;
                            response.IsError = true;
                            response.Mensaje = "Error";
                            await Task.Delay(1000);
                        }
                    }


                }
                catch (Exception ex)
                {
                    intentosRestantes--;
                    Plugins.WriteExceptionLog(ex);
                    response.IsError = true;
                    response.Mensaje = "Error en obtener datos";
                    await Task.Delay(1000);
                }
            }
            return response;
        }


    }
}
