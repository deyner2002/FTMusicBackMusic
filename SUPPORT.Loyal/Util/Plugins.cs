using Newtonsoft.Json;

namespace Support.Loyal.Util
{
    public static class Plugins
    {
        public static void GuardarEnTxt(string Texto, string Path)
        {
            if (System.IO.File.Exists(Path))
            {
                using (var tw = new StreamWriter(Path, false))
                {
                    tw.Write(Texto);
                }
            }
            else
            {
                System.IO.File.WriteAllText(Path, Texto);
            }

        }

        public static string LeerTxt(string Path)
        {
            try
            {
                return File.ReadAllText(Path);
            }
            catch (Exception)
            {
                return string.Empty;
            }

        }

        public static void WriteExceptionLog(Exception ex, string Texto = null)
        {
            var pathCheck = ".";
            var fullPath = Path.GetFullPath(pathCheck);
            string filepath = String.Format(fullPath + "\\Files\\Log");

            if (!Directory.Exists(filepath + "\\" + DateTime.Now.ToShortDateString().Replace("/", "")))
            {
                Directory.CreateDirectory(filepath + "\\" + DateTime.Now.ToShortDateString().Replace("/", ""));
            };

            File.WriteAllText(filepath + "\\" + DateTime.Now.ToShortDateString().Replace("/", "") + "\\" + DateTime.Now.TimeOfDay.Ticks.ToString() + ".txt", JsonConvert.SerializeObject(ex + Texto));
        }

        public static void WriteExceptionLog(string Texto)
        {
            var pathCheck = ".";
            var fullPath = Path.GetFullPath(pathCheck);
            string filepath = String.Format(fullPath + "\\Files\\Log");

            if (!Directory.Exists(filepath + "\\" + DateTime.Now.ToShortDateString().Replace("/", "")))
            {
                Directory.CreateDirectory(filepath + "\\" + DateTime.Now.ToShortDateString().Replace("/", ""));
            };

            File.WriteAllText(filepath + "\\" + DateTime.Now.ToShortDateString().Replace("/", "") + "\\" + DateTime.Now.TimeOfDay.Ticks.ToString() + ".txt", JsonConvert.SerializeObject(Texto));
        }

        public static string LeerScriptTxt(string lCruta)
        {
            string texto;
            var pathCheck = ".";
            var fullPath = Path.GetFullPath(pathCheck);
            string filepath = String.Format(fullPath + lCruta);

            StreamReader sr = new StreamReader(filepath);
            texto = sr.ReadToEnd();
            sr.Close();
            return texto.ToString();
        }

        public static string JoinFecha(string Date)
        {
            string DateString = string.Empty;
            if (!string.IsNullOrEmpty(Date))
            {

                var Dia = Date.Split('-')[0];
                var Mes = Date.Split('-')[1];
                var Anio = Date.Split(' ')[0].Split('-')[2];
                DateString = $"{Anio}-{Mes}-{Dia}";
            }

            return DateString;
        }

        public static string CurrentDayString()
        {
            string Dia = DateTime.Now.Day < 9 ? $"0{DateTime.Now.Day}" : DateTime.Now.Day.ToString();
            string Mes = DateTime.Now.Month < 9 ? $"0{DateTime.Now.Month}" : DateTime.Now.Month.ToString();

            string DateString = $"{DateTime.Now.Year}-{Mes}-{Dia}";


            return DateString;
        }
    }
}
