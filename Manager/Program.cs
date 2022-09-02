using Dapper_BDSQL;
using Dapper_BDSQL.Controller;
using Manager.Model;
using System;
using System.Collections.Generic;
using System.Threading;

namespace Manager
{
    class Program
    {
        static void Main()
        {
            MyLog.Log("----------------------------------------------------------------", LogLevel.Information);
            MyLog.Log("Program start", LogLevel.Information);
            MyLog.Log("Read file to SQLBD-way", LogLevel.Information);
            try
            {
                SQLController controller = new SQLController(@"D:\DBSetting.json");
                controller.Download();
                MyLog.Log("Successed read file to SQLBD-way", LogLevel.Information);

                //Создание подключения типа Singleton используя Dapper через класс DapperLink используя созданную из файла строку подключения
                MyLog.Log("Start connection to SQLBD", LogLevel.Information);
                DapperLink newLink = DapperLink.GetInstance(controller.defaultsetting);
                MyLog.Log("Connection to SQLBD is successful", LogLevel.Information);

                //Чтение данных о продуктах из БД после подключения к базе
                MyLog.Log("Start read list of products, categories and providers from SQLBD", LogLevel.Information);
                List<Product> products = new List<Product>();
                products = newLink.ReadProduct();
                List<Category> categories = new List<Category>();
                categories = newLink.ReadCategory();
                List<Provider> providers = new List<Provider>();
                providers = newLink.ReadProvider();
                MyLog.Log("Products. categories and providers from SQLBD were read", LogLevel.Information);

                //Создание списка переменных
                MyLog.Log("General progarm cicle starts", LogLevel.Information);
                Product newProduct = new Product();
                Category newCategory = new Category();
                Provider newProvider = new Provider();
                string choice, name;
                int tmpId;

                do
                {
                    Console.Clear();
                    Console.WriteLine("----------WORK WITH THE PRODUCT LIST----------\n");
                    Console.WriteLine("\n--------------WORK with PRODUCT-------------;");
                    Console.WriteLine("1. Search and correct the product;");
                    Console.WriteLine("2. Add a new product;");
                    Console.WriteLine("3. Delete a product;");
                    Console.WriteLine("4. Show all products list");
                    Console.WriteLine("\n--------------WORK with CATEGORY-------------;");
                    Console.WriteLine("5. Search and correct the category of product;");
                    Console.WriteLine("6. Add a new product category;");
                    Console.WriteLine("7. Delete a product category;");
                    Console.WriteLine("8. Show all categories of products");
                    Console.WriteLine("\n--------------WORK with PROVIDER-------------;");
                    Console.WriteLine("9. Search the provider of product;");
                    Console.WriteLine("10. Add a new provider of product;");
                    Console.WriteLine("11. Delete a provider of product;");
                    Console.WriteLine("12. Show all list and correct providers");
                    Console.WriteLine("\n0. Exit;");
                    Console.Write("Input the number of operation you want to do: ");
                    choice = Console.ReadLine();
                    switch (choice)
                    {
                        case "1":
                            MyLog.Log("1p was chosen [search and correct product]", LogLevel.Information);
                            Console.Clear();
                            if (products.Count > 0)
                            {
                                MyLog.Log("1p [search and correct product]: Input data", LogLevel.Information);
                                Console.Write("Input name of product you want to search - ");
                                name = Console.ReadLine();
                                MyLog.Log($"1p [search and correct product]: Search product with name [{name}]", LogLevel.Information);
                                if (products.Exists(x => x.Name.ToLower() == name.ToLower()))
                                {
                                    newProduct = products.Find(x => x.Name.ToLower() == name.ToLower());
                                    MyLog.Log($"1p [search and correct product]: The product with name {name} was found", LogLevel.Information);
                                    Console.WriteLine($"\n{products[products.FindIndex(x => x.Name.ToLower() == name.ToLower())].ToString()}\n");
                                    Console.WriteLine("\nDo you want to correct this product? Press Y if Yes and any kay if No\n");
                                    if (Console.ReadKey().Key == ConsoleKey.Y)
                                    {
                                        MyLog.Log($"1p [search and correct product]: Choose operation of correct product with name {name}", LogLevel.Information);
                                        MyLog.Log($"1p [search and correct product]: Inputting new data", LogLevel.Information);
                                        Console.Write("\nInput new name of the product - ");
                                        newProduct.Name = Console.ReadLine();
                                        Console.WriteLine("\nDo you want to change any data of this product? Press Y if Yes and any kay if No\n");
                                        if (Console.ReadKey().Key == ConsoleKey.Y)
                                        {
                                            Console.WriteLine("Input a number of the category from the list below:");
                                            categories = newLink.ReadCategory(true);
                                            tmpId = -1;
                                            do
                                            {
                                                choice = Console.ReadLine();
                                                Int32.TryParse(choice, out tmpId);
                                                if (tmpId <= categories.Count && tmpId > 0) newProduct.CategoryId = categories[tmpId - 1].CategoryId;
                                            } while (tmpId > categories.Count || tmpId <= 0);
                                            Console.WriteLine("Input a number of provider from the list below:");
                                            providers = newLink.ReadProvider(true);
                                            tmpId = -1;
                                            do
                                            {
                                                choice = Console.ReadLine();
                                                Int32.TryParse(choice, out tmpId);
                                                if (tmpId <= providers.Count && tmpId > 0) newProduct.ProviderId = providers[tmpId - 1].ProviderId;
                                            } while (tmpId > providers.Count && tmpId <= 0);
                                            Console.Write("Input new price of the product  you want to change - ");
                                            newProduct.Price = Console.ReadLine();
                                            MyLog.Log($"1p [search and correct product]: Inputed new product with data: name {newProduct.Name} categoryId {newProduct.CategoryId} providerId {newProduct.ProviderId} price {newProduct.Price}", LogLevel.Information);
                                        }
                                        newLink.ChangeProduct(name, newProduct);
                                        MyLog.Log($"1p [search and correct product]: Product in SQLBD with name [{name}] was corrected to new product with name [{newProduct.Name}]", LogLevel.Warning);
                                        products = newLink.ReadProduct();
                                    }
                                }
                                else
                                {
                                    Console.WriteLine($"\nThe product with Name [{name}] didn`t find!");
                                    MyLog.Log($"1p [search and correct product]: The product with name [{name}] in list didn`t find", LogLevel.Warning);
                                }
                            }
                            else
                            {
                                Console.WriteLine("\nThe list of products are empty yet!\n");
                                MyLog.Log("1p [search and correct product]: The list of products are empty yet", LogLevel.Warning);
                            }
                            Console.WriteLine("\nPress any key to continue");
                            Console.ReadKey();
                            MyLog.Log("Exit from 1p [search and correct product]", LogLevel.Information);
                            break;
                        case "2":
                            MyLog.Log("2p was chosen [add product]", LogLevel.Information);
                            Console.Clear();
                            Console.Write("Input name of new product you want to add in the list - ");
                            newProduct.Name = Console.ReadLine();
                            Console.WriteLine("Input a number of the category from the list below:");
                            categories = newLink.ReadCategory(true);
                            tmpId = -1;
                            do {
                                choice = Console.ReadLine();
                                Int32.TryParse(choice, out tmpId);
                                if (tmpId <= categories.Count && tmpId > 0) newProduct.CategoryId = categories[tmpId - 1].CategoryId;
                            } while (tmpId > categories.Count || tmpId <= 0);
                            Console.WriteLine("Input a number of provider from the list below:");
                            providers = newLink.ReadProvider(true);
                            tmpId = -1;
                            do {
                                choice = Console.ReadLine();
                                Int32.TryParse(choice, out tmpId);
                                if (tmpId <= providers.Count && tmpId > 0) newProduct.ProviderId = providers[tmpId - 1].ProviderId;
                            } while(tmpId > providers.Count && tmpId <= 0);
                            Console.Write("Input new price of the product  you want to change - ");
                            newProduct.Price = Console.ReadLine();
                            MyLog.Log($"2p [add product]: Inputed new product with data: name {newProduct.Name} categoryId {newProduct.CategoryId} providerId {newProduct.ProviderId} price {newProduct.Price}", LogLevel.Information);
                            newLink.AddProduct(newProduct);
                            products = newLink.ReadProduct();
                            Thread.Sleep(2000);
                            MyLog.Log("Exit from 2p [add product]", LogLevel.Information);
                            break;
                        case "3":
                            MyLog.Log("3p was chosen [delete product]", LogLevel.Information);
                            Console.Clear();
                            if (products.Count > 0)
                            {
                                MyLog.Log("3p was chosen [delete product]: Inputting name of delete product", LogLevel.Information);
                                Console.Write("Input name of product you want to delete from the list - ");
                                name = Console.ReadLine();
                                if (products.FindIndex(x => x.Name.ToLower() == name.ToLower()) != -1)
                                {
                                    newLink.DeleteProduct("Name", name);
                                    MyLog.Log($"3p [delete product]: The product witn Name [{name}] was found and deleted", LogLevel.Information);
                                    products = newLink.ReadProduct();
                                }
                                else
                                {
                                    Console.WriteLine("\nThe product didn`t find!");
                                    MyLog.Log($"3p [delete product]: The product with Name [{name}] in list didn`t find", LogLevel.Warning);
                                }
                            }
                            else
                            {
                                Console.WriteLine("\nThe list of products are empty yet!\n");
                                MyLog.Log("3p [delete product]: The list of products are empty yet", LogLevel.Warning);
                            }
                            Thread.Sleep(2000);
                            MyLog.Log("Exit from 3p [delete product]", LogLevel.Information);
                            break;
                        case "4":
                            MyLog.Log("4p was chosen [show all products]", LogLevel.Information);
                            Console.Clear();
                            products = newLink.ReadProduct(true);
                            Console.WriteLine("\nPress any key to continue");
                            Console.ReadKey();
                            MyLog.Log("Exit from 4p [show all products]", LogLevel.Information);
                            break;

                        case "5":
                            MyLog.Log("5p was chosen [search and correct the category of product]", LogLevel.Information);
                            Console.Clear();
                            if (categories.Count > 0)
                            {
                                MyLog.Log("5p [search and correct the category of product]: Input data", LogLevel.Information);
                                Console.Write("Input name of category you want to search - ");
                                name = Console.ReadLine();
                                MyLog.Log($"5p [search and correct the category of product]: Search category of product with Name [{name}]", LogLevel.Information);
                                if (categories.Exists(x => x.CategoryName.ToLower() == name.ToLower()))
                                {
                                    newCategory = categories.Find(x => x.CategoryName.ToLower() == name.ToLower());
                                    MyLog.Log($"5p [search and correct the category of product]: The category of product with Name [{name}] was found", LogLevel.Information);
                                    Console.WriteLine($"\n{categories[categories.FindIndex(x => x.CategoryName.ToLower() == name.ToLower())].ToString()}\n");
                                    Console.WriteLine("\nDo you want to correct this category? Press Y if Yes and any kay if No\n");
                                    if (Console.ReadKey().Key == ConsoleKey.Y)
                                    {
                                        MyLog.Log($"5p [search and correct the category of product]: Choose operation of correct the category of product with Name [{name}]", LogLevel.Information);
                                        MyLog.Log($"5p [search and correct the category of product]: Inputting new data", LogLevel.Information);
                                        Console.Write("\nInput new name of the category - ");
                                        newCategory.CategoryName = Console.ReadLine();
                                        MyLog.Log($"5p [search and correct the category of product]: Inputed new category with Name [{newCategory.CategoryName}]", LogLevel.Information);
                                        newLink.ChangeCategory(name, newCategory);
                                        MyLog.Log($"5p [search and correct the category of product]: Category in SQLBD with Name [{name}] was corrected to new category Name [{newCategory.CategoryName}]", LogLevel.Information);
                                        categories = newLink.ReadCategory();
                                    }
                                }
                                else
                                {
                                    Console.WriteLine($"\nThe category of product with Name [{name}] didn`t find!");
                                    MyLog.Log($"5p [search and correct the category of product]: The category of product with Name [{name}] in list didn`t find", LogLevel.Warning);
                                }
                            }
                            else
                            {
                                Console.WriteLine("\nThe list of categories are empty yet!\n");
                                MyLog.Log("5p [search and correct the category of product]: The list of categories are empty yet", LogLevel.Warning);
                            }
                            Console.WriteLine("\nPress any key to continue");
                            Console.ReadKey();
                            MyLog.Log("Exit from 5p [search and correct the category of product]", LogLevel.Information);
                            break;
                        case "6":
                            MyLog.Log("6p was chosen [add the category of product]", LogLevel.Information);
                            Console.Clear();
                            Console.Write("Input name of new category of product you want to add in the list - ");
                            newCategory.CategoryName = Console.ReadLine();
                            MyLog.Log($"6p [add the category of product]: Inputed new category with data: name {newCategory.CategoryName} categoryId {newProduct.CategoryId} providerId {newProduct.ProviderId} price {newProduct.Price}", LogLevel.Information);
                            newLink.AddCategory(newCategory);
                            Console.WriteLine("New category was added!");
                            categories = newLink.ReadCategory();
                            Thread.Sleep(2000);
                            MyLog.Log("Exit from 6p [add the category of product]", LogLevel.Information);
                            break;
                        case "7":
                            MyLog.Log("7p was chosen [delete the category of product]", LogLevel.Information);
                            Console.Clear();
                            if (categories.Count > 0)
                            {
                                MyLog.Log("7p was chosen [delete the category of product]: Inputting name of delete product", LogLevel.Information);
                                Console.Write("Input name of category you want to delete from the list - ");
                                name = Console.ReadLine();
                                if (categories.FindIndex(x => x.CategoryName.ToLower() == name.ToLower()) != -1)
                                {
                                    newLink.DeleteCategory(name);
                                    MyLog.Log($"7p [delete the category of product]: The category of product witn Name [{name}] was found and deleted", LogLevel.Information);
                                    categories = newLink.ReadCategory();
                                }
                                else
                                {
                                    Console.WriteLine("\nThe category didn`t find!");
                                    MyLog.Log($"7p [delete the category of product]: The category with Name [{name}] in list didn`t find", LogLevel.Warning);
                                }
                            }
                            else
                            {
                                Console.WriteLine("\nThe list of categories is empty yet!\n");
                                MyLog.Log("7p [delete the category of product]: The list of categories is empty yet", LogLevel.Warning);
                            }
                            Thread.Sleep(2000);
                            MyLog.Log("Exit from 7p [delete the category of product]", LogLevel.Information);
                            break;
                        case "8":
                            MyLog.Log("8p was chosen [show all the category]", LogLevel.Information);
                            Console.Clear();
                            categories = newLink.ReadCategory(true);
                            Console.WriteLine("\nPress any key to continue");
                            Console.ReadKey();
                            MyLog.Log("Exit from 8p [show all the category]", LogLevel.Information);
                            break;

                        case "9":
                            MyLog.Log("9p was chosen [search and correct the provider of product]", LogLevel.Information);
                            Console.Clear();
                            if (providers.Count > 0)
                            {
                                MyLog.Log("9p [search the provider of product]: Input data", LogLevel.Information);
                                Console.Write("Input short name of provider you want to search - ");
                                name = Console.ReadLine();
                                MyLog.Log($"9p [search the provider of product]: Search provider of product with short Name [{name}]", LogLevel.Information);
                                if (providers.Exists(x => x.ProviderShortName.ToLower() == name.ToLower()))
                                {
                                    newProvider = providers.Find(x => x.ProviderShortName.ToLower() == name.ToLower());
                                    MyLog.Log($"9p [search the provider of product]: The provider of product with short Name [{name}] was found", LogLevel.Information);
                                    Console.WriteLine($"\n{providers[providers.FindIndex(x => x.ProviderShortName.ToLower() == name.ToLower())].ToString()}\n");
                                }
                                else
                                {
                                    Console.WriteLine($"\nThe provider of product with short Name [{name}] didn`t find!");
                                    MyLog.Log($"9p [search the provider of product]: The category of product with short Name [{name}] in list didn`t find", LogLevel.Warning);
                                }
                            }
                            else
                            {
                                Console.WriteLine("\nThe list of providers is empty yet!\n");
                                MyLog.Log("9p [search the provider of product]: The list of providers is empty yet", LogLevel.Warning);
                            }
                            Console.WriteLine("\nPress any key to continue");
                            Console.ReadKey();
                            MyLog.Log("Exit from 9p [search the provider of product]", LogLevel.Information);
                            break;
                        case "10":
                            MyLog.Log("10p was chosen [add the new provider of product]", LogLevel.Information);
                            Console.Clear();
                            Console.Write("\nInput full name of the new provider - ");
                            newProvider.ProviderFullName = Console.ReadLine();
                            Console.Write("\nInput short name of the new provider - ");
                            newProvider.ProviderShortName = Console.ReadLine();
                            Console.Write("\nInput address of the new provider - ");
                            newProvider.Address = Console.ReadLine();
                            newLink.AddProvider(newProvider);
                            Console.WriteLine("New provider was added!");
                            providers = newLink.ReadProvider();
                            Thread.Sleep(2000);
                            MyLog.Log("Exit from 10p [add the new provider of product]", LogLevel.Information);
                            break;
                        case "11":
                            MyLog.Log("11p was chosen [delete the provider of product]", LogLevel.Information);
                            Console.Clear();
                            if (providers.Count > 0)
                            {
                                MyLog.Log("11p was chosen [delete the provider of product]: Inputting id of delete provider", LogLevel.Information);
                                Console.Write("Input a number of the provider from the list below:");
                                providers = newLink.ReadProvider(true);
                                tmpId = -1;
                                do
                                {
                                    choice = Console.ReadLine();
                                    Int32.TryParse(choice, out tmpId);
                                    if (tmpId <= providers.Count && tmpId > 0) newProvider.ProviderId = providers[tmpId - 1].ProviderId;
                                } while (tmpId > providers.Count || tmpId <= 0);
                                
                                newLink.DeleteProvider(tmpId);
                                MyLog.Log($"11p [delete the provider of product]: The provider of product witn Id [{tmpId}] and short Name [{providers[tmpId - 1].ProviderShortName}] was deleted", LogLevel.Information);
                                Console.WriteLine($"The provider of product witn Id [{tmpId}] and short Name [{providers[tmpId - 1].ProviderShortName}] was deleted!");
                                providers = newLink.ReadProvider();
                            }
                            else
                            {
                                Console.WriteLine("\nThe list of providers is empty yet!\n");
                                MyLog.Log("7p [delete the category of product]: The list of categories is empty yet", LogLevel.Warning);
                            }
                            Thread.Sleep(2000);
                            MyLog.Log("Exit from 11p [delete the provider of product]", LogLevel.Information);
                            break;
                        case "12":
                            MyLog.Log("12p was chosen [show all list and correct provider]", LogLevel.Information);
                            Console.Clear();
                            providers = newLink.ReadProvider(true);
                            Console.WriteLine("\nDo you want to correct provider in the list? Press Y if Yes and any kay if No\n");
                            if (Console.ReadKey().Key == ConsoleKey.Y)
                            {
                                Console.Write("Input a number of the provider from the list to change:");
                                tmpId = -1;
                                do
                                {
                                    choice = Console.ReadLine();
                                    Int32.TryParse(choice, out tmpId);
                                        if (tmpId <= providers.Count && tmpId > 0) newProvider = providers[tmpId - 1];
                                        else Console.WriteLine("You inputted incorrect number. Try again!");
                                } while (tmpId > providers.Count || tmpId <= 0);
                                MyLog.Log($"12p [show all list and correct provider]: Inputting new provider data for changing provider", LogLevel.Information);
                                Console.Write("\nInput new full name of the provider - ");
                                newProvider.ProviderFullName = Console.ReadLine();
                                Console.Write("\nInput new short name of the provider - ");
                                newProvider.ProviderShortName = Console.ReadLine();
                                Console.Write("\nInput new address of the provider - ");
                                newProvider.Address = Console.ReadLine();
                                MyLog.Log($"12p [show all list and correct provider]: Inputed all data for changing provider with short Name [{newProvider.ProviderShortName}]", LogLevel.Information);
                                newLink.ChangeProvider(tmpId, newProvider);
                                MyLog.Log($"12p [show all list and correct provider]: Provider in SQLBD with short Name [{newProvider.ProviderShortName}] was corrected", LogLevel.Information);
                                Console.WriteLine($"Provider in SQLBD with short Name [{newProvider.ProviderShortName}] was corrected");
                                categories = newLink.ReadCategory();
                            }
                            MyLog.Log("Exit from 12p [show all list and correct provider]", LogLevel.Information);
                            break;

                    }
                } while (choice != "0");
            }
            catch (Exception ex)
            {
                MyLog.Log(ex.Message);
            }
            MyLog.Log("Program end", LogLevel.Information);
        }
    }
}
