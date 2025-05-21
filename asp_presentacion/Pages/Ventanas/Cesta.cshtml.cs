using lib_dominio.Entidades;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace asp_presentacion.Pages.Ventanas
{
    public class CestaModel : PageModel
    {
        [BindProperty]
        public List<DetallesCompras> Cesta { get; set; } = new();

        public void OnGet()
        {
            Cesta = HttpContext.Session.GetObjectFromJson<List<DetallesCompras>>("Cesta") ?? new List<DetallesCompras>();
        }
    }

}
