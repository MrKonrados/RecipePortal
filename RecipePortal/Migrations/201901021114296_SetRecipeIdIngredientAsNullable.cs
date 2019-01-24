namespace RecipePortal.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SetRecipeIdIngredientAsNullable : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Ingredients", "RecipeId", "dbo.Recipes");
            DropIndex("dbo.Ingredients", new[] { "RecipeId" });
            AlterColumn("dbo.Ingredients", "RecipeId", c => c.Int());
            CreateIndex("dbo.Ingredients", "RecipeId");
            AddForeignKey("dbo.Ingredients", "RecipeId", "dbo.Recipes", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Ingredients", "RecipeId", "dbo.Recipes");
            DropIndex("dbo.Ingredients", new[] { "RecipeId" });
            AlterColumn("dbo.Ingredients", "RecipeId", c => c.Int(nullable: false));
            CreateIndex("dbo.Ingredients", "RecipeId");
            AddForeignKey("dbo.Ingredients", "RecipeId", "dbo.Recipes", "Id", cascadeDelete: true);
        }
    }
}
