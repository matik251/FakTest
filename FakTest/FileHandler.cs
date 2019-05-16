using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;
using System.Net.Http;
using System.Text;

namespace FakTest
{
    public class FileHandler
    {
        DirectoryInfo dir = new DirectoryInfo(".");

        string[] customers =
        {
            "bob",
            "sally"
        };

        string dataString;

        string testFilePath = @".\testfile.txt";

        public void writeMainFile()
        {
           File.WriteAllLines(testFilePath, customers);
        }

        public String readMainFile()
        {
            StreamReader sr = File.OpenText(testFilePath);

            dataString = sr.ReadLine();

            sr.Close();
            return dataString;
        }

        internal void readMain()
        {
            throw new NotImplementedException();
        }

    }
}
