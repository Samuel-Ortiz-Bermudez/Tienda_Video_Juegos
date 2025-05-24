using System.Data.Common;
using lib_dominio.Entidades;
using lib_dominio.Nucleo;
using lib_presentaciones.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace asp_presentacion.Pages.Ventanas.AccionesEmpleados
{
    [Authorize(Roles ="Admin")]
    public class OperarEmpleadosModel : PageModel
    {
        private IEmpleadosPresentacion? IPresentacionEmpleados = null;
        private ICuentasEmpleadosPresentacion? IPresentacionCuentas = null;
        
        public OperarEmpleadosModel(IEmpleadosPresentacion IPresentacionEmpleados, ICuentasEmpleadosPresentacion IPresentacionCuentas)
        {
            try
            {
                this.IPresentacionCuentas = IPresentacionCuentas;
                this.IPresentacionEmpleados = IPresentacionEmpleados;
            }catch(Exception ex)
            {
                LogConversor.Log(ex, ViewData!);
            }
        }

        [BindProperty] public List<Empleados>? ListaEmpleados { get; set; }
        [BindProperty] public Empleados? ActualEmpleado { get; set; }
        [BindProperty] public List<CuentasEmpleados>? ListaCuentas { get; set; }
        [BindProperty] public CuentasEmpleados? ActualCuenta { get; set; }
        [BindProperty] public string? Mensage { get; set; }
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
                var EmpleadosTask = this.IPresentacionEmpleados!.Listar();
                EmpleadosTask.Wait(); 
                var CuentasTask = this.IPresentacionCuentas!.Listar();
                CuentasTask.Wait();

                ListaCuentas = CuentasTask.Result.Where(x => x.Correo!.ToUpper() != "ADMIN@TIENDA.COM").ToList();
                ListaEmpleados = EmpleadosTask.Result.Where( x => x.Nombre!.ToUpper() != "ADMIN").ToList();
            } catch(Exception ex)
            {
                LogConversor.Log(ex, ViewData!);
            }
        }

        public void OnPostBtnNuevo()
        {
            try
            {
                Accion = Enumerables.Ventanas.Crear;
                ActualCuenta = new CuentasEmpleados();
                ActualEmpleado = new Empleados();
            }catch(Exception ex)
            {
                LogConversor.Log(ex, ViewData!);
            }
        }

        public void OnPostBtnEditar(string IdEmpleado, string IdCuenta)
        {
            try
            {
                OnPostIngreso();
                Accion = Enumerables.Ventanas.Editar;
                ActualCuenta = ListaCuentas!.FirstOrDefault(x => x.Id.ToString().Equals(IdCuenta));
                ActualEmpleado = ListaEmpleados!.FirstOrDefault(x => x.Id.ToString().Equals(IdEmpleado));
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
                Task <Empleados>? EmpleadoTask = null;
                Task <CuentasEmpleados>? CuentaTask = null;

                if (ActualEmpleado == null)
                {
                    return;
                }

                if (ActualEmpleado!.Id == 0 && ActualCuenta!.Id == 0)
                {
                    EmpleadoTask = this.IPresentacionEmpleados!.Guardar(ActualEmpleado!)!;
                } else {
                    EmpleadoTask = this.IPresentacionEmpleados!.Modificar(ActualEmpleado!)!;
                }
                EmpleadoTask.Wait();

                ActualCuenta!.Empleado = ActualEmpleado.Id;

                if(ActualCuenta!.Id == 0)
                {
                    CuentaTask = this.IPresentacionCuentas!.Guardar(ActualCuenta!)!;
                } else {
                    CuentaTask = this.IPresentacionCuentas!.Modificar(ActualCuenta!)!;
                }

                CuentaTask.Wait();
                Accion = Enumerables.Ventanas.Listas;
                OnPostIngreso();
            }
            catch(Exception ex)
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
