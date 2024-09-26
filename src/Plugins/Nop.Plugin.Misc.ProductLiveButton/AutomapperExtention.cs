using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.Wordprocessing;
using Nop.Core;
using Nop.Core.Configuration;
using Nop.Core.Infrastructure.Mapper;
using Nop.Services.Plugins;
using Nop.Web.Framework.Models;

namespace Nop.Plugin.Misc.ProductLiveButton;
public static class AutomapperExtention
{
    public static IList<TDestination> Map<TDestination>(this List<object> source)
    {
        //use AutoMapper for mapping objects
        return AutoMapperConfiguration.Mapper.Map<IList<TDestination>>(source);
    }
    public static IList<TModel> ToModelList<TEntity, TModel>(this IList<TEntity> entity)
        where TEntity : BaseEntity where TModel : BaseNopEntityModel
    {
        ArgumentNullException.ThrowIfNull(entity);

        //ArgumentNullException.ThrowIfNull(model);
        return AutoMapperConfiguration.Mapper.Map<IList<TModel>>(entity);
        //return entity.Map<IList<TModel>>();
    }
}
