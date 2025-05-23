﻿using Abstractions.Users;
using Common.Models.Files;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
// using Microsoft.Extensions.Configuration; // Common TODO: extract Report project, extract Background Tasks project
using System.Reflection;
using WorkHunter.Abstractions.Enums;
using WorkHunter.Abstractions.Exports;
using WorkHunter.Abstractions.Imports;
using WorkHunter.Abstractions.Interviews;
using WorkHunter.Abstractions.Notifications;
using WorkHunter.Abstractions.Settings;
using WorkHunter.Abstractions.WorkHunters;
using WorkHunter.Api.Middleware;
using WorkHunter.BackgroundTasks;
using WorkHunter.Models.Config;
using WorkHunter.Models.Dto.Users.Validators;
using WorkHunter.Models.MediatrNotifications.Wresponses;
using WorkHunter.Services.Enums;
using WorkHunter.Services.Exports;
using WorkHunter.Services.Imports;
using WorkHunter.Services.Interviews;
using WorkHunter.Services.MediatrNotificationsHandlers.Wresponses;
using WorkHunter.Services.Notifications;
using WorkHunter.Services.Settings;
using WorkHunter.Services.Users;
using WorkHunter.Services.WorkHunters;

namespace WorkHunter.Api;

public static class ServicesConfiguration
{
    public static IServiceCollection RegisterApplicationServices(this IServiceCollection services, IConfiguration config)
    {
        services.AddSingleton<IAuthorizationMiddlewareResultHandler, UserAccessHandler>();

        services.AddOptions<AuthOptions>()
                .Bind(config.GetSection("AuthOptions"))
                .ValidateDataAnnotations()
                .ValidateOnStart();

        services.AddOptions<StorageOptions>()
                .Bind(config.GetSection("StorageOptions"))
                .ValidateDataAnnotations()
                .ValidateOnStart();

        services.AddOptionsWithValidateOnStart<EmailOptions>()
                .Bind(config.GetSection("EmailOptions"))
                .ValidateDataAnnotations();

        services.AddOptionsWithValidateOnStart<SendUserTaskReminderNotificationOptions>()
                .Bind(config.GetSection("BackgroundTasksOptions:SendUserTaskReminderNotificationOptions"))
                .ValidateDataAnnotations();

        services.AddHttpContextAccessor();

        services.AddHostedService<SendUserTaskReminderNotificationTask>();

        services.AddValidatorsFromAssemblyContaining<LoginDtoValidator>();

        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IWResponseService, WResponseService>();
        services.AddScoped<IVideoInterviewFileService, VideoInterviewFileService>();
        services.AddScoped<IWResponseImportService, WResponseImportService>();
        services.AddScoped<IWResponsesExportService, WResponsesExportService>();
        services.AddScoped<ITaskService, TaskService>();
        services.AddScoped<INotificationsService, NotificationsService>();
        services.AddScoped<IEnumService, EnumService>();
        services.AddScoped<IUserSettingsService, UserSettingsService>();
        services.AddScoped<ISettingsService, SettingsService>();        

        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(typeof(WResponseChangedStatusNotificationHandler).GetTypeInfo().Assembly);
            cfg.RegisterServicesFromAssembly(typeof(WResponseChangedStatusNotification).GetTypeInfo().Assembly);            
        });
        
        return services;
    }
}
