using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;
using lib_dominio.Nucleo;

namespace asp_servicios.Controllers
{
    public class TokenController : ControllerBase
    {
        private Dictionary<string, object> ObtenerDatos()
        {
            var respuesta = new Dictionary<string, object>();
            try
            {
                var datos = new StreamReader(Request.Body).ReadToEnd().ToString();
                if (string.IsNullOrEmpty(datos))
                    datos = "{}";
                return JsonConversor.ConvertirAObjeto(datos);
            }
            catch (Exception ex)
            {
                respuesta["Error"] = ex.Message.ToString();
                return respuesta;
            }
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("Token/Autenticar")]
        public string Autenticar()
        {
            var respuesta = new Dictionary<string, object>();
            try
            {
                var datos = ObtenerDatos();
                if (!datos.ContainsKey("Usuario") ||
                    datos["Usuario"].ToString()! != DatosGenerales.usuario_datos)
                {
                    respuesta["Error"] = "lbNoAutenticacion";
                    return JsonConversor.ConvertirAString(respuesta);
                }

                var tokenHandler = new JwtSecurityTokenHandler();
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[] {
                    new Claim(ClaimTypes.Name, datos["Usuario"].ToString()!)
                }),
                    Expires = DateTime.UtcNow.AddHours(1),
                    SigningCredentials = new SigningCredentials(
                        new SymmetricSecurityKey(Encoding.UTF8.GetBytes(DatosGenerales.clave)),
                        SecurityAlgorithms.HmacSha256Signature)
                };
                var token = tokenHandler.CreateToken(tokenDescriptor);

                respuesta["Token"] = tokenHandler.WriteToken(token);
                return JsonConversor.ConvertirAString(respuesta);
            }
            catch (Exception ex)
            {
                respuesta["Error"] = ex.ToString();
                return JsonConversor.ConvertirAString(respuesta);
            }
        }

        public bool Validate(Dictionary<string, object> data)
        {
            try
            {
                var authorizationHeader = data["Bearer"].ToString();
                authorizationHeader = authorizationHeader!.Replace("Bearer ", "");
                var tokenHandler = new JwtSecurityTokenHandler();
                SecurityToken token = tokenHandler.ReadToken(authorizationHeader);
                if (DateTime.UtcNow > token.ValidTo)
                    return false;
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}