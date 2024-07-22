using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Tasks.Api.Extensions;
using Tasks.Application;
using Tasks.Application.Dto;

namespace Tasks.Api.Controllers
{
    [ApiController]
    [Route("tasks")]
    public class TasksController : ControllerBase
    {
        private readonly ITasksService tasksService;

        public TasksController(ITasksService tasksService)
        {
            this.tasksService = tasksService;
        }

        [HttpGet("{id}")]
        [ProducesResponseType<TaskDetailsResponse>((int)HttpStatusCode.OK)]
        [ProducesResponseType<ErrorResponse>((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetTaskById(
            Guid id,
            CancellationToken cancellationToken = default)
        {
            return (await tasksService.GetTaskById(id, cancellationToken))
                .ToActionResult();
        }

        [HttpGet("search")]
        [ProducesResponseType<TaskSearchResultResponse>((int)HttpStatusCode.OK)]
        public async Task<IActionResult> SearchTasks(
            [FromQuery(Name = "search_phrase")] string? searchPhrase,
            [FromQuery(Name = "state")] string? state,
            [FromQuery(Name = "page_number")] int pageNumber = 0,
            [FromQuery(Name = "page_size")] int pageSize = 10,
            CancellationToken cancellationToken = default)
        {
            return (await tasksService.SearchTasks(searchPhrase, state, pageNumber, pageSize, cancellationToken))
                .ToActionResult();
        }

        [HttpPost]
        [ProducesResponseType<TaskDetailsResponse>((int)HttpStatusCode.Created)]
        [ProducesResponseType<ErrorResponse>((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> CreateTask(
            [FromBody] CreateTaskRequest createTaskRequest, 
            CancellationToken cancellationToken = default)
        {
            return (await tasksService.CreateTask(createTaskRequest, cancellationToken))
                .ToActionResult(result => Created($"tasks/{result.Id}", result));
        }

        [HttpDelete("{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType<ErrorResponse>((int)HttpStatusCode.NotFound)]
        [ProducesResponseType<ErrorResponse>((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> DeleteTaskById(
            Guid id,
            CancellationToken cancellationToken = default)
        {
            return (await tasksService.DeleteTaskById(id, cancellationToken))
                .ToActionResult(_ => NoContent());
        }

        [HttpPatch("{id}/details")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType<ErrorResponse>((int)HttpStatusCode.NotFound)]
        [ProducesResponseType<ErrorResponse>((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> ChangeTaskDetails(
            Guid id,
            [FromBody] ChangeTaskDetailsRequest changeTaskDetailsRequest,
            CancellationToken cancellationToken = default)
        {
            return (await tasksService.ChangeTaskDetails(id, changeTaskDetailsRequest, cancellationToken))
                .ToActionResult();
        }

        [HttpPatch("{id}/state")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType<ErrorResponse>((int)HttpStatusCode.NotFound)]
        [ProducesResponseType<ErrorResponse>((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> ChangeTaskState(
            Guid id,
            [FromBody] ChangeTaskStateRequest changeTaskStateRequest,
            CancellationToken cancellationToken = default)
        {
            return (await tasksService.ChangeTaskState(id, changeTaskStateRequest, cancellationToken))
                .ToActionResult();
        }
    }
}
