using Microsoft.EntityFrameworkCore;

namespace FluidFrameworkDemo.Models
{
    public class EmployeeContext: DbContext
    {
        public EmployeeContext(DbContextOptions<EmployeeContext> record) : base(record)
        {

        }

        public DbSet<Employee> Employees { get; set; }
    }
}
