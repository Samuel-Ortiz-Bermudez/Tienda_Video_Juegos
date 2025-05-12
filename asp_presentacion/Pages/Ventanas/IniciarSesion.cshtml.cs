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

        public void OnPostBtnInicio()
        {
            try
            { 
                Email = string.Empty; 
                Contrasena = string.Empty; 
            }
            catch (Exception ex) { 
                LogConversor.Log(ex, ViewData!); 
            }
        }

        public void OnPostBtEnter()
        {
            try
            {
                if (string.IsNullOrEmpty(Email) && string.IsNullOrEmpty(Contrasena)) 
                {
                    OnPostBtnInicio(); 
                    return; 
                }

                //Realizar la tabla de usuarios y realizar las validaciones
                if ("admin@hola.com,123" != Email + "," + Contrasena) 
                {
                    OnPostBtnInicio(); 
                    return; 
                }
                ViewData["Logged"] = true; 
                HttpContext.Session.SetString("Usuario", Email!); 
                EstaLogueado = true;
                OnPostBtnInicio();
            }
            catch (Exception ex) { 
                LogConversor.Log(ex, ViewData!); 
            }
        }
    }
}
