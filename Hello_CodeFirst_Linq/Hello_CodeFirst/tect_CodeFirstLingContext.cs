using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace Hello_CodeFirst_Linq
{
    public class tect_CodeFirstLingContext : DbContext
    {
        public tect_CodeFirstLingContext()
            : base()
        {

        }

        public DbSet<lecturer> lecturers { get; set; }
        public DbSet<email> emails { get; set; }
        public DbSet<course> courses { get; set; }
        public DbSet<faculty> faculty { get; set; }
        public DbSet<course_lecturer> courses_leturers { get; set; }
    }
}
