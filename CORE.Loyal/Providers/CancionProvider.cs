using CORE.Loyal.DBConnection;
using Microsoft.Extensions.Options;
using Oracle.ManagedDataAccess.Client;
using Support.Loyal.DTOs;
using Support.Loyal.Util;
using System.Data;
using CORE.Loyal.Interfaces.Providers;
using Core.Loyal.Models.FTMUSIC;
using CORE.Loyal.Models.FTMUSIC;
using CORE.Loyal.Models.DTOs;
using System.Net.Sockets;

namespace Core.Loyal.Providers
{
    public class CancionProvider : ICancionProvider
    {
        public ConnectionStrings _ConnectionStrings { get; set; }
        private OracleCommand cmd { get; set; }
        public CancionProvider(IOptions<ConnectionStrings> ConnectionStrings)
        {
            _ConnectionStrings = ConnectionStrings.Value;
            cmd = new OracleCommand();
            cmd.Connection = OracleDBConnectionSingleton.OracleDBConnection.oracleConnection;
        }
        public async Task<List<CancionModel>> GetListCanciones()
        {
            var _outs = new List<CancionModel>();
            try
            {
                await OracleDBConnectionSingleton.OracleDBConnection.oracleConnection.OpenAsync();
                
                cmd.CommandText = "SELECT ID, IDUSUARIO, IDALBUN, NOMBRE,GENERO,LETRA,FECHAPUBLICACION,LINK FROM CANCIONES WHERE ESTADO='A'";
                await cmd.ExecuteNonQueryAsync();

                var adapter = new OracleDataAdapter(cmd);
                var data = new DataSet("Datos");
                adapter.Fill(data);

                await OracleDBConnectionSingleton.OracleDBConnection.oracleConnection.CloseAsync();

                if (data.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow item in data.Tables[0].Rows)
                    {
                        CancionModel cancionModel = new CancionModel
                        {
                            Id = !Object.ReferenceEquals(System.DBNull.Value, item.ItemArray[0]) ? Convert.ToInt64(item.ItemArray[0]) : 0,
                            IdUsuario = !Object.ReferenceEquals(System.DBNull.Value, item.ItemArray[1]) ? Convert.ToInt64(item.ItemArray[1]) : 0,
                            IdAlbun = !Object.ReferenceEquals(System.DBNull.Value, item.ItemArray[2]) ? Convert.ToInt64(item.ItemArray[2]) : 0,
                            Nombre = !Object.ReferenceEquals(System.DBNull.Value, item.ItemArray[3]) ? Convert.ToString(item.ItemArray[3]) : "SIN NOMBRE",
                            Genero = !Object.ReferenceEquals(System.DBNull.Value, item.ItemArray[4]) ? Convert.ToString(item.ItemArray[4]) : "SIN GENERO",
                            Letra = !Object.ReferenceEquals(System.DBNull.Value, item.ItemArray[5]) ? Convert.ToString(item.ItemArray[5]).Replace("-", "\n") : "SIN LETRA",
                            FechaPublicacion = !Object.ReferenceEquals(System.DBNull.Value, item.ItemArray[6]) ? Convert.ToDateTime(item.ItemArray[6]) : null,
                            Link = !Object.ReferenceEquals(System.DBNull.Value, item.ItemArray[7]) ? Convert.ToString(item.ItemArray[7]) : "SIN LINK"
                        };
                        
                        _outs.Add(cancionModel);
                    }
                }

                return _outs;
            }
            catch (Exception ex)
            {
                Plugins.WriteExceptionLog(ex);
            }
            return null;
        }
        
