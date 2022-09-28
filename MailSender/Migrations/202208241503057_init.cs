namespace MailSender.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Emails",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false),
                        Message = c.String(nullable: false),
                        ReceiverId = c.Int(nullable: false),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Receivers", t => t.ReceiverId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.ReceiverId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Receivers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Email = c.String(nullable: false),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.UserId);
            
            AddColumn("dbo.AspNetUsers", "Name", c => c.String());
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Receivers", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Emails", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Emails", "ReceiverId", "dbo.Receivers");
            DropIndex("dbo.Receivers", new[] { "UserId" });
            DropIndex("dbo.Emails", new[] { "UserId" });
            DropIndex("dbo.Emails", new[] { "ReceiverId" });
            DropColumn("dbo.AspNetUsers", "Name");
            DropTable("dbo.Receivers");
            DropTable("dbo.Emails");
        }
    }
}
