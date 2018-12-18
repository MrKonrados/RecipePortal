namespace RecipePortal.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddReqDirectivesInRecipeModel : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Recipes", "AboutDish", c => c.String(nullable: false));
            AlterColumn("dbo.Recipes", "Directions", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Recipes", "Directions", c => c.String());
            AlterColumn("dbo.Recipes", "AboutDish", c => c.String());
        }
    }
}
