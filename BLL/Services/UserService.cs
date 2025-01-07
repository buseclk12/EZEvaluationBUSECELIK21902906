using BLL.DAL;
using BLL.Models;
using BLL.Services.Bases;
using Microsoft.EntityFrameworkCore;

namespace BLL.Services
{
    public class UserService : Service, IService<User, UserModel>
    {
        public UserService(Db db) : base(db)
        {
        }

        public Service Create(User record)
        {
            if (_db.Users.Any(u => u.UserName.ToUpper() == record.UserName.ToUpper().Trim()))
                return Error("A user with the same username already exists!");

            record.UserName = record.UserName.Trim();
            record.Password = record.Password.Trim(); // You may also hash the password here.
            _db.Users.Add(record);
            _db.SaveChanges();

            return Success("User created successfully.");
        }

        public Service Delete(int id)
        {
            var user = _db.Users.Include(u => u.Role).SingleOrDefault(u => u.Id == id);
            if (user == null)
                return Error("User not found!");

            _db.Users.Remove(user);
            _db.SaveChanges();

            return Success("User deleted successfully.");
        }

        public IQueryable<UserModel> Query()
        {
            return _db.Users.Include(u => u.Role).OrderByDescending(u => u.IsActive).ThenBy(u => u.UserName).Select(u => new UserModel() { Record = u });
        }
        public Service Update(User record)
        {
            var user = _db.Users.SingleOrDefault(u => u.Id == record.Id);
            if (user == null)
                return Error("User not found!");

            if (_db.Users.Any(u => u.Id != record.Id && u.UserName.ToUpper() == record.UserName.ToUpper().Trim()))
                return Error("A user with the same username already exists!");

            user.UserName = record.UserName.Trim();
            user.Password = record.Password.Trim(); // Update the password, hash if needed.
            user.RoleId = record.RoleId;
            user.IsActive = record.IsActive;

            _db.Users.Update(user);
            _db.SaveChanges();

            return Success("User updated successfully.");
        }
    }
}
