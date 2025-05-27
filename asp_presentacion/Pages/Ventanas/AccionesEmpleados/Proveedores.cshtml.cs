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

    public class ProveedoresModel : PageModel
    {
        private IProveedoresPresentacion? iPresentacionProveedor = null;

        public ProveedoresModel(IProveedoresPresentacion? iPresentacionProveedor)
        {
            this.iPresentacionProveedor = iPresentacionProveedor;
            Proveedor = new Proveedores();
        }

        [BindProperty] public Enumerables.Ventanas Accion { get; set; }
        [BindProperty] public Proveedores? Proveedor { get; set; }
        [BindProperty] public List<Proveedores>? Lista { get; set; }

        public void OnGet()
        {
            OnPostIngreso();
        }

        public void OnPostIngreso()
        {
            try
            {
                Accion = Enumerables.Ventanas.Listas;

                var proveedoresTask = iPresentacionProveedor!.Listar();
                proveedoresTask.Wait();
                Lista = proveedoresTask.Result;
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
                Proveedor = Lista!.FirstOrDefault(p => p.Id == id);
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

                Task<Proveedores>? guardarProveedor = null;
                if (Proveedor!.Id == 0)
                {
                    guardarProveedor = this.iPresentacionProveedor!.Guardar(Proveedor!)!;
                }
                else
                {
                    guardarProveedor = this.iPresentacionProveedor!.Modificar(Proveedor!)!;
                }
                guardarProveedor.Wait();
                Proveedor = guardarProveedor.Result;

                Accion = Enumerables.Ventanas.Listas;
                OnPostIngreso();
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
