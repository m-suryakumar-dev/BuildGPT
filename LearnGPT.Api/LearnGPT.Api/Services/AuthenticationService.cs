
using LearnGPT.Api.Data;
using LearnGPT.Api.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace LearnGPT.Api.Services
{
    public class AuthenticationService
    {
        private readonly AppDbContext _context;
        private readonly IPasswordHasher<User> _passwordHasher;

        public AuthenticationService(
            AppDbContext context,
            IPasswordHasher<User> passwordHasher)
        {
            _context = context;
            _passwordHasher = passwordHasher;
        }

        public async Task<bool> RegisterUser(
            string name, string email, string password)
        {
            // Step 1: Check whether the email already exists
            email = email.Trim().ToLowerInvariant();

            bool emailExists = await _context.Users
                .AnyAsync(u => u.Email == email);

            if (emailExists)
            {
                return false;
            }

            // Step 2: Create the user
            var user = new User
            {
                Name = name.Trim(),
                Email = email
            };

            // Step 3: Hash the password
            string passwordHash =
                _passwordHasher.HashPassword(user, password);

            // Step 4: Create the user's credential
            user.UserCredential = new UserCredential
            {
                PasswordHash = passwordHash,
                User = user
            };

            // Step 5: Save the user and credential
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<User?> ValidateUser(string email, string password)
        {
            email = email.Trim().ToLowerInvariant();

            var user = await _context.Users
                .Include(u => u.UserCredential)
                .FirstOrDefaultAsync(u => u.Email == email);

            if (user == null || user.UserCredential == null)
            {
                return null;
            }

            var result = _passwordHasher.VerifyHashedPassword(
                user,
                user.UserCredential.PasswordHash,
                password);

            if (result == PasswordVerificationResult.Failed)
            {
                return null;
            }

            return user;
        }

    }
}
