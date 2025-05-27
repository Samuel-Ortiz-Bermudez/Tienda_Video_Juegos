using System;
using lib_dominio.Entidades;
using lib_dominio.Nucleo;
using lib_presentaciones.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace asp_presentacion.Pages.Ventanas
{
    [Authorize(Roles ="Admin, Empleado")]
    public class InventariosModel : PageModel
    {
        [BindProperty]
        public Inventarios? NuevoInventario { get; set; } = new Inventarios();

        private readonly IInventariosPresentacion? _InventariosPresentacion;
        private readonly IVideojuegosPresentacion? _PresentacionJuegos;

        public InventariosModel(IInventariosPresentacion InventariosPresentacion, IVideojuegosPresentacion _PresentacionJuegos)
        {
            this._InventariosPresentacion = InventariosPresentacion;
            this._PresentacionJuegos = _PresentacionJuegos;
        }

        [BindProperty]
        public List<Inventarios>? ListaInventarios { get; set; }
        [BindProperty]
        public List<Videojuegos>? ListaJuegos { get; set; }
        [BindProperty]
        public Enumerables.Ventanas Accion { get; set; }
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
                var tarea = _InventariosPresentacion!.Listar();
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
                var tarea = _InventariosPresentacion!.PorCodigo(new Inventarios { Codigo = Codigo });
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

        public void OnPostMostrarCrear()
        {
            try
            {
                Accion = Enumerables.Ventanas.Crear;
                var taskjuegos = this._PresentacionJuegos!.Listar();
                taskjuegos.Wait();
                ListaJuegos = taskjuegos.Result;
            }
            catch (Exception ex)
            {
                LogConversor.Log(ex, ViewData!);
            }
        }

        public IActionResult OnPostCrearInventario()
        {
            try
            {
                if (NuevoInventario!.Videojuego == 0)
                {
                    Mensaje = "Todos los campos son requeridos.";
                    CargarInventarios();
                    return Page();
                }



                var crear = _InventariosPresentacion!.Guardar(NuevoInventario);
                crear.Wait();

                Mensaje = "Inventario creado exitosamente.";
                Accion = Enumerables.Ventanas.Listas;
            }
            catch (Exception ex)
            {
                LogConversor.Log(ex, ViewData!);
                Mensaje = "Error al crear el inventario.";
            }

            CargarInventarios();
            return Page();
        }

        public void OnPostBtnCerrar()
        {
            try
            {
                Accion = Enumerables.Ventanas.Listas;
                CargarInventarios();
            }
            catch (Exception ex)
            {
                LogConversor.Log(ex, ViewData!);
            }
        }

    }
}
