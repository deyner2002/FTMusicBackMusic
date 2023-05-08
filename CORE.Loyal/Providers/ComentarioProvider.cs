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
    public class ComentarioProvider : IComentarioProvider
    {
        public ConnectionStrings _ConnectionStrings { get; set; }
        private OracleCommand cmd { get; set; }
        public ComentarioProvider(IOptions<ConnectionStrings> ConnectionStrings)
        {
            _ConnectionStrings = ConnectionStrings.Value;
            cmd = new OracleCommand();
            cmd.Connection = OracleDBConnectionSingleton.OracleDBConnection.oracleConnection;
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
                                        ";
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
