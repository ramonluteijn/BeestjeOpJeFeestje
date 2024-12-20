﻿using System.Text;
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


        public async Task<(bool, string)> CreateUser(UserDto user)
        {
            if (await userManager.FindByEmailAsync(user.Email) != null)
            {
                return (false, "Email already in use");
            }

            var newUser = new User
            {
                UserName = user.Email,
                Email = user.Email,
                Rank = user.Rank,
                HouseNumber = user.HouseNumber,
                PhoneNumber = user.PhoneNumber,
                ZipCode = user.ZipCode
            };

            var password = PasswordGenerator();
            var result = await userManager.CreateAsync(newUser, password);
            if (!result.Succeeded)
            {
                return (false, "Something went wrong, please try again.");
            }

            await userManager.AddToRoleAsync(newUser, "customer");
            return (true, password);
        }

        private static string PasswordGenerator()
        {
            var password = new StringBuilder();
            var random = new Random();
            for (var i = 0; i < 8; i++)
            {
                password.Append((char)random.Next(33, 126));
            }
            return password.ToString();
        }

        public UserDto GetUserById(int id)
        {
            var user = userManager.FindByIdAsync(id.ToString()).Result;
            return ConvertCustomerDto(user!);
        }

        public async Task UpdateUser(UserDto userDto)
        {
            var user = await userManager.FindByIdAsync(userDto.Id.ToString());
            user.UserName = userDto.Email;
            user.Email = userDto.Email;
            user.Rank = userDto.Rank;
            user.HouseNumber = userDto.HouseNumber;
            user.PhoneNumber = userDto.PhoneNumber;
            user.ZipCode = userDto.ZipCode;
            await userManager.UpdateAsync(user);
        }
    }
}