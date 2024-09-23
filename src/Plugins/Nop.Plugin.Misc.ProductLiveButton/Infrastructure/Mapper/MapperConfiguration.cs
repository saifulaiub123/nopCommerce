using AutoMapper;
using Nop.Core.Infrastructure.Mapper;
using Nop.Plugin.Misc.ProductLiveButton.Domain;
using Nop.Plugin.Misc.ProductLiveButton.Models;

namespace Nop.Plugin.Misc.ProductLiveButton.Infrastructure.Mapper;
public class MapperConfiguration : Profile, IOrderedMapperProfile
{
    #region Ctor

    public MapperConfiguration()
    {
        CreateMap<ProductDemo, ProductDemoModel>();
        CreateMap<ProductDemoModel, ProductDemo>();
    }

    #endregion

    #region Properties

    /// <summary>
    /// Order of this mapper implementation
    /// </summary>
    public int Order => 1;

    #endregion
}
