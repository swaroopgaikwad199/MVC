using System;
using REDTR.UTILS.SystemIntegrity;

namespace RED.Database.Install
{
    class Program
    {
        static void Main(string[] args)
        {
            Globals.CreateDeafaultFiles(false);
            //InitDB basedata = new InitDB();//"System.Data.SqlClient",@"Data Source=192.168.1.204\SQLEXPRESS;Initial Catalog=RPM;Integrated Security=True");
            if (args.Length == 3)
            {
                if (args[0] == "11223344556677889900") // This acts like Password
                {
                    bool onlyJobTypeOper = args[1] == "0" ? false : true;
                    Console.WriteLine(args[1] + "....." + onlyJobTypeOper.ToString());
                    int addas_num = Convert.ToInt32(args[2]);
                    RED.Database.Install.InitDB.JobTypeAs addAs = (RED.Database.Install.InitDB.JobTypeAs)addas_num;
                    Console.WriteLine(args[2] + "....." + addAs.ToString());

                    InitDB basedata = new InitDB(onlyJobTypeOper, addAs);
                }
                else
                    Console.WriteLine("OOUUUCCCCHHHHH......");

            }
            else
            {
                Console.WriteLine("Enter PASSWORD.....");
                string pass = Console.ReadLine();
                if (pass == "11223344556677889900") // This acts like Password
                {
                    Console.WriteLine("Enter Option 0(DGFT Only)/1(DGFT All).....");
                    string option = Console.ReadLine();
                    int addas_num = Convert.ToInt32(option);
                    if (addas_num == 0 || addas_num == 1)
                    {
                        RED.Database.Install.InitDB.JobTypeAs addAs = (RED.Database.Install.InitDB.JobTypeAs)addas_num;
                        InitDB basedata = new InitDB(true, addAs);
                    }
                }
                else
                {
                    InitDB basedata = new InitDB(true, InitDB.JobTypeAs.DGFTOnly);
                }
            }
        }
    }
}
