﻿using Core.Loyal.Models.FTMUSIC;
using CORE.Loyal.Models.FTMUSIC;

namespace CORE.Loyal.Interfaces.Services
{
    public interface ICancionServices
    {
        Task<List<CancionModel>> GetList();
        Task<long> SaveCancion(CancionModel user);
        Task<CancionModel> ConsultarCancion(int id);
        Task<long> DesactivarCancion(int id);
        Task<long> ModificarCancion(CancionModel cancion);

        Task<List<CancionModel>> ConsultarCancionPorUsuario(int idUsuario);
        Task<List<CancionModel>> ConsultarCancionPorNombre(string nombre);
        Task<long> GuardarComentario(ComentarioModel comentario);
        Task<List<ComentarioModel>> ConsultarComentarioPorCancion(int idCancion);
    }
}
