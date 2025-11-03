namespace ShamTask.Api.Contracts
{
    public record CreateTaskRequest(
    string Title,
    string? Description,
    string Priority,   // "Low" | "Medium" | "High"
    string DueDate,    // "YYYY-MM-DD"
    string Status     // "Pending" | "InProgress" | "Completed"    
);

    public record UpdateTaskRequest(
        string Title,
        string? Description,
        string Priority,
        string DueDate,
        string Status
    );

}
