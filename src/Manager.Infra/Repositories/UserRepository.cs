using System.Linq;
using System.Threading.Tasks;
using Manager.Domain.Entities;
using Manager.Infra.Context;
using Manager.Infra.Intefaces;
using Microsoft.EntityFrameworkCore;

namespace Manager.Infra.Repositories
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        public UserRepository(ManagerContext context) : base(context)
        {
        }

        public async Task<User> GetByEmail(string email)
        {
            var obj = await _context.Users.AsNoTracking().Where(x => x.Email.ToLower() == email.ToLower()).ToListAsync();

            return obj.SingleOrDefault();
        }

        public async Task<List<User>> SearchByEmail(string email)
        {
            return await _context.Users.AsNoTracking().Where(x => x.Email.ToLower().Contains(email.ToLower())).ToListAsync();
        }

        public async Task<List<User>> SearchByName(string name)
        {
            return await _context.Users.AsNoTracking().Where(x => x.Name.ToLower().Contains(name.ToLower())).ToListAsync();
        }
    }
}