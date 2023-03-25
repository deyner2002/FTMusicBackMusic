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
                        response.Mensaje = "Registro Guardado";
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



    }
}
