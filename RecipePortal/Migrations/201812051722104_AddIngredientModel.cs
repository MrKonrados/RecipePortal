namespace RecipePortal.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddIngredientModel : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Ingrediants",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        RecipeId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Recipes", t => t.RecipeId, cascadeDelete: true)
                .Index(t => t.RecipeId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Ingrediants", "RecipeId", "dbo.Recipes");
            DropIndex("dbo.Ingrediants", new[] { "RecipeId" });
            DropTable("dbo.Ingrediants");
        }
    }
}
