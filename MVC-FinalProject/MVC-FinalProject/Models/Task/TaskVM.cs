namespace MVC_FinalProject.Models.Task
{
    public class TaskVM
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsCompleted { get; set; }
        public string AssignedTo { get; set; }
    }
}
