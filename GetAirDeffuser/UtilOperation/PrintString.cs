using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetAirDeffuser.UtilOperation
{
    public class PrintString
    {
        public void Print(string elem)
        {
            string problemParametersElementPath = @"C:\06_Выгрузка\Общий_отчет по параметрам.txt";

            using (StreamWriter writerCath = new StreamWriter(problemParametersElementPath, true))
            {
                writerCath.WriteLine($":::{elem} :::");

            }
        }
    }
}
