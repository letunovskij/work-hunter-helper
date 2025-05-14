using Mapster;

namespace WorkHunter.Services.Utils;

public static class CollectionUtils
{
    /// <summary>
    /// Обновляет целевую коллекцию dst (например, таблица базы данных) из коллекции источника src (например, список dtos с fe приложения). Элементы с ключами-дублями не учитываются 
    /// </summary>
    /// <typeparam name="TDestination"></typeparam>
    /// <typeparam name="TSource"></typeparam>
    /// <typeparam name="Tkey"></typeparam>
    /// <typeparam name="TComparator"></typeparam>
    /// <param name="dst"></param>     
    /// <param name="src"></param>
    /// <param name="keyDestination"></param> 
    /// <param name="keySource"></param>
    /// <param name="comparer">Компаратор для уникального индекса/составного ключа</param>
    /// <param name="removeFromSourceNotFounded"></param>
    public static void UpdateDestinationFromSource<TDestination, TSource, Tkey, TComparator>(
        ICollection<TDestination> dst,
        IEnumerable<TSource> src,
        Func<TDestination, Tkey> keyDestination,
        Func<TSource, Tkey> keySource,
        TComparator comparer,
        bool? removeFromSourceNotFounded = false) where TComparator: IEqualityComparer<Tkey?>
    {
        var toRemove = new List<TDestination>();

        if (dst.Any())
            foreach (var dstItem in dst)
            {
                var srcItem = src.FirstOrDefault(x => comparer.Equals(keySource(x), keyDestination(dstItem)));

                if (srcItem != null)
                    src.Adapt(dstItem);
                else
                    toRemove.Add(dstItem);          
            }

        if (removeFromSourceNotFounded ?? false)
            foreach (var dstItem in toRemove)
                dst.Remove(dstItem);

        if (dst.Any()) 
        { 
            var dstKeysList = dst.Select(keyDestination).ToList();
            foreach (var srcItem in src)
            {
                if (!dstKeysList.Contains(keySource(srcItem)))
                {
                    var dstItem = srcItem.Adapt<TDestination>();
                    dst.Add(dstItem);
                    dstKeysList.Add(keySource(srcItem));
                }
            }
        } else
        {
            foreach (var srcItem in src)
                dst.Add(srcItem.Adapt<TDestination>());
        }
    }

    /// <summary>
    /// Сравненение на равенство коллекций по ключу
    /// </summary>
    /// <typeparam name="TDestination"></typeparam>
    /// <typeparam name="TSource"></typeparam>
    /// <typeparam name="Tkey"></typeparam>
    /// <param name="dst"></param>
    /// <param name="src"></param>
    /// <param name="keyDestination"></param>
    /// <param name="keySource"></param>
    /// <returns></returns>
    internal static bool Equals<TDestination, TSource, Tkey>(
        ICollection<TDestination> dst,
        ICollection<TSource> src,
        Func<TDestination, Tkey> keyDestination,
        Func<TSource, Tkey> keySource)
    {
        var dstKeys = dst.Select(keyDestination).Distinct().ToList();
        var srcKeys = src.Select(keySource).Distinct().ToList();

        if (dstKeys.Count != src.Count)
            return false;

        if (dstKeys.Except(srcKeys).Any())
            return false;

        if (srcKeys.Except(dstKeys).Any())
            return false;

        return true;
    }
}
