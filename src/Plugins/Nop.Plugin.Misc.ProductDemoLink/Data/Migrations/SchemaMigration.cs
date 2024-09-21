﻿using FluentMigrator;
using Nop.Data.Extensions;
using Nop.Data.Migrations;
using Nop.Plugin.Misc.ProductDemoLink.Domain;

namespace Nop.Plugin.Misc.ProductDemoLink.Data.Migrations;

[NopMigration("2024/09/21 08:45:00", "Nop.Plugin.Misc.LiveButton Schema", MigrationProcessType.Installation)]

public class SchemaMigration : AutoReversingMigration
{
    public override void Up()
    {
        Create.TableFor<ProductDemo>();
    }

}
