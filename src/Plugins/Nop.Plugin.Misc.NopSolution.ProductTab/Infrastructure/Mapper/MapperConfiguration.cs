using AutoMapper;
using Nop.Core.Infrastructure.Mapper;
using Nop.Plugin.Misc.VendorRegistration.Domain;
using Nop.Plugin.Misc.VendorRegistration.Models;

namespace Nop.Plugin.Misc.VendorRegistration.Infrastructure.Mapper;
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
