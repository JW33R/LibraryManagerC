
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
        public static void CheckoutFormat()//This function does the formating for the reciept I use pad right and have it loop thorugh each item in the checkoutItems list and have it print out the ID, Title, Type, DaysLate, and Late Fee
        //Along with the total amount they will owe
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
        public static void LateFee(int day, LibraryItem item, CheckoutItem checkout)//This function determines how much the user will have to owe in late fees. It checks to see if the days parameter is larger than the DueDate, and if it is
        //Then it takes the day entered and subtracts the DueDate and puts that into DaysLate then takes the DailyLateFee and multiplies it by the amount of DaysLate and puts that into ItemLateFee. It then adds that late fee to totalLateFees
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
