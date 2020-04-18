namespace coursework02.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Albums",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        CopyNumber = c.String(),
                        ReleaseDate = c.DateTime(nullable: false),
                        Length = c.Int(nullable: false),
                        StudioName = c.String(),
                        IsAgeBar = c.Boolean(nullable: false),
                        FinePerDay = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CoverImagePath = c.String(),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.Artists",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                        Email = c.String(),
                        BirthDate = c.DateTime(nullable: false),
                        Gender = c.String(),
                        PhoneNumber = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Producers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Studio = c.String(nullable: false),
                        DateOfBirth = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Loans",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        MemberId = c.Int(nullable: false),
                        AlbumId = c.Int(nullable: false),
                        LoanTypeId = c.Int(nullable: false),
                        TotalAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        FineAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        IssuedDate = c.DateTime(nullable: false),
                        DueDate = c.DateTime(nullable: false),
                        ReturnedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Albums", t => t.AlbumId, cascadeDelete: true)
                .ForeignKey("dbo.LoanTypes", t => t.LoanTypeId, cascadeDelete: true)
                .ForeignKey("dbo.Members", t => t.MemberId, cascadeDelete: true)
                .Index(t => t.MemberId)
                .Index(t => t.AlbumId)
                .Index(t => t.LoanTypeId);
            
            CreateTable(
                "dbo.LoanTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Description = c.String(),
                        Days = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Members",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FullName = c.String(nullable: false),
                        Address = c.String(nullable: false),
                        Email = c.String(),
                        Contact = c.String(nullable: false),
                        DateOfBirth = c.DateTime(nullable: false),
                        MemberCategoryId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.MemberCategories", t => t.MemberCategoryId, cascadeDelete: true)
                .Index(t => t.MemberCategoryId);
            
            CreateTable(
                "dbo.MemberCategories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Description = c.String(),
                        TotalLoan = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ArtistAlbum",
                c => new
                    {
                        ArtistId = c.Int(nullable: false),
                        AlbumId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ArtistId, t.AlbumId })
                .ForeignKey("dbo.Artists", t => t.ArtistId, cascadeDelete: true)
                .ForeignKey("dbo.Albums", t => t.AlbumId, cascadeDelete: true)
                .Index(t => t.ArtistId)
                .Index(t => t.AlbumId);
            
            CreateTable(
                "dbo.ProducerAlbum",
                c => new
                    {
                        ProducerId = c.Int(nullable: false),
                        AlbumId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ProducerId, t.AlbumId })
                .ForeignKey("dbo.Producers", t => t.ProducerId, cascadeDelete: true)
                .ForeignKey("dbo.Albums", t => t.AlbumId, cascadeDelete: true)
                .Index(t => t.ProducerId)
                .Index(t => t.AlbumId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Loans", "MemberId", "dbo.Members");
            DropForeignKey("dbo.Members", "MemberCategoryId", "dbo.MemberCategories");
            DropForeignKey("dbo.Loans", "LoanTypeId", "dbo.LoanTypes");
            DropForeignKey("dbo.Loans", "AlbumId", "dbo.Albums");
            DropForeignKey("dbo.ProducerAlbum", "AlbumId", "dbo.Albums");
            DropForeignKey("dbo.ProducerAlbum", "ProducerId", "dbo.Producers");
            DropForeignKey("dbo.ArtistAlbum", "AlbumId", "dbo.Albums");
            DropForeignKey("dbo.ArtistAlbum", "ArtistId", "dbo.Artists");
            DropIndex("dbo.ProducerAlbum", new[] { "AlbumId" });
            DropIndex("dbo.ProducerAlbum", new[] { "ProducerId" });
            DropIndex("dbo.ArtistAlbum", new[] { "AlbumId" });
            DropIndex("dbo.ArtistAlbum", new[] { "ArtistId" });
            DropIndex("dbo.Members", new[] { "MemberCategoryId" });
            DropIndex("dbo.Loans", new[] { "LoanTypeId" });
            DropIndex("dbo.Loans", new[] { "AlbumId" });
            DropIndex("dbo.Loans", new[] { "MemberId" });
            DropTable("dbo.ProducerAlbum");
            DropTable("dbo.ArtistAlbum");
            DropTable("dbo.MemberCategories");
            DropTable("dbo.Members");
            DropTable("dbo.LoanTypes");
            DropTable("dbo.Loans");
            DropTable("dbo.Producers");
            DropTable("dbo.Artists");
            DropTable("dbo.Albums");
        }
    }
}
