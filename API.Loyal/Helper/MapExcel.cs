using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace API.Loyal.Helper
{
    public class MapExcel
    {
        //public List<ClienteModel> LeerClientesExcel(IFormFile archivoExcel) 
        //{
        //    Stream stream = archivoExcel.OpenReadStream();
        //    IWorkbook excel = null;

        //    if (Path.GetExtension(archivoExcel.FileName) == ".xlsx")
        //    {
        //        excel = new XSSFWorkbook(stream);
        //    }
        //    else
        //    {
        //        excel = new HSSFWorkbook(stream);
        //    }

        //    ISheet hojaExcel = excel.GetSheetAt(0);

        //    int cantidadFilas = hojaExcel.LastRowNum;

        //    List<ClienteModel> clientes = new List<ClienteModel>();
        //    System.IFormatProvider cultureUS = new System.Globalization.CultureInfo("en-US");
        //    for (int i = 1; i <= cantidadFilas; i++)
        //    {
        //        IRow fila = hojaExcel.GetRow(i);
        //        if (fila != null && fila.PhysicalNumberOfCells == 11)
        //            clientes.Add(new ClienteModel
        //            {
        //                NIC = (fila.GetCell(0) == null || fila.GetCell(0).CellType == CellType.Blank) ? 0 : long.Parse(fila.GetCell(0).ToString()),
        //                NIS = (fila.GetCell(1) == null || fila.GetCell(1).CellType == CellType.Blank) ? 0 : long.Parse(fila.GetCell(1).ToString()),
        //                CEDULA = (fila.GetCell(2) == null || fila.GetCell(2).CellType == CellType.Blank) ? 0 : long.Parse(fila.GetCell(2).ToString()),
        //                NOMBRE = (fila.GetCell(3) == null || fila.GetCell(3).CellType == CellType.Blank) ? null : fila.GetCell(3).ToString(),
        //                APELLIDO = (fila.GetCell(4) == null || fila.GetCell(4).CellType == CellType.Blank) ? null : fila.GetCell(4).ToString(),
        //                DIRECCION = (fila.GetCell(5) == null || fila.GetCell(5).CellType == CellType.Blank) ? null : fila.GetCell(5).ToString(),
        //                TELEFONO = (fila.GetCell(6) == null || fila.GetCell(6).CellType == CellType.Blank) ? 0 : long.Parse(fila.GetCell(6).ToString()),
        //                LATITUD = (fila.GetCell(7) == null || fila.GetCell(7).CellType == CellType.Blank) ? 0 : double.Parse(fila.GetCell(7).ToString(), cultureUS),
        //                LONGITUD = (fila.GetCell(8) == null || fila.GetCell(8).CellType == CellType.Blank) ? 0 : double.Parse(fila.GetCell(8).ToString(), cultureUS),
        //                OBSERVACION = (fila.GetCell(9) == null || fila.GetCell(9).CellType == CellType.Blank) == null ? null : fila.GetCell(9).ToString(),
        //                NOMBRE_LOTE = (fila.GetCell(10) == null || fila.GetCell(10).CellType == CellType.Blank) == null ? null : fila.GetCell(10).ToString()
        //            });
        //    }
        //    return clientes;
        //}
    }
}
