using lib_dominio.Entidades;
using lib_dominio.Nucleo;
using lib_presentaciones.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace asp_presentacion.Pages.Ventanas.AccionesEmpleados
{
    [Authorize]
    public class OperarVideojuegosModel : PageModel
    {
        private IVideojuegosPresentacion? IPresentacionJuegos = null;
        public OperarVideojuegosModel(IVideojuegosPresentacion IPresentacionJuegos)
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

        [BindProperty] public Videojuegos? Juego { get; set; }
        [BindProperty] public Videojuegos? Actual { get; set; }
        [BindProperty] public List<Videojuegos>? ListaJuegos{ get; set; }
        [BindProperty] public Enumerables.Ventanas Accion { get; set; }
        
        [BindProperty] public string? mensaje{ get; set; }

        public void OnPost()
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
                if (!ListaJuegos.Any())
                {
                    mensaje = "No hay juegos en la base de datos.";
                    return;
                }
                Actual = null;
            }
            catch (Exception ex)
            {
                LogConversor.Log(ex, ViewData!);
            }
        }

        public void OnPostBtnNuevo()
        {
            try
            {
                Accion = Enumerables.Ventanas.Crear;

                Actual = new Videojuegos();
            }
            catch (Exception ex)
            {
                LogConversor.Log(ex, ViewData!);
            }
        }
        public void OnPostBtnEditar(string data)
        {
            try
            {
                OnPostIngreso();
                Accion = Enumerables.Ventanas.Editar;
                Actual = ListaJuegos!.FirstOrDefault(j => j.Id.ToString() == data);
            }
            catch (Exception ex)
            {
                LogConversor.Log(ex, ViewData!);
            }
        }
        public void OnPostBtnBorrar(string data)
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
        public void OnPostBtnGuardar()
        {
            try
            {
                Task<Videojuegos>? juegosTask = null;
                if (Actual!.Id == 0)
                {
                    juegosTask = this.IPresentacionJuegos!.Guardar(Actual!)!;
                } else
                {
                    juegosTask = this.IPresentacionJuegos!.Modificar(Actual!)!;
                }
                juegosTask.Wait();
                Accion = Enumerables.Ventanas.Listas;
                OnPostIngreso();
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

    }
}
