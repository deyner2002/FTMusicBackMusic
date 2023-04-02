using CORE.Loyal.DBConnection;
using Microsoft.Extensions.Options;
using Oracle.ManagedDataAccess.Client;
using Support.Loyal.DTOs;
using Support.Loyal.Util;
using System.Data;
using CORE.Loyal.Interfaces.Providers;
using Core.Loyal.Models.FTMUSIC;

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
        public async Task<List<CancionModel>> GetList()
        {
            var _outs = new List<CancionModel>();
            try
            {
                await OracleDBConnectionSingleton.OracleDBConnection.oracleConnection.OpenAsync();
                
                cmd.CommandText = "SELECT ID, IDUSUARIO, IDALBUN, NOMBRE,GENERO,LETRA,FECHAPUBLICACION,NUMEROMEGUSTA,LINK FROM CANCIONES WHERE ESTADO='A'";
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
                            NumeroMegusta = !Object.ReferenceEquals(System.DBNull.Value, item.ItemArray[7]) ? Convert.ToInt64(item.ItemArray[7]) : 0,
                            Link = !Object.ReferenceEquals(System.DBNull.Value, item.ItemArray[8]) ? Convert.ToString(item.ItemArray[8]) : "SIN LINK"
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
                                        (ID, IDUSUARIO, IDALBUN, NOMBRE,GENERO,LETRA,FECHAPUBLICACION,NUMEROMEGUSTA,LINK,ESTADO)
                                        VALUES(SEQUENCECANCION.NEXTVAL,:P_IDUSUARIO,:P_IDALBUN ,:P_NOMBRE, :P_GENERO, :P_LETRA,CURRENT_DATE,0,:P_LINK,'A')
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

                cmd.CommandText = "SELECT ID, IDUSUARIO, IDALBUN, NOMBRE,GENERO,LETRA,FECHAPUBLICACION,NUMEROMEGUSTA,LINK FROM CANCIONES WHERE ID=:P_ID AND ESTADO='A'";
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
                            NumeroMegusta = !Object.ReferenceEquals(System.DBNull.Value, item.ItemArray[7]) ? Convert.ToInt64(item.ItemArray[7]) : 0,
                            Link = !Object.ReferenceEquals(System.DBNull.Value, item.ItemArray[8]) ? Convert.ToString(item.ItemArray[8]) : "SIN LINK"
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

                cmd.CommandText = "SELECT ID, IDUSUARIO, IDALBUN, NOMBRE,GENERO,LETRA,FECHAPUBLICACION,NUMEROMEGUSTA,LINK FROM CANCIONES WHERE ID=:P_ID AND ESTADO='A'";
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
                                        SELECT ID, IDUSUARIO, IDALBUN, NOMBRE,GENERO,LETRA,FECHAPUBLICACION,NUMEROMEGUSTA,LINK FROM CANCIONES WHERE ID=:P_IDB AND ESTADO='A'
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
                                        IDUSUARIO=:P_IDUSUARIO,
                                        IDALBUN=:P_IDALBUN,
                                        NOMBRE=:P_NOMBRE,
                                        GENERO=:P_GENERO,
                                        LETRA=:P_LETRA,
                                        LINK=:P_LINK
                                        WHERE ID = :P_ID AND ESTADO ='A'
                                        ";
                        cmd.Parameters.Clear();
                        cmd.Parameters.Add(new OracleParameter { OracleDbType = OracleDbType.Long, Direction = ParameterDirection.Input, ParameterName = "P_IDUSUARIO", Value = cancion.IdUsuario });
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

                cmd.CommandText = "SELECT ID, IDUSUARIO, IDALBUN, NOMBRE,GENERO,LETRA,FECHAPUBLICACION,NUMEROMEGUSTA,LINK FROM CANCIONES WHERE ESTADO='A' AND IDUSUARIO=:P_IDUSUARIO";
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
                            NumeroMegusta = !Object.ReferenceEquals(System.DBNull.Value, item.ItemArray[7]) ? Convert.ToInt64(item.ItemArray[7]) : 0,
                            Link = !Object.ReferenceEquals(System.DBNull.Value, item.ItemArray[8]) ? Convert.ToString(item.ItemArray[8]) : "SIN LINK"
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

                cmd.CommandText = "SELECT ID, IDUSUARIO, IDALBUN, NOMBRE,GENERO,LETRA,FECHAPUBLICACION,NUMEROMEGUSTA,LINK FROM CANCIONES WHERE ESTADO='A' AND NOMBRE LIKE :P_NOMBRE";
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
                            NumeroMegusta = !Object.ReferenceEquals(System.DBNull.Value, item.ItemArray[7]) ? Convert.ToInt64(item.ItemArray[7]) : 0,
                            Link = !Object.ReferenceEquals(System.DBNull.Value, item.ItemArray[8]) ? Convert.ToString(item.ItemArray[8]) : "SIN LINK"
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







    }
}
