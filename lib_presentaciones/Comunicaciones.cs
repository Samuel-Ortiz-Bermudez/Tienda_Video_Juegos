using lib_dominio.Nucleo;

namespace lib_presentaciones
{
    public class Comunicaciones
    {
        private string? Protocolo = string.Empty,
            Host = string.Empty,
            Servicio = string.Empty,
            token = null;

        public Comunicaciones(string servicio = "",
            string protocolo = "http://",
            string host = "localhost:5256")
        {
            Protocolo = protocolo;
            Host = host;
            Servicio = servicio;
        }

        public Dictionary<string, object> ConstruirUrl(Dictionary<string, object> data, string Metodo)
        {
            data["Url"] = Protocolo + Host + "/" + Servicio + Metodo;
            data["UrlToken"] = Protocolo + Host + "/" + Servicio + "Token/Autenticar";
            return data;
        }

        public async Task<Dictionary<string, object>> Ejecutar(Dictionary<string, object> datos)
        {
            var respuesta = new Dictionary<string, object>();
            try
            {
                respuesta = await Autenticar(datos);
                if (respuesta == null || respuesta.ContainsKey("Error"))
                    return respuesta!;
                respuesta.Clear();

                var url = datos["Url"].ToString();
                datos.Remove("Url");
                datos.Remove("UrlToken");
                datos["Bearer"] = token!;
                var stringData = JsonConversor.ConvertirAString(datos);

                var httpClient = new HttpClient();
                httpClient.Timeout = new TimeSpan(0, 4, 0);

                var message = await httpClient.PostAsync(url, new StringContent(stringData));
                if (!message.IsSuccessStatusCode)
                {
                    respuesta.Add("Error", "lbErrorComunicacion");
                    return respuesta;
                }
                var resp = await message.Content.ReadAsStringAsync();
                httpClient.Dispose(); httpClient = null;
                if (string.IsNullOrEmpty(resp))
                {
                    respuesta.Add("Error", "lbErrorAutenticacion");
                    return respuesta;
                }
                resp = Replace(resp);
                respuesta = JsonConversor.ConvertirAObjeto(resp);
                return respuesta;
            }
            catch (Exception ex)
            {
                respuesta["Error"] = ex.ToString();
                return respuesta;
            }
        }

        private async Task<Dictionary<string, object>> Autenticar(Dictionary<string, object> datos)
        {
            var respuesta = new Dictionary<string, object>();
            try
            {
                var url = datos["UrlToken"].ToString();
                var temp = new Dictionary<string, object>();
                temp["Usuario"] = DatosGenerales.usuario_datos;
                var stringData = JsonConversor.ConvertirAString(temp);

                var httpClient = new HttpClient();
                httpClient.Timeout = new TimeSpan(0, 1, 0);

                var mensaje = await httpClient.PostAsync(url, new StringContent(stringData));
                if (!mensaje.IsSuccessStatusCode)
                {
                    respuesta.Add("Error", "lbErrorComunicacion");
                    return respuesta;
                }
                var resp = await mensaje.Content.ReadAsStringAsync();
                httpClient.Dispose(); httpClient = null;
                if (string.IsNullOrEmpty(resp))
                {
                    respuesta.Add("Error", "lbErrorAutenticacion");
                    return respuesta;
                }
                resp = Replace(resp);
                respuesta = JsonConversor.ConvertirAObjeto(resp);
                token = respuesta["Token"].ToString();
                return respuesta;
            }
            catch (Exception ex)
            {
                respuesta["Error"] = ex.ToString();
                return respuesta;
            }
        }

        private string Replace(string resp)
        {
            return resp.Replace("\\\\r\\\\n", "")
            .Replace("\\r\\n", "")
            .Replace("\\", "")
            .Replace("\\\"", "\"")
            .Replace("\"", "'")
            .Replace("'[", "[")
            .Replace("]'", "]")
            .Replace("'{'", "{'")
            .Replace("\\\\", "\\")
            .Replace("'}'", "'}")
            .Replace("}'", "}")
            .Replace("\\n", "")
            .Replace("\\r", "")
            .Replace(" ", "")
            .Replace("'{", "{")
            .Replace("\"", "")
            .Replace(" ", "")
            .Replace("null", "''");
        }
    }
}