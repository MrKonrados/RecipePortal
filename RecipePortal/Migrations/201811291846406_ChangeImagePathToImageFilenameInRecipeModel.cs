namespace RecipePortal.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeImagePathToImageFilenameInRecipeModel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Recipes", "ImageFilename", c => c.String(maxLength: 255));
            DropColumn("dbo.Recipes", "ImagePath");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Recipes", "ImagePath", c => c.String(maxLength: 1024));
            DropColumn("dbo.Recipes", "ImageFilename");
        }
    }
}
