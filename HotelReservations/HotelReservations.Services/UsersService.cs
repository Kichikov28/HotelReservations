using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HotelReservations.Common;
using HotelReservations.Data;
using HotelReservations.Data.Models;
using HotelReservations.Services.Contracts;
using HotelReservations.ViewModels.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace HotelReservations.Services
{

    public class UsersService : IUsersService
    {
        private readonly UserManager<User> userManager;
        private readonly ApplicationDbContext context;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly SignInManager<User> signInManager;
        private const int ItemsCount = 0;

        public UsersService(UserManager<User> userManager, ApplicationDbContext context, RoleManager<IdentityRole> roleManager, SignInManager<User> signInManager)
        {
            this.userManager = userManager;
            this.context = context;
            this.roleManager = roleManager;
            this.signInManager = signInManager;
        }

        public async Task<string> CreateUserAsync(CreateUserViewModel model)
        {
            User user = new User()
            {
                FirstName = model.FirstName,
                MiddleName= model.MiddleName,
                LastName = model.LastName,
                UCN= model.UCN,
                Email = model.Email,
                PhoneNumber= model.PhoneNumber,
                HireDate=model.HireDate,
                UserName = model.Email,
                Status=model.Status,
            };

            var result = await userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                if (userManager.Users.Count() <= 1)
                {
                    IdentityRole roleUser = new IdentityRole() { Name = GlobalConstants.UserRole };
                    IdentityRole roleAdmin = new IdentityRole() { Name = GlobalConstants.AdminRole };
                    await roleManager.CreateAsync(roleUser);
                    await roleManager.CreateAsync(roleAdmin);
                    await userManager.AddToRoleAsync(user, GlobalConstants.AdminRole);
                }
                else
                {
                    await userManager.AddToRoleAsync(user, GlobalConstants.UserRole);
                }
            }
            return user.Id;
        }

        public async Task<bool> DeleteUserAsync(string id)
        {
            User? user = await GetUserByIdAsync(id);
            if (user != null)
            {
                var result = await userManager.DeleteAsync(user);
                return result.Succeeded;
            }
            return false;
        }



        public async Task<DetailsUserViewModel?> GetUserDetailsAsync(string id)
        {
            DetailsUserViewModel result = null;

            User user = await GetUserByIdAsync(id);

            if (user != null)
            {
                string roles = string.Join(", ", await userManager.GetRolesAsync(user));

                result = new DetailsUserViewModel()
                {
                    Id = user.Id,
                    Name = $"{user.FirstName} {user.MiddleName} {user.LastName}",
                    Email = user.Email != null ? user.Email : "n/a",
                    UCN=user.UCN,
                    Status = user.Status,
                    HireDate= user.HireDate,
                    PhoneNumber = user.PhoneNumber != null ? user.PhoneNumber : "n/a",
                    Role = roles
                };
            }

            return result;
        }

        public async Task<IndexUsersViewModel> GetUsersAsync(IndexUsersViewModel users)
        {
            if (users == null)
            {
                users = new IndexUsersViewModel(0);
            }
            users.ElementsCount = await GetUsersCountAsync(); 
            IQueryable<User> usersFilter = null;

            if (!string.IsNullOrWhiteSpace(users.Filter))
            {
                usersFilter = context.Users.Where(x => x.FirstName.Contains(users.Filter));
            }
            else
            {
                usersFilter = context.Users;
            }

            users.Users = await userManager
                .Users
                .Skip((users.Page - 1) * users.ItemsPerPage)
                .Take(users.ItemsPerPage)
                .Select(x => new IndexUserViewModel()
                {
                    Id = x.Id,
                    Name = $"{x.FirstName} {x.MiddleName} {x.LastName}",
                    Email=x.Email,
                    PhoneNumber = x.PhoneNumber != null ? x.PhoneNumber : "n/a",
                    UCN =x.UCN,
                    Status = x.Status,
                    HireDate=x.HireDate,
                    QuitDate=x.QuitDate,
                    Role = string.Join(", ", userManager.GetRolesAsync(x).GetAwaiter().GetResult())
                })
                .ToListAsync();

            return users;
        }

        public async Task<int> GetUsersCountAsync()
        {
            return await userManager.Users.CountAsync();
        }

        public async Task<EditUserViewModel?> GetUserToEditAsync(string id)
        {
            EditUserViewModel? result = null;

            User? user = await GetUserByIdAsync(id);

            if (user != null)
            {
                result = new EditUserViewModel()
                {
                    Id = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                };
            }

            return result;


        }

        public async Task<string> UpdateUserAsync(EditUserViewModel user)
        {
            User? oldUser = await GetUserByIdAsync(user.Id);

            if (oldUser != null)
            {
                oldUser.FirstName = user.FirstName;
                oldUser.LastName = user.LastName;
                oldUser.Status = user.Status;
                await userManager.UpdateAsync(oldUser);
            }

            return user.Id;
        }

        private async Task<User?> GetUserByIdAsync(string id)
        {
            return await userManager.Users.FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task Logout()
        {
            await signInManager.SignOutAsync();
        }

        public async Task<SignInResult> Login(LoginViewModel model)
        {
            return await signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);
        }  
    }
}
