using MVC_FinalProject.Models.Task;
using MVC_FinalProject.Services.Interfaces;

namespace MVC_FinalProject.Services
{
    public class TaskService : ITaskService
    {
        private readonly HttpClient _httpClient;

        public TaskService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<TaskVM>> GetTasksByUserAsync(string userName)
        {
            var response = await _httpClient.GetFromJsonAsync<List<TaskVM>>($"https://localhost:7004/api/admin/Tasks/GetByUser/{userName}");
            return response;
        }

        public async Task CreateTaskAsync(CreateTaskApi model)
        {
            await _httpClient.PostAsJsonAsync($"https://localhost:7004/api/admin/Tasks/Create", model);
        }

        public async Task CompleteTaskAsync(CompleteTask model)
        {
            await _httpClient.PostAsJsonAsync($"https://localhost:7004/api/admin/Tasks/Complete", model);
        }

        public async Task MarkTaskAsSeenAsync(MarkSeen model)
        {
            var url = $"https://localhost:7004/api/admin/Tasks/MarkSeen/{model.TaskId}";
            var response = await _httpClient.PostAsJsonAsync(url, model);
            response.EnsureSuccessStatusCode();
        }

        public async Task<List<TaskVM>> GetAllAsync()
        {
            var response = await _httpClient.GetFromJsonAsync<List<TaskVM>>("https://localhost:7004/api/admin/Tasks/GetAll");
            return response;
        }

        public async Task DeleteAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"https://localhost:7004/api/admin/Tasks/Delete/{id}");
            response.EnsureSuccessStatusCode();
        }
    }
}
