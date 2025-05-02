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
using Common.Models;
using Common.Utils;
using ClosedXML.Excel;
using Common.Enums;
using WorkHunter.Models.Enums.Import;
using Common.Extensions;

namespace WorkHunter.Services.WorkHunters;

public sealed class WResponseService : IWResponseService
{
    private readonly WorkHunterDbContext dbContext;
    private readonly IUserService userService;
    private readonly IWResponseImportService wresponseImportService;

    public WResponseService(
        WorkHunterDbContext dbContext,
        IUserService userService,
        IWResponseImportService wresponseImportService)
    {
        this.dbContext = dbContext;
        this.userService = userService;
        this.wresponseImportService = wresponseImportService;
    }

    public async Task<WResponseView> GetById(Guid guid)
    {
        var currentUser = await userService.GetCurrent();

        var response = await dbContext.WResponses.SingleOrDefaultAsync(x => x.Id == guid 
                                                                         && x.UserId == currentUser.Id) 
            ?? throw new EntityNotFoundException(nameof(WResponse), guid);

        return response.Adapt<WResponseView>();
    }

    public async Task<IReadOnlyList<WResponseView>> GetAll()
    {
        var currentUser = await userService.GetCurrent();

        var responses = await dbContext.WResponses.Where(x => x.UserId == currentUser.Id
                                                           && !x.IsDeleted)
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
        SetStatusForWResponse(wResponse, dto);
        wResponse.UserId = currentUser.Id;
        dbContext.WResponses.Add(wResponse);

        await dbContext.SaveChangesAsync();

        return await GetById(wResponse.Id);
    }

    internal static void SetStatusForWResponse(WResponse response, WResponseUpdateDto dto)
    {
        if (dto is WResponseCreateDto)
            response.Status = ResponseStatus.Open;

        if (!string.IsNullOrEmpty(dto.AnswerText))
            response.Status = ResponseStatus.InitiallyViewedByEmployee;

        if (dto.IsAnswered != null && dto.IsAnswered.Value)
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
        SetStatusForWResponse(wResponse, dto);
        wResponse.UserId = currentUser.Id;
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

    public async Task<DownloadFile?> ImportNewData(Stream stream)
    {
        var workbook = new XLWorkbook(stream);
        await using var transaction = await dbContext.Database.BeginTransactionAsync();

        try
        {
            await dbContext.WResponses.ExecuteDeleteAsync();
            await ImportData(workbook, ImportMode.RemoveAllThenAdd);

            await transaction.CommitAsync();
            return null;
        }
        catch (Exception)
        {
            await transaction.RollbackAsync();
            dbContext.ChangeTracker.Clear();

            return ExcelUtils.DownloadFile(workbook, $"wresponses-error-{DateTime.Now:s}");
        }
    }

    private async Task ImportData(XLWorkbook workbook, ImportMode mode)
    {
        var wresponsesWorksheet = workbook.Worksheets.First(x => x.Name == WresponsePageType.WresponsePage.GetDescription());

        wresponseImportService.ImportDataToCollection(wresponsesWorksheet);
        if (mode == ImportMode.Add)
            await wresponseImportService.CheckOnExists();

        var error = await wresponseImportService.AddToDb();
        if (error != null)
            wresponseImportService.UpdatePageWithError(wresponsesWorksheet);

        await dbContext.SaveChangesAsync();
    }

    public Task Export()
    {
        throw new NotImplementedException();
    }
}
