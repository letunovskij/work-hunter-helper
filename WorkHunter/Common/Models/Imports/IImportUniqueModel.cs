using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Models.Imports
{
    public interface IImportUniqueModel<in UniqueModel> where UniqueModel : IImportModel
    {
        public bool EqualsByUniqueIndex(UniqueModel? other);
    }
}
