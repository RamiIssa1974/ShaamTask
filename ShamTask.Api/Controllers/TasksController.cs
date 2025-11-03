using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShamTask.Api.Application;
using ShamTask.Api.Contracts;

namespace ShamTask.Api.Controllers
{
    [ApiController]
    [Route("tasks")]
    public class TasksController : ControllerBase
    {
        private readonly TaskService _svc;
        public TasksController(TaskService svc) => _svc = svc;

        [HttpGet]
        public async Task<IActionResult> GetAll() =>
            Ok(await _svc.GetAllAsync());

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateTaskRequest req)
        {
            if (string.IsNullOrWhiteSpace(req.Title) || string.IsNullOrWhiteSpace(req.DueDate))
                return BadRequest("Title and DueDate are required.");

            var created = await _svc.CreateAsync(req);
            return CreatedAtAction(nameof(GetAll), new { id = created.Id }, created);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateTaskRequest req)
        {
            var updated = await _svc.UpdateAsync(id, req);
            return updated is null ? NotFound() : Ok(updated);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var ok = await _svc.DeleteAsync(id);
            return ok ? NoContent() : NotFound();
        }
    }
}
