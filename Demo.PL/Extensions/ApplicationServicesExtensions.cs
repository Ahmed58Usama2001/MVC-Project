using Demo.BLL.Interfaces;
using Demo.BLL.Repositories;

namespace Demo.PL.Extensions
{
    public static class ApplicationServicesExtensions
    {
        public static void AddAplicationServices(this IServiceCollection services)
        {
            //services.AddScoped<IDepartmentRepository, DepartmentRepository>();
            //services.AddScoped<IEmployeeRepository, EmployeeRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
        }
    }
}
