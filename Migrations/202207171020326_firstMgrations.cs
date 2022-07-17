namespace PersonCSV.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class firstMgrations : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.People", "Identity");
        }
        
        public override void Down()
        {
            CreateIndex("dbo.People", "Identity", unique: true, name: "Identity");
        }
    }
}