        public async Task<long> SaveCancion(CancionModel cancion)
        {
            long consecutivo = 0;
            try
            {
                if (cancion.Nombre != null && cancion.Letra != null)
                {
                    cancion.Letra.Replace("\n", "-");
                    await OracleDBConnectionSingleton.OracleDBConnection.oracleConnection.OpenAsync();
                    cmd.CommandText = @"
                                        INSERT INTO CANCIONES
                                        (ID, IDUSUARIO, IDALBUN, NOMBRE,GENERO,LETRA,FECHAPUBLICACION,LINK,ESTADO)
                                        VALUES(SEQUENCECANCION.NEXTVAL,:P_IDUSUARIO,:P_IDALBUN ,:P_NOMBRE, :P_GENERO, :P_LETRA,CURRENT_DATE,:P_LINK,'A')
                                        ";
                    cmd.Parameters.Clear();
                    cmd.Parameters.Add(new OracleParameter { OracleDbType = OracleDbType.Long, Direction = ParameterDirection.Input, ParameterName = "P_IDUSUARIO", Value = cancion.IdUsuario });
                    cmd.Parameters.Add(new OracleParameter { OracleDbType = OracleDbType.Long, Direction = ParameterDirection.Input, ParameterName = "P_IDALBUN", Value = cancion.IdAlbun });
                    cmd.Parameters.Add(new OracleParameter { OracleDbType = OracleDbType.Varchar2, Direction = ParameterDirection.Input, ParameterName = "P_NOMBRE", Value = cancion.Nombre });
                    cmd.Parameters.Add(new OracleParameter { OracleDbType = OracleDbType.Varchar2, Direction = ParameterDirection.Input, ParameterName = "P_GENERO", Value = cancion.Genero });
                    cmd.Parameters.Add(new OracleParameter { OracleDbType = OracleDbType.Varchar2, Direction = ParameterDirection.Input, ParameterName = "P_LETRA", Value = cancion.Letra });
                    cmd.Parameters.Add(new OracleParameter { OracleDbType = OracleDbType.Varchar2, Direction = ParameterDirection.Input, ParameterName = "P_LINK", Value = cancion.Link });

                    await cmd.ExecuteNonQueryAsync();

                    cmd.CommandText = @"
                                        select SEQUENCECANCION.currval from dual
                                        ";
                    await cmd.ExecuteNonQueryAsync();

                    var adapter = new OracleDataAdapter(cmd);
                    var data = new DataSet("Datos");
                    adapter.Fill(data);

                    await OracleDBConnectionSingleton.OracleDBConnection.oracleConnection.CloseAsync();

                    if (data.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow item in data.Tables[0].Rows)
                        {
                            consecutivo = !Object.ReferenceEquals(System.DBNull.Value, item.ItemArray[0]) ? Convert.ToInt64(item.ItemArray[0]) : 0;
                        }
                    }

                    return consecutivo;
                }
                else
                {
                    return -2;
                }

            }
            catch (Exception ex)
            {
                await OracleDBConnectionSingleton.OracleDBConnection.oracleConnection.CloseAsync();
                Plugins.WriteExceptionLog(ex);
                return -1;
            }
        }





        public async Task<CancionModel> ConsultarCancion(int id)
        {
            var _outs = new List<CancionModel>();
            try
            {
                await OracleDBConnectionSingleton.OracleDBConnection.oracleConnection.OpenAsync();

                cmd.CommandText = "SELECT ID, IDUSUARIO, IDALBUN, NOMBRE,GENERO,LETRA,FECHAPUBLICACION,LINK FROM CANCIONES WHERE ID=:P_ID AND ESTADO='A'";
                cmd.Parameters.Clear();
                cmd.Parameters.Add(new OracleParameter { OracleDbType = OracleDbType.Varchar2, Direction = ParameterDirection.Input, ParameterName = "P_ID", Value = id });
                await cmd.ExecuteNonQueryAsync();

                var adapter = new OracleDataAdapter(cmd);
                var data = new DataSet("Datos");
                adapter.Fill(data);

                await OracleDBConnectionSingleton.OracleDBConnection.oracleConnection.CloseAsync();

                if (data.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow item in data.Tables[0].Rows)
                    {
                        CancionModel cancionModel = new CancionModel
                        {
                            Id = !Object.ReferenceEquals(System.DBNull.Value, item.ItemArray[0]) ? Convert.ToInt64(item.ItemArray[0]) : 0,
                            IdUsuario = !Object.ReferenceEquals(System.DBNull.Value, item.ItemArray[1]) ? Convert.ToInt64(item.ItemArray[1]) : 0,
                            IdAlbun = !Object.ReferenceEquals(System.DBNull.Value, item.ItemArray[2]) ? Convert.ToInt64(item.ItemArray[2]) : 0,
                            Nombre = !Object.ReferenceEquals(System.DBNull.Value, item.ItemArray[3]) ? Convert.ToString(item.ItemArray[3]) : "SIN NOMBRE",
                            Genero = !Object.ReferenceEquals(System.DBNull.Value, item.ItemArray[4]) ? Convert.ToString(item.ItemArray[4]) : "SIN GENERO",
                            Letra = !Object.ReferenceEquals(System.DBNull.Value, item.ItemArray[5]) ? Convert.ToString(item.ItemArray[5]).Replace("-", "\n") : "SIN LETRA",
                            FechaPublicacion = !Object.ReferenceEquals(System.DBNull.Value, item.ItemArray[6]) ? Convert.ToDateTime(item.ItemArray[6]) : null,
                            Link = !Object.ReferenceEquals(System.DBNull.Value, item.ItemArray[7]) ? Convert.ToString(item.ItemArray[7]) : "SIN LINK"
                        };

                        _outs.Add(cancionModel);
                    }
                }
                else
                {
                    _outs.Add(null);
                }

                return _outs[0];
            }
            catch (Exception ex)
            {
                Plugins.WriteExceptionLog(ex);
                await OracleDBConnectionSingleton.OracleDBConnection.oracleConnection.CloseAsync();
            }
            return null;
        }






        public async Task<long> DesactivarCancion(int id)
        {
            var _outs = new List<CancionModel>();
            try
            {
                await OracleDBConnectionSingleton.OracleDBConnection.oracleConnection.OpenAsync();

                cmd.CommandText = "SELECT ID, IDUSUARIO, IDALBUN, NOMBRE,GENERO,LETRA,FECHAPUBLICACION,LINK FROM CANCIONES WHERE ID=:P_ID AND ESTADO='A'";
                cmd.Parameters.Clear();
                cmd.Parameters.Add(new OracleParameter { OracleDbType = OracleDbType.Varchar2, Direction = ParameterDirection.Input, ParameterName = "P_ID", Value = id });
                await cmd.ExecuteNonQueryAsync();

                var adapter = new OracleDataAdapter(cmd);
                var data = new DataSet("Datos");
                adapter.Fill(data);

                await OracleDBConnectionSingleton.OracleDBConnection.oracleConnection.CloseAsync();

                if (data.Tables[0].Rows.Count > 0)
                {
                    await OracleDBConnectionSingleton.OracleDBConnection.oracleConnection.OpenAsync();
                    cmd.CommandText = @"
                                        UPDATE CANCIONES SET
                                        ESTADO='I'
                                        WHERE ID=:P_ID
                                        ";
                    cmd.Parameters.Clear();
                    cmd.Parameters.Add(new OracleParameter { OracleDbType = OracleDbType.Varchar2, Direction = ParameterDirection.Input, ParameterName = "P_ID", Value = id });
                    await cmd.ExecuteNonQueryAsync();
                    await OracleDBConnectionSingleton.OracleDBConnection.oracleConnection.CloseAsync();
                    return 1;
                }
                else
                {
                    return 0;
                }

                
            }
            catch (Exception ex)
            {
                await OracleDBConnectionSingleton.OracleDBConnection.oracleConnection.CloseAsync();
                Plugins.WriteExceptionLog(ex);
            }
            return -1;
        }






        public async Task<long> ModificarCancion(CancionModel cancion)
        {
            try
            {

                if (cancion.Nombre != null && cancion.Letra != null)
                {
                    await OracleDBConnectionSingleton.OracleDBConnection.oracleConnection.OpenAsync();

                    cmd.CommandText = @"
                                        SELECT ID, IDUSUARIO, IDALBUN, NOMBRE,GENERO,LETRA,FECHAPUBLICACION,LINK FROM CANCIONES WHERE ID=:P_IDB AND ESTADO='A'
                                        ";
                    cmd.Parameters.Clear();
                    cmd.Parameters.Add(new OracleParameter { OracleDbType = OracleDbType.Varchar2, Direction = ParameterDirection.Input, ParameterName = "P_IDB", Value = cancion.Id });
                    await cmd.ExecuteNonQueryAsync();

                    var adapter = new OracleDataAdapter(cmd);
                    var data = new DataSet("Datos");
                    adapter.Fill(data);



                    if (data.Tables[0].Rows.Count == 1)
                    {

                        cmd.CommandText = @"
                                        UPDATE DBMUSICFTMUSIC.CANCIONES SET
                                        IDALBUN=:P_IDALBUN,
                                        NOMBRE=:P_NOMBRE,
                                        GENERO=:P_GENERO,
                                        LETRA=:P_LETRA,
                                        LINK=:P_LINK
                                        WHERE ID = :P_ID AND ESTADO ='A'
                                        ";
                        cmd.Parameters.Clear();

                        cmd.Parameters.Add(new OracleParameter { OracleDbType = OracleDbType.Long, Direction = ParameterDirection.Input, ParameterName = "P_IDALBUN", Value = cancion.IdAlbun });
                        cmd.Parameters.Add(new OracleParameter { OracleDbType = OracleDbType.Varchar2, Direction = ParameterDirection.Input, ParameterName = "P_NOMBRE", Value = cancion.Nombre });
                        cmd.Parameters.Add(new OracleParameter { OracleDbType = OracleDbType.Varchar2, Direction = ParameterDirection.Input, ParameterName = "P_GENERO", Value = cancion.Genero });
                        cmd.Parameters.Add(new OracleParameter { OracleDbType = OracleDbType.Varchar2, Direction = ParameterDirection.Input, ParameterName = "P_LETRA", Value = cancion.Letra });
                        cmd.Parameters.Add(new OracleParameter { OracleDbType = OracleDbType.Varchar2, Direction = ParameterDirection.Input, ParameterName = "P_LINK", Value = cancion.Link });
                        cmd.Parameters.Add(new OracleParameter { OracleDbType = OracleDbType.Long, Direction = ParameterDirection.Input, ParameterName = "P_ID", Value = cancion.Id });
                        await cmd.ExecuteNonQueryAsync();
                        await OracleDBConnectionSingleton.OracleDBConnection.oracleConnection.CloseAsync();

                    }
                    else
                    {
                        return 0;//no existe
                    }
                    await OracleDBConnectionSingleton.OracleDBConnection.oracleConnection.CloseAsync();


                    return 1;//todo bien
                }
                else
                {
                    return -2;//campos vacios
                }

            }
            catch (Exception ex)
            {
                await OracleDBConnectionSingleton.OracleDBConnection.oracleConnection.CloseAsync();
                Plugins.WriteExceptionLog(ex);
                return -1;//error
            }
        }








        public async Task<List<CancionModel>> ConsultarCancionPorUsuario(int idUsuario)
        {
            var _outs = new List<CancionModel>();
            try
            {
                await OracleDBConnectionSingleton.OracleDBConnection.oracleConnection.OpenAsync();

                cmd.CommandText = "SELECT ID, IDUSUARIO, IDALBUN, NOMBRE,GENERO,LETRA,FECHAPUBLICACION,LINK FROM CANCIONES WHERE ESTADO='A' AND IDUSUARIO=:P_IDUSUARIO";
                cmd.Parameters.Clear();
                cmd.Parameters.Add(new OracleParameter { OracleDbType = OracleDbType.Long, Direction = ParameterDirection.Input, ParameterName = "P_IDUSUARIO", Value = idUsuario });
                await cmd.ExecuteNonQueryAsync();

                var adapter = new OracleDataAdapter(cmd);
                var data = new DataSet("Datos");
                adapter.Fill(data);

                await OracleDBConnectionSingleton.OracleDBConnection.oracleConnection.CloseAsync();

                if (data.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow item in data.Tables[0].Rows)
                    {
                        CancionModel cancionModel = new CancionModel
                        {
                            Id = !Object.ReferenceEquals(System.DBNull.Value, item.ItemArray[0]) ? Convert.ToInt64(item.ItemArray[0]) : 0,
                            IdUsuario = !Object.ReferenceEquals(System.DBNull.Value, item.ItemArray[1]) ? Convert.ToInt64(item.ItemArray[1]) : 0,
                            IdAlbun = !Object.ReferenceEquals(System.DBNull.Value, item.ItemArray[2]) ? Convert.ToInt64(item.ItemArray[2]) : 0,
                            Nombre = !Object.ReferenceEquals(System.DBNull.Value, item.ItemArray[3]) ? Convert.ToString(item.ItemArray[3]) : "SIN NOMBRE",
                            Genero = !Object.ReferenceEquals(System.DBNull.Value, item.ItemArray[4]) ? Convert.ToString(item.ItemArray[4]) : "SIN GENERO",
                            Letra = !Object.ReferenceEquals(System.DBNull.Value, item.ItemArray[5]) ? Convert.ToString(item.ItemArray[5]).Replace("-", "\n") : "SIN LETRA",
                            FechaPublicacion = !Object.ReferenceEquals(System.DBNull.Value, item.ItemArray[6]) ? Convert.ToDateTime(item.ItemArray[6]) : null,
                            Link = !Object.ReferenceEquals(System.DBNull.Value, item.ItemArray[7]) ? Convert.ToString(item.ItemArray[7]) : "SIN LINK"
                        };

                        _outs.Add(cancionModel);
                    }
                }
                else
                {
                    return null;
                }

                return _outs;
            }
            catch (Exception ex)
            {
                Plugins.WriteExceptionLog(ex);
            }
            return null;
        }


















        public async Task<List<CancionModel>> ConsultarCancionPorNombre(string nombre)
        {
            var _outs = new List<CancionModel>();
            try
            {
                await OracleDBConnectionSingleton.OracleDBConnection.oracleConnection.OpenAsync();

                cmd.CommandText = "SELECT ID, IDUSUARIO, IDALBUN, NOMBRE,GENERO,LETRA,FECHAPUBLICACION,LINK FROM CANCIONES WHERE ESTADO='A' AND NOMBRE LIKE :P_NOMBRE";
                cmd.Parameters.Clear();
                cmd.Parameters.Add(new OracleParameter { OracleDbType = OracleDbType.Varchar2, Direction = ParameterDirection.Input, ParameterName = "P_NOMBRE", Value = "%"+nombre+"%" });
                await cmd.ExecuteNonQueryAsync();

                var adapter = new OracleDataAdapter(cmd);
                var data = new DataSet("Datos");
                adapter.Fill(data);

                await OracleDBConnectionSingleton.OracleDBConnection.oracleConnection.CloseAsync();

                if (data.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow item in data.Tables[0].Rows)
                    {
                        CancionModel cancionModel = new CancionModel
                        {
                            Id = !Object.ReferenceEquals(System.DBNull.Value, item.ItemArray[0]) ? Convert.ToInt64(item.ItemArray[0]) : 0,
                            IdUsuario = !Object.ReferenceEquals(System.DBNull.Value, item.ItemArray[1]) ? Convert.ToInt64(item.ItemArray[1]) : 0,
                            IdAlbun = !Object.ReferenceEquals(System.DBNull.Value, item.ItemArray[2]) ? Convert.ToInt64(item.ItemArray[2]) : 0,
                            Nombre = !Object.ReferenceEquals(System.DBNull.Value, item.ItemArray[3]) ? Convert.ToString(item.ItemArray[3]) : "SIN NOMBRE",
                            Genero = !Object.ReferenceEquals(System.DBNull.Value, item.ItemArray[4]) ? Convert.ToString(item.ItemArray[4]) : "SIN GENERO",
                            Letra = !Object.ReferenceEquals(System.DBNull.Value, item.ItemArray[5]) ? Convert.ToString(item.ItemArray[5]).Replace("-", "\n") : "SIN LETRA",
                            FechaPublicacion = !Object.ReferenceEquals(System.DBNull.Value, item.ItemArray[6]) ? Convert.ToDateTime(item.ItemArray[6]) : null,
                            Link = !Object.ReferenceEquals(System.DBNull.Value, item.ItemArray[7]) ? Convert.ToString(item.ItemArray[7]) : "SIN LINK"
                        };

                        _outs.Add(cancionModel);
                    }
                }

                return _outs;
            }
            catch (Exception ex)
            {
                Plugins.WriteExceptionLog(ex);
            }
            return null;
        }










        public async Task<long> GuardarComentarioCancion(ComentarioModel comentario)
        {
            long consecutivo = 0;
            try
            {
                if (comentario.Id != null && comentario.IdUsuario != null && comentario.IdCancion != null)
                {
                    comentario.Mensaje.Replace("\n", "-");
                    await OracleDBConnectionSingleton.OracleDBConnection.oracleConnection.OpenAsync();

                    cmd.CommandText = "SELECT ID, IDUSUARIO, IDALBUN, NOMBRE,GENERO,LETRA,FECHAPUBLICACION,LINK FROM CANCIONES WHERE ID=:P_ID AND ESTADO='A'";
                    cmd.Parameters.Clear();
                    cmd.Parameters.Add(new OracleParameter { OracleDbType = OracleDbType.Varchar2, Direction = ParameterDirection.Input, ParameterName = "P_ID", Value = comentario.IdCancion });
                    await cmd.ExecuteNonQueryAsync();

                    var adapter = new OracleDataAdapter(cmd);
                    var data = new DataSet("Datos");
                    adapter.Fill(data);


                    if (data.Tables[0].Rows.Count > 0)
                    {
                        cmd.CommandText = @"
                                        INSERT INTO COMENTARIOS
                                        (ID, IDUSUARIO, IDCANCION,MENSAJE,FECHA,NOMBREUSUARIO,ESTADO)
                                        VALUES(SEQUENCECOMENTARIOS.NEXTVAL,:P_IDUSUARIO,:P_IDCANCION,:P_MENSAJE,CURRENT_DATE,:P_NOMBREUSUARIO,'A')
                                        ";
                    cmd.Parameters.Clear();
                    cmd.Parameters.Add(new OracleParameter { OracleDbType = OracleDbType.Long, Direction = ParameterDirection.Input, ParameterName = "P_IDUSUARIO", Value = comentario.IdUsuario });
                    cmd.Parameters.Add(new OracleParameter { OracleDbType = OracleDbType.Long, Direction = ParameterDirection.Input, ParameterName = "P_IDCANCION", Value = comentario.IdCancion });
                    cmd.Parameters.Add(new OracleParameter { OracleDbType = OracleDbType.Varchar2, Direction = ParameterDirection.Input, ParameterName = "P_MENSAJE", Value = comentario.Mensaje });
                    cmd.Parameters.Add(new OracleParameter { OracleDbType = OracleDbType.Varchar2, Direction = ParameterDirection.Input, ParameterName = "P_NOMBREUSUARIO", Value = comentario.NombreUsuario });
                        await cmd.ExecuteNonQueryAsync();

                    cmd.CommandText = @"
                                        select SEQUENCECOMENTARIOS.currval from dual
                                        ";
                    await cmd.ExecuteNonQueryAsync();

                    adapter = new OracleDataAdapter(cmd);
                    data = new DataSet("Datos");
                    adapter.Fill(data);

                    await OracleDBConnectionSingleton.OracleDBConnection.oracleConnection.CloseAsync();

                    if (data.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow item in data.Tables[0].Rows)
                        {
                            consecutivo = !Object.ReferenceEquals(System.DBNull.Value, item.ItemArray[0]) ? Convert.ToInt64(item.ItemArray[0]) : 0;
                        }
                    }
                    return consecutivo;
                    }
                    else
                    {
                        return -3;
                    }
                }
                else
                {
                    return -2;
                }

            }
            catch (Exception ex)
            {
                await OracleDBConnectionSingleton.OracleDBConnection.oracleConnection.CloseAsync();
                Plugins.WriteExceptionLog(ex);
                return -1;
            }
        }






        public async Task<List<ComentarioModel>> ConsultarComentarioPorCancion(int idCancion)
        {
            var _outs = new List<ComentarioModel>();
            try
            {
                await OracleDBConnectionSingleton.OracleDBConnection.oracleConnection.OpenAsync();

                cmd.CommandText = "SELECT ID, IDUSUARIO, IDCANCION, MENSAJE,FECHA,NOMBREUSUARIO FROM COMENTARIOS WHERE IDCANCION=:P_IDCANCION AND ESTADO='A'";
                cmd.Parameters.Clear();
                cmd.Parameters.Add(new OracleParameter { OracleDbType = OracleDbType.Long, Direction = ParameterDirection.Input, ParameterName = "P_IDCANCION", Value = idCancion});
                await cmd.ExecuteNonQueryAsync();

                var adapter = new OracleDataAdapter(cmd);
                var data = new DataSet("Datos");
                adapter.Fill(data);

                await OracleDBConnectionSingleton.OracleDBConnection.oracleConnection.CloseAsync();

                if (data.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow item in data.Tables[0].Rows)
                    {
                        ComentarioModel comentarioModel = new ComentarioModel
                        {
                            Id = !Object.ReferenceEquals(System.DBNull.Value, item.ItemArray[0]) ? Convert.ToInt64(item.ItemArray[0]) : 0,
                            IdUsuario = !Object.ReferenceEquals(System.DBNull.Value, item.ItemArray[1]) ? Convert.ToInt64(item.ItemArray[1]) : 0,
                            IdCancion = !Object.ReferenceEquals(System.DBNull.Value, item.ItemArray[2]) ? Convert.ToInt64(item.ItemArray[2]) : 0,
                            Mensaje = !Object.ReferenceEquals(System.DBNull.Value, item.ItemArray[3]) ? Convert.ToString(item.ItemArray[3]) : "SIN COMENTARIO",
                            Fecha = !Object.ReferenceEquals(System.DBNull.Value, item.ItemArray[4]) ? Convert.ToDateTime(item.ItemArray[4]) : null,
                            NombreUsuario= !Object.ReferenceEquals(System.DBNull.Value, item.ItemArray[5]) ? Convert.ToString(item.ItemArray[5]) : "SIN NOMBRE DE USUARIO"
                        };

                        _outs.Add(comentarioModel);
                    }
                }
                else
                {
                    return null;
                }

                return _outs;
            }
            catch (Exception ex)
            {
                Plugins.WriteExceptionLog(ex);
            }
            return null;
        }









        public async Task<long> GuardarLikeCancion(LikeModel like)
        {
            long resultado = 0;
            try
            {
                if (like.Id != null && like.IdUsuario != null && like.IdCancion != null)
                {
                    
                    await OracleDBConnectionSingleton.OracleDBConnection.oracleConnection.OpenAsync();

                    cmd.CommandText = "SELECT ID, IDUSUARIO, IDALBUN, NOMBRE,GENERO,LETRA,FECHAPUBLICACION,LINK FROM CANCIONES WHERE ID=:P_ID AND ESTADO='A'";
                    cmd.Parameters.Clear();
                    cmd.Parameters.Add(new OracleParameter { OracleDbType = OracleDbType.Varchar2, Direction = ParameterDirection.Input, ParameterName = "P_ID", Value = like.IdCancion });
                    await cmd.ExecuteNonQueryAsync();

                    var adapter = new OracleDataAdapter(cmd);
                    var data = new DataSet("Datos");
                    adapter.Fill(data);


                    if (data.Tables[0].Rows.Count > 0)
                    {
                        cmd.CommandText = "SELECT ID FROM LIKES WHERE IDUSUARIO=:P_IDUSUARIO AND IDCANCION=:P_IDCANCION";
                        cmd.Parameters.Clear();
                        cmd.Parameters.Add(new OracleParameter { OracleDbType = OracleDbType.Varchar2, Direction = ParameterDirection.Input, ParameterName = "P_IDUSUARIO", Value = like.IdUsuario });
                        cmd.Parameters.Add(new OracleParameter { OracleDbType = OracleDbType.Varchar2, Direction = ParameterDirection.Input, ParameterName = "P_IDCANCION", Value = like.IdCancion });
                        await cmd.ExecuteNonQueryAsync();

                        adapter = new OracleDataAdapter(cmd);
                        data = new DataSet("Datos");
                        adapter.Fill(data);

                        if (data.Tables[0].Rows.Count == 0 )
                        {
                            cmd.CommandText = @"
                                        INSERT INTO LIKES
                                        (ID, IDUSUARIO, IDCANCION)
                                        VALUES(SEQUENCELIKES.NEXTVAL,:P_IDUSUARIO,:P_IDCANCION)
                                        ";
                            cmd.Parameters.Clear();
                            cmd.Parameters.Add(new OracleParameter { OracleDbType = OracleDbType.Long, Direction = ParameterDirection.Input, ParameterName = "P_IDUSUARIO", Value = like.IdUsuario });
                            cmd.Parameters.Add(new OracleParameter { OracleDbType = OracleDbType.Long, Direction = ParameterDirection.Input, ParameterName = "P_IDCANCION", Value = like.IdCancion });
                            await cmd.ExecuteNonQueryAsync();

                            cmd.CommandText = @"
                                        DELETE FROM DISLIKES WHERE IDUSUARIO=:P_IDUSUARIO AND IDCANCION=:P_IDCANCION
                                        ";
                            cmd.Parameters.Clear();
                            cmd.Parameters.Add(new OracleParameter { OracleDbType = OracleDbType.Long, Direction = ParameterDirection.Input, ParameterName = "P_IDUSUARIO", Value = like.IdUsuario });
                            cmd.Parameters.Add(new OracleParameter { OracleDbType = OracleDbType.Long, Direction = ParameterDirection.Input, ParameterName = "P_IDCANCION", Value = like.IdCancion });
                            await cmd.ExecuteNonQueryAsync();

                            cmd.CommandText = @"
                                        select SEQUENCELIKES.currval from dual
                                        ";
                            await cmd.ExecuteNonQueryAsync();

                            adapter = new OracleDataAdapter(cmd);
                            data = new DataSet("Datos");
                            adapter.Fill(data);

                            await OracleDBConnectionSingleton.OracleDBConnection.oracleConnection.CloseAsync();

                            if (data.Tables[0].Rows.Count > 0)
                            {
                                foreach (DataRow item in data.Tables[0].Rows)
                                {
                                    resultado = !Object.ReferenceEquals(System.DBNull.Value, item.ItemArray[0]) ? Convert.ToInt64(item.ItemArray[0]) : 0;
                                }
                            }
                        }
                        else
                        {
                            cmd.CommandText = @"
                                        DELETE FROM LIKES WHERE IDUSUARIO=:P_IDUSUARIO AND IDCANCION=:P_IDCANCION
                                        ";
                            cmd.Parameters.Clear();
                            cmd.Parameters.Add(new OracleParameter { OracleDbType = OracleDbType.Long, Direction = ParameterDirection.Input, ParameterName = "P_IDUSUARIO", Value = like.IdUsuario });
                            cmd.Parameters.Add(new OracleParameter { OracleDbType = OracleDbType.Long, Direction = ParameterDirection.Input, ParameterName = "P_IDCANCION", Value = like.IdCancion });
                            await cmd.ExecuteNonQueryAsync();
                            resultado = -4;
                        }

                    }
                    else
                    {
                        resultado = -3;
                    }
                }
                else
                {
                    resultado = -2;
                }

            }
            catch (Exception ex)
            {
                await OracleDBConnectionSingleton.OracleDBConnection.oracleConnection.CloseAsync();
                Plugins.WriteExceptionLog(ex);
                return -1;
            }
            await OracleDBConnectionSingleton.OracleDBConnection.oracleConnection.CloseAsync();
            return resultado;
        }




        public async Task<long> GuardarDisLikeCancion(DisLikeModel disLike)
        {
            long resultado = 0;
            try
            {
                if (disLike.Id != null && disLike.IdUsuario != null && disLike.IdCancion != null)
                {

                    await OracleDBConnectionSingleton.OracleDBConnection.oracleConnection.OpenAsync();

                    cmd.CommandText = "SELECT ID, IDUSUARIO, IDALBUN, NOMBRE,GENERO,LETRA,FECHAPUBLICACION,LINK FROM CANCIONES WHERE ID=:P_ID AND ESTADO='A'";
                    cmd.Parameters.Clear();
                    cmd.Parameters.Add(new OracleParameter { OracleDbType = OracleDbType.Varchar2, Direction = ParameterDirection.Input, ParameterName = "P_ID", Value = disLike.IdCancion });
                    await cmd.ExecuteNonQueryAsync();

                    var adapter = new OracleDataAdapter(cmd);
                    var data = new DataSet("Datos");
                    adapter.Fill(data);


                    if (data.Tables[0].Rows.Count > 0)
                    {
                        cmd.CommandText = "SELECT ID FROM DISLIKES WHERE IDUSUARIO=:P_IDUSUARIO AND IDCANCION=:P_IDCANCION";
                        cmd.Parameters.Clear();
                        cmd.Parameters.Add(new OracleParameter { OracleDbType = OracleDbType.Varchar2, Direction = ParameterDirection.Input, ParameterName = "P_IDUSUARIO", Value = disLike.IdUsuario });
                        cmd.Parameters.Add(new OracleParameter { OracleDbType = OracleDbType.Varchar2, Direction = ParameterDirection.Input, ParameterName = "P_IDCANCION", Value = disLike.IdCancion });
                        await cmd.ExecuteNonQueryAsync();

                        adapter = new OracleDataAdapter(cmd);
                        data = new DataSet("Datos");
                        adapter.Fill(data);

                        if (data.Tables[0].Rows.Count == 0)
                        {
                            cmd.CommandText = @"
                                        INSERT INTO DISLIKES
                                        (ID, IDUSUARIO, IDCANCION)
                                        VALUES(SEQUENCEDISLIKES.NEXTVAL,:P_IDUSUARIO,:P_IDCANCION)
                                        ";
                            cmd.Parameters.Clear();
                            cmd.Parameters.Add(new OracleParameter { OracleDbType = OracleDbType.Long, Direction = ParameterDirection.Input, ParameterName = "P_IDUSUARIO", Value = disLike.IdUsuario });
                            cmd.Parameters.Add(new OracleParameter { OracleDbType = OracleDbType.Long, Direction = ParameterDirection.Input, ParameterName = "P_IDCANCION", Value = disLike.IdCancion });
                            await cmd.ExecuteNonQueryAsync();

                            cmd.CommandText = @"
                                        DELETE FROM LIKES WHERE IDUSUARIO=:P_IDUSUARIO AND IDCANCION=:P_IDCANCION
                                        ";
                            cmd.Parameters.Clear();
                            cmd.Parameters.Add(new OracleParameter { OracleDbType = OracleDbType.Long, Direction = ParameterDirection.Input, ParameterName = "P_IDUSUARIO", Value = disLike.IdUsuario });
                            cmd.Parameters.Add(new OracleParameter { OracleDbType = OracleDbType.Long, Direction = ParameterDirection.Input, ParameterName = "P_IDCANCION", Value = disLike.IdCancion });
                            await cmd.ExecuteNonQueryAsync();

                            cmd.CommandText = @"
                                        select SEQUENCEDISLIKES.currval from dual
                                        ";
                            await cmd.ExecuteNonQueryAsync();

                            adapter = new OracleDataAdapter(cmd);
                            data = new DataSet("Datos");
                            adapter.Fill(data);

                            await OracleDBConnectionSingleton.OracleDBConnection.oracleConnection.CloseAsync();

                            if (data.Tables[0].Rows.Count > 0)
                            {
                                foreach (DataRow item in data.Tables[0].Rows)
                                {
                                    resultado = !Object.ReferenceEquals(System.DBNull.Value, item.ItemArray[0]) ? Convert.ToInt64(item.ItemArray[0]) : 0;
                                }
                            }
                        }
                        else
                        {
                            cmd.CommandText = @"
                                        DELETE FROM DISLIKES WHERE IDUSUARIO=:P_IDUSUARIO AND IDCANCION=:P_IDCANCION
                                        ";
                            cmd.Parameters.Clear();
                            cmd.Parameters.Add(new OracleParameter { OracleDbType = OracleDbType.Long, Direction = ParameterDirection.Input, ParameterName = "P_IDUSUARIO", Value = disLike.IdUsuario });
                            cmd.Parameters.Add(new OracleParameter { OracleDbType = OracleDbType.Long, Direction = ParameterDirection.Input, ParameterName = "P_IDCANCION", Value = disLike.IdCancion });
                            await cmd.ExecuteNonQueryAsync();
                            resultado = -4;
                        }

                    }
                    else
                    {
                        resultado = -3;
                    }
                }
                else
                {
                    resultado = -2;
                }

            }
            catch (Exception ex)
            {
                await OracleDBConnectionSingleton.OracleDBConnection.oracleConnection.CloseAsync();
                Plugins.WriteExceptionLog(ex);
                return -1;
            }
            await OracleDBConnectionSingleton.OracleDBConnection.oracleConnection.CloseAsync();
            return resultado;
        }



        public async Task<long> ConsultarNumeroMegustaPorCancion(int idCancion)
        {
            long resultado = 0;
            try
            {
                await OracleDBConnectionSingleton.OracleDBConnection.oracleConnection.OpenAsync();

                cmd.CommandText = "SELECT COUNT(*) FROM LIKES WHERE IDCANCION=:P_IDCANCION";
                cmd.Parameters.Clear();
                cmd.Parameters.Add(new OracleParameter { OracleDbType = OracleDbType.Long, Direction = ParameterDirection.Input, ParameterName = "P_IDCANCION", Value = idCancion });
                await cmd.ExecuteNonQueryAsync();

                var adapter = new OracleDataAdapter(cmd);
                var data = new DataSet("Datos");
                adapter.Fill(data);

                await OracleDBConnectionSingleton.OracleDBConnection.oracleConnection.CloseAsync();

                if (data.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow item in data.Tables[0].Rows)
                    {
                        resultado = !Object.ReferenceEquals(System.DBNull.Value, item.ItemArray[0]) ? Convert.ToInt64(item.ItemArray[0]) : 0;
                    }
                }
                else
                {
                    resultado= -1;
                }
            }
            catch (Exception ex)
            {
                await OracleDBConnectionSingleton.OracleDBConnection.oracleConnection.CloseAsync();
                resultado = -2;
                Plugins.WriteExceptionLog(ex);
            }
            await OracleDBConnectionSingleton.OracleDBConnection.oracleConnection.CloseAsync();
            return resultado;
        }


        public async Task<long> ConsultarNumeroNoMegustaPorCancion(int idCancion)
        {
            long resultado = 0;
            try
            {
                await OracleDBConnectionSingleton.OracleDBConnection.oracleConnection.OpenAsync();

                cmd.CommandText = "SELECT COUNT(*) FROM DISLIKES WHERE IDCANCION=:P_IDCANCION";
                cmd.Parameters.Clear();
                cmd.Parameters.Add(new OracleParameter { OracleDbType = OracleDbType.Long, Direction = ParameterDirection.Input, ParameterName = "P_IDCANCION", Value = idCancion });
                await cmd.ExecuteNonQueryAsync();

                var adapter = new OracleDataAdapter(cmd);
                var data = new DataSet("Datos");
                adapter.Fill(data);

                await OracleDBConnectionSingleton.OracleDBConnection.oracleConnection.CloseAsync();

                if (data.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow item in data.Tables[0].Rows)
                    {
                        resultado = !Object.ReferenceEquals(System.DBNull.Value, item.ItemArray[0]) ? Convert.ToInt64(item.ItemArray[0]) : 0;
                    }
                }
                else
                {
                    resultado = -1;
                }
            }
            catch (Exception ex)
            {
                await OracleDBConnectionSingleton.OracleDBConnection.oracleConnection.CloseAsync();
                resultado = -2;
                Plugins.WriteExceptionLog(ex);
            }
            await OracleDBConnectionSingleton.OracleDBConnection.oracleConnection.CloseAsync();
            return resultado;
        }





        public async Task<CancionCompletaDTO> ConsultarCancionCompleta(int idCancion)
        {
            try
            {
                CancionCompletaDTO cancionCompletaDTO=new CancionCompletaDTO();
                CancionModel cancion = ConsultarCancion(idCancion).Result;
                if (cancion != null)
                {
                    List<ComentarioModel> comentarios = ConsultarComentarioPorCancion(idCancion).Result;
                    long numeroLikes = ConsultarNumeroMegustaPorCancion(idCancion).Result;
                    long numeroDisLikes = ConsultarNumeroNoMegustaPorCancion(idCancion).Result;
                    if (numeroLikes < 0) { numeroLikes = 0; }
                    if (numeroDisLikes < 0) { numeroDisLikes = 0; }
                    cancionCompletaDTO.NumeroDisLikes = numeroDisLikes;
                    cancionCompletaDTO.NumeroLikes = numeroLikes;
                    cancionCompletaDTO.MapearCancion(cancion);
                    cancionCompletaDTO.Comentarios = comentarios;
                    return cancionCompletaDTO;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }








        public async Task<List<InterpretacionModel>> GetListInterpretaciones()
        {
            var _outs = new List<InterpretacionModel>();
            try
            {
                await OracleDBConnectionSingleton.OracleDBConnection.oracleConnection.OpenAsync();

                cmd.CommandText = "SELECT ID, IDUSUARIO, IDALBUN, NOMBRE,GENERO,LETRA,FECHAPUBLICACION,LINK,IDCANCION FROM INTERPRETACIONES WHERE ESTADO='A'";
                await cmd.ExecuteNonQueryAsync();

                var adapter = new OracleDataAdapter(cmd);
                var data = new DataSet("Datos");
                adapter.Fill(data);

                await OracleDBConnectionSingleton.OracleDBConnection.oracleConnection.CloseAsync();

                if (data.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow item in data.Tables[0].Rows)
                    {
                        InterpretacionModel interpretacionModel = new InterpretacionModel
                        {
                            Id = !Object.ReferenceEquals(System.DBNull.Value, item.ItemArray[0]) ? Convert.ToInt64(item.ItemArray[0]) : 0,
                            IdUsuario = !Object.ReferenceEquals(System.DBNull.Value, item.ItemArray[1]) ? Convert.ToInt64(item.ItemArray[1]) : 0,
                            IdAlbun = !Object.ReferenceEquals(System.DBNull.Value, item.ItemArray[2]) ? Convert.ToInt64(item.ItemArray[2]) : 0,
                            Nombre = !Object.ReferenceEquals(System.DBNull.Value, item.ItemArray[3]) ? Convert.ToString(item.ItemArray[3]) : "SIN NOMBRE",
                            Genero = !Object.ReferenceEquals(System.DBNull.Value, item.ItemArray[4]) ? Convert.ToString(item.ItemArray[4]) : "SIN GENERO",
                            Letra = !Object.ReferenceEquals(System.DBNull.Value, item.ItemArray[5]) ? Convert.ToString(item.ItemArray[5]).Replace("-", "\n") : "SIN LETRA",
                            FechaPublicacion = !Object.ReferenceEquals(System.DBNull.Value, item.ItemArray[6]) ? Convert.ToDateTime(item.ItemArray[6]) : null,
                            Link = !Object.ReferenceEquals(System.DBNull.Value, item.ItemArray[7]) ? Convert.ToString(item.ItemArray[7]) : "SIN LINK",
                            IdCancion = !Object.ReferenceEquals(System.DBNull.Value, item.ItemArray[8]) ? Convert.ToInt64(item.ItemArray[8]) : 0
                        };

                        _outs.Add(interpretacionModel);
                    }
                }

                return _outs;
            }
            catch (Exception ex)
            {
                Plugins.WriteExceptionLog(ex);
            }
            return null;
        }




        public async Task<List<InterpretacionModel>> ConsultarInterpretacionPorNombre(string nombre)
        {
            var _outs = new List<InterpretacionModel>();
            try
            {
                await OracleDBConnectionSingleton.OracleDBConnection.oracleConnection.OpenAsync();

                cmd.CommandText = "SELECT ID, IDUSUARIO, IDALBUN, NOMBRE,GENERO,LETRA,FECHAPUBLICACION,LINK,IDCANCION FROM INTERPRETACIONES WHERE ESTADO='A' AND NOMBRE LIKE :P_NOMBRE";
                cmd.Parameters.Clear();
                cmd.Parameters.Add(new OracleParameter { OracleDbType = OracleDbType.Varchar2, Direction = ParameterDirection.Input, ParameterName = "P_NOMBRE", Value = "%" + nombre + "%" });
                await cmd.ExecuteNonQueryAsync();

                var adapter = new OracleDataAdapter(cmd);
                var data = new DataSet("Datos");
                adapter.Fill(data);

                await OracleDBConnectionSingleton.OracleDBConnection.oracleConnection.CloseAsync();

                if (data.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow item in data.Tables[0].Rows)
                    {
                        InterpretacionModel interpretacionModel = new InterpretacionModel
                        {
                            Id = !Object.ReferenceEquals(System.DBNull.Value, item.ItemArray[0]) ? Convert.ToInt64(item.ItemArray[0]) : 0,
                            IdUsuario = !Object.ReferenceEquals(System.DBNull.Value, item.ItemArray[1]) ? Convert.ToInt64(item.ItemArray[1]) : 0,
                            IdAlbun = !Object.ReferenceEquals(System.DBNull.Value, item.ItemArray[2]) ? Convert.ToInt64(item.ItemArray[2]) : 0,
                            Nombre = !Object.ReferenceEquals(System.DBNull.Value, item.ItemArray[3]) ? Convert.ToString(item.ItemArray[3]) : "SIN NOMBRE",
                            Genero = !Object.ReferenceEquals(System.DBNull.Value, item.ItemArray[4]) ? Convert.ToString(item.ItemArray[4]) : "SIN GENERO",
                            Letra = !Object.ReferenceEquals(System.DBNull.Value, item.ItemArray[5]) ? Convert.ToString(item.ItemArray[5]).Replace("-", "\n") : "SIN LETRA",
                            FechaPublicacion = !Object.ReferenceEquals(System.DBNull.Value, item.ItemArray[6]) ? Convert.ToDateTime(item.ItemArray[6]) : null,
                            Link = !Object.ReferenceEquals(System.DBNull.Value, item.ItemArray[7]) ? Convert.ToString(item.ItemArray[7]) : "SIN LINK",
                            IdCancion = !Object.ReferenceEquals(System.DBNull.Value, item.ItemArray[8]) ? Convert.ToInt64(item.ItemArray[8]) : 0
                        };

                        _outs.Add(interpretacionModel);
                    }
                }

                return _outs;
            }
            catch (Exception ex)
            {
                Plugins.WriteExceptionLog(ex);
            }
            return null;
        }



        public async Task<List<InterpretacionModel>> ConsultarInterpretacionPorUsuario(int idUsuario)
        {
            var _outs = new List<InterpretacionModel>();
            try
            {
                await OracleDBConnectionSingleton.OracleDBConnection.oracleConnection.OpenAsync();

                cmd.CommandText = "SELECT ID, IDUSUARIO, IDALBUN, NOMBRE,GENERO,LETRA,FECHAPUBLICACION,LINK,IDCANCION FROM INTERPRETACIONES WHERE ESTADO='A' AND IDUSUARIO=:P_IDUSUARIO";
                cmd.Parameters.Clear();
                cmd.Parameters.Add(new OracleParameter { OracleDbType = OracleDbType.Long, Direction = ParameterDirection.Input, ParameterName = "P_IDUSUARIO", Value = idUsuario });
                await cmd.ExecuteNonQueryAsync();

                var adapter = new OracleDataAdapter(cmd);
                var data = new DataSet("Datos");
                adapter.Fill(data);

                await OracleDBConnectionSingleton.OracleDBConnection.oracleConnection.CloseAsync();

                if (data.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow item in data.Tables[0].Rows)
                    {
                        InterpretacionModel interpretacionModel = new InterpretacionModel
                        {
                            Id = !Object.ReferenceEquals(System.DBNull.Value, item.ItemArray[0]) ? Convert.ToInt64(item.ItemArray[0]) : 0,
                            IdUsuario = !Object.ReferenceEquals(System.DBNull.Value, item.ItemArray[1]) ? Convert.ToInt64(item.ItemArray[1]) : 0,
                            IdAlbun = !Object.ReferenceEquals(System.DBNull.Value, item.ItemArray[2]) ? Convert.ToInt64(item.ItemArray[2]) : 0,
                            Nombre = !Object.ReferenceEquals(System.DBNull.Value, item.ItemArray[3]) ? Convert.ToString(item.ItemArray[3]) : "SIN NOMBRE",
                            Genero = !Object.ReferenceEquals(System.DBNull.Value, item.ItemArray[4]) ? Convert.ToString(item.ItemArray[4]) : "SIN GENERO",
                            Letra = !Object.ReferenceEquals(System.DBNull.Value, item.ItemArray[5]) ? Convert.ToString(item.ItemArray[5]).Replace("-", "\n") : "SIN LETRA",
                            FechaPublicacion = !Object.ReferenceEquals(System.DBNull.Value, item.ItemArray[6]) ? Convert.ToDateTime(item.ItemArray[6]) : null,
                            Link = !Object.ReferenceEquals(System.DBNull.Value, item.ItemArray[7]) ? Convert.ToString(item.ItemArray[7]) : "SIN LINK",
                            IdCancion = !Object.ReferenceEquals(System.DBNull.Value, item.ItemArray[8]) ? Convert.ToInt64(item.ItemArray[8]) : 0
                        };

                        _outs.Add(interpretacionModel);
                    }
                }
                else
                {
                    return null;
                }

                return _outs;
            }
            catch (Exception ex)
            {
                Plugins.WriteExceptionLog(ex);
            }
            return null;
        }




        public async Task<long> SaveInterpretacion(InterpretacionModel interpretacion)
        {
            long consecutivo = 0;
            try
            {
                if (interpretacion.Nombre != null && interpretacion.Letra != null)
                {


                    await OracleDBConnectionSingleton.OracleDBConnection.oracleConnection.OpenAsync();
                    cmd.CommandText = "SELECT ID, IDUSUARIO, IDALBUN, NOMBRE,GENERO,LETRA,FECHAPUBLICACION,LINK FROM CANCIONES WHERE ID=:P_ID AND ESTADO='A'";
                    cmd.Parameters.Clear();
                    cmd.Parameters.Add(new OracleParameter { OracleDbType = OracleDbType.Varchar2, Direction = ParameterDirection.Input, ParameterName = "P_ID", Value = interpretacion.IdCancion });
                    await cmd.ExecuteNonQueryAsync();

                    var adapter = new OracleDataAdapter(cmd);
                    var data = new DataSet("Datos");
                    adapter.Fill(data);


                    if (data.Tables[0].Rows.Count > 0)
                    {
                        interpretacion.Letra.Replace("\n", "-");
                        
                        cmd.CommandText = @"
                                        INSERT INTO INTERPRETACIONES
                                        (ID, IDUSUARIO, IDALBUN, NOMBRE,GENERO,LETRA,FECHAPUBLICACION,LINK,ESTADO,IDCANCION)
                                        VALUES(SEQUENCECANCION.NEXTVAL,:P_IDUSUARIO,:P_IDALBUN ,:P_NOMBRE, :P_GENERO, :P_LETRA,CURRENT_DATE,:P_LINK,'A',:P_IDCANCION)
                                        ";
                        cmd.Parameters.Clear();
                        cmd.Parameters.Add(new OracleParameter { OracleDbType = OracleDbType.Long, Direction = ParameterDirection.Input, ParameterName = "P_IDUSUARIO", Value = interpretacion.IdUsuario });
                        cmd.Parameters.Add(new OracleParameter { OracleDbType = OracleDbType.Long, Direction = ParameterDirection.Input, ParameterName = "P_IDALBUN", Value = interpretacion.IdAlbun });
                        cmd.Parameters.Add(new OracleParameter { OracleDbType = OracleDbType.Varchar2, Direction = ParameterDirection.Input, ParameterName = "P_NOMBRE", Value = interpretacion.Nombre });
                        cmd.Parameters.Add(new OracleParameter { OracleDbType = OracleDbType.Varchar2, Direction = ParameterDirection.Input, ParameterName = "P_GENERO", Value = interpretacion.Genero });
                        cmd.Parameters.Add(new OracleParameter { OracleDbType = OracleDbType.Varchar2, Direction = ParameterDirection.Input, ParameterName = "P_LETRA", Value = interpretacion.Letra });
                        cmd.Parameters.Add(new OracleParameter { OracleDbType = OracleDbType.Varchar2, Direction = ParameterDirection.Input, ParameterName = "P_LINK", Value = interpretacion.Link });
                        cmd.Parameters.Add(new OracleParameter { OracleDbType = OracleDbType.Long, Direction = ParameterDirection.Input, ParameterName = "P_IDCANCION", Value = interpretacion.IdCancion });

                        await cmd.ExecuteNonQueryAsync();

                        cmd.CommandText = @"
                                        select SEQUENCECANCION.currval from dual
                                        ";
                        await cmd.ExecuteNonQueryAsync();

                        adapter = new OracleDataAdapter(cmd);
                        data = new DataSet("Datos");
                        adapter.Fill(data);

                        

                        if (data.Tables[0].Rows.Count > 0)
                        {
                            foreach (DataRow item in data.Tables[0].Rows)
                            {
                                consecutivo = !Object.ReferenceEquals(System.DBNull.Value, item.ItemArray[0]) ? Convert.ToInt64(item.ItemArray[0]) : 0;
                            }
                        }
                    }
                    else
                    {
                        consecutivo = -3;
                    }
                    await OracleDBConnectionSingleton.OracleDBConnection.oracleConnection.CloseAsync();
                    return consecutivo;
                    
                }
                else
                {
                    return -2;
                }

            }
            catch (Exception ex)
            {
                await OracleDBConnectionSingleton.OracleDBConnection.oracleConnection.CloseAsync();
                Plugins.WriteExceptionLog(ex);
                return -1;
            }
        }


        public async Task<long> GuardarLikeInterpretacion(LikeModel like)
        {
            long resultado = 0;
            try
            {
                if (like.Id != null && like.IdUsuario != null && like.IdCancion != null)
                {

                    await OracleDBConnectionSingleton.OracleDBConnection.oracleConnection.OpenAsync();

                    cmd.CommandText = "SELECT ID, IDUSUARIO, IDALBUN, NOMBRE,GENERO,LETRA,FECHAPUBLICACION,LINK FROM INTERPRETACIONES WHERE ID=:P_ID AND ESTADO='A'";
                    cmd.Parameters.Clear();
                    cmd.Parameters.Add(new OracleParameter { OracleDbType = OracleDbType.Varchar2, Direction = ParameterDirection.Input, ParameterName = "P_ID", Value = like.IdCancion });
                    await cmd.ExecuteNonQueryAsync();

                    var adapter = new OracleDataAdapter(cmd);
                    var data = new DataSet("Datos");
                    adapter.Fill(data);


                    if (data.Tables[0].Rows.Count > 0)
                    {
                        cmd.CommandText = "SELECT ID FROM LIKES WHERE IDUSUARIO=:P_IDUSUARIO AND IDCANCION=:P_IDCANCION";
                        cmd.Parameters.Clear();
                        cmd.Parameters.Add(new OracleParameter { OracleDbType = OracleDbType.Varchar2, Direction = ParameterDirection.Input, ParameterName = "P_IDUSUARIO", Value = like.IdUsuario });
                        cmd.Parameters.Add(new OracleParameter { OracleDbType = OracleDbType.Varchar2, Direction = ParameterDirection.Input, ParameterName = "P_IDCANCION", Value = like.IdCancion });
                        await cmd.ExecuteNonQueryAsync();

                        adapter = new OracleDataAdapter(cmd);
                        data = new DataSet("Datos");
                        adapter.Fill(data);

                        if (data.Tables[0].Rows.Count == 0)
                        {
                            cmd.CommandText = @"
                                        INSERT INTO LIKES
                                        (ID, IDUSUARIO, IDCANCION)
                                        VALUES(SEQUENCELIKES.NEXTVAL,:P_IDUSUARIO,:P_IDCANCION)
                                        ";
                            cmd.Parameters.Clear();
                            cmd.Parameters.Add(new OracleParameter { OracleDbType = OracleDbType.Long, Direction = ParameterDirection.Input, ParameterName = "P_IDUSUARIO", Value = like.IdUsuario });
                            cmd.Parameters.Add(new OracleParameter { OracleDbType = OracleDbType.Long, Direction = ParameterDirection.Input, ParameterName = "P_IDCANCION", Value = like.IdCancion });
                            await cmd.ExecuteNonQueryAsync();

                            cmd.CommandText = @"
                                        DELETE FROM DISLIKES WHERE IDUSUARIO=:P_IDUSUARIO AND IDCANCION=:P_IDCANCION
                                        ";
                            cmd.Parameters.Clear();
                            cmd.Parameters.Add(new OracleParameter { OracleDbType = OracleDbType.Long, Direction = ParameterDirection.Input, ParameterName = "P_IDUSUARIO", Value = like.IdUsuario });
                            cmd.Parameters.Add(new OracleParameter { OracleDbType = OracleDbType.Long, Direction = ParameterDirection.Input, ParameterName = "P_IDCANCION", Value = like.IdCancion });
                            await cmd.ExecuteNonQueryAsync();

                            cmd.CommandText = @"
                                        select SEQUENCELIKES.currval from dual
                                        ";
                            await cmd.ExecuteNonQueryAsync();

                            adapter = new OracleDataAdapter(cmd);
                            data = new DataSet("Datos");
                            adapter.Fill(data);

                            await OracleDBConnectionSingleton.OracleDBConnection.oracleConnection.CloseAsync();

                            if (data.Tables[0].Rows.Count > 0)
                            {
                                foreach (DataRow item in data.Tables[0].Rows)
                                {
                                    resultado = !Object.ReferenceEquals(System.DBNull.Value, item.ItemArray[0]) ? Convert.ToInt64(item.ItemArray[0]) : 0;
                                }
                            }
                        }
                        else
                        {
                            cmd.CommandText = @"
                                        DELETE FROM LIKES WHERE IDUSUARIO=:P_IDUSUARIO AND IDCANCION=:P_IDCANCION
                                        ";
                            cmd.Parameters.Clear();
                            cmd.Parameters.Add(new OracleParameter { OracleDbType = OracleDbType.Long, Direction = ParameterDirection.Input, ParameterName = "P_IDUSUARIO", Value = like.IdUsuario });
                            cmd.Parameters.Add(new OracleParameter { OracleDbType = OracleDbType.Long, Direction = ParameterDirection.Input, ParameterName = "P_IDCANCION", Value = like.IdCancion });
                            await cmd.ExecuteNonQueryAsync();
                            resultado = -4;
                        }

                    }
                    else
                    {
                        resultado = -3;
                    }
                }
                else
                {
                    resultado = -2;
                }

            }
            catch (Exception ex)
            {
                await OracleDBConnectionSingleton.OracleDBConnection.oracleConnection.CloseAsync();
                Plugins.WriteExceptionLog(ex);
                return -1;
            }
            await OracleDBConnectionSingleton.OracleDBConnection.oracleConnection.CloseAsync();
            return resultado;
        }




        public async Task<long> GuardarDisLikeInterpretacion(DisLikeModel disLike)
        {
            long resultado = 0;
            try
            {
                if (disLike.Id != null && disLike.IdUsuario != null && disLike.IdCancion != null)
                {

                    await OracleDBConnectionSingleton.OracleDBConnection.oracleConnection.OpenAsync();

                    cmd.CommandText = "SELECT ID, IDUSUARIO, IDALBUN, NOMBRE,GENERO,LETRA,FECHAPUBLICACION,LINK FROM INTERPRETACIONES WHERE ID=:P_ID AND ESTADO='A'";
                    cmd.Parameters.Clear();
                    cmd.Parameters.Add(new OracleParameter { OracleDbType = OracleDbType.Varchar2, Direction = ParameterDirection.Input, ParameterName = "P_ID", Value = disLike.IdCancion });
                    await cmd.ExecuteNonQueryAsync();

                    var adapter = new OracleDataAdapter(cmd);
                    var data = new DataSet("Datos");
                    adapter.Fill(data);


                    if (data.Tables[0].Rows.Count > 0)
                    {
                        cmd.CommandText = "SELECT ID FROM DISLIKES WHERE IDUSUARIO=:P_IDUSUARIO AND IDCANCION=:P_IDCANCION";
                        cmd.Parameters.Clear();
                        cmd.Parameters.Add(new OracleParameter { OracleDbType = OracleDbType.Varchar2, Direction = ParameterDirection.Input, ParameterName = "P_IDUSUARIO", Value = disLike.IdUsuario });
                        cmd.Parameters.Add(new OracleParameter { OracleDbType = OracleDbType.Varchar2, Direction = ParameterDirection.Input, ParameterName = "P_IDCANCION", Value = disLike.IdCancion });
                        await cmd.ExecuteNonQueryAsync();

                        adapter = new OracleDataAdapter(cmd);
                        data = new DataSet("Datos");
                        adapter.Fill(data);

                        if (data.Tables[0].Rows.Count == 0)
                        {
                            cmd.CommandText = @"
                                        INSERT INTO DISLIKES
                                        (ID, IDUSUARIO, IDCANCION)
                                        VALUES(SEQUENCEDISLIKES.NEXTVAL,:P_IDUSUARIO,:P_IDCANCION)
                                        ";
                            cmd.Parameters.Clear();
                            cmd.Parameters.Add(new OracleParameter { OracleDbType = OracleDbType.Long, Direction = ParameterDirection.Input, ParameterName = "P_IDUSUARIO", Value = disLike.IdUsuario });
                            cmd.Parameters.Add(new OracleParameter { OracleDbType = OracleDbType.Long, Direction = ParameterDirection.Input, ParameterName = "P_IDCANCION", Value = disLike.IdCancion });
                            await cmd.ExecuteNonQueryAsync();

                            cmd.CommandText = @"
                                        DELETE FROM LIKES WHERE IDUSUARIO=:P_IDUSUARIO AND IDCANCION=:P_IDCANCION
                                        ";
                            cmd.Parameters.Clear();
                            cmd.Parameters.Add(new OracleParameter { OracleDbType = OracleDbType.Long, Direction = ParameterDirection.Input, ParameterName = "P_IDUSUARIO", Value = disLike.IdUsuario });
                            cmd.Parameters.Add(new OracleParameter { OracleDbType = OracleDbType.Long, Direction = ParameterDirection.Input, ParameterName = "P_IDCANCION", Value = disLike.IdCancion });
                            await cmd.ExecuteNonQueryAsync();

                            cmd.CommandText = @"
                                        select SEQUENCEDISLIKES.currval from dual
                                        ";
                            await cmd.ExecuteNonQueryAsync();

                            adapter = new OracleDataAdapter(cmd);
                            data = new DataSet("Datos");
                            adapter.Fill(data);

                            await OracleDBConnectionSingleton.OracleDBConnection.oracleConnection.CloseAsync();

                            if (data.Tables[0].Rows.Count > 0)
                            {
                                foreach (DataRow item in data.Tables[0].Rows)
                                {
                                    resultado = !Object.ReferenceEquals(System.DBNull.Value, item.ItemArray[0]) ? Convert.ToInt64(item.ItemArray[0]) : 0;
                                }
                            }
                        }
                        else
                        {
                            cmd.CommandText = @"
                                        DELETE FROM DISLIKES WHERE IDUSUARIO=:P_IDUSUARIO AND IDCANCION=:P_IDCANCION
                                        ";
                            cmd.Parameters.Clear();
                            cmd.Parameters.Add(new OracleParameter { OracleDbType = OracleDbType.Long, Direction = ParameterDirection.Input, ParameterName = "P_IDUSUARIO", Value = disLike.IdUsuario });
                            cmd.Parameters.Add(new OracleParameter { OracleDbType = OracleDbType.Long, Direction = ParameterDirection.Input, ParameterName = "P_IDCANCION", Value = disLike.IdCancion });
                            await cmd.ExecuteNonQueryAsync();
                            resultado = -4;
                        }

                    }
                    else
                    {
                        resultado = -3;
                    }
                }
                else
                {
                    resultado = -2;
                }

            }
            catch (Exception ex)
            {
                await OracleDBConnectionSingleton.OracleDBConnection.oracleConnection.CloseAsync();
                Plugins.WriteExceptionLog(ex);
                return -1;
            }
            await OracleDBConnectionSingleton.OracleDBConnection.oracleConnection.CloseAsync();
            return resultado;
        }


        public async Task<long> GuardarComentarioInterpretacion(ComentarioModel comentario)
        {
            long consecutivo = 0;
            try
            {
                if (comentario.Id != null && comentario.IdUsuario != null && comentario.IdCancion != null)
                {
                    comentario.Mensaje.Replace("\n", "-");
                    await OracleDBConnectionSingleton.OracleDBConnection.oracleConnection.OpenAsync();

                    cmd.CommandText = "SELECT ID, IDUSUARIO, IDALBUN, NOMBRE,GENERO,LETRA,FECHAPUBLICACION,LINK FROM INTERPRETACIONES WHERE ID=:P_ID AND ESTADO='A'";
                    cmd.Parameters.Clear();
                    cmd.Parameters.Add(new OracleParameter { OracleDbType = OracleDbType.Varchar2, Direction = ParameterDirection.Input, ParameterName = "P_ID", Value = comentario.IdCancion });
                    await cmd.ExecuteNonQueryAsync();

                    var adapter = new OracleDataAdapter(cmd);
                    var data = new DataSet("Datos");
                    adapter.Fill(data);


                    if (data.Tables[0].Rows.Count > 0)
                    {
                        cmd.CommandText = @"
                                        INSERT INTO COMENTARIOS
                                        (ID, IDUSUARIO, IDCANCION,MENSAJE,FECHA,NOMBREUSUARIO)
                                        VALUES(SEQUENCECOMENTARIOS.NEXTVAL,:P_IDUSUARIO,:P_IDCANCION,:P_MENSAJE,CURRENT_DATE,:P_NOMBREUSUARIO)
                                        ";
                        cmd.Parameters.Clear();
                        cmd.Parameters.Add(new OracleParameter { OracleDbType = OracleDbType.Long, Direction = ParameterDirection.Input, ParameterName = "P_IDUSUARIO", Value = comentario.IdUsuario });
                        cmd.Parameters.Add(new OracleParameter { OracleDbType = OracleDbType.Long, Direction = ParameterDirection.Input, ParameterName = "P_IDCANCION", Value = comentario.IdCancion });
                        cmd.Parameters.Add(new OracleParameter { OracleDbType = OracleDbType.Varchar2, Direction = ParameterDirection.Input, ParameterName = "P_MENSAJE", Value = comentario.Mensaje });
                        cmd.Parameters.Add(new OracleParameter { OracleDbType = OracleDbType.Varchar2, Direction = ParameterDirection.Input, ParameterName = "P_NOMBREUSUARIO", Value = comentario.NombreUsuario });
                        await cmd.ExecuteNonQueryAsync();

                        cmd.CommandText = @"
                                        select SEQUENCECOMENTARIOS.currval from dual
                                        ";
                        await cmd.ExecuteNonQueryAsync();

                        adapter = new OracleDataAdapter(cmd);
                        data = new DataSet("Datos");
                        adapter.Fill(data);

                        await OracleDBConnectionSingleton.OracleDBConnection.oracleConnection.CloseAsync();

                        if (data.Tables[0].Rows.Count > 0)
                        {
                            foreach (DataRow item in data.Tables[0].Rows)
                            {
                                consecutivo = !Object.ReferenceEquals(System.DBNull.Value, item.ItemArray[0]) ? Convert.ToInt64(item.ItemArray[0]) : 0;
                            }
                        }
                        return consecutivo;
                    }
                    else
                    {
                        return -3;
                    }
                }
                else
                {
                    return -2;
                }

            }
            catch (Exception ex)
            {
                await OracleDBConnectionSingleton.OracleDBConnection.oracleConnection.CloseAsync();
                Plugins.WriteExceptionLog(ex);
                return -1;
            }
        }



        public async Task<InterpretacionModel> ConsultarInterpretacion(int id)
        {
            var _outs = new List<InterpretacionModel>();
            try
            {
                await OracleDBConnectionSingleton.OracleDBConnection.oracleConnection.OpenAsync();

                cmd.CommandText = "SELECT ID, IDUSUARIO, IDALBUN, NOMBRE,GENERO,LETRA,FECHAPUBLICACION,LINK,IDCANCION FROM INTERPRETACIONES WHERE ID=:P_ID AND ESTADO='A'";
                cmd.Parameters.Clear();
                cmd.Parameters.Add(new OracleParameter { OracleDbType = OracleDbType.Varchar2, Direction = ParameterDirection.Input, ParameterName = "P_ID", Value = id });
                await cmd.ExecuteNonQueryAsync();

                var adapter = new OracleDataAdapter(cmd);
                var data = new DataSet("Datos");
                adapter.Fill(data);

                await OracleDBConnectionSingleton.OracleDBConnection.oracleConnection.CloseAsync();

                if (data.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow item in data.Tables[0].Rows)
                    {
                        InterpretacionModel interpretacionModel = new InterpretacionModel
                        {
                            Id = !Object.ReferenceEquals(System.DBNull.Value, item.ItemArray[0]) ? Convert.ToInt64(item.ItemArray[0]) : 0,
                            IdUsuario = !Object.ReferenceEquals(System.DBNull.Value, item.ItemArray[1]) ? Convert.ToInt64(item.ItemArray[1]) : 0,
                            IdAlbun = !Object.ReferenceEquals(System.DBNull.Value, item.ItemArray[2]) ? Convert.ToInt64(item.ItemArray[2]) : 0,
                            Nombre = !Object.ReferenceEquals(System.DBNull.Value, item.ItemArray[3]) ? Convert.ToString(item.ItemArray[3]) : "SIN NOMBRE",
                            Genero = !Object.ReferenceEquals(System.DBNull.Value, item.ItemArray[4]) ? Convert.ToString(item.ItemArray[4]) : "SIN GENERO",
                            Letra = !Object.ReferenceEquals(System.DBNull.Value, item.ItemArray[5]) ? Convert.ToString(item.ItemArray[5]).Replace("-", "\n") : "SIN LETRA",
                            FechaPublicacion = !Object.ReferenceEquals(System.DBNull.Value, item.ItemArray[6]) ? Convert.ToDateTime(item.ItemArray[6]) : null,
                            Link = !Object.ReferenceEquals(System.DBNull.Value, item.ItemArray[7]) ? Convert.ToString(item.ItemArray[7]) : "SIN LINK",
                            IdCancion = !Object.ReferenceEquals(System.DBNull.Value, item.ItemArray[8]) ? Convert.ToInt64(item.ItemArray[8]) : 0
                        };

                        _outs.Add(interpretacionModel);
                    }
                }
                else
                {
                    _outs.Add(null);
                }

                return _outs[0];
            }
            catch (Exception ex)
            {
                Plugins.WriteExceptionLog(ex);
                await OracleDBConnectionSingleton.OracleDBConnection.oracleConnection.CloseAsync();
            }
            return null;
        }

        public async Task<InterpretacionCompletaDTO> ConsultarInterpretacionCompleta(int id)
        {
            try
            {
                InterpretacionCompletaDTO interpretacionCompletaDTO = new InterpretacionCompletaDTO();
                InterpretacionModel cancion = ConsultarInterpretacion(id).Result;
                if (cancion != null)
                {
                    List<ComentarioModel> comentarios = ConsultarComentarioPorCancion(id).Result;
                    long numeroLikes = ConsultarNumeroMegustaPorCancion(id).Result;
                    long numeroDisLikes = ConsultarNumeroNoMegustaPorCancion(id).Result;
                    if (numeroLikes < 0) { numeroLikes = 0; }
                    if (numeroDisLikes < 0) { numeroDisLikes = 0; }
                    interpretacionCompletaDTO.NumeroDisLikes = numeroDisLikes;
                    interpretacionCompletaDTO.NumeroLikes = numeroLikes;
                    interpretacionCompletaDTO.MapearInterpretacion(cancion);
                    interpretacionCompletaDTO.Comentarios = comentarios;
                    return interpretacionCompletaDTO;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        public async Task<long> DesactivarInterpretacion(int id)
        {
            var _outs = new List<InterpretacionModel>();
            try
            {
                await OracleDBConnectionSingleton.OracleDBConnection.oracleConnection.OpenAsync();

                cmd.CommandText = "SELECT ID, IDUSUARIO, IDALBUN, NOMBRE,GENERO,LETRA,FECHAPUBLICACION,LINK FROM INTERPRETACIONES WHERE ID=:P_ID AND ESTADO='A'";
                cmd.Parameters.Clear();
                cmd.Parameters.Add(new OracleParameter { OracleDbType = OracleDbType.Varchar2, Direction = ParameterDirection.Input, ParameterName = "P_ID", Value = id });
                await cmd.ExecuteNonQueryAsync();

                var adapter = new OracleDataAdapter(cmd);
                var data = new DataSet("Datos");
                adapter.Fill(data);

                await OracleDBConnectionSingleton.OracleDBConnection.oracleConnection.CloseAsync();

                if (data.Tables[0].Rows.Count > 0)
                {
                    await OracleDBConnectionSingleton.OracleDBConnection.oracleConnection.OpenAsync();
                    cmd.CommandText = @"
                                        UPDATE INTERPRETACIONES SET
                                        ESTADO='I'
                                        WHERE ID=:P_ID
                                        ";
                    cmd.Parameters.Clear();
                    cmd.Parameters.Add(new OracleParameter { OracleDbType = OracleDbType.Varchar2, Direction = ParameterDirection.Input, ParameterName = "P_ID", Value = id });
                    await cmd.ExecuteNonQueryAsync();
                    await OracleDBConnectionSingleton.OracleDBConnection.oracleConnection.CloseAsync();
                    return 1;
                }
                else
                {
                    return 0;
                }


            }
            catch (Exception ex)
            {
                await OracleDBConnectionSingleton.OracleDBConnection.oracleConnection.CloseAsync();
                Plugins.WriteExceptionLog(ex);
            }
            return -1;
        }


        public async Task<long> ModificarInterpretacion(InterpretacionModel interpretacion)
        {
            try
            {

                if (interpretacion.Nombre != null && interpretacion.Letra != null)
                {
                    await OracleDBConnectionSingleton.OracleDBConnection.oracleConnection.OpenAsync();

                    cmd.CommandText = @"
                                        SELECT ID, IDUSUARIO, IDALBUN, NOMBRE,GENERO,LETRA,FECHAPUBLICACION,LINK FROM INTERPRETACIONES WHERE ID=:P_IDB AND ESTADO='A'
                                        ";
                    cmd.Parameters.Clear();
                    cmd.Parameters.Add(new OracleParameter { OracleDbType = OracleDbType.Varchar2, Direction = ParameterDirection.Input, ParameterName = "P_IDB", Value = interpretacion.Id });
                    await cmd.ExecuteNonQueryAsync();

                    var adapter = new OracleDataAdapter(cmd);
                    var data = new DataSet("Datos");
                    adapter.Fill(data);



                    if (data.Tables[0].Rows.Count == 1)
                    {

                        cmd.CommandText = @"
                                        UPDATE DBMUSICFTMUSIC.INTERPRETACIONES SET
                                        IDALBUN=:P_IDALBUN,
                                        NOMBRE=:P_NOMBRE,
                                        GENERO=:P_GENERO,
                                        LETRA=:P_LETRA,
                                        LINK=:P_LINK
                                        WHERE ID = :P_ID AND ESTADO ='A'
                                        ";
                        cmd.Parameters.Clear();
                        
                        cmd.Parameters.Add(new OracleParameter { OracleDbType = OracleDbType.Long, Direction = ParameterDirection.Input, ParameterName = "P_IDALBUN", Value = interpretacion.IdAlbun });
                        cmd.Parameters.Add(new OracleParameter { OracleDbType = OracleDbType.Varchar2, Direction = ParameterDirection.Input, ParameterName = "P_NOMBRE", Value = interpretacion.Nombre });
                        cmd.Parameters.Add(new OracleParameter { OracleDbType = OracleDbType.Varchar2, Direction = ParameterDirection.Input, ParameterName = "P_GENERO", Value = interpretacion.Genero });
                        cmd.Parameters.Add(new OracleParameter { OracleDbType = OracleDbType.Varchar2, Direction = ParameterDirection.Input, ParameterName = "P_LETRA", Value = interpretacion.Letra });
                        cmd.Parameters.Add(new OracleParameter { OracleDbType = OracleDbType.Varchar2, Direction = ParameterDirection.Input, ParameterName = "P_LINK", Value = interpretacion.Link });
                        cmd.Parameters.Add(new OracleParameter { OracleDbType = OracleDbType.Long, Direction = ParameterDirection.Input, ParameterName = "P_ID", Value = interpretacion.Id });
                        await cmd.ExecuteNonQueryAsync();
                        await OracleDBConnectionSingleton.OracleDBConnection.oracleConnection.CloseAsync();

                    }
                    else
                    {
                        return 0;//no existe
                    }
                    await OracleDBConnectionSingleton.OracleDBConnection.oracleConnection.CloseAsync();


                    return 1;//todo bien
                }
                else
                {
                    return -2;//campos vacios
                }

            }
            catch (Exception ex)
            {
                await OracleDBConnectionSingleton.OracleDBConnection.oracleConnection.CloseAsync();
                Plugins.WriteExceptionLog(ex);
                return -1;//error
            }
        }

        public async Task<long> ValidarLikeYDislikePorUsuario(int IdUsuario, int IdCancion) 
        {
            long salida = -1;//campos vacios
            if (IdUsuario != null && IdCancion != null)
            {
                try
                {
                    await OracleDBConnectionSingleton.OracleDBConnection.oracleConnection.OpenAsync();
                    cmd.CommandText = @"
                                        SELECT ID FROM LIKES WHERE IDCANCION=:P_ID AND IDUSUARIO=:P_IDUSUARIO
                                        ";
                    cmd.Parameters.Clear();
                    cmd.Parameters.Add(new OracleParameter { OracleDbType = OracleDbType.Varchar2, Direction = ParameterDirection.Input, ParameterName = "IDCANCION", Value = IdCancion });
                    cmd.Parameters.Add(new OracleParameter { OracleDbType = OracleDbType.Varchar2, Direction = ParameterDirection.Input, ParameterName = "IDUSUARIO", Value = IdUsuario });
                    await cmd.ExecuteNonQueryAsync();
                    
                    var adapter = new OracleDataAdapter(cmd);
                    var data = new DataSet("Datos");
                    adapter.Fill(data);

                    if (data.Tables[0].Rows.Count == 1)
                    {
                        salida = 1;//Like
                    }
                    else
                    {
                        cmd.CommandText = @"
                                        SELECT ID FROM DISLIKES WHERE IDCANCION=:P_ID AND IDUSUARIO=:P_IDUSUARIO
                                        ";
                        cmd.Parameters.Clear();
                        cmd.Parameters.Add(new OracleParameter { OracleDbType = OracleDbType.Varchar2, Direction = ParameterDirection.Input, ParameterName = "IDCANCION", Value = IdCancion });
                        cmd.Parameters.Add(new OracleParameter { OracleDbType = OracleDbType.Varchar2, Direction = ParameterDirection.Input, ParameterName = "IDUSUARIO", Value = IdUsuario });
                        await cmd.ExecuteNonQueryAsync();

                        adapter = new OracleDataAdapter(cmd);
                        data = new DataSet("Datos");
                        adapter.Fill(data);
                        if (data.Tables[0].Rows.Count == 1)
                        {
                            salida = 2;//Dislike
                        }
                        else
                        {
                            salida = 0;//nada
                        }
                    }
                }
                catch(Exception ex)
                {
                    salida = -2;//Error
                }
                await OracleDBConnectionSingleton.OracleDBConnection.oracleConnection.CloseAsync();
            }
                return salida;//campos vacios
        }











        public async Task<long> DesactivarComentario(int id)
        {
            var _outs = new List<ComentarioModel>();
            try
            {
                await OracleDBConnectionSingleton.OracleDBConnection.oracleConnection.OpenAsync();

                cmd.CommandText = "SELECT ID FROM COMENTARIOS WHERE ID=:P_ID AND ESTADO='A'";
                cmd.Parameters.Clear();
                cmd.Parameters.Add(new OracleParameter { OracleDbType = OracleDbType.Varchar2, Direction = ParameterDirection.Input, ParameterName = "P_ID", Value = id });
                await cmd.ExecuteNonQueryAsync();

                var adapter = new OracleDataAdapter(cmd);
                var data = new DataSet("Datos");
                adapter.Fill(data);

                await OracleDBConnectionSingleton.OracleDBConnection.oracleConnection.CloseAsync();

                if (data.Tables[0].Rows.Count > 0)
                {
                    await OracleDBConnectionSingleton.OracleDBConnection.oracleConnection.OpenAsync();
                    cmd.CommandText = @"
                                        UPDATE COMENTARIOS SET
                                        ESTADO='I'
                                        WHERE ID=:P_ID
                                        ";
                    cmd.Parameters.Clear();
                    cmd.Parameters.Add(new OracleParameter { OracleDbType = OracleDbType.Varchar2, Direction = ParameterDirection.Input, ParameterName = "P_ID", Value = id });
                    await cmd.ExecuteNonQueryAsync();
                    await OracleDBConnectionSingleton.OracleDBConnection.oracleConnection.CloseAsync();
                    return 1;
                }
                else
                {
                    return 0;
                }


            }
            catch (Exception ex)
            {
                await OracleDBConnectionSingleton.OracleDBConnection.oracleConnection.CloseAsync();
                Plugins.WriteExceptionLog(ex);
            }
            return -1;
        }







        public async Task<long> ModificarComentario(ComentarioModel comentario)
        {
            try
            {

                if (comentario.IdCancion != null && comentario.IdUsuario != null)
                {
                    await OracleDBConnectionSingleton.OracleDBConnection.oracleConnection.OpenAsync();

                    cmd.CommandText = @"
                                        SELECT ID FROM COMENTARIOS WHERE ID=:P_IDB AND ESTADO='A'
                                        ";
                    cmd.Parameters.Clear();
                    cmd.Parameters.Add(new OracleParameter { OracleDbType = OracleDbType.Varchar2, Direction = ParameterDirection.Input, ParameterName = "P_IDB", Value = comentario.Id });
                    await cmd.ExecuteNonQueryAsync();

                    var adapter = new OracleDataAdapter(cmd);
                    var data = new DataSet("Datos");
                    adapter.Fill(data);



                    if (data.Tables[0].Rows.Count == 1)
                    {

                        cmd.CommandText = @"
                                        UPDATE DBMUSICFTMUSIC.COMENTARIOS SET
                                        MENSAJE=:P_MENSAJE,
                                        FECHA=:P_FECHA
                                        WHERE ID = :P_ID AND ESTADO ='A'
                        loc                ";
                        cmd.Parameters.Clear();

                        cmd.Parameters.Add(new OracleParameter { OracleDbType = OracleDbType.Long, Direction = ParameterDirection.Input, ParameterName = "P_MENSAJE", Value = comentario.Mensaje });
                        cmd.Parameters.Add(new OracleParameter { OracleDbType = OracleDbType.Date, Direction = ParameterDirection.Input, ParameterName = "P_FECHA", Value = comentario.Fecha });
                        cmd.Parameters.Add(new OracleParameter { OracleDbType = OracleDbType.Long, Direction = ParameterDirection.Input, ParameterName = "P_ID", Value = comentario.Id });
                        await cmd.ExecuteNonQueryAsync();
                        await OracleDBConnectionSingleton.OracleDBConnection.oracleConnection.CloseAsync();

                    }
                    else
                    {
                        return 0;//no existe
                    }
                    await OracleDBConnectionSingleton.OracleDBConnection.oracleConnection.CloseAsync();


                    return 1;//todo bien
                }
                else
                {
                    return -2;//campos vacios
                }

            }
            catch (Exception ex)
            {
                await OracleDBConnectionSingleton.OracleDBConnection.oracleConnection.CloseAsync();
                Plugins.WriteExceptionLog(ex);
                return -1;//error
            }
        }






    }
}
