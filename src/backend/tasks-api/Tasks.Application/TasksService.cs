using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using LanguageExt;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using Tasks.Application.Dto;
using Tasks.Domain;

namespace Tasks.Application
{
    public class TasksService : ITasksService
    {
        private readonly ILogger<TasksService> logger;
        private readonly IUnitOfWork unitOfWork;
        private readonly ITaskRepository taskRepository;
        private readonly IMapper mapper;
        private readonly IValidator<CreateTaskRequest> createTaskRequestValidator;

        public TasksService(
            ILogger<TasksService> logger,
            IUnitOfWork unitOfWork,
            ITaskRepository taskRepository,
            IMapper mapper,
            IValidator<CreateTaskRequest> createTaskRequestValidator)
        {
            this.logger = logger;
            this.unitOfWork = unitOfWork;
            this.taskRepository = taskRepository;
            this.mapper = mapper;
            this.createTaskRequestValidator = createTaskRequestValidator;
        }

        public async System.Threading.Tasks.Task<Either<Error, TaskDetailsResponse>> GetTaskById(Guid id, CancellationToken cancellationToken)
        {
            logger.LogInformation("Getting task by id {id}", id);

            return await (await GetTaskFromRepositoryById(id, cancellationToken))
                .BindAsync(Map<TaskDetailsResponse>);
        }

        public async System.Threading.Tasks.Task<Either<Error, TaskSearchResultResponse>> SearchTasks(string? searchPhrase, string? state, int pageNumber, int pageSize, CancellationToken cancellationToken)
        {
            logger.LogInformation("Searching tasks {searchPhrase}, {state}, {pageNumber}, {pageSize}", searchPhrase, state, pageNumber, pageSize);

            return await (await SearchTasksInRepository(searchPhrase, state, pageNumber, pageSize, cancellationToken))
                .BindAsync(x => Map<TaskSearchResultResponse>(x));
        }

        public async System.Threading.Tasks.Task<Either<Error, TaskDetailsResponse>> CreateTask(CreateTaskRequest createTaskRequest, CancellationToken cancellationToken)
        {
            logger.LogInformation("Creating task {createTaskRequest}", createTaskRequest);

            return await (await ValidateRequest(createTaskRequestValidator, createTaskRequest, cancellationToken))
                .BindAsync(async _ => await BeginUnitOfWork(cancellationToken))
                .BindAsync(async _ => await CreateTask(createTaskRequest.Name, createTaskRequest.Description, createTaskRequest.State))
                .BindAsync(task =>
                {
                    taskRepository.AddTask(task);
                    return (Either<Error, Task>)task;
                })
                .BindAsync(async task => await CommitUnitOfWork(task, cancellationToken))
                .BindAsync(Map<TaskDetailsResponse>);
        }

        public async System.Threading.Tasks.Task<Either<Error, Unit>> DeleteTaskById(Guid id, CancellationToken cancellationToken)
        {
            logger.LogInformation("Deleting task by id {id}", id);

            return await (await BeginUnitOfWork(cancellationToken))
                .BindAsync(async _ => await GetTaskFromRepositoryById(id, cancellationToken))
                .BindAsync(task =>
                {
                    taskRepository.RemoveTask(task);
                    return (Either<Error, Task>)task;
                })
                .BindAsync(async _ => await CommitUnitOfWork(Unit.Default, cancellationToken));
        }

        public async System.Threading.Tasks.Task<Either<Error, TaskDetailsResponse>> ChangeTaskDetails(Guid id, ChangeTaskDetailsRequest changeTaskDetailsRequest, CancellationToken cancellationToken)
        {
            logger.LogInformation("Changing task details by id {id}, {changeTaskDetailsRequest}", id, changeTaskDetailsRequest);

            return await (await BeginUnitOfWork(cancellationToken))
                .BindAsync(async _ => await GetTaskFromRepositoryById(id, cancellationToken))
                .BindAsync(task =>
                {
                    task.ChangeDetails(changeTaskDetailsRequest.Name, changeTaskDetailsRequest.Description);
                    return (Either<Error, Task>)task;
                })
                .BindAsync(async task => await CommitUnitOfWork(task, cancellationToken))
                .BindAsync(Map<TaskDetailsResponse>);
        }

        public async System.Threading.Tasks.Task<Either<Error, TaskDetailsResponse>> ChangeTaskState(Guid id, ChangeTaskStateRequest changeTaskStateRequest, CancellationToken cancellationToken)
        {
            logger.LogInformation("Changing task state by id {id}, {changeTaskStateRequest}", id, changeTaskStateRequest);

            return await (await BeginUnitOfWork(cancellationToken))
                .BindAsync(async _ => await GetTaskFromRepositoryById(id, cancellationToken))
                .BindAsync(task =>
                {
                    var state = mapper.Map<TaskState>(changeTaskStateRequest.State);
                    task.ChangeState(state);
                    return (Either<Error, Task>)task;
                })
                .BindAsync(async task => await CommitUnitOfWork(task, cancellationToken))
                .BindAsync(Map<TaskDetailsResponse>);
        }

