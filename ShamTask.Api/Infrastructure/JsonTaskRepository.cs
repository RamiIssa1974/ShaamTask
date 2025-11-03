using ShamTask.Api.Domain.Models;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ShamTask.Api.Infrastructure
{

    public class JsonTaskRepository : ITaskRepository
    {
        private readonly string _filePath;
        private static readonly SemaphoreSlim _lock = new(1, 1);
        private static readonly JsonSerializerOptions _jsonOptions = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = true
        };

        public JsonTaskRepository(IWebHostEnvironment env)
        {
            _jsonOptions.Converters.Add(new JsonStringEnumConverter());

            _filePath = Path.Combine(env.ContentRootPath, "Data", "tasks.json");
            Directory.CreateDirectory(Path.GetDirectoryName(_filePath)!);

            if (!File.Exists(_filePath))
            {
                File.WriteAllText(_filePath, "[]");
            }
        }

        public async Task<List<TaskItem>> GetAllAsync()
        {
            await _lock.WaitAsync();
            try
            {
                return await ReadAllInternalAsync();
            }
            finally { _lock.Release(); }
        }

        private async Task SaveAllAsync(List<TaskItem> items)
        {
            var json = JsonSerializer.Serialize(items, _jsonOptions);
            await File.WriteAllTextAsync(_filePath, json);
        }

        public async Task<TaskItem?> GetByIdAsync(int id)
        {
            var items = await GetAllAsync();
            return items.FirstOrDefault(x => x.Id == id);
        }

        public async Task<TaskItem> AddAsync(TaskItem task)
        {
            await _lock.WaitAsync();
            try
            {
                var items = await ReadAllInternalAsync();  
                task.Id = (items.Count == 0) ? 1 : items.Max(i => i.Id) + 1;
                items.Add(task);
                await SaveAllInternalAsync(items);        
                return task;
            }
            finally { _lock.Release(); }
        }

        public async Task<TaskItem?> UpdateAsync(TaskItem task)
        {
            await _lock.WaitAsync();
            try
            {
                var items = await ReadAllInternalAsync();
                var idx = items.FindIndex(i => i.Id == task.Id);
                if (idx == -1) return null;
                items[idx] = task;
                await SaveAllInternalAsync(items);
                return task;
            }
            finally { _lock.Release(); }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            await _lock.WaitAsync();
            try
            {
                var items = await ReadAllInternalAsync();
                var removed = items.RemoveAll(i => i.Id == id) > 0;
                if (removed) await SaveAllInternalAsync(items);
                return removed;
            }
            finally { _lock.Release(); }
        }

        private async Task<List<TaskItem>> ReadAllInternalAsync()
        {
            var json = await File.ReadAllTextAsync(_filePath);
            return JsonSerializer.Deserialize<List<TaskItem>>(json, _jsonOptions) ?? new();
        }

        private async Task SaveAllInternalAsync(List<TaskItem> items)
        {
            var json = JsonSerializer.Serialize(items, _jsonOptions);
            await File.WriteAllTextAsync(_filePath, json);
        }

    }
}
