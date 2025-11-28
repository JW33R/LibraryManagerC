using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManager
{
    internal class LibraryItem
    {
        public int ItemID { get; set; }
        public string Title { get; set; }
        public string ItemType { get; set; }
        public decimal DailyLateFee { get; set; }

        public bool IsCheckedOut { get; set; }
        public decimal ItemLateFee { get; set; }
        public int DaysLate { get; set; }
        public LibraryItem(int itemID, string title, string itemType, decimal dailyLateFee)
        {
            ItemID = itemID;
            Title = title;
            ItemType = itemType;
            DailyLateFee = dailyLateFee;

        }
        public void DisplayInfo()
        {
            Console.WriteLine($"ID: {ItemID}, Title: {Title}, Type: {ItemType}, Daily Late Fee: ${DailyLateFee}");
        }
    }
}
