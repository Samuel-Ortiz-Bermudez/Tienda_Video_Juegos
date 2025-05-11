using lib_dominio.Nucleo;

namespace asp_servicios.Nucleo
{
    public class Configuracion
    {
        private static Dictionary<string, string>? datos;

        public static string? ObtenerValor(string? key)
        {
            // return Servicio(key);
            return Local(key);
        }

        private static string? Servicio(string? key)
        {
            var response = string.Empty;
            if (Startup.Configuration != null)
                response = Startup.Configuration!
                    .GetSection(key!).Value;

            if (Startup.Configuration != null)
                response = Startup.Configuration!
                    .GetSection("Settings")
                    .GetSection(key!).Value;

            if (Startup.Configuration != null &&
                string.IsNullOrEmpty(response))
                response = Startup.Configuration!
                    .GetConnectionString(key!)!;
            return response;
        }

        private static string? Local(string? key)
        {
            if (datos != null)
                return datos![key!];
            if (!File.Exists(DatosGenerales.ruta_json))
                return null;
            datos = new Dictionary<string, string>();
            StreamReader jsonStream = File.OpenText(DatosGenerales.ruta_json);
            var json = jsonStream.ReadToEnd();
            datos = JsonConversor.ConvertirAObjeto<Dictionary<string, string>>(json)!;
            return datos![key!];
        }
    }
}