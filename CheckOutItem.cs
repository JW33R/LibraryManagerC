using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManager
{
    internal class CheckoutItem
    {
        public static List<LibraryItem> CheckoutItems = new();

        public LibraryItem Item { get; set; }
        public int DueDate { get; set; }

        public static decimal totalLateFees = 0;
        public CheckoutItem(LibraryItem item, int dueDate = 3)
        {
            Item = item;
            DueDate = dueDate;
        }
        public static void CheckoutFormat()
        {
            Console.WriteLine("------------- Checkout Receipt ------------");
            Console.WriteLine("ID".PadRight(10) + "Title".PadRight(20) + "Type".PadRight(11) + "Days Late".PadRight(13) + "Fee");
            foreach (var item in CheckoutItems)
            {
                Console.WriteLine($"{item.ItemID.ToString().PadRight(10)}{item.Title.PadRight(20)}{item.ItemType.PadRight(11)}{item.DaysLate.ToString().PadRight(13)}${item.ItemLateFee}");
            }
            Console.WriteLine($"Total Estimated Late Fees: ${totalLateFees}");
            Console.WriteLine("-------------------------------------------");
            totalLateFees = 0;
        }
        public static void LateFee(int day, LibraryItem item)
        {
            if (day >= 3)
            {
                item.DaysLate = day - 3;
                item.ItemLateFee = item.DailyLateFee * item.DaysLate;
                totalLateFees += item.ItemLateFee;
            }
        }
    }
}
