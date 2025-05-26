using lib_dominio.Entidades;
using lib_dominio.Nucleo;
using lib_presentaciones.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace asp_presentacion.Pages.Ventanas
{
    public class InventariosModel : PageModel
    {
        [BindProperty]
        public Inventarios NuevoInventario { get; set; } = new Inventarios();

        private readonly IInventariosPresentacion _InventariosPresentacion;

        public InventariosModel(IInventariosPresentacion InventariosPresentacion)
        {
            _InventariosPresentacion = InventariosPresentacion;
        }

        [BindProperty]
        public List<Inventarios>? ListaInventarios { get; set; }

        [BindProperty]
        public string? Mensaje { get; set; }

        public void OnGet()
        {
            CargarInventarios();
        }

        private void CargarInventarios()
        {
            try
            {
                var tarea = _InventariosPresentacion.Listar();
                tarea.Wait();
                ListaInventarios = tarea.Result;

                // Opcional: filtrar solo juegos activos si deseas
                // ListaInventarios = ListaInventarios.Where(i => i._Videojuego?.Estado == true).ToList();
            }
            catch (Exception ex)
            {
                LogConversor.Log(ex, ViewData!);
                Mensaje = "Error al cargar Inventarios.";
            }
        }

        public IActionResult OnPostActualizarStock(string Codigo, int NuevaCantidad)
        {
            try
            {
                var tarea = _InventariosPresentacion.PorCodigo(new Inventarios { Codigo = Codigo });
                tarea.Wait();

                var Inventarios = tarea.Result.FirstOrDefault();
                if (Inventarios == null)
                {
                    Mensaje = "Registro no encontrado.";
                    return Page();
                }

                Inventarios.Cantidad = NuevaCantidad;
                var actualizar = _InventariosPresentacion.Modificar(Inventarios);
                actualizar.Wait();

                Mensaje = "Stock actualizado correctamente.";
            }
            catch (Exception ex)
            {
                LogConversor.Log(ex, ViewData!);
                Mensaje = "Error al actualizar el stock.";
            }

            CargarInventarios();
            return Page();
        }

        public IActionResult OnPostCrearInventario()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(NuevoInventario.Codigo) || NuevoInventario.Videojuego == 0)
                {
                    Mensaje = "Todos los campos son requeridos.";
                    CargarInventarios();
                    return Page();
                }

                var crear = _InventariosPresentacion.Guardar(NuevoInventario);
                crear.Wait();

                Mensaje = "Inventario creado exitosamente.";
            }
            catch (Exception ex)
            {
                LogConversor.Log(ex, ViewData!);
                Mensaje = "Error al crear el inventario.";
            }

            CargarInventarios();
            return Page();
        }

    }
}
