using TodoWebService.Models.DTOs.Pagination;
using TodoWebService.Models.DTOs.Todo;
using TodoWebService.Models.Entities;

namespace TodoWebService.Services
{
    public interface ITodoService
    {
        Task<TodoItemDto?> GetTodoItem(string userId, int id);
        Task<TodoItemDto?> ChangeTodoItemStatus(string userId, int id, bool isCompleted);
        Task<TodoItemDto?> CreateTodo(string userId, CreateTodoItemRequest request);
        Task<bool> DeleteTodo(string userId, int id);
        Task<bool> UpdateTodoItemNotify(int id, bool notify);
        Task<PaginatedListDto<TodoItemDto>> GetTodoItems(string userId, int page, int pageSize, string? search, bool? isCompleted);
        Task<List<TodoItem>> GetAllTodoItems();
    }
}