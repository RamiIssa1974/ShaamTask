public enum Priority { Low, Medium, High }
public enum Status { Pending, InProgress, Completed }

namespace ShamTask.Api.Domain.Models
{
    public class TaskItem
    {
        public int Id { get; set; }
        public string Title { get; set; } = "";
        public string? Description { get; set; }
        public Priority Priority { get; set; } = Priority.Medium;
        public DateOnly DueDate { get; set; }
        public Status Status { get; set; } = Status.Pending;
    }
}
