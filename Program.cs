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
        public static void MenuChoice() // This function clears the screen a starts a while loop and continues it while tempChoice is not equal to 10. It displays the menu and asks for user input then compares if the input was equal
        //to tempChoice and if it is it executes the provided method inside the switch case loop. If 10 is entered, it exits the program
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
        public static void ViewItems()//Clears the screen and loads in the catalog from a file. Then it goes through a foreach loop to loop through each item inside of the catalog list
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
        public static void PressContinue()//A function I made to quickly make a blank space and allow for a pause in the program where the user enters "Enter" or anything they want until enter is pressed
        {
            Console.WriteLine();
            Console.WriteLine("Press Enter to Continue...");
            Console.ReadLine();
        }
        public static void ClearScreen()//This is another function I made to be able to quickly clear a screen and add those dashed lines for a more appealing look to the console
        {
            Console.Clear();
            Console.WriteLine("-----------------------------");
        }
        public static void DisplayMenu()//This function simply displays the choices avaliable for the user to choice and this is what gets displayed in the MenuChoice function
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
        public static void AddItems()//This function allows the user to add an item to the Catalog by first asking the user to enter an ItemID, it again tries to convert the input to an int, and if successful, goes to the next input, where
        //the user is asked for a title, then the user is asked for an item type, which can only be book or DVD. If a book or a DVD isn't entered, it exits the fuction, and they have to try again. Then they are prompted to enter a daily late fee.
        //TryParse will then try to convert the answer into a decimal and if successful, they also have to enter a number between .25 and .75. If all that goes without problems, then a new instance of libraryItem is made, and that instance
        //is added to the catalog list, and the catalog is saved to file.
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
        public static void SaveCatalog()//This function saves the catalog list to file. It does this by getting a name of the file, It deletes the previous file, then goes through each item inside of catalog and adds it to the file. 
            //I also added a \n for how I later read the lines when I load them back into the program.
        {
            ClearScreen();
            string FileName = "Catalog.csv";
            File.Delete(FileName);
            foreach (LibraryItem item in Catalog)
            {
                File.AppendAllText(FileName, $"{item.ItemID},{item.Title},{item.ItemType},{item.DailyLateFee},\n");

            }
        }
        public static void LoadCatalog()//This function loads the catalog file back into the list. It again makes a name for this file, it checks to see if the file exists, if not then it creates a catalog with a default value, adds that item to catalog and saves it.
        //If it does exist, it will use the ReadAlLines method to read every line inside the file(thats why I used the \n in the saving function). It will clear the catalog list and then use a foreach loop to convert and split every
        //item to its correct format. I used the replace method to replace all white space with nothing instead of trimming the title and itemType. I then did line.Split(",") to split each line up by commas and put them into another array
        //that way, everything would be in the correct order. Each time the foreach runs, the parts array gets updated to equal the next line in the lines array. Then I am able to make an instance of libraryItem and add all of the requirements to it
        //then add it to the catalog
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
        public static void CheckingOutAnItem()//This function asks the user to enter the ID of the item, which we then again use TryParse for, and if successful, I run through each item in the checkedoutItems list to see if they have an item checked out with the same ID already
        //and if they do, it will tell them and exit the function. If they don't, it will go through each item in the catalog list until the ID entered equals the ID of the item, and if it does, then it checks to see if they have a book or DVD.
        //I do this to determine whether or not they had a due date of 7 days or 3 days. After checking, it will make a new checkoutItem instance and add that to the Checkout List(I use this list to keep track of the DueDates of the items) 
        //and adds the LibraryItem inside the checkoutItem to the checkoutItems list
        {
            ClearScreen();
            ViewItems();
            Console.WriteLine("Checkout an Item");
            Console.WriteLine("----------------");
            Console.Write("Enter the ID of the item you wish to check out: ");
            var tempID = Console.ReadLine();
            if (int.TryParse(tempID, out int id)) 
            {
                foreach (var checkoutItem in CheckoutItem.CheckoutItems)
                {
                    if (checkoutItem.ItemID == id)
                    {
                        Console.WriteLine("Item is already checked out.");
                        PressContinue();
                        return;
                    }
                }
                foreach (var item in Catalog)
                {
                    if (item.ItemID == id)
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
            else
            {
                Console.WriteLine("That is not a valid ID, exiting function");
                PressContinue();
                return;
            }
            
        }
        public static void ReturnItems()//This function checks to see if the checkoutItems list is empty and if it is then they can't return anything, so it exits the function. If it's not empty, it asks the user the ID they want to remove.
        //It uses TryParse for input validation, then goes through each item in the checkoutItems list and see if the ID entered is equaled to the ID of one of the items. If it is, it removes that item, and if after removing the item
        //The checkoutList is empty, it deletes the checkout file that way the user can't load back in and load there file back and still have a checked out item they returned. If it's not empty after that, then the checkoutList is saved to prevent
        //the problem stated a sentence ago.
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
                            break;
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
        public static void CheckedOutItems()//This function goes through each item in the CheckedoutItems list and displays them to the user
        {
            ClearScreen();
            Console.WriteLine("Checked Out Items:");
            foreach (var item in CheckoutItem.CheckoutItems)
            {
                item.DisplayInfo();
            }
            PressContinue();
        }
        public static void ViewCheckoutReceipt()//This function checks to see if the user has any items checked out and if they don't then it exits the function. If there are items, then it goes through each item in the checkoutItems list
        //and asks the user how long they have had the item checked out for. Takes that input and puts it into the latefee function. Another thing I did was use a counter to keep track of the index number I was at. I used this to find out
        //the index that the CheckoutItem was at, so I could grab the dueDate and the specific checkoutItem that the LibraryItem belongs to. After the calculations are done and each item is done it will display the receipt.
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
                var tempDays = Console.ReadLine();
                if (int.TryParse(tempDays, out int days))
                {
                    var moreItem = CheckoutItem.Checkout[counter];
                    CheckoutItem.LateFee(days, item, moreItem);
                    ClearScreen();
                    counter++;
                }
                else
                {
                    Console.WriteLine("That is not a valid input, exiting function");
                    PressContinue();
                    return;
                }                
            }
            CheckoutItem.CheckoutFormat();
            PressContinue();
        }
        public static void SaveCheckoutList(string endComment)//This function checks to see if the checkoutItem list is empty and if so then exits the function. If not then it grabs the name and deletes the file. Then it goes through each item and appends them to the file
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
        public static void LoadCheckoutList()//This function grabs the name checks to see if the file exists, then does the exact same thing the catalog did, but asks for 2 new values which are daysLate and itemLateFee. 
        //The values are added to the new instance, and the properties that belong to LibraryItem also get set as the two values of daysLate and itemLateFee. It then adds the item to the Checkout list and newItem to the CheckoutItems list
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
        public static void DeleteCheckoutList()//This function grabs the name and checks to see if the file exists. If it does, then it deletes the file and if it doesn't exist, it exits the function
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
        public static void DeleteCatalog()//This function asks the user if they want to delete the catalog and if they put 1(yes), then it checks to see if the catalog exists and deletes it, and if 2(no), it exits.
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
        public static void DeleteAnEntryInCatalog()//This function asks the user for an ID of an item they would like to delete and then goes through every item in catalog and finds which item matches the user input.
        //When it finds it, it removes that item and saves the catalog 
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
        public static void EnterACode()//This function asks the user if they are a admin or a guest. If 1(Admin), they are prompted to enter a code. If the code is right, then they are displayed a secret admin menu where they can do more functions
        //If they enter the password wrong then they are a guest automatically, or if they click 2, then they are a guest.
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
        public static void DisplayAdminMenu()//This function displays the AdminMenu and gets user input. If they chose 1, then it takes them to the DeleteCatalog function, and if they chose 2, then they get taken to the DeleteAnEntryInCatalog function
        //If they pick 3, then they exit back to the guest menu
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
