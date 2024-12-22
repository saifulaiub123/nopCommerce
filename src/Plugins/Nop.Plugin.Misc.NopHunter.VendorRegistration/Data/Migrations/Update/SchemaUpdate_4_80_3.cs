using FluentMigrator;
using Nop.Data.Migrations;
using Nop.Data;
using Nop.Plugin.Misc.VendorRegistration.Domain;
using Nop.Core.Domain.Catalog;
using Nop.Core.Domain.Affiliates;
using Nop.Data.Extensions;
using System.Data;

namespace Nop.Plugin.Misc.VendorRegistration.Data.Migrations.Update;
[NopMigration("2024/09/25 08:45:00", "Nop.Plugin.Misc.LiveButton Schema update 4.80.3", MigrationProcessType.Update)]

public class SchemaUpdate_4_80_3 : Migration
{
    public override void Up()
    {
        if (!DataSettingsManager.IsDatabaseInstalled())
            return;
        if (Schema.Table(nameof(ProductDemo)).Column("ShowInProductPictureBottom").Exists())
            Delete.Column("ShowInProductPictureBottom").FromTable(nameof(ProductDemo));

        if (!Schema.Table(nameof(ProductDemo)).Exists())
            Create.Table("Test123")
            .WithColumn("Id").AsInt32().NotNullable().PrimaryKey().Identity()
            .WithColumn("ProductId").AsInt32().ForeignKey<Product>().OnDelete(Rule.None);
        //.WithColumn(nameof(Affiliate.AddressId)).AsInt32().ForeignKey<Address>().OnDelete(Rule.None);
    }
    public override void Down()
    {
        
    }

}
