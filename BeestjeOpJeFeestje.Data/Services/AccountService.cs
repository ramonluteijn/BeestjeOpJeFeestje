using BeestjeOpJeFeestje.Data.Dtos;
using BeestjeOpJeFeestje.Repository.Models;
using Microsoft.AspNetCore.Identity;

namespace BeestjeOpJeFeestje.Data.Services
{
    public class AccountService(SignInManager<User> signInManager, UserManager<User> userManager)
    {
        public async Task<User?> Login(string loginInput, string password)
        {
            var user = await userManager.FindByEmailAsync(loginInput);
            if (user != null)
            {
                var result = await signInManager.PasswordSignInAsync(user, password, isPersistent: false, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    return user;
                }
            }
            return null;
        }

        public async Task Logout()
        {
            await signInManager.SignOutAsync();
        }

        public IEnumerable<UserDto> GetAllUsers()
        {
            var users = userManager.Users
                .OrderBy(user => user.UserName)
                .ToList();

            var customers = users
                .Where(user => userManager.IsInRoleAsync(user, "customer").Result)
                .Select(ConvertCustomerDto)
                .ToList();
            return customers;
        }

        private UserDto ConvertCustomerDto(User user)
        {
            return new UserDto
            {
                Id = user.Id,
                Name = user.UserName,
                Email = user.Email,
                Rank = user.Rank,
                HouseNumber = user.HouseNumber,
                ZipCode = user.ZipCode
            };
        }
    }
}