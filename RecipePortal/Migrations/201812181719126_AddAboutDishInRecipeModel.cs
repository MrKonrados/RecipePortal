namespace RecipePortal.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddAboutDishInRecipeModel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Recipes", "AboutDish", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Recipes", "AboutDish");
        }
    }
}
