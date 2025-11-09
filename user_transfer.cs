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
        public static string GeneratebookID()
        {
            const string prefix = "BK";
            Random random = new Random();

            int code = random.Next(0, 1000); // generates from 0–999
            string formattedCode = code.ToString("D3"); // ensures 3 digits (e.g., 007, 123)

            return prefix + formattedCode;
        }
        public static string GenerateISBN()
        {
            const string prefix = "ISBN";
            Random random = new Random();

            // Generate a random 9-digit number (for example 000000001 to 999999999)
            int group = random.Next(100, 999);        // Example: 978
            int publisher = random.Next(1000, 9999);  // Example: 1234
            int title = random.Next(10000, 99999);    // Example: 56789

            string isbnBody = $"{group}{publisher}{title}";

            // Calculate a simple checksum (not official ISBN-13 math, just formatted)
            int sum = 0;
            foreach (char c in isbnBody)
                sum += c - '0';

            int checkDigit = sum % 10;

            return $"{prefix}-{isbnBody}{checkDigit}";
        }
        public static string GenerateShelfNumber()
        {
            const string prefix = "SH";
            Random random = new Random();

            int section = random.Next(1, 100);  // 01–99 → shelf section
            int shelf = random.Next(1, 1000);   // 001–999 → shelf number

            string formattedSection = section.ToString("D2"); // 2 digits
            string formattedShelf = shelf.ToString("D3");     // 3 digits

            return $"{prefix}{formattedSection}-{formattedShelf}";
        }
    }

}
