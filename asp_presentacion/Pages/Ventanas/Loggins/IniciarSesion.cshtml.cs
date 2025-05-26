using System.Security.Claims;
using lib_dominio.Entidades;
using lib_dominio.Nucleo;
using lib_presentaciones.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.IdentityModel.Tokens;

namespace asp_presentacion.Pages.Ventanas.Loggins
{
    public class IniciarSesionModel : PageModel
    {
        private ICuentasClientesPresentacion? iPresentacionClientes = null;
        private ICuentasEmpleadosPresentacion? iPresentacionEmpleados = null;
        public IniciarSesionModel(ICuentasEmpleadosPresentacion iPresentacionEmpleados, ICuentasClientesPresentacion iPresentacionClientes)
        {
            try
            {
                this.iPresentacionEmpleados = iPresentacionEmpleados;
                EmpleadoSesion = new CuentasEmpleados();

                this.iPresentacionClientes = iPresentacionClientes;
                ClienteSesion = new CuentasClientes();

                Hasher = new PasswordHasher<string>();
            }
            catch (Exception ex)
            {
                LogConversor.Log(ex, ViewData!);
            }
        }

        public bool EstaLogueado = false;
        [BindProperty] public string? Mensaje { get; set; }
        [BindProperty] public string? Correo { get; set; }
        [BindProperty] public string? Contrasena { get; set; }

        [BindProperty] public CuentasClientes? ClienteSesion { get; set; }
        [BindProperty] public List<CuentasClientes>? ClienteCuenta { get; set; }

        [BindProperty] public CuentasEmpleados? EmpleadoSesion { get; set; }
        [BindProperty] public List<CuentasEmpleados>? EmpleadoCuenta { get; set; }
        private PasswordHasher<string>? Hasher { get; set; }
        private PasswordVerificationResult Resultado { get; set; }
        private bool Validacion { get; set; }
        private string[]? Partes { get; set; }

        public void OnGet()
        {
            OnPostBtnIniciar();
        }

        public void OnPostClean()
        {
            try
            {
                Correo = string.Empty;
                Contrasena = string.Empty;
            }
            catch (Exception ex)
            {
                LogConversor.Log(ex, ViewData!);
            }
        }

        public void OnPostBtnIniciar()
        {
            try
            {
                if (string.IsNullOrEmpty(Correo) && string.IsNullOrEmpty(Contrasena))
                {
                    OnPostClean();
                    return;
                }

                Partes = Correo!.Split('@');

                
                if (Partes[1] == "tienda.com")
                {
                    OnPostEmpleado();
                    if (Validacion == true) return;
                } 
                else
                {
                    OnPostCliente();
                    if (Validacion == true) return;
                }
                OnPostClean();
            }
            catch (Exception ex)
            {
                LogConversor.Log(ex, ViewData!);
            }
        }

        public async void OnPostCliente()
        {
            try
            {
                ClienteSesion!.Correo = Correo;
                ClienteSesion!.Contrasena = Contrasena;

                var taskClientesSesion = iPresentacionClientes!.PorCorreo(ClienteSesion!);
                taskClientesSesion.Wait();
                ClienteCuenta = taskClientesSesion.Result;

                if (ClienteCuenta == null)
                {
                    ClienteSesion = null;
                    Validacion = false;
                    return;
                }

                if (string.IsNullOrEmpty(EmpleadoSesion!.Contrasena))

                Hasher = new PasswordHasher<string>();
                Resultado = Hasher!.VerifyHashedPassword(null!, ClienteCuenta[0]!.Contrasena!, ClienteSesion!.Contrasena!);

                if (!ClienteSesion!.Correo!.ToUpper().Equals(ClienteCuenta[0].Correo!.ToUpper()) || Resultado == PasswordVerificationResult.Failed)
                {
                    ClienteSesion = null;
                    OnPostClean();
                    Mensaje = "Contraseña o Correo incorrectos.";
                    Validacion = false;
                    return;
                }

                if (ClienteSesion.Correo.ToUpper().Equals(ClienteCuenta[0].Correo!.ToUpper()) && (Resultado == PasswordVerificationResult.Success || Resultado == PasswordVerificationResult.SuccessRehashNeeded))
                {
                    ViewData["Logged"] = true;
                    HttpContext.Session.SetString(Partes![0], Correo!);

                    var claims = new List<Claim> {
                    new Claim(ClaimTypes.Name, Partes[0]),
                    new Claim("Correo", ClienteSesion.Correo),
                    new Claim(ClaimTypes.Role, "Cliente"),
                    new Claim("Id", ClienteCuenta[0].Cliente!.ToString())
                    };
                    
                    var claimIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimIdentity));

                    HttpContext.Response.Redirect("/Ventanas/Videojuegos");
                    Validacion = true;
                }
                return;
            }
            catch (Exception ex)
            {
                LogConversor.Log(ex, ViewData!);
            }
        }

        public async void OnPostEmpleado()
        {
            try
            {
                EmpleadoSesion!.Correo = Correo;
                EmpleadoSesion!.Contrasena = Contrasena;
                var taskEmpleadosSesion = iPresentacionEmpleados!.PorCorreo(EmpleadoSesion!);
                taskEmpleadosSesion.Wait();
                EmpleadoCuenta = taskEmpleadosSesion.Result;

                if (EmpleadoCuenta == null)
                {
                    EmpleadoSesion = null;
                    Validacion = false;
                    return;
                }

                if (string.IsNullOrEmpty(EmpleadoSesion!.Contrasena))

                Hasher = new PasswordHasher<string>();
                Resultado = Hasher!.VerifyHashedPassword(null!, EmpleadoCuenta[0]!.Contrasena!, EmpleadoSesion!.Contrasena!);

                if (!EmpleadoSesion!.Correo!.ToUpper().Equals(EmpleadoCuenta[0].Correo!.ToUpper()) || Resultado == PasswordVerificationResult.Failed)
                {
                    EmpleadoSesion = null;
                    OnPostClean();
                    Mensaje = "Contraseña o Correo incorrectos.";
                    Validacion = false;
                    return;
                }

                if (EmpleadoSesion.Correo.ToUpper().Equals(EmpleadoCuenta[0].Correo!.ToUpper()) && (Resultado == PasswordVerificationResult.Success || Resultado == PasswordVerificationResult.SuccessRehashNeeded))
                {
                    ViewData["Logged"] = true;
                    HttpContext.Session.SetString(Partes![0], Correo!);

                    var claims = new List<Claim> {
                            new Claim(ClaimTypes.Name, Partes[0]),
                            new Claim("Correo", EmpleadoSesion.Correo),
                            new Claim(ClaimTypes.Role, EmpleadoCuenta[0].Rol!),
                            new Claim("Id", EmpleadoCuenta[0].Empleado!.ToString())
                        };

                    var claimIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimIdentity));

                    HttpContext.Response.Redirect("/Ventanas/Videojuegos");
                    Validacion = true;
                }
                return;
            }
            catch (Exception ex)
            {
                LogConversor.Log(ex, ViewData!);
            }
        }
    }
}
