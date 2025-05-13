using Abstractions.Users;
using MediatR;
using Moq;
using WorkHunter.Data;
using WorkHunter.Models.Dto.WorkHunters;
using WorkHunter.Models.Entities.WorkHunters;
using WorkHunter.Models.Enums;
using WorkHunter.Services.WorkHunters;

namespace WorkHunter.UnitTests.WResponseServiceTests;

[TestClass]
public sealed class SetStatusForWResponseTest
{
    [TestMethod]
    public void SetStatusForWResponseTest_SetStatusInitiallyViewedByMe()
    {
        Mock<IWorkHunterDbContext> mockWorkHunterDbContext = new();
        Mock<IUserService> mockUserService = new();
        Mock<IMediator> mockMediatorService = new();
        var mockWresponseService = new WResponseService(mockWorkHunterDbContext.Object, mockUserService.Object, mockMediatorService.Object);

        var wResponse = new WResponse() { IsAnswered = false, UserId = "1", VacancyUrl = "test@test.ru" };
        WResponseService.SetStatusForWResponse(wResponse, new WResponseUpdateDto() { AnswerText = "", IsAnswered= true });

        Assert.IsFalse(wResponse.IsAnswered);
        Assert.AreEqual(ResponseStatus.InitiallyViewedByMe, wResponse.Status);
    }

    [TestMethod]
    public void SetStatusForWResponseTest_SetStatusInitiallyViewedByEmployee()
    {
        Mock<IWorkHunterDbContext> mockWorkHunterDbContext = new();
        Mock<IUserService> mockUserService = new();
        Mock<IMediator> mockMediatorService = new();
        var mockWresponseService = new WResponseService(mockWorkHunterDbContext.Object, mockUserService.Object, mockMediatorService.Object);

        var wResponse = new WResponse() { IsAnswered = false, UserId = "1", VacancyUrl = "test@test.ru" };
        WResponseService.SetStatusForWResponse(wResponse, new WResponseUpdateDto() { AnswerText = "Response from employee", IsAnswered = false });

        Assert.IsFalse(wResponse.IsAnswered);
        Assert.AreEqual(ResponseStatus.InitiallyViewedByEmployee, wResponse.Status);
    }
}
