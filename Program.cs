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
            ViewItems();
            MenuChoice();
        }
        public static void MenuChoice()
        {
            ClearScreen();
            string tempChoice = "0";
            while (tempChoice != "9")
            {
                ClearScreen();
                DisplayMenu();
                tempChoice = Console.ReadLine();
                if (int.TryParse(tempChoice, out int choice) && choice >= 1 && choice <= 9)
                {
                    switch (choice)
                    {
                        case 1:
                            AddItems();
                            break;
                        case 2:
                            ReturnItems();
                            break;
                        case 3:
                            ViewItems();
                            break;
                        case 4:
                            CheckingOutAnItem();
                            break;
                        case 5:
                            ViewCheckoutReceipt();
                            break;
                        case 6:
                            SaveCheckoutList();
                            break;
                        case 7:
                            LoadCheckoutList();
                            break;
                        case 8:
                            DeleteCheckoutList();
                            break;
                        default:
                            Console.WriteLine("Goodbye...");
                            break;
                    }
                }
                else
                {
                    Console.WriteLine("Invalid input. Please enter a number between 1 and 9.");
                    PressContinue();
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
            Console.WriteLine("8. Delete Checkout List");
            Console.WriteLine("9. Exit");

        }
        public static void AddItems()
        {
            ClearScreen();
            Console.WriteLine("Add a Library Item");
            Console.WriteLine("------------------");
            Console.Write("Enter Item ID: ");
            var itemID = Console.ReadLine();
            if (int.TryParse(itemID, out int tempID))
            {
                foreach (LibraryItem item in Catalog)
                {
                    if (item.ItemID == tempID)
                    {
                        Console.WriteLine("That ID is already in use, exiting function");
                        PressContinue();
                        return;
                    }
                }
            }
            else 
            {
                Console.WriteLine("That is not a valid ID, exiting function");
                PressContinue();
                return;
            }
            Console.Write("Enter Title: ");
            string title = Console.ReadLine();
            Console.Write("Enter Item Type (Book or DVD): ");
            string itemType = Console.ReadLine();
            if (itemType.ToUpper() != "BOOK" && itemType.ToUpper() != "DVD")
            {
                Console.WriteLine("That is not the correct type, exiting function");
                PressContinue();
                return;
            }
            Console.Write("Enter Daily Late Fee: ");
            var dailyLateFee = Console.ReadLine();
            if (decimal.TryParse(dailyLateFee, out decimal fee))
            {
                if (fee < .25M || fee > .75M)
                {
                    Console.WriteLine("Daily late fee cannot be less than a quarter or more than 75 cents, exiting function");
                    PressContinue();
                    return;
                }
            }
            else
            {
                Console.WriteLine("That is not a valid daily late fee, exiting function");
                PressContinue();
                return;
            }
            LibraryItem Item = new(tempID, title, itemType, fee);
            Catalog.Add(Item);
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
        }
        public static void CheckingOutAnItem()
        {
            ClearScreen();
            ViewItems();
            Console.WriteLine("Checkout an Item");
            Console.WriteLine("----------------");
            Console.Write("Enter the ID of the item you wish to check out: ");
            int tempID = Convert.ToInt32(Console.ReadLine());
            var selectedItem = Catalog.FirstOrDefault(item => item.ItemID == tempID);
            if (selectedItem != null)
            {
                foreach (var checkoutItem in CheckoutItem.CheckoutItems)
                {
                    if (checkoutItem.ItemID == tempID)
                    {
                        Console.WriteLine("Item is already checked out.");
                        PressContinue();
                        return;
                    }
                }
                CheckoutItem item = new(selectedItem);
                CheckoutItem.CheckoutItems.Add(item.Item);
                Console.WriteLine($"You have checked out: {selectedItem.Title}");
                Console.WriteLine($"Due date is in 3 days.");
                
            }
            else
            {
                Console.WriteLine("Item not found or already checked out.");
            }
            PressContinue();
        }
        public static void ReturnItems()
        {
            if (CheckoutItem.CheckoutItems.Count == 0)
            {
                ClearScreen();
                Console.WriteLine("No items are currently checked out.");
                PressContinue();
                return;
            }
            ClearScreen();
            CheckedOutItems();
            Console.WriteLine("Return an Item");
            Console.WriteLine("--------------");
            Console.Write("Enter the ID of the item you wish to return: ");
            int tempID = Convert.ToInt32(Console.ReadLine());
            var selectedItem = CheckoutItem.CheckoutItems.FirstOrDefault(item => item.ItemID == tempID);
            if (selectedItem != null && selectedItem.ItemID == tempID)
            {
                CheckoutItem.CheckoutItems.RemoveAll(i => i.ItemID == tempID);
                Console.WriteLine($"You have returned: {selectedItem.Title}");
            }
            else
            {
                Console.WriteLine("Item not found or not checked out.");
            }
            PressContinue();    
        }
        public static void CheckedOutItems()
        {
            ClearScreen();
            Console.WriteLine("Checked Out Items:");
            foreach (var item in CheckoutItem.CheckoutItems)
            {
                item.DisplayInfo();
            }
            PressContinue();
        }
        public static void ViewCheckoutReceipt()
        {
            ClearScreen();
            if (CheckoutItem.CheckoutItems.Count == 0)
            {
                Console.WriteLine("No items are currently checked out.");
                PressContinue();
                return;
            }
            CheckedOutItems();
            ClearScreen();
            Console.WriteLine("Checkout Receipt:");
            foreach (var item in CheckoutItem.CheckoutItems)
            {
                item.DisplayInfo();
                Console.WriteLine("How long have you had the item checked out for? (in days)");
                int tempDays = Convert.ToInt32(Console.ReadLine());
                CheckoutItem.LateFee(tempDays, item);
                ClearScreen();
            }
            CheckoutItem.CheckoutFormat();
            PressContinue();
        }
        public static void SaveCheckoutList()
        {
            if (CheckoutItem.CheckoutItems.Count == 0)
            {
                ClearScreen();
                Console.WriteLine("There is nothing to save.");
                PressContinue();
                return;
            }
            ClearScreen();
            string FileName = "CheckoutList.csv";
            File.Delete(FileName);
            foreach (var item in CheckoutItem.CheckoutItems)
            {
                File.AppendAllText(FileName, $"{item.ItemID}, {item.Title}, {item.ItemType}, {item.DailyLateFee}, {item.DaysLate}, {item.ItemLateFee}\n");
            }
            Console.WriteLine("Checkout list saved successfully!");
            PressContinue();
        }
        public static void LoadCheckoutList()
        {
            ClearScreen();
            string FileName = "CheckoutList.csv";
            if (File.Exists(FileName))
            {
                string[] lines = File.ReadAllLines(FileName);
                CheckoutItem.CheckoutItems.Clear();
                foreach (string line in lines)
                {
                    line.Replace(" ", "");
                    string[] parts = line.Split(',');
                    int itemID = Convert.ToInt32(parts[0]);
                    string title = parts[1];
                    string itemType = parts[2];
                    decimal dailyLateFee = Convert.ToDecimal(parts[3]);
                    int daysLate = Convert.ToInt32(parts[4]);
                    decimal itemLateFee = Convert.ToDecimal(parts[5]);
                    LibraryItem newItem = new(itemID, title, itemType, dailyLateFee)
                    {
                        DaysLate = daysLate,
                        ItemLateFee = itemLateFee
                    };
                    CheckoutItem item = new(newItem);
                    CheckoutItem.CheckoutItems.Add(newItem);
                }
                Console.WriteLine("Checkout list loaded successfully!");
                PressContinue();
            }
            else
            {
                Console.WriteLine("No previous checkout list found.");
                PressContinue();
            }
        }
        public static void DeleteCheckoutList() 
        { 
            ClearScreen();
            var fileName = "CheckoutList.csv";
            if (File.Exists(fileName))
            {
                File.Delete(fileName);
                Console.WriteLine("File Deleted Successfully");
                PressContinue();
            }
            else
            {
                Console.WriteLine("File Does not exist");
                PressContinue();
            }
            
        }
    }
}