using System.Data.Common;
using System.Diagnostics.Metrics;
using System.Net.Quic;
using System.Runtime.InteropServices;

namespace LibraryManager
{
    internal class Program
    {
        public static List<LibraryItem> Catalog = new();

        static void Main(string[] args)
        {
            LoadCatalog();
            MenuChoice();
        }
        public static void MenuChoice()
        {
            ClearScreen();
            int tempChoice = 0;
            while (tempChoice != 8)
            {
                ClearScreen();
                DisplayMenu();
                tempChoice = Convert.ToInt32(Console.ReadLine());

                switch (tempChoice)
                {
                    case 1:
                        AddItems();
                        break;
                    case 2:
                        //ReturnItems();
                        break;
                    case 3:
                        ViewItems();
                        break;
                    case 4:
                        CheckoutItem myInstance = new();
                        myInstance.TakeItemOut();
                        break;
                    case 5:
                        //ViewCheckoutReceipt();
                        break;
                    case 6:
                        //SaveCheckoutList();
                        break;
                    case 7:
                        //LoadCheckoutList();
                        break;
                    default:
                        Console.WriteLine("Goodbye...");
                        break;

                }
            }
            Environment.Exit(0);
        }
        public static void ViewItems()
        {
            ClearScreen();
            LoadCatalog();
            Console.WriteLine("Available Items:");
            foreach (LibraryItem item in Catalog)
            {
                item.DisplayInfo();
            }
            PressContinue();
        }
        public static void PressContinue()
        {
            Console.WriteLine();
            Console.WriteLine("Press Enter to Continue...");
            Console.ReadLine();
        }
        public static void ClearScreen()
        {
            Console.Clear();
            Console.WriteLine("-----------------------------");
        }
        public static void DisplayMenu()
        {
            Console.WriteLine("1. Add a library Item");
            Console.WriteLine("2. Return an Item");
            Console.WriteLine("3. View available Items");
            Console.WriteLine("4. Check out an Item");
            Console.WriteLine("5. View Checkout receipt");
            Console.WriteLine("6. Save Checkout List to File");
            Console.WriteLine("7. Load Previous Checkout List from File");
            Console.WriteLine("8. Exit");

        }
        public static void AddItems()
        {
            ClearScreen();
            Console.WriteLine("Add a Library Item");
            Console.WriteLine("------------------");
            Console.Write("Enter Item ID: ");
            int itemID = Convert.ToInt32(Console.ReadLine());
            Console.Write("Enter Title: ");
            string title = Console.ReadLine();
            Console.Write("Enter Item Type (e.g., book, DVD): ");
            string itemType = Console.ReadLine();
            Console.Write("Enter Daily Late Fee: ");
            decimal dailyLateFee = Convert.ToDecimal(Console.ReadLine());
            LibraryItem newItem = new(itemID, title, itemType, dailyLateFee);
            Catalog.Add(newItem);
            Console.WriteLine("Item added successfully!");
            SaveCatalog();
            PressContinue();
        }
        public static void SaveCatalog()
        {
            ClearScreen();
            string FileName = "Catalog.csv";
            File.Delete(FileName);
            foreach (LibraryItem item in Catalog)
            {
                File.AppendAllText(FileName, $"{item.ItemID}, {item.Title}, {item.ItemType}, {item.DailyLateFee},\n");

            }
        }
        public static void LoadCatalog()
        {
            ClearScreen();
            string FileName = "Catalog.csv";
            if (File.Exists(FileName))
            {
                string[] lines = File.ReadAllLines(FileName);
                Catalog.Clear();
                foreach (string line in lines)
                {
                    line.Replace(" ", "");
                    string[] parts = line.Split(',');

                    int itemID = Convert.ToInt32(parts[0]);
                    string title = parts[1];
                    string itemType = parts[2];
                    decimal dailyLateFee = Convert.ToDecimal(parts[3]);

                    LibraryItem newItem = new(itemID, title, itemType, dailyLateFee);
                    Catalog.Add(newItem);
                }
            }
            else
            {
                LibraryItem newItem = new(101, "The Great Gatsby", "Book", 0.25m);
                Catalog.Add(newItem);
                SaveCatalog();
            }
            ClearScreen();
            Console.WriteLine("Available Items:");
            foreach (LibraryItem item in Catalog)
            {
                item.DisplayInfo();
            }
            PressContinue();
        }
    }
}