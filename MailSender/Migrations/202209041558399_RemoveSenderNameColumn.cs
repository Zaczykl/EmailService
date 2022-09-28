namespace MailSender.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveSenderNameColumn : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Emails", "Sender");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Emails", "Sender", c => c.String());
        }
    }
}
