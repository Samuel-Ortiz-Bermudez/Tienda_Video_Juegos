using lib_dominio.Nucleo;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;


namespace asp_presentacion.Pages.Ventanas.AccionesClientes
{
    public class AñadirCestaModel : PageModel
    {
        public void OnPostAñadirACesta(int juegoId)
        {
            try
            {
                
                //var juego = ListaJuegos?.FirstOrDefault(j => j.Id == juegoId);
                //if (juego != null)
                {
                    
                }
            }
            catch (Exception ex)
            {
                // Maneja el error de forma adecuada
                LogConversor.Log(ex, ViewData!);
            }
        }

    }

}

