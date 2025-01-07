using BLL.DAL;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using BLL.Controllers.Bases;

namespace MVC.Controllers
{
    public class DbController : MvcController
    {
        private readonly Db _db;

        public DbController(Db db)
        {
            _db = db;
        }

        public IActionResult Seed()
        {
            _db.Users.RemoveRange(_db.Users.ToList());
            _db.Roles.RemoveRange(_db.Roles.ToList());

            _db.Roles.Add(new Role()
            {
                Name = "Admin",
                Users = new List<User>()
                {
                    new User()
                    {
                        UserName = "admin",
                        Password = "admin",
                        IsActive = true
                    }
                }
            });
            _db.Roles.Add(new Role()
            {
                Name = "User",
                Users = new List<User>()
                {
                    new User()
                    {
                        UserName = "user",
                        Password = "user",
                        IsActive = true
                    }
                }
            });

            _db.SaveChanges();

            return Content("<label style=\"color:red;\">Database seed successful.</label>", "text/html", Encoding.UTF8);
        }
    }
}
