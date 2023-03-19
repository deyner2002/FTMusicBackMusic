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
                
                cmd.CommandText = "SELECT CONSECUTIVO, NOMBRE, CONSECUTIVOUSUARIO, LETRA FROM TCANCION";
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
                            CONSECUTIVO = !Object.ReferenceEquals(System.DBNull.Value, item.ItemArray[0]) ? Convert.ToInt64(item.ItemArray[0]) : 0,
                            NOMBRE = !Object.ReferenceEquals(System.DBNull.Value, item.ItemArray[1]) ? Convert.ToString(item.ItemArray[1]) : "SIN NOMBRE",
                            CONSECUTIVOUSUARIO = !Object.ReferenceEquals(System.DBNull.Value, item.ItemArray[2]) ? Convert.ToInt64(item.ItemArray[2]) : 0,
                            LETRA = !Object.ReferenceEquals(System.DBNull.Value, item.ItemArray[3]) ? Convert.ToString(item.ItemArray[3]) : "SIN LETRA"
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
                if (cancion.NOMBRE != null && cancion.LETRA != null && cancion.CONSECUTIVOUSUARIO != 0)
                {
                    await OracleDBConnectionSingleton.OracleDBConnection.oracleConnection.OpenAsync();
                    cmd.CommandText = @"
                                        INSERT INTO TCANCION
                                        (CONSECUTIVO, NOMBRE, CONSECUTIVOUSUARIO, LETRA)
                                        VALUES(SEQUENCECANCION.NEXTVAL, :P_NOMBRE, :P_CONSECUTIVOUSUARIO, :P_LETRA)
                                        ";
                    cmd.Parameters.Clear();
                    cmd.Parameters.Add(new OracleParameter { OracleDbType = OracleDbType.Varchar2, Direction = ParameterDirection.Input, ParameterName = "P_NOMBRE", Value = cancion.NOMBRE });
                    cmd.Parameters.Add(new OracleParameter { OracleDbType = OracleDbType.Long, Direction = ParameterDirection.Input, ParameterName = "P_CONSECUTIVOUSUARIO", Value = cancion.CONSECUTIVOUSUARIO });
                    cmd.Parameters.Add(new OracleParameter { OracleDbType = OracleDbType.Varchar2, Direction = ParameterDirection.Input, ParameterName = "P_LETRA", Value = cancion.LETRA });

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
                Plugins.WriteExceptionLog(ex);
                return -1;
            }
        }
    }
}
