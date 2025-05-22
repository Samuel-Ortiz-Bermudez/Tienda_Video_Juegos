using lib_dominio.Entidades;
using lib_dominio.Nucleo;
using lib_presentaciones.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace asp_presentacion.Pages.Ventanas
{


    public class CestaModel : PageModel

    {

        [BindProperty]
        public List<DetallesCompras> Cesta { get; set; } = new();
        public decimal Total { get; set; }

        public void OnGet()
        {
            
            Cesta = HttpContext.Session.GetObjectFromJson<List<DetallesCompras>>("Cesta") ?? new List<DetallesCompras>();
            Total = Cesta.Sum(c => c.Subtotal);
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

        public async Task<IActionResult> OnPostConfirmarCompraAsync()
        {
            try
            {
                var cesta = HttpContext.Session.GetObjectFromJson<List<DetallesCompras>>("Cesta");
                if (cesta == null || !cesta.Any())
                {
                    TempData["Mensaje"] = "No hay productos en la cesta.";
                    return RedirectToPage();
                }

                var compra = new Compras
                {
                    FechaVenta = DateTime.Now,
                    MetodoPago = MetodoSeleccionado,
                    Cliente = clienteId,
                    Empleado = empleadoId,
                    DetallesCompra = Cesta
                };

                compra.CalculoTotal();

                var resultado = await _comprasPresentacion.Guardar(compra);

                // Limpia la sesión
                HttpContext.Session.Remove("Cesta");

                TempData["Mensaje"] = "Compra realizada exitosamente.";
            }
            catch (Exception ex)
            {
                LogConversor.Log(ex, ViewData!);
                TempData["Mensaje"] = "Error al confirmar la compra.";
            }

            return RedirectToPage("/Ventanas/Videojuegos");
        }
    }


}
}