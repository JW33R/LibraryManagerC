
namespace LibraryManager
{
    internal class CheckoutItem
    {
        public static List<LibraryItem> CheckoutItems = new();
        public static List<CheckoutItem> Checkout = new();

        public LibraryItem Item { get; set; }
        public int DueDate { get; set; }

        public static decimal totalLateFees = 0;
        public CheckoutItem(LibraryItem item, int dueDate = 3)
        {
            Item = item;
            DueDate = dueDate;
        }
        //I used AI to help me figure out a better way to format the checkout receipt. I asked it: What are some methods to format in c#?
        //It suggested a few other methods along with PadRight which I decided to use because it was easy to understand and implement.
        public static void CheckoutFormat()
        {
            Console.WriteLine("------------- Checkout Receipt ------------");
            Console.WriteLine("ID".PadRight(7) + "Title".PadRight(25) + "Type".PadRight(15) + "Days Late".PadRight(13) + "Fee");
            foreach (var item in CheckoutItems)
            {
                Console.WriteLine($"{item.ItemID.ToString().PadRight(7)}{item.Title.PadRight(25)}{item.ItemType.PadRight(15)}{item.DaysLate.ToString().PadRight(13)}${item.ItemLateFee}");
            }
            Console.WriteLine($"Total Estimated Late Fees: ${totalLateFees}");
            Console.WriteLine("-------------------------------------------");
            totalLateFees = 0;
        }
        public static void LateFee(int day, LibraryItem item, CheckoutItem checkout)
        {
            if (day > checkout.DueDate)
            {
                item.DaysLate = day - checkout.DueDate;
                item.ItemLateFee = item.DailyLateFee * item.DaysLate;
                totalLateFees += item.ItemLateFee;
            }

        }
    }
}
