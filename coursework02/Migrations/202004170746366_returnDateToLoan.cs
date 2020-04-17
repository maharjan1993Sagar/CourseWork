namespace coursework02.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class returnDateToLoan : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Loans", "DueDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.Loans", "ReturnedDate", c => c.DateTime());
            DropColumn("dbo.Loans", "ReturnDate");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Loans", "ReturnDate", c => c.DateTime(nullable: false));
            DropColumn("dbo.Loans", "ReturnedDate");
            DropColumn("dbo.Loans", "DueDate");
        }
    }
}
