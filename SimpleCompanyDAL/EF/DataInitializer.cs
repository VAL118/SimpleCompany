using System.Collections.Generic;
using System.Data.Entity;
using SimpleCompanyDAL.Models;

namespace SimpleCompanyDAL.EF
{
    // Initialization of the database with test data.
    public class DataInitializer : DropCreateDatabaseAlways<SimpleCompanyEntities>
    {
        protected override void Seed(SimpleCompanyEntities context)
        {
            var departments = new List<Department>
            {
                new Department {Name = "Development"},
                new Department {Name = "Analytics"},
                new Department {Name = "Testing"},
            };
            departments.ForEach(x => context.Departments.Add(x));

            var users = new List<User>
            {
                new User {FirstName = "Vasily", LastName = "Petrov", Department = departments[0]},
                new User {FirstName = "Petr", LastName = "Ivanov", Department = departments[0]},
                new User {FirstName = "Ivan", LastName = "Fedorov", Department = departments[0]},
                new User {FirstName = "Anton", LastName = "Pavlov", Department = departments[1]},
                new User {FirstName = "Pavel", LastName = "Durov", Department = departments[1]},
                new User {FirstName = "Andrey", LastName = "Komarov", Department = departments[2]},
            };
            users.ForEach(x => context.Users.Add(x));

            context.SaveChanges();
        }
    }
}
