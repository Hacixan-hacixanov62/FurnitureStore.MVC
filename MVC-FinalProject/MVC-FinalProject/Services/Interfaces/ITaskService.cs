using MVC_FinalProject.Models.Task;

namespace MVC_FinalProject.Services.Interfaces
{
    public interface ITaskService
    {
        Task<List<TaskVM>> GetAllAsync();
        Task<List<TaskVM>> GetTasksByUserAsync(string userName);
        Task CreateTaskAsync(CreateTaskApi model);
        Task CompleteTaskAsync(CompleteTask model);
        Task MarkTaskAsSeenAsync(MarkSeen model);
        Task DeleteAsync(int id);
    }
}
