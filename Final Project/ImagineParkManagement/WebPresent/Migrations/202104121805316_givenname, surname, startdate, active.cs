namespace WebPresent.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class givennamesurnamestartdateactive : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "GivenName", c => c.String());
            AddColumn("dbo.AspNetUsers", "Surname", c => c.String());
            AddColumn("dbo.AspNetUsers", "StartDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.AspNetUsers", "Active", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "Active");
            DropColumn("dbo.AspNetUsers", "StartDate");
            DropColumn("dbo.AspNetUsers", "Surname");
            DropColumn("dbo.AspNetUsers", "GivenName");
        }
    }
}
