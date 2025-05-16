using Abstractions.Users;
using Common.Exceptions;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using WorkHunter.Abstractions.Notifications;
using WorkHunter.Data;
using WorkHunter.Models.Dto.Notifications;
using WorkHunter.Models.Entities.Notifications;
using WorkHunter.Models.Enums;
using WorkHunter.Models.Views.Notifications;
using WorkHunter.Models.Views.Users;

namespace WorkHunter.Services.Notifications;

public sealed class TaskService : ITaskService
{
    private readonly IUserService userService;
    private readonly IWorkHunterDbContext workHunterDbContext;
    private readonly INotificationsService notificationsService;
    private readonly ILogger<TaskService> logger;
    //userTaskTypeService

    public TaskService(
        IUserService userService, 
        IWorkHunterDbContext workHunterDbContext, 
        INotificationsService notificationsService,
        ILogger<TaskService> logger)
    {
        this.userService = userService;
        this.workHunterDbContext = workHunterDbContext;
        this.notificationsService = notificationsService;
        this.logger = logger;
    }

    public Task CompletedBatchByHand(IEnumerable<int> ids, string reason)
    {
        throw new NotImplementedException();
    }

    public Task CompletedByHand(int id, string reason)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> CreateTask(TaskType type, Guid wresponseId, UserTaskDto? dto = null)
    {
        var wresponse = await workHunterDbContext.WResponses.FirstOrDefaultAsync(x => x.Id == wresponseId) ?? throw new EntityNotFoundException($"Отклик {wresponseId} не найден!");

        var responsible = await userService.GetById(wresponse.UserId);

        var userTaskType = await workHunterDbContext.UserTaskTypes.FirstOrDefaultAsync(x => x.Type == type) ?? throw new EntityNotFoundException("Тип задачи не найден!");

        if (!userTaskType.IsActive) 
            return false;

        var currentTime = DateTime.UtcNow;

        var task = new UserTask()
        {
            TypeId = userTaskType.Id,
            Status = UserTaskStatus.Open,
            ResponsibleId = responsible.Id,
            LastNotificationDate = currentTime,
            Created = currentTime,
            WResponseId = wresponseId,
            Text = ""
        };

        var reminderNotification = GetTaskTemplate(userTaskType, task, responsible, dto);
        workHunterDbContext.UserTasks.Add(task);
        await workHunterDbContext.SaveChangesAsync();
        // await SendUserNotification(task, reminderNotification); TODO send via microservice

        return true;
    }

    private static string? GetTaskTemplate(UserTaskType userTaskType, UserTask task, UserView responsible, UserTaskDto? dto = null)
    {
        string? initialNotificationText = string.Empty;
        switch (userTaskType.Type)
        {
            case TaskType.SendDocuments:
                //task.Text = userTaskType.TaskText.Replace();
                initialNotificationText = userTaskType.InitialNotificationText;
                break;
            default: break;
        }

        return initialNotificationText;
    }

    public async Task<UserTaskView> Get(int id)
    {
        var userTask = await workHunterDbContext.UserTasks
                                                .Include(x => x.Responsible)
                                                .Include(x => x.WResponse)
                                                .Include(x => x.Type)
                                                .AsSplitQuery()
                                                .AsNoTracking()
                                                .FirstOrDefaultAsync(x => x.Id == id) ?? throw new EntityNotFoundException($"Задача {id} не найдена!");

        var currentUser = await userService.GetCurrent();

        if (currentUser?.Id != userTask.ResponsibleId)
            throw new BusinessErrorException($"Задача {id} не доступна!");

        return userTask.Adapt<UserTaskView>();
    }

    public Task<IReadOnlyList<UserTaskView>> GetAll(TaskStatus status, TaskType? type = null, DateTime? created = null)
    {
        throw new NotImplementedException();
    }

    public Task<IReadOnlyList<UserTaskView>> GetForCurrentUser(TaskStatus status, DateTime? created = null)
    {
        throw new NotImplementedException();
    }

    public async Task SendReminderNotifications()
    {
        var userTasks = await workHunterDbContext.UserTasks
                                                .Include(x => x.Responsible)
                                                .Include(x => x.WResponse)
                                                .Include(x => x.Type)
                                                .AsSplitQuery()
                                                .AsNoTracking()
                                                .Where(x => x.Status == UserTaskStatus.Open)
                                                .ToListAsync();

        // TODO check is reminder notifications on tasks for each user is active

        foreach (var userTask in userTasks)
        {
            if (userTask.Type == null)
            {
                logger.LogError("У задачи {UserTask} не определен тип", userTask);
                continue;
            }

            // TODO: for active reminders get last log on notification date for user and task: var lastLog = await notificationService.GetLastLog( task, email, DateTime.UtcNow)
            var reminderNotification = GetTaskTemplate(userTask.Type, userTask, userTask.Responsible.Adapt<UserView>());

            if (string.IsNullOrEmpty(reminderNotification))
            {
                logger.LogError("У задачи {UserTask} типа {userTaskType} не определен текст нотификации", userTask, userTask.Type);
                continue;
            }

            var isSent = await notificationsService.SendTaskReminder(userTask, userTask.Responsible, reminderNotification);
            // TODO if (isSent) notificationsService.AddLog(notification.Id, emails);
        }
    }
}
