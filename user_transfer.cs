using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace librarysystem
{
    internal class Class1
    {
    }
    public class user_data 
    {
        public static string currentuser { get; set; }
        public static string user_id { get;  set; }
        public static string real_name { get; set; }
        public static string email { get; set; }
    }
    public static class IDGenerator
    {
        public static string GenerateUserID()
        {
            const string prefix = "UR";
            Random random = new Random();

            int code = random.Next(0, 1000); // generates from 0–999
            string formattedCode = code.ToString("D3"); // ensures 3 digits (e.g., 007, 123)

            return prefix + formattedCode;
        }
    }

}
