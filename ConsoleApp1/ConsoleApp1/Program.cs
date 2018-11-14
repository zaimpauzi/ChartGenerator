using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            var tableList = new List<SWITCHTABLE>();
            var db = new SwitchesDataContext();
            var query = from switchTable in db.SWITCHTABLEs where switchTable.ROOM1 != null && switchTable.ROOM2 != null select new { switchTable.ROOM1, switchTable.ROOM2};
            var result = query.ToArray();
            foreach (var item in result) //Scan by row
            {
                var switchRoom1 = item.ROOM1;
                var switchRoom2 = item.ROOM2;
            }


        }
    }
}
