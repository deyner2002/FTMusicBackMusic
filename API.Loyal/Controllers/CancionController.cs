using Core.Loyal.Models.FTMUSIC;
using CORE.Loyal.Interfaces.Services;
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
        public async Task<ResponseModels> GetList()
        {
            ResponseModels response = new ResponseModels();

            try
            {
                response.Datos = _provider.GetList().Result;
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
        public async Task<ResponseModels> Save(CancionModel user)
        {
            ResponseModels response = new ResponseModels();

            try
            {
                response.Datos = _provider.SaveCancion(user).Result;
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
                        response.Mensaje = "La cancion llamada: "+user.Nombre+" ha sido guardada";
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



    }
}
