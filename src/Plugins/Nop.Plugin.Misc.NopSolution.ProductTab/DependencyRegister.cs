//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Autofac;
//using AutoMapper;
//using Nop.Core.Infrastructure;

//namespace Nop.Plugin.Misc.VendorRegistration;
//public class DependencyRegistrar : IDependencyRegistrar
//{
//    public void Register(ContainerBuilder builder, ITypeFinder typeFinder, NopConfig config)
//    {
//        // Register AutoMapper
//        builder.Register(c => new MapperConfiguration(cfg =>
//        {
//            // Load all mapping profiles in the assembly
//            cfg.AddMaps(AppDomain.CurrentDomain.GetAssemblies());
//        })).AsSelf().SingleInstance();

//        builder.Register(c => c.Resolve<MapperConfiguration>().CreateMapper()).As<IMapper>().InstancePerLifetimeScope();
//    }

//    public int Order => 0;
//}
