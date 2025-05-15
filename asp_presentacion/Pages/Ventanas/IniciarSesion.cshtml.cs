using System.ComponentModel.DataAnnotations;
using lib_dominio.Entidades;
using lib_dominio.Nucleo;
using lib_presentaciones.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace asp_presentacion.Pages.Ventanas
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
            var variable_session = HttpContext.Session.GetString("Usuario"); 
            if (!String.IsNullOrEmpty(variable_session)) 
            { EstaLogueado = true; return; }

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

                string[] partes = this.Correo!.Split('@');


                //Validacion de que sea cuenta de empleado
                if (partes[1] == "tienda.com") 
                {
                    EmpleadoSesion!.Correo = this.Correo;
                    EmpleadoSesion!.Contrasena = this.Contrasena;
                    var taskEmpleadosSesion = this.iPresentacionEmpleados!.PorCorreo(EmpleadoSesion!);
                    taskEmpleadosSesion.Wait();
                    EmpleadoCuenta = taskEmpleadosSesion.Result;

                    //Validacion de credenciales

                    if (EmpleadoCuenta == null)
                    {
                        EmpleadoSesion = null;
                        return;
                    }

                    if ((EmpleadoSesion.Correo != EmpleadoCuenta[0].Correo) || (EmpleadoSesion.Contrasena != EmpleadoCuenta[0].Contrasena))
                    {
                        EmpleadoSesion = null;
                        OnPostBtnClean();
                        Mensaje = "Contraseña o Correo incorrectos.";
                        return;
                    }
                    if ((EmpleadoSesion.Correo == EmpleadoCuenta[0].Correo) && (EmpleadoSesion.Contrasena == EmpleadoCuenta[0].Contrasena))
                    {
                        ViewData["Logged"] = true;
                        HttpContext.Session.SetString(partes[0], Correo!);
                        EstaLogueado = true;
                        HttpContext.Response.Redirect("/Ventanas/Videojuegos");
                    }
                    return; 
                }

                
                ClienteSesion!.Correo = this.Correo;
                ClienteSesion!.Contrasena = this.Contrasena;
                var taskClientesSesion = this.iPresentacionClientes!.PorCorreo(ClienteSesion!);
                taskClientesSesion.Wait();
                ClienteCuenta = taskClientesSesion.Result;

                if (ClienteCuenta == null)
                {
                    ClienteSesion = null;
                    return;
                }


                if ((ClienteSesion.Correo != ClienteCuenta[0].Correo) || (ClienteSesion.Contrasena != ClienteCuenta[0].Contrasena))
                {
                    ClienteSesion = null;
                    OnPostBtnClean();
                    Mensaje = "Contraseña o Correo incorrectos.";
                    return;
                }

                if ((ClienteSesion.Correo == ClienteCuenta[0].Correo) && (ClienteSesion.Contrasena == ClienteCuenta[0].Contrasena))
                {
                    ViewData["Logged"] = true;
                    HttpContext.Session.SetString(partes[0], Correo!);
                    EstaLogueado = true;
                    HttpContext.Response.Redirect("/Ventanas/Videojuegos");
                    return;
                }

                HttpContext.Response.Redirect("/Ventanas/RegistroUsuario");
                OnPostBtnClean();
            }
            catch (Exception ex) { 
                LogConversor.Log(ex, ViewData!); 
            }
        }
    }
}
