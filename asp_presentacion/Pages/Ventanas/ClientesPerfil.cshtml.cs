using lib_dominio.Nucleo;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using lib_presentaciones.Interfaces;
using lib_dominio.Entidades;

namespace asp_presentacion.Pages.Ventanas
{
    [Authorize]
    public class ClientesPerfilModel : PageModel
    {
        private ICuentasClientesPresentacion? iPresentacionCuenta = null;
        private IClientesPresentacion? iPresentacionCliente = null;
        private IComprasPresentacion? iPresentacionCompras = null;
        private IDetallesComprasPresentacion? iPresentacionDetallesCompras = null;

        public ClientesPerfilModel(ICuentasClientesPresentacion iPresentacionCuenta, IClientesPresentacion iPresentacionCliente, IComprasPresentacion? iPresentacionCompras, IDetallesComprasPresentacion? iPresentacionDetallesCompras)
        {
            try
            {
                this.iPresentacionCliente = iPresentacionCliente;
                Cliente = new Clientes();

                this.iPresentacionCuenta = iPresentacionCuenta;
                Cuenta = new CuentasClientes(); 
                
                this.iPresentacionCompras = iPresentacionCompras;
                Actual = new Compras();

                this.iPresentacionDetallesCompras = iPresentacionDetallesCompras;
            }
            catch (Exception ex)
            {
                LogConversor.Log(ex, ViewData!);
            }
        }

        [BindProperty] public Enumerables.Ventanas Accion { get; set; }
        [BindProperty] public CuentasClientes? Cuenta { get; set; }
        [BindProperty] public List<CuentasClientes>? CuentaCliente { get; set; }
        [BindProperty] public Clientes? Cliente { get; set; }
        [BindProperty] public List<DetallesCompras>? DetallesCompra { get; set; }
        [BindProperty] public List<DetallesCompras>? ListaDetalles { get; set; }

        [BindProperty] public Compras? Actual { get; set; }
        [BindProperty] public List<Compras>? Lista { get; set; }

        [BindProperty] public string? Usuario { get; set; }

        public void OnGet()
        {
                OnPostIngreso();
        }

        public void OnPostIngreso()
        {
            try 
            {
                Usuario = User.Identity!.Name;
                Cuenta!.Correo = User.Claims.FirstOrDefault(c => c.Type == "Correo")?.Value;
                var cliente = this.iPresentacionCuenta!.PorCorreo(Cuenta);
                cliente.Wait();
                CuentaCliente = cliente.Result;

                Cliente = CuentaCliente[0]._Cliente;
                Accion = Enumerables.Ventanas.Listas;
                
                var ComprasLista = this.iPresentacionCompras!.Listar();
                ComprasLista.Wait();
                Lista = ComprasLista!.Result.Where(x => x.Cliente == Cliente!.Id).ToList();
            } 
            catch (Exception ex) {
                LogConversor.Log(ex, ViewData!);
            }

        }

        public void OnPostBtnCambiar()
        {
            try
            {
                OnPostIngreso();
                Accion = Enumerables.Ventanas.Editar;
            } 
            catch (Exception ex) {
                LogConversor.Log(ex, ViewData!);
            }
        }

        public void OnPostBtnGuardar()
        {
            try
            {
                var guardar = this.iPresentacionCliente!.Modificar(Cliente);
                guardar.Wait();
                Cliente = guardar.Result;
                Accion = Enumerables.Ventanas.Listas;
                OnPostIngreso();
            }
            catch (Exception ex)
            {
                LogConversor.Log(ex, ViewData!);
            }
        }

        public void OnPostBtnEliminar()
        {
            try
            {
                OnPostIngreso();
                Accion = Enumerables.Ventanas.Borrar;

            }
            catch (Exception ex)
            {
                LogConversor.Log(ex, ViewData!);
            }
        }

        public virtual void OnPostBtnDetalles(string Id)
        {
            try
            {
                OnPostIngreso();
                Accion = Enumerables.Ventanas.Detalles;
                var detallesComprasTask = this.iPresentacionDetallesCompras!.Listar();
                detallesComprasTask.Wait();

                ListaDetalles = detallesComprasTask.Result;

                DetallesCompra = ListaDetalles.Where(x => x.Compra.ToString() == Id).ToList();
            }
            catch (Exception ex)
            {
                LogConversor.Log(ex, ViewData!);
            }
        }

        public void OnPostBtnCerrar()
        {
            try
            {
                Accion = Enumerables.Ventanas.Listas;
                OnPostIngreso();
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
