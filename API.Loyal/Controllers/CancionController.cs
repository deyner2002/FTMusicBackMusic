﻿using Core.Loyal.Models.FTMUSIC;
using CORE.Loyal.Interfaces.Services;
using CORE.Loyal.Models.DTOs;
using CORE.Loyal.Models.FTMUSIC;
using Microsoft.AspNetCore.Mvc;
using Support.Loyal.DTOs;
using Support.Loyal.Util;

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

            try
            {
                response.Datos = _provider.GetListCanciones().Result;
                if (response.Datos != null)
                {
                    response.IsError = false;
                    response.Mensaje = "Ok";
                }
                else
                {
                    response.IsError = true;
                    response.Mensaje = "Error en obtener datos";
                }

            }
            catch (Exception ex)
            {
                Plugins.WriteExceptionLog(ex);
                response.IsError = true;
                response.Mensaje = "Error en obtener datos";
            }

            return response;
        }

        [HttpPost]
        [Route("Save")]
        public async Task<ResponseModels> SaveCancion(CancionModel cancion)
        {
            ResponseModels response = new ResponseModels();

            try
            {
                response.Datos = _provider.SaveCancion(cancion).Result;
                long codigoRespuesta = long.Parse(response.Datos.ToString());
                if (codigoRespuesta == -2)
                {
                    response.IsError = true;
                    response.Mensaje = "Error: hay campos nulos que son obligatorios";
                }
                else
                {
                    if (codigoRespuesta == -1)
                    {
                        response.IsError = true;
                        response.Mensaje = "Error del sistema";
                    }
                    else
                    {
                        response.IsError = false;
                        response.Mensaje = "La cancion llamada: "+cancion.Nombre+" ha sido guardada";
                    }
                }

            }
            catch (Exception ex)
            {
                Plugins.WriteExceptionLog(ex);
                response.IsError = true;
                response.Mensaje = "Error del sistema";
            }

            return response;
        }





        [HttpGet]
        [Route("ConsultarCancion")]
        public async Task<ResponseModels> ConsultarCancion(int id)
        {
            ResponseModels response = new ResponseModels();

            try
            {
                response.Datos = _provider.ConsultarCancion(id).Result;
                if (response.Datos != null)
                {
                    response.IsError = false;
                    response.Mensaje = "Ok";
                }
                else
                {
                    response.IsError = true;
                    response.Mensaje = "La cancion consultada no existe";
                }

            }
            catch (Exception ex)
            {
                Plugins.WriteExceptionLog(ex);
                response.IsError = true;
                response.Mensaje = "Error en obtener datos";
            }

            return response;
        }




        [HttpGet]
        [Route("DesactivarCancion")]
        public async Task<ResponseModels> DesactivarCancion(int id)
        {
            ResponseModels response = new ResponseModels();

            try
            {
                response.Datos = _provider.DesactivarCancion(id).Result;
                long codigoRespuesta = long.Parse(response.Datos.ToString());
                if (codigoRespuesta == 1)
                {
                    response.IsError = false;
                    response.Mensaje = "Cancion Desactivada";
                }
                if (codigoRespuesta == 0)
                {
                    response.IsError = true;
                    response.Mensaje = "La cancion a desactivar no existe";
                }
                if (codigoRespuesta == -1)
                {
                    response.IsError = true;
                    response.Mensaje = "Error";
                }

            }
            catch (Exception ex)
            {
                Plugins.WriteExceptionLog(ex);
                response.IsError = true;
                response.Mensaje = "Error en obtener datos";
            }

            return response;
        }





        [HttpPost]
        [Route("ModificarCancion")]
        public async Task<ResponseModels> ModificarCancion(CancionModel cancion)
        {
            ResponseModels response = new ResponseModels();

            try
            {
                response.Datos = _provider.ModificarCancion(cancion).Result;
                long codigoRespuesta = long.Parse(response.Datos.ToString());
                if (codigoRespuesta == 1)
                {
                    response.IsError = false;
                    response.Mensaje = "Cancion Modificada";
                }
                if (codigoRespuesta == 0)
                {
                    response.IsError = true;
                    response.Mensaje = "La cancion a Modificar no existe";
                }
                if (codigoRespuesta == -1)
                {
                    response.IsError = true;
                    response.Mensaje = "Error";
                }
                if (codigoRespuesta == -2)
                {
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

            return response;
        }







        [HttpGet]
        [Route("ConsultarCancionPorUsuario")]
        public async Task<ResponseModels> ConsutlarCancionPorUsuario(int idUsuario)
        {
            ResponseModels response = new ResponseModels();

            try
            {
                response.Datos = _provider.ConsultarCancionPorUsuario(idUsuario).Result;
                if (response.Datos != null)
                {
                    response.IsError = false;
                    response.Mensaje = "Ok";
                }
                else
                {
                    response.IsError = true;
                    response.Mensaje = "No se encontraron canciones de este usuario";
                }

            }
            catch (Exception ex)
            {
                Plugins.WriteExceptionLog(ex);
                response.IsError = true;
                response.Mensaje = "Error en obtener datos";
            }
            return response;
        }




        [HttpGet]
        [Route("ConsultarCancionPorNombre")]
        public async Task<ResponseModels> ConsultarCancionPorNombre(string nombre)
        {
            ResponseModels response = new ResponseModels();

            try
            {
                response.Datos = _provider.ConsultarCancionPorNombre(nombre).Result;
                if (response.Datos != null)
                {
                    response.IsError = false;
                    response.Mensaje = "Ok";
                }
                else
                {
                    response.IsError = true;
                    response.Mensaje = "Error en obtener datos";
                }

            }
            catch (Exception ex)
            {
                Plugins.WriteExceptionLog(ex);
                response.IsError = true;
                response.Mensaje = "Error en obtener datos";
            }

            return response;
        }








        [HttpPost]
        [Route("GuardarComentarioCancion")]
        public async Task<ResponseModels> GuardarComentarioCancion(ComentarioModel comentario)
        {
            ResponseModels response = new ResponseModels();

            try
            {
                response.Datos = _provider.GuardarComentarioCancion(comentario).Result;
                long codigoRespuesta = long.Parse(response.Datos.ToString());
                if (codigoRespuesta == -2)
                {
                    response.IsError = true;
                    response.Mensaje = "Error: hay campos nulos que son obligatorios";
                }
                if (codigoRespuesta == -1)
                {
                    response.IsError = true;
                    response.Mensaje = "Error del sistema";
                }

                if (codigoRespuesta == -3)
                {
                    response.IsError = true;
                    response.Mensaje = "La cancion a la que se quiere registrar el comentario, no existe";
                }

                if (codigoRespuesta > 0)
                {
                    response.IsError = false;
                    response.Mensaje = "El comentario " + codigoRespuesta + " Ha sido guardado";
                }

            }
            catch (Exception ex)
            {
                Plugins.WriteExceptionLog(ex);
                response.IsError = true;
                response.Mensaje = "Error del sistema";
            }

            return response;
        }







        [HttpGet]
        [Route("ConsultarComentarioPorCancion")]
        public async Task<ResponseModels> ConsultarComentarioPorCancion(int idCancion)
        {
            ResponseModels response = new ResponseModels();

            try
            {
                response.Datos = _provider.ConsultarComentarioPorCancion(idCancion).Result;
                if (response.Datos != null)
                {
                    response.IsError = false;
                    response.Mensaje = "Ok";
                }
                else
                {
                    response.IsError = true;
                    response.Mensaje = "No se encontraron comentarios de esta cancion";
                }

            }
            catch (Exception ex)
            {
                Plugins.WriteExceptionLog(ex);
                response.IsError = true;
                response.Mensaje = "Error en obtener datos";
            }
            return response;
        }





        [HttpPost]
        [Route("GuardarLikeCancion")]
        public async Task<ResponseModels> GuardarLikeCancion(LikeModel like)
        {
            ResponseModels response = new ResponseModels();

            try
            {
                response.Datos = _provider.GuardarLikeCancion(like).Result;
                long codigoRespuesta = long.Parse(response.Datos.ToString());
                if (codigoRespuesta == -1)
                {
                    response.IsError = true;
                    response.Mensaje = "Error del sistema";
                }

                if (codigoRespuesta == -2)
                {
                    response.IsError = true;
                    response.Mensaje = "Error: hay campos nulos que son obligatorios";
                }

                if (codigoRespuesta == -3)
                {
                    response.IsError = true;
                    response.Mensaje = "La cancion a la que se quiere registrar el like, no existe";
                }

                if (codigoRespuesta == -4)
                {
                    response.IsError = false;
                    response.Mensaje = "Like eliminado de la cancion";
                }

                if (codigoRespuesta > 0)
                {
                    response.IsError = false;
                    response.Mensaje = "El like " + codigoRespuesta + " Ha sido guardado";
                }

            }
            catch (Exception ex)
            {
                Plugins.WriteExceptionLog(ex);
                response.IsError = true;
                response.Mensaje = "Error del sistema";
            }

            return response;
        }

        [HttpPost]
        [Route("GuardarDisLikeCancion")]
        public async Task<ResponseModels> GuardarDisLikeCancion(DisLikeModel disLike)
        {
            ResponseModels response = new ResponseModels();

            try
            {
                response.Datos = _provider.GuardarDisLikeCancion(disLike).Result;
                long codigoRespuesta = long.Parse(response.Datos.ToString());
                if (codigoRespuesta == -1)
                {
                    response.IsError = true;
                    response.Mensaje = "Error del sistema";
                }

                if (codigoRespuesta == -2)
                {
                    response.IsError = true;
                    response.Mensaje = "Error: hay campos nulos que son obligatorios";
                }

                if (codigoRespuesta == -3)
                {
                    response.IsError = true;
                    response.Mensaje = "La cancion a la que se quiere registrar el dislike, no existe";
                }

                if (codigoRespuesta == -4)
                {
                    response.IsError = false;
                    response.Mensaje = "DisLike eliminado de la cancion";
                }

                if (codigoRespuesta > 0)
                {
                    response.IsError = false;
                    response.Mensaje = "El DisLike " + codigoRespuesta + " Ha sido guardado";
                }

            }
            catch (Exception ex)
            {
                Plugins.WriteExceptionLog(ex);
                response.IsError = true;
                response.Mensaje = "Error del sistema";
            }

            return response;
        }




        [HttpPost]
        [Route("ConsultarNumeroMegustaPorCancion")]
        public async Task<ResponseModels> ConsultarNumeroMegustaPorCancion(int idCancion)
        {
            ResponseModels response = new ResponseModels();

            try
            {
                response.Datos = _provider.ConsultarNumeroMegustaPorCancion(idCancion).Result;
                long codigoRespuesta = long.Parse(response.Datos.ToString());
                if (codigoRespuesta == -2)
                {
                    response.IsError = true;
                    response.Mensaje = "Error del sistema";
                }

                if (codigoRespuesta == -1)
                {
                    response.IsError = true;
                    response.Mensaje = "La cancion en cuestion no tiene likes asignados";
                }

                if (codigoRespuesta > 0)
                {
                    response.IsError = false;
                    response.Mensaje = "El numero de Likes es de: "+codigoRespuesta;
                }

            }
            catch (Exception ex)
            {
                Plugins.WriteExceptionLog(ex);
                response.IsError = true;
                response.Mensaje = "Error del sistema";
            }

            return response;
        }

        [HttpPost]
        [Route("ConsultarNumeroNoMegustaPorCancion")]
        public async Task<ResponseModels> ConsultarNumeroNoMegustaPorCancion(int idCancion)
        {
            ResponseModels response = new ResponseModels();

            try
            {
                response.Datos = _provider.ConsultarNumeroNoMegustaPorCancion(idCancion).Result;
                long codigoRespuesta = long.Parse(response.Datos.ToString());
                if (codigoRespuesta == -2)
                {
                    response.IsError = true;
                    response.Mensaje = "Error del sistema";
                }

                if (codigoRespuesta == -1)
                {
                    response.IsError = true;
                    response.Mensaje = "La cancion en cuestion no tiene Dislikes asignados";
                }

                if (codigoRespuesta > 0)
                {
                    response.IsError = false;
                    response.Mensaje = "El numero de DisLikes es de: " + codigoRespuesta;
                }

            }
            catch (Exception ex)
            {
                Plugins.WriteExceptionLog(ex);
                response.IsError = true;
                response.Mensaje = "Error del sistema";
            }

            return response;
        }



        [HttpPost]
        [Route("ConsultarCancionCompleta")]
        public async Task<ResponseModels> ConsultarCancionCompleta(int idCancion)
        {
            ResponseModels response = new ResponseModels();
            try
            {
                response.Datos = _provider.ConsultarCancionCompleta(idCancion).Result;
                if (response.Datos!=null)
                {
                    response.IsError = false;
                    response.Mensaje = "la cancion se ha consultado correctamente";
                }
                else
                {
                    response.IsError = true;
                    response.Mensaje = "no se han encontrado registros pertenecientes a la cancion identificada como: "+idCancion;
                }
                
            }
            catch(Exception ex)
            {
                Plugins.WriteExceptionLog(ex);
                response.IsError = true;
                response.Mensaje = "Error del sistema";
            }
            return response;
        }





        [HttpGet]
        [Route("GetListInterpretaciones")]
        public async Task<ResponseModels> GetListInterpretaciones()
        {
            ResponseModels response = new ResponseModels();

            try
            {
                response.Datos = _provider.GetListInterpretaciones().Result;
                if (response.Datos != null)
                {
                    response.IsError = false;
                    response.Mensaje = "Ok";
                }
                else
                {
                    response.IsError = true;
                    response.Mensaje = "Error en obtener datos";
                }

            }
            catch (Exception ex)
            {
                Plugins.WriteExceptionLog(ex);
                response.IsError = true;
                response.Mensaje = "Error en obtener datos";
            }

            return response;
        }


        [HttpGet]
        [Route("ConsultarInterpretacionPorNombre")]
        public async Task<ResponseModels> ConsultarInterpretacionPorNombre(string nombre)
        {
            ResponseModels response = new ResponseModels();

            try
            {
                response.Datos = _provider.ConsultarInterpretacionPorNombre(nombre).Result;
                if (response.Datos != null)
                {
                    response.IsError = false;
                    response.Mensaje = "Ok";
                }
                else
                {
                    response.IsError = true;
                    response.Mensaje = "Error en obtener datos";
                }

            }
            catch (Exception ex)
            {
                Plugins.WriteExceptionLog(ex);
                response.IsError = true;
                response.Mensaje = "Error en obtener datos";
            }

            return response;
        }





        [HttpGet]
        [Route("ConsultarInterpretacionPorUsuario")]
        public async Task<ResponseModels> ConsutlarInterpretacionPorUsuario(int idUsuario)
        {
            ResponseModels response = new ResponseModels();

            try
            {
                response.Datos = _provider.ConsultarInterpretacionPorUsuario(idUsuario).Result;
                if (response.Datos != null)
                {
                    response.IsError = false;
                    response.Mensaje = "Ok";
                }
                else
                {
                    response.IsError = true;
                    response.Mensaje = "No se encontraron canciones de este usuario";
                }

            }
            catch (Exception ex)
            {
                Plugins.WriteExceptionLog(ex);
                response.IsError = true;
                response.Mensaje = "Error en obtener datos";
            }
            return response;
        }



        [HttpPost]
        [Route("SaveInterpretacion")]
        public async Task<ResponseModels> SaveInterpretacion(InterpretacionModel user)
        {
            ResponseModels response = new ResponseModels();

            try
            {
                response.Datos = _provider.SaveInterpretacion(user).Result;
                long codigoRespuesta = long.Parse(response.Datos.ToString());
                if (codigoRespuesta == -2)
                {
                    response.IsError = true;
                    response.Mensaje = "Error: hay campos nulos que son obligatorios";
                }
                else
                {
                    if (codigoRespuesta == -1)
                    {
                        response.IsError = true;
                        response.Mensaje = "Error del sistema";
                    }
                    if (codigoRespuesta >=0)
                    {
                        response.IsError = false;
                        response.Mensaje = "La Interpretacion llamada: " + user.Nombre + " ha sido guardada";
                    }
                    if (codigoRespuesta ==-3)
                    {
                        response.IsError = true;
                        response.Mensaje = "La cancion a la que se hace referencia en la interpretacion no existe";
                    }
                }

            }
            catch (Exception ex)
            {
                Plugins.WriteExceptionLog(ex);
                response.IsError = true;
                response.Mensaje = "Error del sistema";
            }

            return response;
        }




        [HttpPost]
        [Route("GuardarLikeInterpretacion")]
        public async Task<ResponseModels> GuardarLikeInterpretacion(LikeModel like)
        {
            ResponseModels response = new ResponseModels();

            try
            {
                response.Datos = _provider.GuardarLikeInterpretacion(like).Result;
                long codigoRespuesta = long.Parse(response.Datos.ToString());
                if (codigoRespuesta == -1)
                {
                    response.IsError = true;
                    response.Mensaje = "Error del sistema";
                }

                if (codigoRespuesta == -2)
                {
                    response.IsError = true;
                    response.Mensaje = "Error: hay campos nulos que son obligatorios";
                }

                if (codigoRespuesta == -3)
                {
                    response.IsError = true;
                    response.Mensaje = "La interpretacion a la que se quiere registrar el like, no existe";
                }

                if (codigoRespuesta == -4)
                {
                    response.IsError = false;
                    response.Mensaje = "Like eliminado de la interpretacion";
                }

                if (codigoRespuesta > 0)
                {
                    response.IsError = false;
                    response.Mensaje = "El like " + codigoRespuesta + " Ha sido guardado";
                }

            }
            catch (Exception ex)
            {
                Plugins.WriteExceptionLog(ex);
                response.IsError = true;
                response.Mensaje = "Error del sistema";
            }

            return response;
        }

        [HttpPost]
        [Route("GuardarDisLikeInterpretacion")]
        public async Task<ResponseModels> GuardarDisLikeInterpretacion(DisLikeModel disLike)
        {
            ResponseModels response = new ResponseModels();

            try
            {
                response.Datos = _provider.GuardarDisLikeInterpretacion(disLike).Result;
                long codigoRespuesta = long.Parse(response.Datos.ToString());
                if (codigoRespuesta == -1)
                {
                    response.IsError = true;
                    response.Mensaje = "Error del sistema";
                }

                if (codigoRespuesta == -2)
                {
                    response.IsError = true;
                    response.Mensaje = "Error: hay campos nulos que son obligatorios";
                }

                if (codigoRespuesta == -3)
                {
                    response.IsError = true;
                    response.Mensaje = "La interpretacion a la que se quiere registrar el dislike, no existe";
                }

                if (codigoRespuesta == -4)
                {
                    response.IsError = false;
                    response.Mensaje = "DisLike eliminado de la interpretacion";
                }

                if (codigoRespuesta > 0)
                {
                    response.IsError = false;
                    response.Mensaje = "El DisLike " + codigoRespuesta + " Ha sido guardado";
                }

            }
            catch (Exception ex)
            {
                Plugins.WriteExceptionLog(ex);
                response.IsError = true;
                response.Mensaje = "Error del sistema";
            }

            return response;
        }

        [HttpPost]
        [Route("GuardarComentarioInterpretacion")]
        public async Task<ResponseModels> GuardarComentarioInterpretacion(ComentarioModel comentario)
        {
            ResponseModels response = new ResponseModels();

            try
            {
                response.Datos = _provider.GuardarComentarioInterpretacion(comentario).Result;
                long codigoRespuesta = long.Parse(response.Datos.ToString());
                if (codigoRespuesta == -2)
                {
                    response.IsError = true;
                    response.Mensaje = "Error: hay campos nulos que son obligatorios";
                }
                if (codigoRespuesta == -1)
                {
                    response.IsError = true;
                    response.Mensaje = "Error del sistema";
                }

                if (codigoRespuesta == -3)
                {
                    response.IsError = true;
                    response.Mensaje = "La Interpretacion a la que se quiere registrar el comentario, no existe";
                }

                if (codigoRespuesta > 0)
                {
                    response.IsError = false;
                    response.Mensaje = "El comentario " + codigoRespuesta + " Ha sido guardado";
                }

            }
            catch (Exception ex)
            {
                Plugins.WriteExceptionLog(ex);
                response.IsError = true;
                response.Mensaje = "Error del sistema";
            }

            return response;
        }




        [HttpPost]
        [Route("ConsultarInterpretacionCompleta")]
        public async Task<ResponseModels> ConsultarInterpretacionCompleta(int id)
        {
            ResponseModels response = new ResponseModels();
            try
            {
                response.Datos = _provider.ConsultarInterpretacionCompleta(id).Result;
                if (response.Datos != null)
                {
                    response.IsError = false;
                    response.Mensaje = "la cancion se ha consultado correctamente";
                }
                else
                {
                    response.IsError = true;
                    response.Mensaje = "no se han encontrado registros pertenecientes a la Interpretacion identificada como: " + id;
                }

            }
            catch (Exception ex)
            {
                Plugins.WriteExceptionLog(ex);
                response.IsError = true;
                response.Mensaje = "Error del sistema";
            }
            return response;
        }


        [HttpGet]
        [Route("DesactivarInterpretacion")]
        public async Task<ResponseModels> DesactivarInterpretacion(int id)
        {
            ResponseModels response = new ResponseModels();

            try
            {
                response.Datos = _provider.DesactivarInterpretacion(id).Result;
                long codigoRespuesta = long.Parse(response.Datos.ToString());
                if (codigoRespuesta == 1)
                {
                    response.IsError = false;
                    response.Mensaje = "Interpretacion Desactivada";
                }
                if (codigoRespuesta == 0)
                {
                    response.IsError = true;
                    response.Mensaje = "La interpretacion a desactivar no existe";
                }
                if (codigoRespuesta == -1)
                {
                    response.IsError = true;
                    response.Mensaje = "Error";
                }

            }
            catch (Exception ex)
            {
                Plugins.WriteExceptionLog(ex);
                response.IsError = true;
                response.Mensaje = "Error en obtener datos";
            }

            return response;
        }



        [HttpPost]
        [Route("ModificarInterpretacion")]
        public async Task<ResponseModels> ModificarInterpretacion(InterpretacionModel interpretacion)
        {
            ResponseModels response = new ResponseModels();

            try
            {
                response.Datos = _provider.ModificarInterpretacion(interpretacion).Result;
                long codigoRespuesta = long.Parse(response.Datos.ToString());
                if (codigoRespuesta == 1)
                {
                    response.IsError = false;
                    response.Mensaje = "Interpretacion Modificada";
                }
                if (codigoRespuesta == 0)
                {
                    response.IsError = true;
                    response.Mensaje = "La Interpretacion a Modificar no existe";
                }
                if (codigoRespuesta == -1)
                {
                    response.IsError = true;
                    response.Mensaje = "Error";
                }
                if (codigoRespuesta == -2)
                {
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

            return response;
        }



    }
}
