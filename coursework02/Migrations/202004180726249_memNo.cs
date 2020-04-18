namespace coursework02.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class memNo : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Members", "MemberNo", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Members", "MemberNo");
        }
    }
}
