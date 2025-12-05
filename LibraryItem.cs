
namespace LibraryManager
{
    internal class LibraryItem
    {
        public int ItemID { get; set; }
        public string Title { get; set; }
        public string ItemType { get; set; }
        public decimal DailyLateFee { get; set; }
        public decimal ItemLateFee { get; set; }
        public int DaysLate { get; set; }
        public LibraryItem(int itemID, string title, string itemType, decimal dailyLateFee)
        {
            ItemID = itemID;
            Title = title;
            ItemType = itemType;
            DailyLateFee = dailyLateFee;

        }
        public void DisplayInfo()//This function displays the item in LibraryItem
        {
            Console.WriteLine($"ID: {ItemID}, Title: {Title}, Type: {ItemType}, Daily Late Fee: ${DailyLateFee}");
        }
    }
}
