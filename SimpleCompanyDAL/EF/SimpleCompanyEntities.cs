using System.Data.Entity;
using SimpleCompanyDAL.Models;

namespace SimpleCompanyDAL.EF
{
    public class SimpleCompanyEntities : DbContext
    {
        public SimpleCompanyEntities()
            : base("name=SimpleCompanyEntities")
        {
        }

        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Department> Departments { get; set; }
    }
}