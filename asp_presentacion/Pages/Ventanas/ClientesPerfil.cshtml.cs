using lib_dominio.Nucleo;
using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using lib_presentaciones.Interfaces;
using lib_dominio.Entidades;
using System.Security.Claims;

namespace asp_presentacion.Pages.Ventanas
{
    [Authorize]
    public class ClientesPerfilModel : PageModel
    {
        private ICuentasClientesPresentacion? iPresentacionCuenta = null;
        private IClientesPresentacion? iPresentacionCliente = null;
        private IComprasPresentacion? iPresentacionCompras = null;

        public ClientesPerfilModel(ICuentasClientesPresentacion iPresentacionCuenta, IClientesPresentacion iPresentacionCliente, IComprasPresentacion? iPresentacionCompras)
        {
            try
            {
                this.iPresentacionCliente = iPresentacionCliente;
                Cliente = new Clientes();

                this.iPresentacionCuenta = iPresentacionCuenta;
                Cuenta = new CuentasClientes(); 
                
                this.iPresentacionCompras= iPresentacionCompras;
                Actual = new Compras();
            }
            catch (Exception ex)
            {
                LogConversor.Log(ex, ViewData!);
            }
        }

        [BindProperty] public CuentasClientes? Cuenta { get; set; }
        [BindProperty] public List<CuentasClientes>? CuentaCliente { get; set; }
        [BindProperty] public Clientes? Cliente { get; set; }
        [BindProperty] public Compras? Actual{ get; set; }
        [BindProperty] public List<Compras>? lista { get; set; }


        public void OnGet()
        {
            OnPostIngreso();
        }

        public void OnPostIngreso()
        {
            try 
            {
                ViewData["Usuario"] = User.Identity!.Name;
                Cuenta!.Correo = User.Claims.FirstOrDefault(c => c.Type == "Correo")?.Value;
                var cliente = this.iPresentacionCuenta!.PorCorreo(Cuenta);
                cliente.Wait();
                CuentaCliente = cliente.Result;

                Cliente = CuentaCliente[0]._Cliente;
                CuentaCliente[0] = null;

                var ComprasLista = this.iPresentacionCompras!.Listar();
                ComprasLista.Wait();

                lista = ComprasLista!.Result.Where(x => x.Cliente == Cliente!.Id).ToList();

            } 
            catch (Exception ex) {
                LogConversor.Log(ex, ViewData!);
            }

        }

        public void OnPostBtnActualizar()
        {

        }

        public async Task<IActionResult> OnPostBtnCerrarSesion()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToPage("/Ventanas/Videojuegos");
        }
    }
}
