﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TodoWebService.Models.DTOs.Todo;
using TodoWebService.Providers;
using TodoWebService.Services;

namespace TodoWebService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TodoController : ControllerBase
    {
        private readonly IRequestUserProvider _userProvider;
        private readonly ITodoService _todoService;

        public TodoController(IRequestUserProvider userProvider, ITodoService todoService)
        {
            _userProvider = userProvider;
            _todoService = todoService;
        }

        [HttpGet("get")]
        public async Task<ActionResult<TodoItemDto>> Get(int id)
        {
            var user = _userProvider.GetUserInfo();

            var item = await _todoService.GetTodoItem(user!.id, id);


            return item is not null
                ? item // => Ok(item)
                : NotFound();
        }

        [HttpPost("create")]
        public async Task<ActionResult<TodoItemDto>> Create([FromBody] CreateTodoItemRequest request)
        {
            var user = _userProvider.GetUserInfo();

            var createdItem = await _todoService.CreateTodo(user!.id, request);

            return CreatedAtAction(nameof(Get), new { id = createdItem!.Id }, createdItem);
        }

        // api/todo/1/status
        [HttpPatch("{id}/status")]
        public async Task<ActionResult<TodoItemDto>> ChangeStatus(int id, [FromBody] bool isCompleted)
        {
            var user = _userProvider.GetUserInfo();

            var todoItem = await _todoService.ChangeTodoItemStatus(user!.id, id, isCompleted);

            return todoItem is not null
                ? todoItem : NotFound();
        }

        [HttpDelete("delete/{id}")]
        public async Task<ActionResult<bool>> Delete(int id)
        {
            var user = _userProvider.GetUserInfo();
            var result = await _todoService.DeleteTodo(user!.id, id);
            return result ? result : NotFound();
        }

    }
}
