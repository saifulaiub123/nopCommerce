using FluentMigrator;
using Nop.Data;
using Nop.Data.Extensions;
using Nop.Data.Migrations;
using Nop.Plugin.Misc.ProductLiveButton.Domain;

namespace Nop.Plugin.Misc.ProductLiveButton.Data.Migrations;

[NopMigration("2024/09/21 08:45:00", "Nop.Plugin.Misc.LiveButton Schema", MigrationProcessType.Installation)]

public class SchemaMigration : Migration
{
    public override void Up()
    {
        if (!DataSettingsManager.IsDatabaseInstalled())
            return;
        if (!Schema.Table(nameof(ProductDemo)).Exists())
            Create.TableFor<ProductDemo>();
    }
    public override void Down()
    {

    }
}
