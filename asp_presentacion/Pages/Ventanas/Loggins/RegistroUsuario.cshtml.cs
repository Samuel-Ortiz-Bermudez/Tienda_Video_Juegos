using lib_dominio.Nucleo;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using lib_dominio.Entidades;
using lib_presentaciones.Interfaces;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;


namespace asp_presentacion.Pages.Ventanas.Loggins
{
    public class RegistroUsuarioModel : PageModel
    {
        private ICuentasClientesPresentacion? iPresentacionCuenta= null;
        private IClientesPresentacion? iPresentacionCliente = null;
        public RegistroUsuarioModel(ICuentasClientesPresentacion iPresentacionCuenta, IClientesPresentacion iPresentacionCliente)
        {
            try
            {
                this.iPresentacionCliente = iPresentacionCliente;
                Cliente = new Clientes();

                this.iPresentacionCuenta = iPresentacionCuenta;
                Cuenta = new CuentasClientes();
            }
            catch (Exception ex)
            {
                LogConversor.Log(ex, ViewData!);
            }
        }
        private bool validacion { get; set; }

        [BindProperty] public string? Mensaje { get; set; }
        [BindProperty] public CuentasClientes? Cuenta { get; set; }
        [BindProperty] public List<CuentasClientes>? ExisteCuenta { get; set; }
        [BindProperty] public Clientes? Cliente { get; set; }
        [BindProperty] public List<Clientes>? ExisteCliente{ get; set; }

        public void OnGet()
        {
            OnPostClean();
        }

        public void OnPostClean()
        {
            try
            {
                Cuenta!.Correo = string.Empty;
                Cuenta!.Contrasena = string.Empty;
                Cliente!.Direccion = string.Empty;
                Cliente!.Nombre = string.Empty;
                Cliente!.Telefono = string.Empty;
                Cliente!.Cedula = string.Empty;
            }
            catch (Exception ex)
            {
                LogConversor.Log(ex, ViewData!);
            }
        } 

        public async void OnPostBtnRegistrar()
        {
            try
            {
                OnPostValidarCampos();
                if (validacion == false) return;

                OnPostExiste();
                if (validacion == false) return;

                this.iPresentacionCliente!.Guardar(Cliente!).Wait();
                this.iPresentacionCuenta!.Guardar(Cuenta!).Wait();

                string[] partes = this.Cuenta!.Correo!.Split("@");
                
                var taskClienteId= this.iPresentacionCliente!.PorCedula(Cliente!);
                taskClienteId.Wait();

                ViewData["Logged"] = true;
                HttpContext.Session.SetString(partes[0], this.Cuenta!.Correo!);

                var claims = new List<Claim> {
                    new Claim(ClaimTypes.Name, partes[0]),
                    new Claim("Correo", Cuenta.Correo),
                    new Claim(ClaimTypes.Role, "Cliente"),
                    new Claim("Id", taskClienteId.Result.FirstOrDefault()!.Id.ToString())
                };

                var claimIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimIdentity));

                HttpContext.Response.Redirect("/Ventanas/Videojuegos");
                return;
            }
            catch (Exception ex)
            {
                LogConversor.Log(ex, ViewData!);
            }
        }

        public void OnPostValidarCampos()
        {
            try
            {
                if (Cliente == null && Cuenta == null)
                {
                    Mensaje = "Todos los campos son obligatorios.";
                    validacion = false;
                    return;
                }

                if (!this.Cuenta!.Correo!.Contains(".com"))
                {
                    Mensaje = "El correo debe ser valido y completo.";
                    validacion = false;
                    return;
                }

                if (string.IsNullOrWhiteSpace(this.Cuenta.Contrasena) || !Regex.IsMatch(this.Cuenta.Contrasena, @"\d"))
                {
                    Mensaje = "La contraseña debe tener almenos un número.";
                    validacion = false;
                    return;
                }
                validacion = true;
                return;
            }
            catch (Exception ex)
            {
                LogConversor.Log(ex, ViewData!);
                validacion = false;
            }
        }

        public void OnPostExiste()
        {
            try
            {
                var taskExisteCliente = this.iPresentacionCliente!.PorCedula(Cliente!);
                taskExisteCliente.Wait();
                ExisteCliente = taskExisteCliente.Result;

                var taskExisteCuenta = this.iPresentacionCuenta!.PorCorreo(Cuenta!);
                taskExisteCuenta.Wait();
                ExisteCuenta = taskExisteCuenta.Result;

                if (ExisteCliente.Any() || ExisteCuenta.Any())
                {
                    Cliente = null;
                    Cuenta = null;
                    OnPostClean();
                    Mensaje = "Cuenta ya registrada.";
                    validacion = false;
                    return;
                }
                validacion = true;
                return;
            }
            catch (Exception ex)
            {
                LogConversor.Log(ex, ViewData!);
                validacion = false;
            }
        }
    }
}
