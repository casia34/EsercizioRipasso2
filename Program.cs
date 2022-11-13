using System.Text.RegularExpressions;

namespace EsercizioRipasso2
{
    internal class Program
    {
        static void Main(string[] args)
        {
            ClassConn db = new ClassConn();

            db.connDb();
            if (db.isDBOpen)
            {
                db.InsertDataIntheTable();
               
            }



        }
    }
}