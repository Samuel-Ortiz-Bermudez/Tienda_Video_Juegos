using asp_presentacion.Datos;
using lib_dominio.Entidades;
using lib_repositorios.Implementaciones;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;


namespace asp_presentacion.Pages.Ventanas
{
    public class VideojuegosModel : PageModel
    {

        private readonly ApplicationDbContext _context;

        public VideojuegosModel(ApplicationDbContext context)
        {
            _context = context;
        }
        public List<Videojuegos> ListaVideojuegos { get; set; } = new();

        [BindProperty(SupportsGet = true)]
        public string? Desarrolladora { get; set; }

        public async Task OnGetAsync()
        {

            var query = _context.Videojuegos!.AsQueryable();
            if (!string.IsNullOrEmpty(Desarrolladora))
                query = query.Where(v => v.Desarrolladora == Desarrolladora);

            ListaVideojuegos = await query.ToListAsync();

        }
    }
}
