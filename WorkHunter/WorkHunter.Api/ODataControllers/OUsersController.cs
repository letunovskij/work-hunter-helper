using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.EntityFrameworkCore;
using WorkHunter.Data;
using WorkHunter.Models.Entities.Users;

namespace WorkHunter.Api.ODataControllers
{
    public class OUsersController : ODataController
    {
        private readonly IWorkHunterDbContext dbContext;

        public OUsersController(
            IWorkHunterDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [EnableQuery]
        public async Task<ActionResult<IEnumerable<User>>> Get()
        {
            return Ok(await dbContext.Users.Include(x => x.Responses).AsSplitQuery().AsNoTracking().ToListAsync());
        }
    }
}
