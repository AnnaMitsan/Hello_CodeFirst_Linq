using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Transactions; 

namespace Hello_CodeFirst_Linq
{
    public class Transaction_test
    {
        public void StartOwnTransactionWithinContext()
        {
            using (var ctx = new tect_CodeFirstLingContext())
            {
                using (var dbContextTransaction = ctx.Database.BeginTransaction())
                {
                    try
                    {
                        var lecturerList = ctx.lecturers.ToList<lecturer>();
                        //Perform create operation
                        Console.WriteLine("Perform create operation");
                        ctx.lecturers.Add(new lecturer() { lc_id = "L_2", lc_fname = "Second lecturer" });
                        //Execute Inser, Update & Delete queries in the database
                        ctx.SaveChanges();

                        var lects1 = ctx.lecturers.ToList<lecturer>();
                        

                        //Perform Update operation
                        Console.WriteLine("Perform Update operation");
                        var lecturerList1 = ctx.lecturers.ToList<lecturer>();
                        lecturer lecturerToUpdate = lecturerList1.Where(s => s.lc_fname == "Second lecturer").FirstOrDefault<lecturer>();
                        lecturerToUpdate.lc_fname = "Edited second lecturer";
                        //Execute Inser, Update & Delete queries in the database
                        ctx.SaveChanges();

                        dbContextTransaction.Commit();
                    }
                    catch (Exception)
                    {
                        dbContextTransaction.Rollback();
                    }
                }
            }
        } 
    }
}


