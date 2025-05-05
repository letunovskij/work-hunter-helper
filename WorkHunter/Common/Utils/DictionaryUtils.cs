using Common.Models.Imports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Utils
{
    public static class DictionaryUtils
    {
        public static (bool Contains, int? KeyOfOriginalRow) ContainsByKey<TImportModel>(
            Dictionary<int, TImportModel> importingCollection,
            IImportUniqueModel<TImportModel> importingModel) where TImportModel : IImportModel
        {
            if (importingModel != null)
                foreach (var entry in importingCollection)
                    if (importingModel.EqualsByUniqueIndex(entry.Value))
                        return (true, entry.Key);

            return (false, null);
        }
    }
}
