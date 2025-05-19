using lib_dominio.Nucleo;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;

namespace asp_presentacion.Pages.Ventanas.Perfiles
{
    [Authorize]
    public class EmpleadosPerfilModel : PageModel
    {
        public void OnGet()
        {
            OnPostIngreso();
        }

        [BindProperty] public Enumerables.Ventanas Accion { get; set; }

        public void OnPostIngreso()
        {
            try
            {
                Accion = Enumerables.Ventanas.Listas;
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
