using lib_dominio.Nucleo;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using lib_dominio.Entidades;
using lib_presentaciones.Interfaces;
using System.Text.RegularExpressions;


namespace asp_presentacion.Pages.Ventanas
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

        [BindProperty] public string? Mensaje { get; set; }
        
        [BindProperty] public string? Nombre { get; set; }
        [BindProperty] public string? Cedula { get; set; }
        [BindProperty] public string? Direccion { get; set; }
        [BindProperty] public string? Telefono { get; set; }
        [BindProperty] public string? Correo { get; set; }
        [BindProperty] public string? Contrasena { get; set; }

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
                Correo = string.Empty;
                Contrasena = string.Empty;
                Direccion = string.Empty;
                Nombre = string.Empty;
                Telefono = string.Empty;
                Cedula = string.Empty;
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
                if (string.IsNullOrWhiteSpace(this.Nombre) ||
                    string.IsNullOrWhiteSpace(this.Telefono) ||
                    string.IsNullOrWhiteSpace(this.Cedula) ||
                    string.IsNullOrWhiteSpace(this.Direccion) ||
                    string.IsNullOrWhiteSpace(this.Contrasena) ||
                    string.IsNullOrWhiteSpace(this.Correo))
                {
                    Mensaje = "Todos los campos son obligatorios.";
                    return;
                }

                if (!this.Correo.Contains(".com"))
                {
                    Mensaje = "El correo debe ser valido y completo.";
                    return;
                }

                if (string.IsNullOrWhiteSpace(this.Contrasena) || !Regex.IsMatch(this.Contrasena, @"\d"))
                {
                    Mensaje = "La contraseña debe tener almenos un número.";
                    return;
                }

                Cliente!.Nombre = this.Nombre;
                Cliente!.Telefono = this.Telefono;
                Cliente!.Cedula = this.Cedula;
                Cliente!.Direccion = this.Direccion;

                Cuenta!.Contrasena = this.Contrasena;
                Cuenta!.Correo = this.Correo;

                var taskExisteCliente = this.iPresentacionCliente!.PorCedula(Cliente!);
                taskExisteCliente.Wait();
                ExisteCliente = taskExisteCliente.Result;

                var taskExisteCuenta = this.iPresentacionCuenta!.PorCorreo(Cuenta!);
                taskExisteCuenta.Wait();
                ExisteCuenta= taskExisteCuenta.Result;

                if (ExisteCliente.Any() || ExisteCuenta.Any())
                {
                    Cliente = null;
                    Cuenta = null;
                    OnPostClean();
                    Mensaje = "Cuenta ya registrada.";
                    return;
                }

                this.iPresentacionCliente!.Guardar(Cliente!).Wait();
                this.iPresentacionCuenta!.Guardar(Cuenta!).Wait();

                string[] partes = this.Correo!.Split("@");

                ViewData["Logged"] = true;
                HttpContext.Session.SetString(partes[0], Correo!);
                HttpContext.Response.Redirect("/Ventanas/Videojuegos");
                return;
            }
            catch (Exception ex)
            {
                LogConversor.Log(ex, ViewData!);

            }
        }
    }
}
