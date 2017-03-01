using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Hello_CodeFirst_Linq
{
    [Table("faculties")]
    public class faculty
    {
        [Key]
        public string facl_id { get; set; }
        public string facl_name { get; set; }
        public string university { get; set; }
        public string state { get; set; }
        public string country { get; set; }

        public virtual ICollection<course> course { get; set; }
    }
}
