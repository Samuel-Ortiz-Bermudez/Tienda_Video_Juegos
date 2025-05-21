using lib_dominio.Nucleo;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;
using lib_presentaciones.Interfaces;
using lib_dominio.Entidades;
using System.Security.Claims;

namespace asp_presentacion.Pages.Ventanas.Perfiles
{
    [Authorize]
    public class EmpleadosPerfilModel : PageModel
    {
        private ICuentasEmpleadosPresentacion? iPresentacionCuenta = null;

        public EmpleadosPerfilModel(ICuentasEmpleadosPresentacion iPresentacionCuenta)
        {
            try
            {
                this.iPresentacionCuenta = iPresentacionCuenta;
                Cuenta = new CuentasEmpleados();
            }
            catch (Exception ex)
            {
                LogConversor.Log(ex, ViewData!);
            }
        }

        [BindProperty] public Enumerables.Ventanas Accion { get; set; }
        [BindProperty] public CuentasEmpleados? Cuenta { get; set; }
        [BindProperty] public Empleados? Empleado { get; set; }
        [BindProperty] public List<CuentasEmpleados>? CuentaEmpleado { get; set; }
        [BindProperty] public string? Usuario { get; set; }
        [BindProperty] public string? Rol { get; set; }
        public void OnGet()
        {
            OnPostIngreso();
        }


        public void OnPostIngreso()
        {
            try
            {
                Accion = Enumerables.Ventanas.Listas;
                Usuario = User.Identity!.Name;
                Rol = User.Claims.FirstOrDefault(r => r.Type == ClaimTypes.Role)?.Value;
                Cuenta!.Correo = User.Claims.FirstOrDefault(c => c.Type == "Correo")?.Value;
                var empleado = this.iPresentacionCuenta!.PorCorreo(Cuenta);
                empleado.Wait();
                CuentaEmpleado = empleado.Result;

                Empleado = CuentaEmpleado[0]._Empleado!;
            }
            catch (Exception ex)
            {
                LogConversor.Log(ex, ViewData!);
            }
        }

        public async Task<IActionResult> OnPostBtnCerrarSesion()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToPage("/Ventanas/Videojuegos");
        }
    }
}
