using lib_dominio.Entidades;
using lib_dominio.Nucleo;
using lib_presentaciones.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;

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

        public void OnGet()
        {
            OnPostBtnInicio();
        }

        public void OnPostBtnClean()
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

        public async void OnPostBtnInicio()
        {
            try
            {
                if (string.IsNullOrEmpty(Correo) && string.IsNullOrEmpty(Contrasena))
                {
                    OnPostBtnClean();
                    return;
                }

                string[] partes = Correo!.Split('@');


                //Validacion de que sea cuenta de empleado
                if (partes[1] == "tienda.com")
                {
                    EmpleadoSesion!.Correo = Correo;
                    EmpleadoSesion!.Contrasena = Contrasena;
                    var taskEmpleadosSesion = iPresentacionEmpleados!.PorCorreo(EmpleadoSesion!);
                    taskEmpleadosSesion.Wait();
                    EmpleadoCuenta = taskEmpleadosSesion.Result;

                    //Validacion de credenciales

                    if (EmpleadoCuenta == null)
                    {
                        EmpleadoSesion = null;
                        return;
                    }

                    if (!EmpleadoSesion.Correo.ToUpper().Equals(EmpleadoCuenta[0].Correo!.ToUpper()) || EmpleadoSesion.Contrasena != EmpleadoCuenta[0].Contrasena)
                    {
                        EmpleadoSesion = null;
                        OnPostBtnClean();
                        Mensaje = "Contraseña o Correo incorrectos.";
                        return;
                    }
                    if (EmpleadoSesion.Correo.ToUpper().Equals(EmpleadoCuenta[0].Correo!.ToUpper()) && EmpleadoSesion.Contrasena == EmpleadoCuenta[0].Contrasena)
                    {
                        ViewData["Logged"] = true;
                        HttpContext.Session.SetString(partes[0], Correo!);

                        var claims = new List<Claim> {
                            new Claim(ClaimTypes.Name, partes[0]),
                            new Claim("Correo", EmpleadoSesion.Correo),
                            new Claim(ClaimTypes.Role, EmpleadoCuenta[0].Rol!),
                            new Claim("Id", EmpleadoCuenta[0].Empleado!.ToString())
                        };

                        var claimIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimIdentity));

                        HttpContext.Response.Redirect("/Ventanas/Videojuegos");
                    }
                    return;
                }


                ClienteSesion!.Correo = Correo;
                ClienteSesion!.Contrasena = Contrasena;
                var taskClientesSesion = iPresentacionClientes!.PorCorreo(ClienteSesion!);
                taskClientesSesion.Wait();
                ClienteCuenta = taskClientesSesion.Result;

                if (ClienteCuenta == null)
                {
                    ClienteSesion = null;
                    return;
                }


                if (!ClienteSesion.Correo.ToUpper().Equals(ClienteCuenta[0].Correo!.ToUpper()) || ClienteSesion.Contrasena != ClienteCuenta[0].Contrasena)
                {
                    ClienteSesion = null;
                    OnPostBtnClean();
                    Mensaje = "Contraseña o Correo incorrectos.";
                    return;
                }

                if (ClienteSesion.Correo.ToUpper().Equals(ClienteCuenta[0].Correo!.ToUpper()) && ClienteSesion.Contrasena == ClienteCuenta[0].Contrasena)
                {
                    ViewData["Logged"] = true;
                    HttpContext.Session.SetString(partes[0], Correo!);

                    var claims = new List<Claim> {
                        new Claim(ClaimTypes.Name, partes[0]),
                        new Claim("Correo", ClienteSesion.Correo),
                        new Claim(ClaimTypes.Role, "Cliente"),
                        new Claim("Id", ClienteCuenta[0].Cliente!.ToString())

                    };

                    var claimIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimIdentity));


                    HttpContext.Response.Redirect("/Ventanas/Videojuegos");
                    return;
                }

                HttpContext.Response.Redirect("/Ventanas/RegistroUsuario");
                OnPostBtnClean();
            }
            catch (Exception ex)
            {
                LogConversor.Log(ex, ViewData!);
            }
        }
    }
}
