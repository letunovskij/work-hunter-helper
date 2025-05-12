using Microsoft.AspNetCore.Mvc;
using WorkHunter.Abstractions.Notifications;
using WorkHunter.Models.Constants;
using WorkHunter.Models.Dto.Notifications;
using WorkHunter.Models.Enums;

namespace WorkHunter.Api.Endpoints;

internal static class TasksEndpoints
{
    internal static void MapTasksEndpoints(this IEndpointRouteBuilder routes)
    {
        var routeGroup = routes.MapGroup("user-tasks")
                               .WithTags("UserTasks")
                               .WithOpenApi();

        routeGroup.MapGet("{id}", async (int id, ITaskService service) => await service.Get(id))
            .RequireAuthorization(AppPolicies.All)
            .WithDescription("Получить задачу текущего пользователя по id");
        //.RequireCors(options => options.AllowAnyOrigin());

        routeGroup.MapPost(string.Empty, async (TaskType type, Guid wresponseId, [FromBody] UserTaskDto? dto, ITaskService service) 
            => await service.CreateTask(type, wresponseId, dto))
            .RequireAuthorization(AppPolicies.All)
            .WithDescription("Получить задачу текущего пользователя по id");
        //.RequireCors(options => options.AllowAnyOrigin());
    }
}