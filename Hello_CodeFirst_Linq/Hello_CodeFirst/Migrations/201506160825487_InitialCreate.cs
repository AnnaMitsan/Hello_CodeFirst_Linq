namespace Hello_CodeFirst_Linq.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.email",
                c => new
                    {
                        em_Id = c.Int(nullable: false, identity: true),
                        em_value = c.String(),
                        lc_id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.em_Id)
                .ForeignKey("dbo.lecturers", t => t.lc_id)
                .Index(t => t.lc_id);
            
            CreateTable(
                "dbo.lecturers",
                c => new
                    {
                        lc_id = c.String(nullable: false, maxLength: 128),
                        lc_fname = c.String(),
                        lc_lname = c.String(),
                        phone = c.String(),
                        address = c.String(),
                        city = c.String(),
                        state = c.String(),
                        zip = c.String(),
                    })
                .PrimaryKey(t => t.lc_id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.email", "lc_id", "dbo.lecturers");
            DropIndex("dbo.email", new[] { "lc_id" });
            DropTable("dbo.lecturers");
            DropTable("dbo.email");
        }
    }
}
