using FluentMigrator;
using System.Data;

namespace RecipeBook.Infrastructure.Migrations.Versions;

[Migration(DatabaseVersions.IMAGES_FOR_RECIPES, "Add collumn on recipe table to save images")]
public class Version0000003 : VersionBase
{
    private const string RECIPE_TABLE_NAME = "Recipes";

    public override void Up()
    {
        Alter.Table(RECIPE_TABLE_NAME).AddColumn("ImageIdentifier").AsString().Nullable();
    }
}

