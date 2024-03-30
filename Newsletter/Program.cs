using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Newsletter.Newsletter;
using System;
using System.Data;
using System.IO;

namespace Newsletter
{
    class MainClass
    {
        static string? connectionString;

        static void Main()
        {
            var builder = new ConfigurationBuilder();
            string path = Directory.GetCurrentDirectory();
            builder.SetBasePath(path);
            builder.AddJsonFile("appsettings.json");
            var config = builder.Build();
            connectionString = config.GetConnectionString("DefaultConnection");

            try
            {
                while (true)
                {
                    Console.Clear();
                    Console.WriteLine("1. Show all Customers");
                    Console.WriteLine("2. Show all Emails");
                    Console.WriteLine("3. Show all Sections");
                    Console.WriteLine("4. Show all Promotional Items");
                    Console.WriteLine("5. Show all Cities");
                    Console.WriteLine("6. Show all Countries");
                    Console.WriteLine("7. Show Customers from Specific City");
                    Console.WriteLine("8. Show Customers from Specific Country");
                    Console.WriteLine("9. Show Promotional Items for Specific Country");
                    Console.WriteLine("10. Insert new Customer");
                    Console.WriteLine("11. Insert new Country");
                    Console.WriteLine("12. Insert new City");
                    Console.WriteLine("13. Insert new Section");
                    Console.WriteLine("14. Insert new Promotional Item");
                    Console.WriteLine("15. Update Customer Information");
                    Console.WriteLine("16. Delete Customer Information");
                    Console.WriteLine("17. Delete Country Information");
                    Console.WriteLine("18. Delete Section Information");
                    Console.WriteLine("19. Delete Promotional Item Information");
                    Console.WriteLine("20. Update Country Info");
                    Console.WriteLine("21. Update City Info");
                    Console.WriteLine("22. Update Section Info");
                    Console.WriteLine("23. Update Promotional Item Info");
                    Console.WriteLine("0. Exit");
                    int result = int.Parse(Console.ReadLine()!);
                    switch (result)
                    {
                        case 1:
                            ShowAllCustomers();
                            break;
                        case 2:
                            ShowAllCustomersEmails();
                            break;
                        case 3:
                            ShowAllSections();
                            break;
                        case 4:
                            ShowAllPromotionalItems();
                            break;
                        case 5:
                            ShowAllCities();
                            Console.ReadKey();
                            break;
                        case 6:
                            ShowAllCountries();
                            Console.ReadKey();
                            break;
                        case 7:
                            ShowCustomersFromSpecificCity();
                            break;
                        case 8:
                            ShowCustomersFromSpecificCountry();
                            break;
                        case 9:
                            ShowPromotionalItemsForSpecificCountry();
                            break;
                        case 10:
                            InsertNewCustomer();
                            break;
                        case 11:
                            InsertNewCountry();
                            break;
                        case 12:
                            InsertNewCity();
                            break;
                        case 13:
                            InsertNewSection();
                            break;
                        case 14:
                            InsertNewPromotionalItem();
                            break;
                        case 15:
                            UpdateCustomerInfo();
                            break;
                        case 16:
                            DeleteCustomerInfo();
                            break;
                        case 17:
                            DeleteCountryInfo();
                            break;
                        case 18:
                            DeleteSectionInfo();
                            break;
                        case 19:
                            DeletePromotionalItemInfo();
                            break;
                        case 20:
                            UpdateCountryInfo();
                            break;
                        case 21:
                            UpdateCityInfo();
                            break;
                        case 22:
                            UpdateSectionInfo();
                            break;
                        case 23:
                            UpdatePromotionalItemInfo();
                            break;

                        case 0:
                            return;
                        default:
                            Console.WriteLine("Invalid option. Please try again.");
                            Console.ReadKey();
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }

        static void ShowAllCustomers()
        {
            Console.Clear();
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                var customers = db.Query<Customer>("SELECT * FROM Customers");
                int iter = 0;
                foreach (var customer in customers)
                    Console.WriteLine($"Customer #{++iter}: {customer.FullName}, {customer.BirthDate}, Gender: {(customer.Gender ? "Male" : "Female")}, Email: {customer.Email}");
            }
            Console.ReadKey();
        }

        static void ShowAllCustomersEmails()
        {
            Console.Clear();
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                var emails = db.Query<string>("SELECT Email FROM Customers");
                int iter = 0;
                foreach (var email in emails)
                    Console.WriteLine($"Customer #{++iter} Email: {email}");
            }
            Console.ReadKey();
        }

        static void ShowAllSections()
        {
            Console.Clear();
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                var sections = db.Query<string>("SELECT Name FROM Sections");
                int iter = 0;
                foreach (var section in sections)
                    Console.WriteLine($"Section #{++iter}: {section}");
            }
            Console.ReadKey();
        }

