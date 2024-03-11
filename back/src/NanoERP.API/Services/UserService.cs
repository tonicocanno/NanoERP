using Microsoft.EntityFrameworkCore;
using NanoERP.API.Data;
using NanoERP.API.Domain.Entities;

namespace NanoERP.API.Services
{
    public class UserService(DataContext db)
    {
        private readonly DataContext _db = db;
        
        public IEnumerable<User> Get()
        {
            return [.. _db.Users.AsNoTracking()];
        }

        public User? GetById(string id)
        {
            return _db.Users.FirstOrDefault(u => u.StringId == id);
        }

        public User? GetByUsernameOrEmail(IUser user)
        {
            return _db.Users.AsNoTracking().FirstOrDefault(u => u.Username == user.Username || u.Email == user.Email);
        }

        public void Update(User user)
        {
            _db.Users.Update(user);
            _db.SaveChanges();
        }

        public void Delete(User user)
        {
            _db.Users.Remove(user);
            _db.SaveChanges();
        }

        public void Create(User user)
        {
            _db.Users.Add(user);
            _db.SaveChanges();
        }

        public void Truncate()
        {
            _db.Users.RemoveRange(_db.Users);
            _db.SaveChanges();
        }
    }
}