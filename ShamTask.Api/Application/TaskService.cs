using ShamTask.Api.Contracts;
using ShamTask.Api.Domain.Models;
using ShamTask.Api.Infrastructure;

namespace ShamTask.Api.Application
{
    public class TaskService
    {
        private readonly ITaskRepository _repo;

        public TaskService(ITaskRepository repo) => _repo = repo;

        public static Priority ParsePriority(string p) =>
            Enum.TryParse<Priority>(p, true, out var val) ? val : Priority.Medium;

        public static Status ParseStatus(string s) =>
            Enum.TryParse<Status>(s, true, out var val) ? val : Status.Pending;

        public async Task<List<TaskItem>> GetAllAsync() => await _repo.GetAllAsync();

        public async Task<TaskItem> CreateAsync(CreateTaskRequest req)
        {
            var task = new TaskItem
            {
                Title = req.Title.Trim(),
                Description = string.IsNullOrWhiteSpace(req.Description) ? null : req.Description.Trim(),
                Priority = ParsePriority(req.Priority),
                DueDate = DateOnly.Parse(req.DueDate),
                Status = ParseStatus(req.Status)
            };
           

            return await _repo.AddAsync(task);
        }

        public async Task<TaskItem?> UpdateAsync(int id, UpdateTaskRequest req)
        {
            var existing = await _repo.GetByIdAsync(id);
            if (existing is null) return null;

            existing.Title = req.Title.Trim();
            existing.Description = string.IsNullOrWhiteSpace(req.Description) ? null : req.Description.Trim();
            existing.Priority = ParsePriority(req.Priority);
            existing.DueDate = DateOnly.Parse(req.DueDate);
            existing.Status = ParseStatus(req.Status);

            return await _repo.UpdateAsync(existing);
        }

        public async Task<bool> DeleteAsync(int id) => await _repo.DeleteAsync(id);
        
    }
}
