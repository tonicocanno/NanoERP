using Microsoft.EntityFrameworkCore;
using NanoERP.API.Data;
using NanoERP.API.Domain.Entities;

namespace NanoERP.API.Services
{
    public class UserService(DataContext db) : ServiceBase<User>(db)
    {
        private readonly DataContext _db = db;

        public async Task<User?> FindByEmailAsync(string email)
        {
            return await _db.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<User?> FindByUsernameOrEmailAsync(IUser user)
        {
            return await _db.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Username == user.Username || u.Email == user.Email);
        }
    }
}