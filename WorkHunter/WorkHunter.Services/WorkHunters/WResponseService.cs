using Common.Exceptions;
using Mapster;
using Microsoft.EntityFrameworkCore;
using WorkHunter.Abstractions.WorkHunters;
using WorkHunter.Data;
using WorkHunter.Models.Dto.WorkHunters;
using WorkHunter.Models.Entities.WorkHunters;
using WorkHunter.Models.Views.WorkHunters;
using Abstractions.Users;
using WorkHunter.Models.Enums;
using MediatR;
using WorkHunter.Models.MediatrNotifications.Wresponses;

namespace WorkHunter.Services.WorkHunters;

public sealed class WResponseService : IWResponseService
{
    private readonly IWorkHunterDbContext dbContext;
    private readonly IUserService userService;
    private readonly IMediator mediator;

    public WResponseService(
        IWorkHunterDbContext dbContext,
        IUserService userService,
        IMediator mediator)
    {
        this.dbContext = dbContext;
        this.userService = userService;
        this.mediator = mediator;
    }

    public async Task<WResponseView> GetById(Guid guid)
    {
        var currentUser = await userService.GetCurrent();

        var response = await dbContext.WResponses
                                      .Include(x => x.User)
                                      .AsNoTracking()
                                      .SingleOrDefaultAsync(x => x.Id == guid 
                                                              && x.UserId == currentUser.Id) 
            ?? throw new EntityNotFoundException(nameof(WResponse), guid);

        return response.Adapt<WResponseView>();
    }

    public async Task<IReadOnlyList<WResponseView>> GetAll()
    {
        var currentUser = await userService.GetCurrent();

        var responses = await dbContext.WResponses.Where(x => x.UserId == currentUser.Id
                                                           && !x.IsDeleted)
                                                  .Include(x => x.User)
                                                  .AsNoTracking()
                                                  .ToListAsync();

        return responses.Adapt<IReadOnlyList<WResponseView>>();
    }

    public async Task<WResponseView> Create(WResponseCreateDto dto)
    {
        var currentUser = await userService.GetCurrent();

        if (await dbContext.WResponses.AnyAsync(x => x.UserId == currentUser.Id
                                                  && string.Equals(x.VacancyUrl, dto.VacancyUrl.Trim().ToUpper())))
            throw new BusinessErrorException($"Отклик на вакансию {dto.VacancyUrl} уже был сделан!");

        var wResponse = dto.Adapt<WResponse>();
        wResponse.UserId = currentUser.Id;
        await SetStatus(wResponse, dto);

        dbContext.WResponses.Add(wResponse);

        await dbContext.SaveChangesAsync();

        return await GetById(wResponse.Id);
    }

    internal async Task SetStatus(WResponse response, WResponseUpdateDto dto)
    {
        var previousStatus = response.Status;

        SetStatusForWResponse(response, dto);

        if (previousStatus != response.Status)
            await mediator.Publish(new WResponseChangedStatusNotification() { Wresponse = response });
    }

    internal static void SetStatusForWResponse(WResponse response, WResponseUpdateDto dto)
    {
        if (dto is WResponseCreateDto)
            response.Status = ResponseStatus.Open;

        if (!string.IsNullOrEmpty(dto.AnswerText) && response.Status != ResponseStatus.InitiallyViewedByEmployee)
            response.Status = ResponseStatus.InitiallyViewedByEmployee;

        if (dto.IsAnswered != null && dto.IsAnswered.Value && response.Status != ResponseStatus.InitiallyViewedByMe)
            response.Status = ResponseStatus.InitiallyViewedByMe;
    }

    public async Task<WResponseView> Update(Guid guid, WResponseUpdateDto dto)
    {
        var currentUser = await userService.GetCurrent();

        var wResponse = await dbContext.WResponses.SingleOrDefaultAsync(x => x.UserId == currentUser.Id
                                                                          && x.Id == guid)
            ?? throw new EntityNotFoundException(nameof(WResponse), guid);

        CheckIsDeleted(wResponse);

        dto.Adapt(wResponse);
        wResponse.UserId = currentUser.Id;
        await SetStatus(wResponse, dto);
        await dbContext.SaveChangesAsync();

        return await GetById(wResponse.Id);
    }

    private static void CheckIsDeleted(WResponse wResponse)
    {
        if (wResponse.IsDeleted)
            throw new EntityDeletedException(nameof(WResponse), wResponse.Id);
    }

    public async Task Delete(Guid guid)
    {
        var currentUser = await userService.GetCurrent();

        var wResponse = await dbContext.WResponses.SingleOrDefaultAsync(x => x.UserId == currentUser.Id
                                                                          && x.Id == guid)
            ?? throw new EntityNotFoundException(nameof(WResponse), guid);

        CheckIsDeleted(wResponse);

        wResponse.IsDeleted = true;

        await dbContext.SaveChangesAsync();
    }
}
