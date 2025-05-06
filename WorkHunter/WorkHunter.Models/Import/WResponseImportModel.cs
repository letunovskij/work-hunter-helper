using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Attributes;
using Common.Extensions;
using Common.Models.Imports;
using DocumentFormat.OpenXml.Drawing.Charts;

namespace WorkHunter.Models.Import
{
    public sealed class WResponseImportModel : IImportModel, IEquatable<WResponseImportModel?>, IImportUniqueModel<WResponseImportModel>
    {
        [ImportColumn(Name = WResponseImportModelConstants.Email, IsRequired = false)]
        public string? Email { get; set; }

        [ImportColumn(Name = WResponseImportModelConstants.Email, IsRequired = true)]
        public string? UserId { get; set; }

        [ImportColumn(Name = WResponseImportModelConstants.VacancyUrl, IsRequired = true)]
        public string? VacancyUrl { get; set; }

        // TODO add other fields

        public override bool Equals(object? obj) => Equals(obj as WResponseImportModel);

        public bool Equals(WResponseImportModel? model) => model is WResponseImportModel other
            && UserId.EqualsIgnoreCase(other.UserId)
            && VacancyUrl.EqualsIgnoreCase(other.VacancyUrl)
            && Email.EqualsIgnoreCase(other.Email);

        public override int GetHashCode() => HashCode.Combine(Email, UserId);

        public bool EqualsByUniqueIndex(WResponseImportModel? model) => model is WResponseImportModel other
            && VacancyUrl.EqualsIgnoreCase(other.VacancyUrl)
            && Email.EqualsIgnoreCase(other.Email);
    }

    public static class WResponseImportModelConstants
    {
        public const string Email = "Email";

        public const string UserId = "UserId";

        public const string VacancyUrl = "VacancyUrl";
    }
}
