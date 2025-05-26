using lib_dominio.Entidades;
using lib_dominio.Nucleo;
using lib_presentaciones.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Identity.Client;
using Microsoft.IdentityModel.Tokens;

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
        [BindProperty] public string? Mensaje { get; set; }
        public Compras? Compra { get; set; }
        public decimal Total { get; set; }
        public bool CantidadBool { get; set; }

        public void OnGet()
        {
            try
            {
                Cesta = HttpContext.Session.GetObjectFromJson<List<DetallesCompras>>("Cesta") ?? new List<DetallesCompras>();
                if (Cesta.IsNullOrEmpty())
                {
                    HttpContext.Response.Redirect("/Ventanas/Videojuegos");
                }
                Total = Cesta.Sum(c => c.Subtotal);
                OnPostCargarSelect();
            }
            catch(Exception ex)
            {
                LogConversor.Log(ex, ViewData!);
            }
            
        }

        public void OnPostCargarSelect()
        {

            try
            {
                var taskEmpleados = this.IPresentacionEmpleados!.Listar();
                taskEmpleados.Wait();

                ListaEmpleados = taskEmpleados.Result.Where(x => x.Nombre != "Admin").ToList();
            }
            catch (Exception ex)
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
                    Mensaje = "Cantidad actualizada en la cesta.";

                }
                else
                {
                    Mensaje = "Juego no encontrado en la cesta.";
                }
            }
            catch (Exception ex)
            {
                LogConversor.Log(ex, ViewData!);
                Mensaje = "Error al actualizar la cesta.";
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
                    Mensaje = "No hay productos en la cesta.";
                    return;
                }
                
                OnPostValidacionCantidad();
                
                if ( CantidadBool == false) return;
                

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
                OnPostActualizarInventario();
                HttpContext.Session.Remove("Cesta");

                Mensaje = "Compra realizada exitosamente.";
                return;
            }
            catch (Exception ex)
            {
                LogConversor.Log(ex, ViewData!);
                Mensaje = "Error al confirmar la compra.";
            }
            
        }

        public void OnPostValidacionCantidad()
        {
            try
            {
                var cesta = HttpContext.Session.GetObjectFromJson<List<DetallesCompras>>("Cesta");

                var InventariosTask = this.IPresentacionInvetarios!.Listar();
                InventariosTask.Wait();
                var InventarioLista = InventariosTask.Result;

                foreach (var detalle in cesta!)
                {
                    var stock = InventarioLista.FirstOrDefault(x => x.Videojuego == detalle.Videojuego);
                    if (stock == null)
                    {
                        Mensaje = "No hay stock de " + detalle._Videojuego!.Nombre;
                        CantidadBool = false;
                        return;
                    }
                    if (stock.Cantidad <= 0 )
                    {
                        Mensaje = "No hay stock de " + detalle._Videojuego!.Nombre;
                        CantidadBool = false;
                        return;
                    }
                    if (detalle.Cantidad > stock.Cantidad)
                    {
                        Mensaje = "No hay suficiente stock de " + detalle._Videojuego!.Nombre;
                        CantidadBool = false;
                        return;
                    }
                }
                CantidadBool = true;
                return;
            }
            catch (Exception ex)
            {
                LogConversor.Log(ex, ViewData!);
                Mensaje = "Error al confirmar la compra.";
                CantidadBool = false;
                return;
            }
        }
        public void OnPostActualizarInventario()
        {
            try
            {
                var cesta = HttpContext.Session.GetObjectFromJson<List<DetallesCompras>>("Cesta");

                var InventariosTask = this.IPresentacionInvetarios!.Listar();
                InventariosTask.Wait();
                var InventarioLista = InventariosTask.Result;

                foreach (var detalle in cesta!)
                {
                    var stock = InventarioLista.FirstOrDefault(x => x.Videojuego == detalle.Videojuego);
                    stock!.Cantidad = stock.Cantidad - detalle!.Cantidad;
                    
                    var ActualizacionTask = this.IPresentacionInvetarios!.Modificar(stock!);
                    ActualizacionTask.Wait();
                }
                CantidadBool = true;
                return;
            }
            catch (Exception ex)
            {
                LogConversor.Log(ex, ViewData!);
                Mensaje = "Error al confirmar la compra.";
                CantidadBool = false;
                return;
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
                Mensaje = "Error al confirmar la compra.";
            }
        }

        
    }
}
