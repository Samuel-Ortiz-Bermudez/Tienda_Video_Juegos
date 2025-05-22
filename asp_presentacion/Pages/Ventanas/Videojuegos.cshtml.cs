using lib_dominio.Entidades;
using lib_dominio.Nucleo;
using lib_presentaciones.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace asp_presentacion.Pages.Ventanas
{
    public class VideojuegosModel : PageModel
    {
        private IVideojuegosPresentacion? IPresentacionJuegos = null;
        public VideojuegosModel(IVideojuegosPresentacion IPresentacionJuegos)
        {
            try
            {
                this.IPresentacionJuegos = IPresentacionJuegos;
                Juego = new Videojuegos();

            }
            catch (Exception ex)
            {
                LogConversor.Log(ex, ViewData!);
            }
        }

        [BindProperty] public Enumerables.Ventanas Accion { get; set; }
        [BindProperty] public Videojuegos? Juego { get; set; }
        [BindProperty] public List<Videojuegos>? ListaJuegos { get; set; }
        [BindProperty] public List<Videojuegos>? ListaFiltrada { get; set; }
        [BindProperty] public string? Desarrolladora { get; set; }
        [BindProperty] public string? Mensaje { get; set; }

        public void OnGet()
        {
            OnPostIngreso();
        }

        public void OnPostIngreso()
        {
            try
            {
                Accion = Enumerables.Ventanas.Listas;
                var juegosTask = this.IPresentacionJuegos!.Listar();
                juegosTask.Wait();

                ListaJuegos = juegosTask.Result;
            }
            catch (Exception ex)
            {
                LogConversor.Log(ex, ViewData!);
            }
        }

        public void OnPostBtnFiltro()
        {
            try
            {
                Accion = Enumerables.Ventanas.Filtro;
                Desarrolladora = this.Desarrolladora;
                if (Desarrolladora == "Todas")
                {
                    OnPostIngreso();
                    return;
                }
                var juegosTask = this.IPresentacionJuegos!.Listar();
                juegosTask.Wait();

                ListaJuegos = juegosTask.Result;

                ListaFiltrada = ListaJuegos.Where(j => j.Desarrolladora!.ToUpper().Equals(Desarrolladora!.ToUpper())).ToList();
                if (!ListaFiltrada.Any())
                {
                    OnPostIngreso();
                    Mensaje = "No hay videojuegos de esta desarrolladora";
                    return;
                }
            }
            catch (Exception ex)
            {
                LogConversor.Log(ex, ViewData!);
            }
        }


        public IActionResult OnPostAgregarACesta(int id)
        {
            try
            {
                var juegoTask = IPresentacionJuegos!.PorCodigo(new Videojuegos { Id = id });
                juegoTask.Wait();
                var juegoCesta = juegoTask.Result.FirstOrDefault();

                if (juegoCesta == null)
                {
                    Mensaje = "Juego no encontrado.";
                    return RedirectToPage();
                }

                // se obtiene la cesta de la sesión
                var cesta = HttpContext.Session.GetObjectFromJson<List<DetallesCompras>>("Cesta") ?? new List<DetallesCompras>();


                var existente = cesta.FirstOrDefault(d => d.Videojuego == id);
                if (existente != null)
                {
                    existente.Cantidad++;
                    existente.CalculoSubtotal();
                }
                else
                {
                    var nuevoDetalle = new DetallesCompras
                    {
                        Videojuego = juegoCesta.Id,
                        _Videojuego = juegoCesta,
                        Cantidad = 1,
                        Subtotal = juegoCesta.Precio
                    };
                    nuevoDetalle.CalculoSubtotal();
                    cesta.Add(nuevoDetalle);
                }


                HttpContext.Session.SetObjectAsJson("Cesta", cesta);
                TempData["Mensaje"] = "Agregado a la cesta.";
            }
            catch (Exception ex)
            {
                LogConversor.Log(ex, ViewData!);
                TempData["Mensaje"] = "Error al agregar a la cesta.";
            }

            return RedirectToPage();

        }
    }
}