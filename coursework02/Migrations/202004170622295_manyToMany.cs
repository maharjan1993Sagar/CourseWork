namespace coursework02.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class manyToMany : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ArtistAlbums", "AlbumId", "dbo.Albums");
            DropForeignKey("dbo.ArtistAlbums", "ArtistId", "dbo.Artists");
            DropForeignKey("dbo.ArtistAlbums", "ProducerId", "dbo.Producers");
            DropIndex("dbo.ArtistAlbums", new[] { "ArtistId" });
            DropIndex("dbo.ArtistAlbums", new[] { "AlbumId" });
            DropIndex("dbo.ArtistAlbums", new[] { "ProducerId" });
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
            
            DropTable("dbo.ArtistAlbums");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.ArtistAlbums",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ArtistId = c.Int(),
                        AlbumId = c.Int(nullable: false),
                        ProducerId = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
            DropForeignKey("dbo.ProducerAlbum", "AlbumId", "dbo.Albums");
            DropForeignKey("dbo.ProducerAlbum", "ProducerId", "dbo.Producers");
            DropForeignKey("dbo.ArtistAlbum", "AlbumId", "dbo.Albums");
            DropForeignKey("dbo.ArtistAlbum", "ArtistId", "dbo.Artists");
            DropIndex("dbo.ProducerAlbum", new[] { "AlbumId" });
            DropIndex("dbo.ProducerAlbum", new[] { "ProducerId" });
            DropIndex("dbo.ArtistAlbum", new[] { "AlbumId" });
            DropIndex("dbo.ArtistAlbum", new[] { "ArtistId" });
            DropTable("dbo.ProducerAlbum");
            DropTable("dbo.ArtistAlbum");
            CreateIndex("dbo.ArtistAlbums", "ProducerId");
            CreateIndex("dbo.ArtistAlbums", "AlbumId");
            CreateIndex("dbo.ArtistAlbums", "ArtistId");
            AddForeignKey("dbo.ArtistAlbums", "ProducerId", "dbo.Producers", "Id");
            AddForeignKey("dbo.ArtistAlbums", "ArtistId", "dbo.Artists", "Id");
            AddForeignKey("dbo.ArtistAlbums", "AlbumId", "dbo.Albums", "id", cascadeDelete: true);
        }
    }
}
