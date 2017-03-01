namespace Hello_CodeFirst_Linq.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ElseTables : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.courses",
                c => new
                    {
                        course_id = c.String(nullable: false, maxLength: 128),
                        course_name = c.String(),
                        type = c.String(),
                        facl_id = c.String(maxLength: 128),
                        size = c.Int(),
                        marks = c.Decimal(precision: 18, scale: 2),
                        quantity = c.Int(),
                        begin_date = c.DateTime(),
                        contract = c.Short(nullable: false),
                    })
                .PrimaryKey(t => t.course_id)
                .ForeignKey("dbo.faculties", t => t.facl_id)
                .Index(t => t.facl_id);
            
            CreateTable(
                "dbo.faculties",
                c => new
                    {
                        facl_id = c.String(nullable: false, maxLength: 128),
                        facl_name = c.String(),
                        university = c.String(),
                        state = c.String(),
                        country = c.String(),
                    })
                .PrimaryKey(t => t.facl_id);
            
            CreateTable(
                "dbo.course_lecturers",
                c => new
                    {
                        course_id = c.String(nullable: false, maxLength: 128),
                        lc_id = c.String(nullable: false, maxLength: 128),
                        lc_order = c.Short(nullable: false),
                        share = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => new { t.course_id, t.lc_id })
                .ForeignKey("dbo.courses", t => t.course_id, cascadeDelete: true)
                .ForeignKey("dbo.lecturers", t => t.lc_id, cascadeDelete: true)
                .Index(t => t.course_id)
                .Index(t => t.lc_id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.course_lecturers", "lc_id", "dbo.lecturers");
            DropForeignKey("dbo.course_lecturers", "course_id", "dbo.courses");
            DropForeignKey("dbo.courses", "facl_id", "dbo.faculties");
            DropIndex("dbo.course_lecturers", new[] { "lc_id" });
            DropIndex("dbo.course_lecturers", new[] { "course_id" });
            DropIndex("dbo.courses", new[] { "facl_id" });
            DropTable("dbo.course_lecturers");
            DropTable("dbo.faculties");
            DropTable("dbo.courses");
        }
    }
}
