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

        public CheckoutItem(LibraryItem item, int dueDate)
        {
            Item = item;
            DueDate = dueDate;
        }

        /*
        int dueDate = 3;
        public void TakeItemOut()
        {
            Program.ClearScreen();
            Console.WriteLine("Enter the ID of the item you wish to check out: ");
            int tempID = Convert.ToInt32(Console.ReadLine());
            var selectedItem = Program.Catalog.FirstOrDefault(item => item.ItemID == tempID && !item.IsCheckedOut);
            if (selectedItem != null)
            {
                selectedItem.IsCheckedOut = true;
                CheckoutItems.Add(selectedItem);
                Console.WriteLine($"You have checked out: {selectedItem.Title}");
                Console.WriteLine($"Due date is in {dueDate} days.");
            }
            else
            {
                Console.WriteLine("Item not found or already checked out.");
            }
            Program.PressContinue();
        }*/
    }
}
