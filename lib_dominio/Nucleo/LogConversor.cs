namespace lib_dominio.Nucleo
{
    public class LogConversor
    {
        public static void Log(Exception exception,
            IDictionary<string, object> ViewData)
        {
            if (ViewData == null)
                return;
            var mensaje = exception.Message.ToString();
            if (exception.InnerException != null)
                mensaje = exception.InnerException!.Message.ToString();

            if (mensaje.Length >= 110)
                mensaje = mensaje.Substring(0, 110);

            //var msg = lib_lenguajes.RsErrores.ResourceManager.GetString(mensaje);
            //if (!string.IsNullOrEmpty(msg))
            //{
            //    ViewData!["Mensaje"] = msg;
            //    return;
            //}
            ViewData!["Mensaje"] = mensaje;
        }
    }
}