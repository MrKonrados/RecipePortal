namespace RecipePortal.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RenameIngrediantToIngredient : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.Ingrediants", newName: "Ingredients");
        }
        
        public override void Down()
        {
            RenameTable(name: "dbo.Ingredients", newName: "Ingrediants");
        }
    }
}
