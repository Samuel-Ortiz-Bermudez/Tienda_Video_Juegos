using lib_dominio.Entidades;
using lib_dominio.Nucleo;
using lib_presentaciones.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace asp_presentacion.Pages.Ventanas
{
    [Authorize]
    public class CestaModel : PageModel
    {
        private IComprasPresentacion? IcomprasPresentacion = null;
        private IEmpleadosPresentacion? IPresentacionEmpleados = null;
        private IDetallesComprasPresentacion? IPresentacionDetalles = null;
        private IInventariosPresentacion? IPresentacionInvetarios = null;

        public CestaModel(IComprasPresentacion IcomprasPresentacion, IEmpleadosPresentacion IPresentacionEmpleados, IDetallesComprasPresentacion IPresentacionDetalles, IInventariosPresentacion IPresentacionInvetarios)
        {
            try
            {
                this.IcomprasPresentacion = IcomprasPresentacion;
                this.IPresentacionEmpleados = IPresentacionEmpleados;
                this.IPresentacionDetalles = IPresentacionDetalles;
                this.IPresentacionInvetarios = IPresentacionInvetarios;

                Compra = new Compras();
            }
            catch (Exception ex)
            {
                LogConversor.Log(ex, ViewData!);
            }
        }

        [BindProperty] public List<DetallesCompras>? Cesta { get; set; }
        [BindProperty] public List<Empleados>? ListaEmpleados { get; set; }
        [BindProperty] public string? MetodoSeleccionado { get; set; }
        [BindProperty] public int EmpleadoSeleccionado { get; set; }
        public Compras? Compra { get; set; }
        public decimal Total { get; set; }

        public void OnGet()
        {
            try
            {
                Cesta = HttpContext.Session.GetObjectFromJson<List<DetallesCompras>>("Cesta") ?? new List<DetallesCompras>();
                Total = Cesta.Sum(c => c.Subtotal);
                var taskEmpleados = this.IPresentacionEmpleados!.Listar();
                taskEmpleados.Wait();

                ListaEmpleados = taskEmpleados.Result.Where( x => x.Nombre != "Admin").ToList();
            }
            catch(Exception ex)
            {
                LogConversor.Log(ex, ViewData!);
            }
            
        }


        public IActionResult OnPostEliminarDeCesta(int id)
        {
            try
            {
                var cesta = HttpContext.Session.GetObjectFromJson<List<DetallesCompras>>("Cesta") ?? new List<DetallesCompras>();

                var item = cesta.FirstOrDefault(d => d.Videojuego == id);
                if (item != null)
                {
                    item.Cantidad--;
                    if (item.Cantidad <= 0)
                    {
                        cesta.Remove(item);
                    }
                    else
                    {
                        item.CalculoSubtotal();
                    }

                    HttpContext.Session.SetObjectAsJson("Cesta", cesta);
                    TempData["Mensaje"] = "Cantidad actualizada en la cesta.";

                }
                else
                {
                    TempData["Mensaje"] = "Juego no encontrado en la cesta.";
                }
            }
            catch (Exception ex)
            {
                LogConversor.Log(ex, ViewData!);
                TempData["Mensaje"] = "Error al actualizar la cesta.";
            }

            return RedirectToPage();
        }

        public void OnPostConfirmarCompra()
        {
            try
            {
                var cesta = HttpContext.Session.GetObjectFromJson<List<DetallesCompras>>("Cesta");
                if (cesta == null || !cesta.Any())
                {
                    TempData["Mensaje"] = "No hay productos en la cesta.";
                    RedirectToPage();
                    return;
                }


                Compra!.FechaVenta = DateTime.Now;
                Compra!.MetodoPago = MetodoSeleccionado;
                Compra!.Cliente = int.Parse(User.Claims.FirstOrDefault(c => c.Type == "Id")?.Value!);
                Compra!.Empleado = EmpleadoSeleccionado;
                Compra!.DetallesCompra = cesta;

                Compra!.CalculoTotal();
                Compra!.DetallesCompra = null;
                var taskGuardar = this.IcomprasPresentacion!.Guardar(Compra);
                taskGuardar.Wait();

                OnPostGuardarDetalles();

                HttpContext.Session.Remove("Cesta");

                TempData["Mensaje"] = "Compra realizada exitosamente.";
                RedirectToPage("/Ventanas/Videojuegos");
                return;
            }
            catch (Exception ex)
            {
                LogConversor.Log(ex, ViewData!);
                TempData["Mensaje"] = "Error al confirmar la compra.";
            }
            
        }

        public void OnPostGuardarDetalles()
        {
            try
            {
                var cesta = HttpContext.Session.GetObjectFromJson<List<DetallesCompras>>("Cesta");



                var ultimaCompra = this.IcomprasPresentacion!.PorCliente(Compra);
                ultimaCompra.Wait();
                var idCompra = ultimaCompra.Result.OrderByDescending(c => c.Id).FirstOrDefault()!.Id;


                foreach (var detalle in cesta!)
                {
                    detalle.Compra = idCompra;
                    detalle._Videojuego = null;
                    var guardarDetalle = this.IPresentacionDetalles!.Guardar(detalle);
                    guardarDetalle.Wait();
                }
                
                return;
            }
            catch (Exception ex)
            {
                LogConversor.Log(ex, ViewData!);
                TempData["Mensaje"] = "Error al confirmar la compra.";
            }
        }
    }
}
