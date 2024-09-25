using System.Data;
using FluentMigrator.Builders.Create.Table;
using Nop.Core.Domain.Catalog;
using Nop.Data.Extensions;
using Nop.Data.Mapping.Builders;
using Nop.Plugin.Misc.ProductLiveButton.Domain;

namespace Nop.Plugin.Misc.ProductLiveButton.Data.Mapping;
public class ProductLiveButtonBuilder : NopEntityBuilder<ProductDemo>
{
    public override void MapEntity(CreateTableExpressionBuilder table)
    {
        table.WithColumn(nameof(ProductDemo.ProductId)).AsInt32().ForeignKey<Product>(onDelete: Rule.None)
        .WithColumn(nameof(ProductDemo.DemoLink)).AsString(1000).Nullable();
        //.WithColumn(nameof(ProductDemo.ShowInProductPictureBottom)).AsBoolean().WithDefaultValue(false);
    }
}