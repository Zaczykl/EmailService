namespace MailSender.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EditAttachment : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.EmailAttachments", "FileName", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.EmailAttachments", "FileName");
        }
    }
}
