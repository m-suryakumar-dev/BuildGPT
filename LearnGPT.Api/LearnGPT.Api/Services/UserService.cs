using LearnGPT.Api.Data;
using LearnGPT.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace LearnGPT.Api.Services
{
    public class UserService
    {
        public readonly AppDbContext _context;

        public UserService(AppDbContext context)
        {
            _context = context;
        }
        public async Task<List<User>> GetAllUsers()
        {
            return await _context.Users.ToListAsync();
        }
    }
}
