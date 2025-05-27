using lib_dominio.Entidades;
using lib_dominio.Nucleo;
using lib_presentaciones.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace asp_presentacion.Pages.Ventanas.Perfiles
{
    [Authorize(Roles = "Admin,Empleado")]

    public class SuministrosModel : PageModel
    {
        

        private ISuministrosPresentacion? iPresentacionSuministro = null;
        private IProveedoresPresentacion? iPresentacionProveedor = null;
        private IVideojuegosPresentacion? iPresentacionVideojuego = null;

        public SuministrosModel(ISuministrosPresentacion? iPresentacionSuministro, IProveedoresPresentacion? iPresentacionProveedor,
        IVideojuegosPresentacion? iPresentacionVideojuego)
        {
            this.iPresentacionSuministro = iPresentacionSuministro;
            this.iPresentacionProveedor = iPresentacionProveedor;
            this.iPresentacionVideojuego = iPresentacionVideojuego;
            Suministro = new Suministros();
            
        }

        [BindProperty] public Enumerables.Ventanas Accion { get; set; }
        [BindProperty] public Suministros? Suministro { get; set; }
        [BindProperty] public List<Suministros>? Lista { get; set; }
        [BindProperty] public List<Proveedores>? ListaProveedores { get; set; }
        [BindProperty] public List<Videojuegos>? ListaVideojuegos { get; set; }


        public void OnGet()
        {
            OnPostIngreso();
        }

        public void OnPostIngreso()
        {
            try
            {
                Accion = Enumerables.Ventanas.Listas;

                var SuministrosTask = iPresentacionSuministro!.Listar();
                var ProveedoresTask = iPresentacionProveedor!.Listar();
                var VideojuegosTask = iPresentacionVideojuego!.Listar();

                Task.WaitAll(SuministrosTask, ProveedoresTask, VideojuegosTask);

                Lista = SuministrosTask.Result;
                ListaProveedores = ProveedoresTask.Result;
                ListaVideojuegos = VideojuegosTask.Result;
                foreach (var s in Lista!)
                {
                    s._Proveedor = ListaProveedores!.FirstOrDefault(p => p.Id == s.Proveedor);
                    s._Videojuego = ListaVideojuegos!.FirstOrDefault(v => v.Id == s.Videojuego);
                }
            }
            catch (Exception ex)
            {
                LogConversor.Log(ex, ViewData!);
            }
        }


        public void OnPostBtnEditar(int id)
        {
            try
            {
                OnPostIngreso();
                Accion = Enumerables.Ventanas.Editar;
                Suministro = Lista!.FirstOrDefault(p => p.Id == id);
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
                var guardarSuministro = iPresentacionSuministro!.Modificar(Suministro);
                guardarSuministro.Wait();
                Suministro = guardarSuministro.Result;

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

        public async Task<IActionResult> OnPostBtnCerrarSesion()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToPage("/Ventanas/Videojuegos");
        }
    }
}