        private async System.Threading.Tasks.Task<Either<Error, Unit>> ValidateRequest<TRequest>(IValidator<TRequest> validator, TRequest request, CancellationToken cancellationToken)
        {
            var validationResult = await validator.ValidateAsync(request, cancellationToken);
            if (validationResult.IsValid)
            {
                return Unit.Default;
            }
            else
            {
                logger.LogError("Validation failed {request}, {validationResult}", request, validationResult);
                return new ValidationError(validationResult);
            }
        }

        private async System.Threading.Tasks.Task<Either<Error, Unit>> BeginUnitOfWork(CancellationToken cancellationToken)
        {
            await unitOfWork.Begin(cancellationToken);
            return Unit.Default;
        }

        private async System.Threading.Tasks.Task<Either<Error, TResult>> CommitUnitOfWork<TResult>(TResult result, CancellationToken cancellationToken)
        {
            await unitOfWork.Commit(cancellationToken);
            return result;
        }

        private System.Threading.Tasks.Task<Either<Error, Task>> CreateTask(string name, string? description, string state)
        {
            var taskState = mapper.Map<TaskState>(state);
            var task = new Task(name, description, taskState);
            return System.Threading.Tasks.Task.FromResult((Either<Error, Task>)task);
        }

        private async System.Threading.Tasks.Task<Either<Error, Task>> GetTaskFromRepositoryById(Guid id, CancellationToken cancellationToken)
        {
            var taskId = mapper.Map<TaskId>(id);
            var task = await taskRepository.GetTaskById(taskId, cancellationToken);
            return task is not null
                ? task
                : new Error(HttpStatusCode.NotFound, $"Task {id} not found");
        }

        private async System.Threading.Tasks.Task<Either<Error, (Task[] Tasks, int TotalCount)>> SearchTasksInRepository(string? searchPhrase, string? state, int pageNumber, int pageSize, CancellationToken cancellationToken)
        {
            var taskState = mapper.Map<TaskState?>(state);
            var result = await taskRepository.SearchTasks(searchPhrase, taskState, pageNumber, pageSize, cancellationToken);
            return result;
        }

        private System.Threading.Tasks.Task<Either<Error, TResult>> Map<TResult>(object source)
        {
            var result = mapper.Map<TResult>(source);
            return System.Threading.Tasks.Task.FromResult((Either<Error, TResult>)result);
        }
    }
    public interface ITasksService
    {
        System.Threading.Tasks.Task<Either<Error, TaskDetailsResponse>> GetTaskById(Guid id, CancellationToken cancellationToken);
        System.Threading.Tasks.Task<Either<Error, TaskSearchResultResponse>> SearchTasks(string? searchPhrase, string? state, int pageNumber, int pageSize, CancellationToken cancellationToken);
        System.Threading.Tasks.Task<Either<Error, TaskDetailsResponse>> CreateTask(CreateTaskRequest createTaskRequest, CancellationToken cancellationToken);
        System.Threading.Tasks.Task<Either<Error, Unit>> DeleteTaskById(Guid id, CancellationToken cancellationToken);
        System.Threading.Tasks.Task<Either<Error, TaskDetailsResponse>> ChangeTaskDetails(Guid id, ChangeTaskDetailsRequest changeTaskDetailsRequest, CancellationToken cancellationToken);
        System.Threading.Tasks.Task<Either<Error, TaskDetailsResponse>> ChangeTaskState(Guid id, ChangeTaskStateRequest changeTaskStateRequest, CancellationToken cancellationToken);
    }
    public record Error(HttpStatusCode StatusCode, string Message, object? Details = null);
    public record ValidationError : Error
    {
        public ValidationError(ValidationResult validationResult)
            : base(HttpStatusCode.BadRequest, FormatMessage(validationResult), FormatDetails(validationResult))
        {
        }

        private static string FormatMessage(ValidationResult validationResult)
        {
            return "Validation failed. " + string.Join(", ", validationResult.Errors
                .Select(x => $"{x.PropertyName}: {x.ErrorMessage}"));
        }

        private static List<KeyValuePair<string, string>> FormatDetails(ValidationResult validationResult)
        {
            return validationResult.Errors
                .Select(x => new KeyValuePair<string, string>(x.PropertyName, x.ErrorMessage))
                .ToList();
        }
    };
}
