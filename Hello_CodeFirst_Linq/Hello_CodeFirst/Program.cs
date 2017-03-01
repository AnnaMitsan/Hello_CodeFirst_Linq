using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Data.EntityClient;
using System.Data;

namespace Hello_CodeFirst_Linq
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var ctx = new tect_CodeFirstLingContext())
            {
                List<string> script = null;
                try
                {
                    try
                    {
                         DB_init(ctx);
                    }
                    catch (Exception)
                    {
                        Console.Write("Initialization error ?\r\n");
                    }
                    int a;
                    
                    do
                    {
                        Console.WriteLine(@"Please,  type the number:                       
                        1.  Clear and insert to the DB by script
                        2.  Insert lecturer with email (one-to-many)
                        3.  Delete lecturer with email (one-to-many)
                        4.  Add new email to lecturer
                        5.  Update lecturer
                       
                        ");
                        try
                        {
                            Console.SetBufferSize(Console.LargestWindowWidth, Console.LargestWindowHeight);
                            a = int.Parse(Console.ReadLine());
                            switch (a)
                            {
                                case 1:
                                    DB_bulk_read(ref script, "insrt_ling.sql");
                                    //Be aware that the email table has the identity key
                                    foreach (var item in script)
                                    {
                                        SQL_ins_qry(ctx, item.ToString());
                                    }
                                    lecturers_print();
                                    break;
                                case 2:
                                    Console.WriteLine("Insert lecturer with email (one-to-many) ");
                                    Lect_email_ins_qry();
                                    lecturers_print();
                                    emails_print();
                                    break;
                                case 3:
                                    Console.WriteLine("Delete lecturer with email (one-to-many) ");
                                    Lect_email_del_qry();
                                    lecturers_print();
                                    emails_print();
                                    break;
                                case 4:
                                    Console.WriteLine("Add new email to lecturer ");
                                    Lect_add_email_qry();
                                    lecturers_print();
                                    emails_print();
                                    break;
                                case 5:
                                    Console.WriteLine("Update lecturer name ");
                                    Lect_upd_qry();
                                    lecturers_print();
                                    break;
                                default:
                                    Console.WriteLine("Exit");
                                    break;
                            }

                        }
                        catch (System.Exception e)
                        {
                            Console.WriteLine("Error: " + e.Message);
                        }
                        finally
                        {
                            Console.ReadLine();
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine("Press Spacebar to exit; press any key to continue");
                            Console.ForegroundColor = ConsoleColor.White;
                        }
                    }
                    while (Console.ReadKey().Key != ConsoleKey.Spacebar);

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

                Console.ReadKey();
               
            }
        }

        static void Lect_email_ins_qry()
        {
            using (var ctx = new tect_CodeFirstLingContext())
            {
                using (var dbContextTransaction = ctx.Database.BeginTransaction())
                {
                    try
                    {
                        var lecturerList = ctx.lecturers.ToList<lecturer>();
                        var lecturerItems = lecturerList.OrderByDescending(i =>
                        i.lc_id).Take<lecturer>(1);
                        Console.WriteLine("Find lecturer max key ");
                        Console.WriteLine("Max: {0}", lecturerItems.ElementAt(0).lc_id);
                        Console.WriteLine("Insert new key"); string new_lc_id =
                        Console.ReadLine().ToString();
                        Console.WriteLine("Insert new first name"); string new_lc_fname =
                        Console.ReadLine().ToString();
                        Console.WriteLine("Insert new last name"); string new_lc_lname =
                        Console.ReadLine().ToString();
                        Console.WriteLine("Insert new email"); string new_email =
                        Console.ReadLine().ToString();
                        List<lecturer> chlect = ctx.lecturers.ToList();
                        lecturer lectToUpd = chlect.Where(i => i.lc_id ==
                        new_lc_id).FirstOrDefault<lecturer>();
                        if (lectToUpd == null)
                        {
                            List<email> chmail = ctx.emails.ToList();
                            email emailToUpdate = chmail.Where(s => s.lc_id ==
                            new_lc_id).FirstOrDefault<email>();
                            if (emailToUpdate == null)
                            {
                                ctx.lecturers.Add(new lecturer()
                                {
                                    lc_id = new_lc_id,
                                    lc_fname
                                = new_lc_fname,
                                    lc_lname = new_lc_lname
                                });
                                ctx.emails.Add(new email
                                {
                                    em_value = new_email,
                                    lc_id =
                                new_lc_id
                                });
                                Console.WriteLine("Are you sure ? Type Y ");
                                if (Console.ReadKey().Key == ConsoleKey.Y)
                                {
                                    ctx.SaveChanges();
                                    dbContextTransaction.Commit();
                                }
                                else
                                {
                                    dbContextTransaction.Rollback();
                                }
                            }
                            else
                            {
                                Console.WriteLine("Foreign key problem, email for this lc_id exists");
                                dbContextTransaction.Rollback();
                            }
                        }
                        else
                        {
                            Console.WriteLine("Duplicate key problem, lc_id exists");
                            dbContextTransaction.Rollback();
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        dbContextTransaction.Rollback();
                    }
                }
            }
        }


        static void Lect_add_email_qry()
        {
            using (var ctx = new tect_CodeFirstLingContext())
            {
                using (var dbContextTransaction = ctx.Database.BeginTransaction())
                {
                    try
                    {
                        Console.WriteLine("Type update lector key");
                        string new_lc_id = Console.ReadLine().ToString();
                        Console.WriteLine("Type email to add");
                        string new_email = Console.ReadLine().ToString();
                        var itemNamesByCategory = from i in ctx.emails
                                                  where i.em_value == new_email
                                                  group i by i.lc_id into g
                                                  select new { Lecturer_inst = g.Key, Items = g };
                        StringBuilder res = new StringBuilder();
                        foreach (var item in itemNamesByCategory)
                        {
                            res.AppendLine("Lecturer: " + item.Lecturer_inst);
                            foreach (var group in item.Items)
                            {
                                res.AppendLine(group.lc_id + " " + group.em_value);
                                res.AppendLine();
                            }
                        }
                        Console.WriteLine();
                        Console.WriteLine("All lecturers with such email: \r\n" + res);
                        Console.WriteLine();
                        var itemNamesByCategory1 = itemNamesByCategory
                        .Where(i => i.Lecturer_inst == new_lc_id); ;
                        StringBuilder res1 = new StringBuilder();
                        foreach (var item in itemNamesByCategory1)
                        {
                            res1.AppendLine("Lecturer: " + item.Lecturer_inst);
                            foreach (var group in item.Items)
                            {
                                res1.AppendLine(group.lc_id + " " + group.em_value);
                                res1.AppendLine();
                            }
                        }
                        if (itemNamesByCategory1.Count() == 0)
                        {
                            Console.WriteLine();
                            Console.WriteLine("Are you sure ? Type Y ");
                            if (Console.ReadKey().Key == ConsoleKey.Y)
                            {
                                ctx.emails.Add(new email() { lc_id = new_lc_id, em_value = new_email });
                                ctx.SaveChanges();
                                dbContextTransaction.Commit();
                            }
                            else
                            {
                                dbContextTransaction.Rollback();
                            }
                        }
                        else
                        {
                            Console.WriteLine("Duplicate email :" + res1);
                            dbContextTransaction.Rollback();
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        dbContextTransaction.Rollback();
                    }
                }
            }
        }


        static void Lect_upd_qry()
        {
            using (var ctx = new tect_CodeFirstLingContext())
            {
                using (var dbContextTransaction = ctx.Database.BeginTransaction())
                {
                    try
                    {
                        Console.WriteLine("Type lecturer key");
                        string new_lc_id = Console.ReadLine().ToString();
                        Console.WriteLine("Type first name");
                        string new_lc_fname = Console.ReadLine().ToString();
                        Console.WriteLine("Type last name");
                        string new_lc_lname = Console.ReadLine().ToString();
                        List<lecturer> lecturers_to_upd = ctx.lecturers
                        .Where(u => u.lc_id == new_lc_id).ToList();
                        Console.WriteLine("Are you sure ? Type Y ");
                        if (Console.ReadKey().Key == ConsoleKey.Y)
                        {
                            foreach (var item in lecturers_to_upd)
                            {
                                item.lc_fname = new_lc_fname;
                                item.lc_lname = new_lc_lname;
                            }
                            ctx.SaveChanges();
                            dbContextTransaction.Commit();
                        }
                        else
                        {
                            dbContextTransaction.Rollback();
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        dbContextTransaction.Rollback();
                    }
                }
            }
        }



        static void Lect_email_del_qry()
        {
            using (var ctx = new tect_CodeFirstLingContext())
            {
                using (var dbContextTransaction = ctx.Database.BeginTransaction())
                {
                    try
                    {
                        Console.WriteLine("Type delete key");
                        string new_lc_id = Console.ReadLine().ToString();
                        List<lecturer> lecturers_to_del = ctx.lecturers
                        .Where(u => u.lc_id == new_lc_id).ToList();
                        List<email> emails_to_del = ctx.lecturers
                        .Where(u => u.lc_id == new_lc_id)
                        .SelectMany(u => u.emails)
                        .OrderBy(p => p.em_Id)
                        .ToList();
                        Console.WriteLine("Are you sure ? Type Y ");
                        if (Console.ReadKey().Key == ConsoleKey.Y)
                        {
                            foreach (var item in emails_to_del)
                            {
                                ctx.emails.Remove(item);
                            }
                            foreach (var item in lecturers_to_del)
                            {
                                ctx.lecturers.Remove(item);
                            }
                            ctx.SaveChanges();
                            dbContextTransaction.Commit();
                        }
                        else
                        {
                            dbContextTransaction.Rollback();
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        dbContextTransaction.Rollback();
                    }
                }
            }
        }




        static void lecturers_print()
        {
            using (var ctx = new tect_CodeFirstLingContext())
            {
                List<lecturer> mylist = ctx.lecturers.ToList<lecturer>();
                Console.WriteLine("Lecturers list:");
                foreach (lecturer u in mylist)
                {
                    Console.WriteLine("{0}: {1} {2} ", u.lc_id, u.lc_fname, u.lc_lname);
                }
            }
        }
        static void emails_print()
        {
            using (var ctx = new tect_CodeFirstLingContext())
            {
                List<email> mylist = ctx.emails.ToList<email>();
                Console.WriteLine("email list:");
                foreach (email u in mylist)
                {
                    Console.WriteLine("{0}: {1} {2} ", u.em_Id, u.em_value, u.lc_id);
                }
            }
        }

        static void SQL_ins_qry(tect_CodeFirstLingContext ctx, string sqlstr)
        {
            try
            {
                var qry = ctx.Database.ExecuteSqlCommand(sqlstr);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            }


            static void DB_init(tect_CodeFirstLingContext ctx)
        {
            // Initialize database at this moment
            ctx.Database.Initialize(false);
        }

        static void DB_bulk_read(ref List<string> script, string filepath)
        {
            script = new List<string>();
            using (var sr = new StreamReader(filepath))
            {
                while (sr.Peek() >= 0)
                    script.Add(sr.ReadLine());
            }
        }



    }
}

