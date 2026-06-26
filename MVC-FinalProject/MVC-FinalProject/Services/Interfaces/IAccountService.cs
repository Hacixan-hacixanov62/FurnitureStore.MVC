using System.Net.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MVC_FinalProject.Helpers;
using MVC_FinalProject.Helpers.Constants;
using MVC_FinalProject.Models.Account;

namespace MVC_FinalProject.Services.Interfaces
{
    public interface IAccountService
    {
        Task<HttpResponseMessage> Register(Register model);
        Task<HttpResponseMessage> Login(Login model);
        Task<string> ResetPasswordAsync(UserPassword model);
        Task<ResponseObject> ForgetPasswordAsync(string email);
        Task<List<UserRole>> GetAllUsersAsync();
        Task<string> AddRoleAsync(string username, string roleName);
        Task<string> RemoveRoleAsync(string username, string roleName);
        Task<List<string>> GetAllRolesAsync();
        Task<List<string>> GetUserRolesAsync(string username);

        Task<string> SendMessageToAdminAsync(AdminMessage model);
        Task<List<string>> GetAdminsEmailsAsync();

        Task<string> BlockUserAsync(string username, int minutes);
        Task<string> UnblockUserAsync(string userId);
        Task<UserRole> GetUserByUsernameAsync(string username);
        Task<List<UserRole>> GetAllBlockedUsersAsync();

        Task<string> UpdateEmailAsync(UpdateEmail model, string token);
        Task<string> UpdateUsernameAsync(UpdateUsername model, string token);
        Task<AccountInfo> GetCurrentUserAsync(string token);

    }
}
