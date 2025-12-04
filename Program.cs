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
            EnterACode();
            ViewItems();
            MenuChoice();
        }
        public static void MenuChoice()
        {
            ClearScreen();
            string tempChoice = "0";
            while (tempChoice != "10")
            {
                ClearScreen();
                DisplayMenu();
                tempChoice = Console.ReadLine();
                //I used AI to help me understand how TryParse works. I already knew how Parse worked but was wondering how TryParse worked because it looked like it could be useful
                //for input validation. I asked it: How does TryParse work in C#? It told me that TryParse attempts to convert a string to a specific data type
                //and returns a bool indicating whether the conversion was successful or not. So basically it will try to convert it and if it doesn't work 
                //it won't throw an error like parse would. Instead of a bunch of if statements I thought this would be a better and cleaner way.
                if (int.TryParse(tempChoice, out int choice) && choice >= 1 && choice <= 10)
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
                            SaveCheckoutList("Item saved to Checkout List!");
                            break;
                        case 7:
                            LoadCheckoutList();
                            break;
                        case 8:
                            DeleteCheckoutList();
                            break;
                        case 9:
                            EnterACode();
                            break;
                        default:
                            Console.WriteLine("Goodbye...");
                            break;
                    }
                }
                else
                {
                    Console.WriteLine("Invalid input. Please enter a number between 1 and 10.");
                    PressContinue();
                }
            }
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
            Console.WriteLine("9. Switch Accounts");
            Console.WriteLine("10. Exit");

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
            LibraryItem Item = new(tempID, title.Trim(), itemType.Trim(), fee);
            Catalog.Add(Item);
            SaveCatalog();
            Console.WriteLine("Item added successfully!");
            PressContinue();
        }
        public static void SaveCatalog()
        {
            ClearScreen();
            string FileName = "Catalog.csv";
            File.Delete(FileName);
            foreach (LibraryItem item in Catalog)
            {
                File.AppendAllText(FileName, $"{item.ItemID},{item.Title},{item.ItemType},{item.DailyLateFee},\n");

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
                LibraryItem newItem = new(101,"The Great Gatsby","Book",0.25m);
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
            foreach (var checkoutItem in CheckoutItem.CheckoutItems)
            {
                if (checkoutItem.ItemID == tempID)
                {
                    Console.WriteLine("Item is already checked out.");
                    PressContinue();
                    return;
                }
            }
            foreach (var item in Catalog)
            {
                if (item.ItemID == tempID)
                {
                    if (item.ItemType.ToUpper().Trim() == "BOOK")
                    {
                        CheckoutItem newItem = new(item, 7);
                        CheckoutItem.Checkout.Add(newItem);
                        CheckoutItem.CheckoutItems.Add(newItem.Item);
                        Console.WriteLine($"You have checked out: {item.Title}");
                        Console.WriteLine($"Due date is in {newItem.DueDate} days.");
                    }
                    else if (item.ItemType.ToUpper().Trim() == "DVD")
                    {
                        CheckoutItem newItem1 = new(item);
                        CheckoutItem.Checkout.Add(newItem1);
                        CheckoutItem.CheckoutItems.Add(newItem1.Item);
                        Console.WriteLine($"You have checked out: {item.Title}");
                        Console.WriteLine($"Due date is in {newItem1.DueDate} days.");
                    }
                }
            }
            Console.WriteLine("Item checked out successfully!");
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
            var tempID = Console.ReadLine();
            if (int.TryParse(tempID, out int id))
            {
                foreach (var item in CheckoutItem.CheckoutItems)
                {
                    if (item.ItemID == id)
                    {
                        CheckoutItem.CheckoutItems.Remove(item);
                        if (CheckoutItem.CheckoutItems.Count == 0)
                        {
                            Console.WriteLine($"You have returned: {item.Title}");
                            Console.WriteLine("All items have been returned.");
                            File.Delete("CheckoutList.csv");
                            PressContinue();
                        }
                        else
                        {
                            SaveCheckoutList(($"You have returned: {item.Title}"));
                        }
                    }
                    else
                    {
                        Console.WriteLine("Item not found or not checked out.");
                        PressContinue();
                    }
                }
            }
            else
            {
                Console.WriteLine("That is not a valid ID, exiting function");
                PressContinue();
                return;
            }
           
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
            int counter = 0;
            ClearScreen();
            if (CheckoutItem.CheckoutItems.Count == 0)
            {
                Console.WriteLine("No items are currently checked out.");
                PressContinue();
                return;
            }
            Console.WriteLine("Checkout Receipt:");
            foreach (var item in CheckoutItem.CheckoutItems)
            {
                item.DisplayInfo();
                Console.WriteLine("How long have you had the item checked out for? (in days)");
                int tempDays = Convert.ToInt32(Console.ReadLine());
                var moreItem = CheckoutItem.Checkout[counter];
                CheckoutItem.LateFee(tempDays, item, moreItem);
                ClearScreen();
                counter++;
            }
            CheckoutItem.CheckoutFormat();
            PressContinue();
        }
        public static void SaveCheckoutList(string endComment)
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
            Console.WriteLine(endComment);
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
                    CheckoutItem.Checkout.Add(item);
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
        public static void DeleteCatalog() 
        {             
            ClearScreen();
            Console.WriteLine("You sure? (1 = Yes. 2 = No.)");
            var tempChoice = Console.ReadLine();
            if (int.TryParse(tempChoice, out int choice))
            {
                if (choice == 1)
                {
                    var fileName = "Catalog.csv";
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
                else
                {
                    return;
                }
            }
            else
            {
                Console.WriteLine("Thats not an option, exiting function");
                PressContinue();
                return;
            }
        }
        public static void DeleteAnEntryInCatalog()
        {
            ViewItems();
            Console.WriteLine("Which Item would you like to delete? (Enter ID)");
            var tempChoice = Console.ReadLine();
            if (int.TryParse(tempChoice, out int choice))
            {
                foreach (var item in Catalog)
                {
                    if (item.ItemID == choice)
                    {
                        Catalog.Remove(item);
                        SaveCatalog();
                        Console.WriteLine("Item Deleted Successfully");
                        PressContinue();
                    }
                    else
                    {
                        Console.WriteLine("Item not found.");
                        PressContinue();
                    }
                }
            }
            else
            {
                Console.WriteLine("That is not a valid ID, exiting function");
                PressContinue();
                return;
            }
        }
        public static void EnterACode()
        {
            ClearScreen();
            Console.WriteLine("1. Admin");
            Console.WriteLine("2. Guest");
            var tempChoice = Console.ReadLine();
            if (int.TryParse(tempChoice, out int choice))
            {
                if (choice == 1)
                {
                    Console.WriteLine("Enter your employee code:");
                    var tempCode = Console.ReadLine();
                    if (int.TryParse(tempCode, out int code))
                    {
                        if (code == 1234)
                        {
                            DisplayAdminMenu();
                        }
                        else
                        {
                            Console.WriteLine("Not the right code");
                            PressContinue();
                            return;
                        }
                    }
                    else
                    {
                        Console.WriteLine("That is not a valid option, you're a guest now.");
                        PressContinue();
                        return;
                    }
                }
                else
                {
                    Console.Write("You are a guest!");
                    PressContinue();
                    return;
                }
            }
            else
            {
                Console.WriteLine("That is not a valid option, you're a guest now.");
                PressContinue();
                return;
            }
        }
        public static void DisplayAdminMenu()
        {
            Console.WriteLine("1. Delete the catalog");
            Console.WriteLine("2. Delete an entry in the catalog");
            Console.WriteLine("3. Exit");
            var tempChoice = Console.ReadLine();
            if (int.TryParse(tempChoice, out int choice))
            {
                if (choice == 1)
                {
                    DeleteCatalog();
                }
                else if (choice == 2)
                {
                    DeleteAnEntryInCatalog();
                }
                else
                {
                    return;
                }
            }
            else
            {
                Console.WriteLine("That is not a valid option, exiting function");
                PressContinue();
                return;
            }
        }
    }
}