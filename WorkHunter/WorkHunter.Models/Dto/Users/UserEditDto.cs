using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkHunter.Models.Dto.Users
{
    public record UserEditDto
    {
        // Identity name
        public required string UserName { get; init; }

        // Alias for FE
        public required string Name { get; init; }

        public required string Email { get; init; }

        public IReadOnlyList<string> Roles { get; init; }
    }
}
