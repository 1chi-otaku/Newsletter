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
                        case 0:
                            return;
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
    }
}
