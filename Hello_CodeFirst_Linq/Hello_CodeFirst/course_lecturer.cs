using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Hello_CodeFirst_Linq
{
    [Table("course_lecturers")]
    public class course_lecturer
    {
        [Key]
        [Column(Order = 1)]
        public string course_id { get; set; }
        [Key]
        [Column(Order = 2)]
        public string lc_id { get; set; }
        public short lc_order { get; set; }
        public decimal share { get; set; }

        public virtual lecturer lecturer { get; set; }
        public virtual course course { get; set; }
    }
}
