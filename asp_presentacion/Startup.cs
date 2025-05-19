using lib_presentaciones.Implementaciones;
using asp_presentacion.Datos;
using lib_presentaciones.Interfaces;
using Microsoft.EntityFrameworkCore;



namespace asp_presentacion
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public static IConfiguration? Configuration { set; get; }

        public void ConfigureServices(WebApplicationBuilder builder, IServiceCollection services)
        {
            // Presentaciones
            services.AddDbContext<ApplicationDbContext>(option => option.UseSqlServer(Configuration.GetConnectionString("TiendaConnection")));
            services.AddScoped<IClientesPresentacion, ClientesPresentacion>();
            services.AddScoped<IComprasPresentacion, ComprasPresentacion>();
            services.AddScoped<IDetallesComprasPresentacion, DetallesComprasPresentacion>();
            services.AddScoped<IEmpleadosPresentacion, EmpleadosPresentacion>();
            services.AddScoped<IInventariosPresentacion, InventariosPresentacion>();
            services.AddScoped<IProveedoresPresentacion, ProveedoresPresentacion>();
            services.AddScoped<ISuministrosPresentacion, SuministrosPresentacion>();
            services.AddScoped<IVideojuegosPresentacion, VideojuegosPresentacion>();
            services.AddScoped<ICuentasClientesPresentacion, CuentasClientesPresentacion>();
            services.AddScoped<ICuentasEmpleadosPresentacion, CuentasEmpleadosPresentacion>();

            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddRazorPages();
            
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30);
            });
        }

        public void Configure(WebApplication app, IWebHostEnvironment env)
        {
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
            }
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthorization();
            app.MapRazorPages();
            app.UseSession();
            app.Run();
        }
    }
}