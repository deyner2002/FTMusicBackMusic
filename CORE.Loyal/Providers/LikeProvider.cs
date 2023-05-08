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
    public class LikeProvider : ILikeProvider
    {
        public ConnectionStrings _ConnectionStrings { get; set; }
        private OracleCommand cmd { get; set; }
        public LikeProvider(IOptions<ConnectionStrings> ConnectionStrings)
        {
            _ConnectionStrings = ConnectionStrings.Value;
            cmd = new OracleCommand();
            cmd.Connection = OracleDBConnectionSingleton.OracleDBConnection.oracleConnection;
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

    }
}
