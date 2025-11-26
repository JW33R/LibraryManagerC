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
        

        public CheckoutItem(LibraryItem item, int dueDate = 3)
        {
            Item = item;
            DueDate = dueDate;
        }
        public static void CheckoutFormat()
        {
            Console.WriteLine("------------- Checkout Receipt ------------");

        }
    }
}
