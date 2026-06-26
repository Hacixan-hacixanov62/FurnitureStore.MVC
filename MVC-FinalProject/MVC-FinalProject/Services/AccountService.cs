using MVC_FinalProject.Helpers.Constants;
using MVC_FinalProject.Models.Account;
using MVC_FinalProject.Services.Interfaces;
using Register = MVC_FinalProject.Models.Account.Register;
using Login = MVC_FinalProject.Models.Account.Login;
using System.Text;
using System.Text.Json;
using Newtonsoft.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using System.Net.Http.Headers;
using MVC_FinalProject.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace MVC_FinalProject.Services
{
    public class AccountService : IAccountService
    {
        private readonly HttpClient _httpClient;
        public AccountService(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
        }

        public async Task<HttpResponseMessage> Register(Register model)
        {
            var jsonData = JsonConvert.SerializeObject(model);
            StringContent stringContent = new StringContent(jsonData, Encoding.UTF8, "application/json");
            return await _httpClient.PostAsync($"{Urls.AccountClientUrl}Register", stringContent);
        }
        public async Task<HttpResponseMessage> Login(Login model)
        {
            return await _httpClient.PostAsJsonAsync($"{Urls.AccountClientUrl}Login", model);
        }

        public async Task<ResponseObject> ForgetPasswordAsync(string email)
        {
            if (string.IsNullOrEmpty(email))
                return new ResponseObject { ResponseMessage = "Email cannot be empty.", StatusCode = 400 };

            var content = new StringContent($"\"{email}\"", Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync($"{Urls.AccountClientUrl}ForgetPassword", content);

            var responseString = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                var result = JsonConvert.DeserializeObject<ResponseObject>(responseString);
                return result;
            }
            return new ResponseObject
            {
                ResponseMessage = responseString,
                StatusCode = (int)response.StatusCode
            };
        }

        public async Task<string> ResetPasswordAsync(UserPassword model)
        {
            if (model == null) return "Invalid request.";

            var response = await _httpClient.PostAsJsonAsync($"{Urls.AccountClientUrl}ResetPassword", model);

            return await response.Content.ReadAsStringAsync();
        }
        public async Task<string> AddRoleAsync(string username, string roleName)
        {
            var response = await _httpClient.PostAsync(
                $"{Urls.AccountUrl}AddRole?username={username}&roleName={roleName}", null);

            return await response.Content.ReadAsStringAsync();
        }
        public async Task<string> RemoveRoleAsync(string username, string roleName)
        {

            var response = await _httpClient.PostAsync($"{Urls.AccountUrl}RemoveRole?username={username}&roleName={roleName}", null);
            return await response.Content.ReadAsStringAsync();
        }
        public async Task<List<UserRole>> GetAllUsersAsync()
        {
            var response = await _httpClient.GetAsync($"{Urls.AccountUrl}GetAllUsers");

            if (!response.IsSuccessStatusCode)
                return new List<UserRole>();

            return await response.Content.ReadFromJsonAsync<List<UserRole>>();
        }

        public async Task<AccountInfo> GetCurrentUserAsync(string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.GetAsync($"{Urls.AccountClientUrl}GetCurrentUser");
            if (!response.IsSuccessStatusCode) return null;

            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<AccountInfo>(json);
        }


        public async Task<List<string>> GetAllRolesAsync()
        {
            var response = await _httpClient.GetAsync($"{Urls.AccountUrl}GetAllRoles");

            if (!response.IsSuccessStatusCode)
                return new List<string>();

            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<string>>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
        }

        public async Task<List<string>> GetUserRolesAsync(string username)
        {
            var response = await _httpClient.GetAsync($"{Urls.AccountUrl}GetUserRoles?username={username}");

            if (!response.IsSuccessStatusCode)
                return new List<string>();

            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<string>>(json);
        }
        public async Task<string> SendMessageToAdminAsync(AdminMessage model)
        {
            var response = await _httpClient.PostAsJsonAsync($"{Urls.AccountUrl}SendMessageToAdmin", model);
            return await response.Content.ReadAsStringAsync();

        }

        public async Task<List<string>> GetAdminsEmailsAsync()
        {
            var response = await _httpClient.GetAsync($"{Urls.AccountUrl}GetAdminsEmails");

            if (!response.IsSuccessStatusCode)
                return new List<string>();

            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<string>>(json);
        }

        public async Task<string> BlockUserAsync(string username, int minutes)
        {
            var response = await _httpClient.PostAsync($"{Urls.AccountUrl}BlockUser?username={username}&minutes={minutes}", null);
            var content = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
                return "An error occurred while trying to block the user.";

            try
            {
                var result = JsonSerializer.Deserialize<StringResponse>(content, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                return result?.Message ?? "No response message.";
            }
            catch
            {
                return "Failed to parse response.";
            }
        }
        public async Task<string> UnblockUserAsync(string userId)
        {
            var response = await _httpClient.PostAsync($"{Urls.AccountUrl}UnblockUser?userId={userId}", null);
            var content = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
                return "An error occurred while trying to unblock the user.";

            try
            {
                var result = JsonSerializer.Deserialize<StringResponse>(content, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                return result?.Message ?? "No response message.";
            }
            catch
            {
                return "Failed to parse response.";
            }
        }
        public async Task<UserRole> GetUserByUsernameAsync(string username)
        {
            var response = await _httpClient.GetAsync($"{Urls.AccountUrl}GetUserByUsername?username={username}");
            if (!response.IsSuccessStatusCode) return null;

            return await response.Content.ReadFromJsonAsync<UserRole>();
        }
        public async Task<List<UserRole>> GetAllBlockedUsersAsync()
        {
            var response = await _httpClient.GetAsync($"{Urls.AccountUrl}GetAllBlockedUsers");
            if (!response.IsSuccessStatusCode)
                return new List<UserRole>();

            return await response.Content.ReadFromJsonAsync<List<UserRole>>();
        }
        public async Task<string> UpdateEmailAsync(UpdateEmail model, string token)
        {
            var json = JsonConvert.SerializeObject(model);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var request = new HttpRequestMessage(HttpMethod.Put, $"{Urls.AccountClientUrl}UpdateEmail");
            request.Content = content;
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.SendAsync(request);
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> UpdateUsernameAsync(UpdateUsername model, string token)
        {
            var json = JsonConvert.SerializeObject(model);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var request = new HttpRequestMessage(HttpMethod.Put, $"{Urls.AccountClientUrl}UpdateUsername");
            request.Content = content;
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.SendAsync(request);
            return await response.Content.ReadAsStringAsync();
        }

    }
}
