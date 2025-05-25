using lib_dominio.Entidades;
using lib_dominio.Nucleo;
using lib_presentaciones.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace asp_presentacion.Pages.Ventanas.AccionesEmpleados
{
    [Authorize(Roles ="Admin")]
    public class AuditoriasModel : PageModel
    {
        private IAuditoriasPresentacion? IPresentacionAuditorias = null;
        public AuditoriasModel(IAuditoriasPresentacion IPresentacionAuditorias)
        {
            try
            {
                this.IPresentacionAuditorias = IPresentacionAuditorias;
            }catch(Exception ex)
            {
                LogConversor.Log(ex, ViewData!);
            }
        }

        [BindProperty] public List<Auditorias>? ListaAuditorias { get; set; }
        [BindProperty] public List<Auditorias>? ListaAuditoriasTabla { get; set; }
        [BindProperty] public List<Auditorias>? ListaAuditoriasAccion { get; set; }
        [BindProperty] public string? FiltroTabla { get; set; }
        [BindProperty] public string? FiltroAccion { get; set; }
        [BindProperty] public string? Mensaje { get; set; }
        [BindProperty] public Enumerables.Ventanas Accion { get; set; }
        public void OnGet()
        {
            OnPostIngreso();
        }

        public void OnPostIngreso()
        {
            try
            {
                Accion = Enumerables.Ventanas.Listas;
                var taskAuditorias = this.IPresentacionAuditorias!.Listar();
                taskAuditorias.Wait();

                ListaAuditorias = taskAuditorias.Result;
            }
            catch (Exception ex)
            {
                LogConversor.Log(ex, ViewData!);
            }
        }

        public void OnPostBtnFiltroTabla()
        {
            try
            {
                OnPostIngreso();
                Accion = Enumerables.Ventanas.FiltroTabla;

                if (FiltroTabla == null)
                {
                    Mensaje = "No existe esa tabla.";
                    return; 
                }
                ListaAuditoriasTabla = ListaAuditorias!.Where(x => x.Tabla!.ToUpper().Equals(FiltroTabla!.ToUpper())).ToList();
            }
            catch (Exception ex)
            {
                LogConversor.Log(ex, ViewData!);
            }
        }

        public void OnPostBtnFiltroAccion()
        {
            try
            {
                OnPostIngreso();
                Accion = Enumerables.Ventanas.FiltroAccion;

                if (FiltroAccion == null)
                {
                    Mensaje = "No existe esa accion.";
                    return;
                }

                ListaAuditoriasAccion = ListaAuditorias!.Where(x => x.Accion!.ToUpper().Equals(FiltroAccion!.ToUpper())).ToList();
            }
            catch (Exception ex)
            {
                LogConversor.Log(ex, ViewData!);
            }
        }
    }
}
