using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Hello_CodeFirst_Linq
{   
    [Table("email")]
    public class email
    {
        [Key]
        public int em_Id { get; set; }
        public string em_value { get; set; }

        public string lc_id { get; set; }

        // even email have 1 field for foreign key from lecturer,  virtual allow for it to be overridden in a derived class
        public virtual lecturer lecturer { get; set; }
    }
}
