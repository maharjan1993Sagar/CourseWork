namespace coursework02.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CopyNumberToAlbum : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Albums", "CopyNumber", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Albums", "CopyNumber");
        }
    }
}
