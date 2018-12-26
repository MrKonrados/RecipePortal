namespace RecipePortal.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateRatingModel : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Ratings",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Score = c.Int(nullable: false),
                        RecipeId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Recipes", t => t.RecipeId, cascadeDelete: true)
                .Index(t => t.RecipeId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Ratings", "RecipeId", "dbo.Recipes");
            DropIndex("dbo.Ratings", new[] { "RecipeId" });
            DropTable("dbo.Ratings");
        }
    }
}
