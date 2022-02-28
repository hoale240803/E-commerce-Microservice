using Microsoft.EntityFrameworkCore;
using WebAppRazor.Data;
using WebAppRazor.Repositories;

namespace WebAppRazor
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            #region database services

            //// use in-memory database
            //services.AddDbContext<AspnetRunContext>(c =>
            //    c.UseInMemoryDatabase("AspnetRunConnection"));

            // add database dependecy
            services.AddDbContext<WebAppRazorContext>(c =>
                c.UseSqlServer(Configuration.GetConnectionString("WebAppRazorConnection")));

            #endregion database services


            #region seed data

            #endregion



            #region project services

            // add repository dependecy
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<ICartRepository, CartRepository>();
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<IContactRepository, ContactRepository>();

            // for call api gateway
            //services.AddHttpClient<ICatalogService, CatalogService>(c =>
            //     c.BaseAddress = new Uri(Configuration["ApiSettings:GatewayAddress"]));
            //    services.AddHttpClient<IBasketService, BasketService>(c =>
            //    c.BaseAddress = new Uri(Configuration["ApiSettings:GatewayAddress"]));
            //services.AddHttpClient<IOrderService, OrderService>(c =>
            //    c.BaseAddress = new Uri(Configuration["ApiSettings:GatewayAddress"]));

            #endregion project services

            services.AddRazorPages();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
            });
        }
    }
}