        static void ShowAllPromotionalItems()
        {
            Console.Clear();
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                var promotionalItems = db.Query<string>("SELECT Name FROM PromotionalItem");
                int iter = 0;
                foreach (var item in promotionalItems)
                    Console.WriteLine($"Promotional Item #{++iter}: {item}");
            }
            Console.ReadKey();
        }

        static void ShowAllCities()
        {
            Console.Clear();
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                var cities = db.Query<string>("SELECT Name FROM Cities");
                int iter = 0;
                foreach (var city in cities)
                    Console.WriteLine($"City #{++iter}: {city}");
            }
            
        }

        static void ShowAllCountries()
        {
            Console.Clear();
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                var countries = db.Query<string>("SELECT Name FROM Countries");
                int iter = 0;
                foreach (var country in countries)
                    Console.WriteLine($"Country #{++iter}: {country}");
            }
            
        }

        static void ShowCustomersFromSpecificCity()
        {
            
            Console.Clear();
            ShowAllCities();
            Console.WriteLine("Enter the name of the city:");
            string cityName = Console.ReadLine();
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                var customers = db.Query<Customer>("SELECT * FROM Customers WHERE CityID IN (SELECT ID FROM Cities WHERE Name = @CityName)", new { CityName = cityName });
                int iter = 0;
                foreach (var customer in customers)
                    Console.WriteLine($"Customer #{++iter}: {customer.FullName}, {customer.BirthDate}, Gender: {(customer.Gender ? "Male" : "Female")}, Email: {customer.Email}");
            }
            Console.ReadKey();
        }

        static void ShowCustomersFromSpecificCountry()
        {
            Console.Clear();
            ShowAllCountries();
            Console.WriteLine("Enter the name of the country:");
            string countryName = Console.ReadLine();
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                var customers = db.Query<Customer>("SELECT * FROM Customers WHERE CityID IN (SELECT ID FROM Cities WHERE CountryID IN (SELECT ID FROM Countries WHERE Name = @CountryName))", new { CountryName = countryName });
                int iter = 0;
                foreach (var customer in customers)
                    Console.WriteLine($"Customer #{++iter}: {customer.FullName}, {customer.BirthDate}, Gender: {(customer.Gender ? "Male" : "Female")}, Email: {customer.Email}");
            }
            Console.ReadKey();
        }

        static void ShowPromotionalItemsForSpecificCountry()
        {
            Console.Clear();
            ShowAllCountries();
            Console.WriteLine("Enter the name of the country:");
            string countryName = Console.ReadLine();
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                var promotionalItems = db.Query<string>("SELECT Name FROM PromotionalItem WHERE ID IN (SELECT PromotionalItemID FROM PromotionalSections WHERE SectionID IN (SELECT ID FROM Sections WHERE ID IN (SELECT SectionID FROM CustomersSections WHERE CustomerID IN (SELECT ID FROM Customers WHERE CityID IN (SELECT ID FROM Cities WHERE CountryID IN (SELECT ID FROM Countries WHERE Name = @CountryName))))))", new { CountryName = countryName });
                int iter = 0;
                foreach (var item in promotionalItems)
                    Console.WriteLine($"Promotional Item #{++iter}: {item}");
            }
            Console.ReadKey();
        }

        static void InsertNewCustomer()
        {
            Console.Clear();
            Console.WriteLine("Enter customer full name:");
            string fullName = Console.ReadLine();
            Console.WriteLine("Enter customer birth date (YYYY-MM-DD):");
            DateTime birthDate = DateTime.Parse(Console.ReadLine()!);
            Console.WriteLine("Enter customer email:");
            string email = Console.ReadLine();
            Console.WriteLine("Enter customer gender (M/F):");
            string genderInput = Console.ReadLine()?.ToUpperInvariant();
            bool gender = genderInput == "M";

            using (IDbConnection db = new SqlConnection(connectionString))
            {
                var sqlQuery = "INSERT INTO Customers (FullName, BirthDate, Gender, Email) VALUES (@FullName, @BirthDate, @Gender, @Email)";
                int rowsAffected = db.Execute(sqlQuery, new { FullName = fullName, BirthDate = birthDate, Gender = gender, Email = email });
                if (rowsAffected > 0)
                    Console.WriteLine("Customer successfully inserted.");
                else
                    Console.WriteLine("Failed to insert customer.");
            }
            Console.ReadKey();
        }
        static void InsertNewCountry()
        {
            Console.Clear();
            Console.WriteLine("Enter country name:");
            string countryName = Console.ReadLine();

            using (IDbConnection db = new SqlConnection(connectionString))
            {
                var sqlQuery = "INSERT INTO Countries (Name) VALUES (@CountryName)";
                int rowsAffected = db.Execute(sqlQuery, new { CountryName = countryName });
                if (rowsAffected > 0)
                    Console.WriteLine("Country successfully inserted.");
                else
                    Console.WriteLine("Failed to insert country.");
            }
            Console.ReadKey();
        }
        static void InsertNewCity()
        {
            Console.Clear();
            Console.WriteLine("Enter city name:");
            string cityName = Console.ReadLine();
            Console.WriteLine("Enter country ID:");
            int countryId = int.Parse(Console.ReadLine()!);

            using (IDbConnection db = new SqlConnection(connectionString))
            {
                var sqlQuery = "INSERT INTO Cities (Name, CountryID) VALUES (@CityName, @CountryID)";
                int rowsAffected = db.Execute(sqlQuery, new { CityName = cityName, CountryID = countryId });
                if (rowsAffected > 0)
                    Console.WriteLine("City successfully inserted.");
                else
                    Console.WriteLine("Failed to insert city.");
            }
            Console.ReadKey();
        }
        static void InsertNewSection()
        {
            Console.Clear();
            Console.WriteLine("Enter section name:");
            string sectionName = Console.ReadLine();

            using (IDbConnection db = new SqlConnection(connectionString))
            {
                var sqlQuery = "INSERT INTO Sections (Name) VALUES (@SectionName)";
                int rowsAffected = db.Execute(sqlQuery, new { SectionName = sectionName });
                if (rowsAffected > 0)
                    Console.WriteLine("Section successfully inserted.");
                else
                    Console.WriteLine("Failed to insert section.");
            }
            Console.ReadKey();
        }
        static void InsertNewPromotionalItem()
        {
            Console.Clear();
            Console.WriteLine("Enter promotional item name:");
            string itemName = Console.ReadLine();

            using (IDbConnection db = new SqlConnection(connectionString))
            {
                var sqlQuery = "INSERT INTO PromotionalItem (Name) VALUES (@ItemName)";
                int rowsAffected = db.Execute(sqlQuery, new { ItemName = itemName });
                if (rowsAffected > 0)
                    Console.WriteLine("Promotional item successfully inserted.");
                else
                    Console.WriteLine("Failed to insert promotional item.");
            }
            Console.ReadKey();
        }

        static void UpdateCustomerInfo()
        {
            Console.Clear();
            Console.WriteLine("Enter customer ID:");
            int customerId = int.Parse(Console.ReadLine()!);
            Console.WriteLine("Enter new full name:");
            string fullName = Console.ReadLine();
            Console.WriteLine("Enter new birth date (YYYY-MM-DD):");
            DateTime birthDate = DateTime.Parse(Console.ReadLine()!);
            Console.WriteLine("Enter new email:");
            string email = Console.ReadLine();
            Console.WriteLine("Enter new gender (M/F):");
            string genderInput = Console.ReadLine()?.ToUpperInvariant();
            bool gender = genderInput == "M";

            using (IDbConnection db = new SqlConnection(connectionString))
            {
                var sqlQuery = "UPDATE Customers SET FullName = @FullName, BirthDate = @BirthDate, Gender = @Gender, Email = @Email WHERE ID = @CustomerID";
                int rowsAffected = db.Execute(sqlQuery, new { FullName = fullName, BirthDate = birthDate, Gender = gender, Email = email, CustomerID = customerId });
                if (rowsAffected > 0)
                    Console.WriteLine("Customer information successfully updated.");
                else
                    Console.WriteLine("Failed to update customer information.");
            }
            Console.ReadKey();
        }

        static void UpdateCountryInfo()
        {
            Console.Clear();
            Console.WriteLine("Enter country name:");
            string countryName = Console.ReadLine();
            Console.WriteLine("Enter new country name:");
            string newCountryName = Console.ReadLine();

            using (IDbConnection db = new SqlConnection(connectionString))
            {
                var sqlQuery = "UPDATE Countries SET Name = @NewCountryName WHERE Name = @CountryName";
                int rowsAffected = db.Execute(sqlQuery, new { NewCountryName = newCountryName, CountryName = countryName });
                if (rowsAffected > 0)
                    Console.WriteLine("Country information successfully updated.");
                else
                    Console.WriteLine("Failed to update country information.");
            }
            Console.ReadKey();
        }

        static void UpdateCityInfo()
        {
            Console.Clear();
            Console.WriteLine("Enter city name:");
            string cityName = Console.ReadLine();
            Console.WriteLine("Enter new city name:");
            string newCityName = Console.ReadLine();

            using (IDbConnection db = new SqlConnection(connectionString))
            {
                var sqlQuery = "UPDATE Cities SET Name = @NewCityName WHERE Name = @CityName";
                int rowsAffected = db.Execute(sqlQuery, new { NewCityName = newCityName, CityName = cityName });
                if (rowsAffected > 0)
                    Console.WriteLine("City information successfully updated.");
                else
                    Console.WriteLine("Failed to update city information.");
            }
            Console.ReadKey();
        }

        static void UpdateSectionInfo()
        {
            Console.Clear();
            Console.WriteLine("Enter section name:");
            string sectionName = Console.ReadLine();
            Console.WriteLine("Enter new section name:");
            string newSectionName = Console.ReadLine();

            using (IDbConnection db = new SqlConnection(connectionString))
            {
                var sqlQuery = "UPDATE Sections SET Name = @NewSectionName WHERE Name = @SectionName";
                int rowsAffected = db.Execute(sqlQuery, new { NewSectionName = newSectionName, SectionName = sectionName });
                if (rowsAffected > 0)
                    Console.WriteLine("Section information successfully updated.");
                else
                    Console.WriteLine("Failed to update section information.");
            }
            Console.ReadKey();
        }

        static void UpdatePromotionalItemInfo()
        {
            Console.Clear();
            Console.WriteLine("Enter promotional item name:");
            string itemName = Console.ReadLine();
            Console.WriteLine("Enter new promotional item name:");
            string newItemName = Console.ReadLine();

            using (IDbConnection db = new SqlConnection(connectionString))
            {
                var sqlQuery = "UPDATE PromotionalItem SET Name = @NewItemName WHERE Name = @ItemName";
                int rowsAffected = db.Execute(sqlQuery, new { NewItemName = newItemName, ItemName = itemName });
                if (rowsAffected > 0)
                    Console.WriteLine("Promotional item information successfully updated.");
                else
                    Console.WriteLine("Failed to update promotional item information.");
            }
            Console.ReadKey();
        }

        static void DeleteCustomerInfo()
        {
            Console.Clear();
            Console.WriteLine("Enter customer ID:");
            int customerId = int.Parse(Console.ReadLine());

            using (IDbConnection db = new SqlConnection(connectionString))
            {
                var sqlQuery = "DELETE FROM Customers WHERE ID = @CustomerId";
                int rowsAffected = db.Execute(sqlQuery, new { CustomerId = customerId });
                if (rowsAffected > 0)
                    Console.WriteLine("Customer information successfully deleted.");
                else
                    Console.WriteLine("Failed to delete customer information.");
            }
            Console.ReadKey();
        }

        static void DeleteCountryInfo()
        {
            Console.Clear();
            Console.WriteLine("Enter country name:");
            string countryName = Console.ReadLine();

            using (IDbConnection db = new SqlConnection(connectionString))
            {
                var sqlQuery = "DELETE FROM Countries WHERE Name = @CountryName";
                int rowsAffected = db.Execute(sqlQuery, new { CountryName = countryName });
                if (rowsAffected > 0)
                    Console.WriteLine("Country information successfully deleted.");
                else
                    Console.WriteLine("Failed to delete country information.");
            }
            Console.ReadKey();
        }
        static void DeleteSectionInfo()
        {
            Console.Clear();
            Console.WriteLine("Enter section ID:");
            int sectionId = int.Parse(Console.ReadLine());

            using (IDbConnection db = new SqlConnection(connectionString))
            {
                var sqlQuery = "DELETE FROM Sections WHERE ID = @SectionId";
                int rowsAffected = db.Execute(sqlQuery, new { SectionId = sectionId });
                if (rowsAffected > 0)
                    Console.WriteLine("Section information successfully deleted.");
                else
                    Console.WriteLine("Failed to delete section information.");
            }
            Console.ReadKey();
        }
        static void DeletePromotionalItemInfo()
        {
            Console.Clear();
            Console.WriteLine("Enter promotional item ID:");
            int itemId = int.Parse(Console.ReadLine());

            using (IDbConnection db = new SqlConnection(connectionString))
            {
                var sqlQuery = "DELETE FROM PromotionalItem WHERE ID = @ItemId";
                int rowsAffected = db.Execute(sqlQuery, new { ItemId = itemId });
                if (rowsAffected > 0)
                    Console.WriteLine("Promotional item information successfully deleted.");
                else
                    Console.WriteLine("Failed to delete promotional item information.");
            }
            Console.ReadKey();
        }


    }
}
