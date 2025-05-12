using lib_dominio.Nucleo;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace asp_presentacion.Pages.Ventanas
{
    public class IniciarSesionModel : PageModel
    {
        public bool EstaLogueado = false; 
        [BindProperty] public string? Email { get; set; }
        [BindProperty] public string? Contrasena { get; set; }
        public void OnGet() 
        { 
            var variable_session = HttpContext.Session.GetString("Usuario"); 
            if (!String.IsNullOrEmpty(variable_session)) 
            { EstaLogueado = true; return; } 
        }

        public void OnPostBtnClean()
        {
            try
            {
                Email = string.Empty;
                Contrasena = string.Empty;
            }
            catch (Exception ex)
            {
                LogConversor.Log(ex, ViewData!);
            }
        }

        public void OnPostBtnInicio()
        {
            try
            {
                if (string.IsNullOrEmpty(Email) && string.IsNullOrEmpty(Contrasena)) 
                {
                    OnPostBtnClean(); 
                    return; 
                }

                Console.WriteLine(Email);
                Console.WriteLine(Contrasena);

                //Realizar la tabla de usuarios y realizar las validaciones
                if ("admin@hola.com,123" != Email + "," + Contrasena) 
                {
                    OnPostBtnClean(); 
                    return; 
                }
                ViewData["Logged"] = true; 
                HttpContext.Session.SetString("Usuario", Email!); 
                EstaLogueado = true;
                OnPostBtnClean();
            }
            catch (Exception ex) { 
                LogConversor.Log(ex, ViewData!); 
            }
        }
    }
}
