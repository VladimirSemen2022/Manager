using Dapper;
using Dapper.Contrib.Extensions;
using Manager.Model;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Work_with_SQL_Table__HW_4_;

namespace Dapper_BDSQL.Controller
{
    class DapperLink
    {
        static string link;
        public SqlConnection connection { get; private set; }

        private DapperLink()     //Создание подключения к базе SQL
        {
            connection = new SqlConnection(link);
            connection.Open();      //Открытие соединения с базой SQL
            MyLog.Log("Connection to SQL-base", LogLevel.Information);
        }

        private static DapperLink _instance;     //Внутренняя переменная класса, хранящая соединение с базой 
        public static DapperLink GetInstance(DBSettings settings)     //Метод создания подключения к базе SQL
        {
            if (_instance == null)
            {
                link = settings.ToString();
                _instance = new DapperLink();
            }
            return _instance;
        }

        public List<Provider> ReadProvider(bool show=false)     //Чтение списка поставщиков, если show = true то считанные из базы данные вывести на экран
        {
            MyLog.Log("Start providers read from SQL-base", LogLevel.Information);
            //List<Provider> providers = (List<Provider>)connection.Query<Provider>("SELECT * FROM [Provider];");
            List<Provider> providers = connection.GetAll<Provider>().ToList();
            if (providers.Count != 0)
            {
                MyLog.Log("Providers list was read from SQL-base successfully", LogLevel.Information);
                if (show)
                {
                    Console.WriteLine();
                    int iter = 1;
                    foreach (var item in providers)
                    {
                        Console.WriteLine($"{iter++}) {item.ToString()}");
                    }
                    //providers.ToList().ForEach(Console.WriteLine);
                }
            }
            else
                MyLog.Log("Providers list wasn`t read from SQL-base or is empty", LogLevel.Warning);
            return providers;
        }
        public List<Category> ReadCategory(bool show=false)     //Чтение списка категорий продуктов, если show = true то считанные из базы данные вывести на экран
        {
            MyLog.Log("Start categories read from SQL-base", LogLevel.Information);
            //List<Category> categories = (List<Category>)connection.Query<Category>("SELECT * FROM [Category];");
            List<Category> categories = connection.GetAll<Category>().ToList();
            if (categories.Count != 0)
            {
                MyLog.Log("Categories list was read from SQL-base successfully", LogLevel.Information);
                if (show)
                {
                    Console.WriteLine();
                    int iter = 1;
                    foreach (var item in categories)
                    {
                        Console.WriteLine($"{iter++}) {item.ToString()}");
                    }
                    //categories.ToList().ForEach(Console.WriteLine);
                }
            }
            else
                MyLog.Log("Categories list wasn`t read from SQL-base or is empty", LogLevel.Warning);
            return categories;
        }
        public List<Product> ReadProduct(bool show=false)       //Чтение списка продуктов, если show = true то считанные из базы данные вывести на экран
        {
            MyLog.Log("Start products read from SQL-base", LogLevel.Information);
            //List<Product> products = (List<Product>)connection.Query<Product>("SELECT * FROM [Product];");
            List<Product> products = connection.GetAll<Product>().ToList();
            if (products.Count != 0)
            {
                MyLog.Log("Products list was read from SQL-base successfully", LogLevel.Information);
                if (show)
                {
                    Console.WriteLine();
                    int iter = 1;
                    foreach (var item in products)
                    {
                        Console.WriteLine($"{iter++}) {item.ToString()}");
                    }
                    //if (show) products.ToList().ForEach(Console.WriteLine);
                }
            }
            else
                MyLog.Log("Products list wasn`t read from SQL-base or is empty", LogLevel.Warning);
            return products;
        }

        public void AddProvider (Provider newProvider)          //Добавление нового поставщика продуктов
        {
            if (newProvider != null)
            {
                MyLog.Log($"Start adding new provider [{newProvider.ProviderFullName}] to SQL-base", LogLevel.Information) ;
                //int rows = connection.Execute($"INSERT INTO [dbo].[Provider]([ProviderFullName],[ProviderShortName],[Address])VALUES (\'{newProvider.ProviderFullName}\', \'{newProvider.ProviderShortName}\', \'{newProvider.Address}\');");
                long rows = connection.Insert<Provider>(newProvider);
                if (rows > 0)
                {
                    MyLog.Log($"New provider [{newProvider.ProviderFullName}] was added to SQL-base successfully", LogLevel.Information);
                    Console.WriteLine($"Provider {newProvider.ProviderFullName} was added to SQL-base!");
                }
                else
                    MyLog.Log($"New provider [{newProvider.ProviderFullName}] wasn`t added to SQL-base", LogLevel.Warning);
            }
        }

        public void AddCategory(Category newCategory)           //Добавление новой категории продукта
        {
            if (newCategory != null)
            {
                MyLog.Log($"Start adding new product category [{newCategory.CategoryName}] to SQL-base", LogLevel.Information);
                //int rows = connection.Execute($"INSERT INTO [dbo].[Category]([CategoryName])VALUES (\'{newCategory.CategoryName}\');");
                long rows = connection.Insert<Category>(newCategory);
                if (rows > 0)
                {
                    MyLog.Log($"New product category [{newCategory.CategoryName}] was added to SQL-base successfully", LogLevel.Information);
                    Console.WriteLine($"Category {newCategory.CategoryName} was added to SQL-base!");
                }
                else
                    MyLog.Log($"New product category [{newCategory.CategoryName}] wasn`t added to SQL-base", LogLevel.Warning);
            }
        }

        public void AddProduct(Product newProduct)               //Добавление нового продукта
        {
            if (newProduct != null)
            {
                MyLog.Log($"Start adding new product category [{newProduct.Name}] to SQL-base", LogLevel.Information);
                //int rows = connection.Execute($"INSERT INTO [dbo].[Product]([Name],[CategoryId],[ProviderId],[Price])VALUES (\'{newProduct.Name}\', \'{newProduct.CategoryId}\',  \'{newProduct.ProviderId}\', \'{newProduct.Price}\');");
                long rows = connection.Insert<Product>(newProduct);
                if (rows > 0)
                {
                    MyLog.Log($"New product [{newProduct.Name}] was added to SQL-base successfully", LogLevel.Information);
                    Console.WriteLine($"Product [{newProduct.Name}] was added to SQL-base!");
                }
                else
                    MyLog.Log($"New product [{newProduct.Name}] wasn`t added to SQL-base", LogLevel.Warning);
            }
        }

        public void ChangeProvider(int id, Provider newProvider) //Выбор и изменение поставщика по его id
        {
            if (ReadProvider().Count > 1)
            {
                //var sqlQuery = $"UPDATE Provider SET ProviderFullName = @ProviderFullName, ProviderShortName = @ProviderShortName, Address = @Address WHERE ProviderId = \'{id}\'";
                MyLog.Log($"Start changing provider with ProviderId [{id}] in SQL-base", LogLevel.Information);
                //int rows = connection.Execute(sqlQuery, newProvider);
                bool rows = connection.Update<Provider>(newProvider);
                if (rows)
                {
                    MyLog.Log($"Provider with ProviderId [{id}] was changed in SQL-base successfully", LogLevel.Information);
                    Console.WriteLine($"Provider with ProviderId [{id}] was changed successfully!");
                }
                else
                    MyLog.Log($"Provider with ProviderId [{id}] wasn`t changed in SQL-base", LogLevel.Warning);
            }
        }

        public void ChangeCategory(string name, Category newCategory)   //Выбор и изменение категории продукта по его имени
        {
            if (ReadCategory().Count > 1)
            {
                //MyLog.Log($"Start changing the product category with Name [{name}] in SQL-base", LogLevel.Information);
                var sqlQuery = $"UPDATE Category SET CategoryName = @CategoryName WHERE CategoryName = \'{name}\'";
                //int rows = connection.Execute(sqlQuery, newCategory);
                bool rows = connection.Update<Category>(newCategory);
                if (rows)
                {
                    MyLog.Log($"The product category with Name [{name}] was changed in SQL-base successfully", LogLevel.Information);
                    Console.WriteLine($"Category with Name [{name}] was changed successfully!");
                }
                else
                    MyLog.Log($"The product category with Name [{name}] wasn`t changed in SQL-base", LogLevel.Warning);
            }
        }

        public void ChangeProduct(string name, Product newProduct)      //Выбор и изменение продукта по его имени
        {
            if (ReadProduct().Count > 1)
            {
                MyLog.Log($"Start changing the product with Name [{name}] in SQL-base", LogLevel.Information);
                //var sqlQuery = $"UPDATE Product SET Name = @Name, CategoryId = @CategoryId, ProviderId = @ProviderId, Price = @Price WHERE Name = \'{name}\'";
                //int rows = connection.Execute(sqlQuery, newProduct);
                bool rows = connection.Update<Product>(newProduct);
                if (rows)
                {
                    MyLog.Log($"The product with Name [{name}] was changed in SQL-base successfully", LogLevel.Information);
                    Console.WriteLine($"Product with Name [{name}] was changed!");
                }
                else
                    MyLog.Log($"The product with Name [{name}] wasn`t changed in SQL-base", LogLevel.Warning);
            }
        }

        public void DeleteProvider (int id)                             //Удаление в базе поставщика продуктов с указанным id
        {
            List<Provider> providers = ReadProvider();
            MyLog.Log($"Checking provider with ProviderId [{id}] in SQL-base", LogLevel.Information);
            if (providers.Count > 1)
            {
                MyLog.Log($"Checking the product list for the presence of the corresponding provider with ProviderId [{id}] in SQL-base", LogLevel.Information);
                List<Product> products = ReadProduct();
                if (products.Count > 0 && products.Where(x => x.ProviderId == id).Any())        //Проверка наличия в продуктах привязанного провайдера с удаляемым Id
                {
                    MyLog.Log($"The products with ProviderId [{id}] in SQL-base was found", LogLevel.Information);
                    Console.WriteLine("Some products in the list have the provider which you want to delete! You want to delete those products with providers Yes/No?");
                    MyLog.Log($"The request to deleting products with ProviderId [{id}] in SQL-base was found", LogLevel.Information);
                    if (Console.ReadKey().Key == ConsoleKey.Y)
                    {
                        MyLog.Log($"The request to deleting products with ProviderId [{id}] in SQL-base was approved", LogLevel.Information);
                        foreach (var item in products)
                        {
                            if (item.ProviderId == id)
                                DeleteProduct(item);
                        }
                        //int rows = connection.Execute($"DELETE FROM [dbo].[Provider] WHERE ProviderId='{id}'");
                        bool rows = connection.Delete<Provider>(providers.First(x=>x.Id ==id));
                        if (rows)
                        {
                            MyLog.Log($"The provider with ProviderId [{id}] was deleted in SQL-base successfully", LogLevel.Information);
                            Console.WriteLine($"Provider with ProviderId [{id}] was deleted!");
                        }
                        else
                            MyLog.Log($"Provider with ProviderId [{id}] wasn`t deleted in SQL-base", LogLevel.Warning);
                    }
                    else
                    {
                        MyLog.Log($"The request to deleting products with ProviderId [{id}] in SQL-base was denied", LogLevel.Information);
                        Console.WriteLine($"DELETE operation has been canceled");
                    }
                }
                else
                {
                    //int rows = connection.Execute($"DELETE FROM [dbo].[Providers] WHERE Id='{id}'");
                    bool rows = connection.Delete<Provider>(providers.First(x => x.Id == id));
                    if (rows)
                    {
                        MyLog.Log($"Provider with ProviderId [{id}] was deleted in SQL-base successfully", LogLevel.Information);
                        Console.WriteLine($"Provider with ProviderId [{id}] was deleted!");
                    }
                    else
                        MyLog.Log($"Provider with ProviderId [{id}] wasn`t deleted in SQL-base", LogLevel.Warning);
                }
            }
            else
            {
                MyLog.Log($"The list of the roviders is empty", LogLevel.Information);
                Console.WriteLine($"The list of the roviders is empty!");
            }
        }

        public void DeleteCategory(int id)                         //Удаление в базе категории продуктов с именем name
        {
            List<Category> categories = ReadCategory();
            MyLog.Log($"Checking the category of product with Id [{id}] in SQL-base", LogLevel.Information);
            if (categories.Count > 1)
            {
                MyLog.Log($"Checking the list of the products for the presence of the corresponding category with Id [{id}] in SQL-base", LogLevel.Information);
                List<Product> products = ReadProduct();
                //int catId = categories.First(x => x.CategoryName.ToString().Equals(name.ToString())).CategoryId;
                if (products.Count > 0 && products.Where(x=>x.CategoryId==id).Any())             //Проверка наличия в продуктах привязанной категории с Id соответсвующим именем name
                {
                    MyLog.Log($"The products with CategoryId [{id}] in SQL-base was found", LogLevel.Information);
                    Console.WriteLine($"Some products in the list have the category which you want to delete! You want to delete those products with categoryId [{id}] Yes/No?");
                    MyLog.Log($"The request to deleting products with CategoryId [{id}] in SQL-base was found", LogLevel.Information);
                    if (Console.ReadKey().Key == ConsoleKey.Y)
                    {
                        MyLog.Log($"The request to deleting products with CategoryId [{id}] in SQL-base was approved", LogLevel.Information);
                        foreach (var item in products)
                        {
                            if (item.CategoryId==id)
                                DeleteProduct(item);
                        }
                        MyLog.Log($"Start deleting the category of product with CategoryId [{id}] in SQL-base", LogLevel.Information);
                        //int rows = connection.Execute($"DELETE FROM [dbo].[Category] WHERE CategoryId='{id}'");
                        bool rows = connection.Delete<Category>(categories.First(x => x.Id == id));
                        if (rows)
                        {
                            MyLog.Log($"The category of product with Id [{id}] was deleted in SQL-base successfully", LogLevel.Information);
                            Console.WriteLine($"The category of product with Id [{id}] was deleted!");
                        }
                        else
                            MyLog.Log($"The category of product with Id [{id}] wasn`t deleted in SQL-base", LogLevel.Warning);
                    }
                    else
                    {
                        MyLog.Log($"The request to deleting category of product with Id [{id}] in SQL-base was denied", LogLevel.Information);
                        Console.WriteLine($"DELETING operation has been canceled");
                    }
                }
                else
                {
                    //int rows = connection.Execute($"DELETE FROM [dbo].[Category] WHERE CategoryId='{id}'");
                    bool rows = connection.Delete<Category>(categories.First(x => x.Id == id));
                    if (rows)
                    {
                        MyLog.Log($"The category of product with Id [{id}] was deleted in SQL-base successfully", LogLevel.Information);
                        Console.WriteLine($"The category of product with Id [{id}] was deleted!");
                    }
                    else
                        MyLog.Log($"The category of product with Id [{id}] wasn`t deleted in SQL-base", LogLevel.Warning);
                }
            }
            else
            {
                MyLog.Log($"The list of the categories is empty", LogLevel.Information);
                Console.WriteLine($"The list of the categories is empty!");
            }
        }

        public void DeleteProduct(Product product)              //Удаление в базе продукта с полем Name по значению name
        {
            if (ReadProduct().Count > 1)
            {
                MyLog.Log($"Start deleting the product with Name [{product.Name}] in SQL-base", LogLevel.Information);
                //int rows = connection.Execute($"DELETE FROM [dbo].[Product] WHERE Name='{name}'");
                bool rows = connection.Delete<Product>(product);
                if (rows)
                {
                    MyLog.Log($"The product with Name [{product.Name}] was deleted in SQL-base successfully", LogLevel.Information);
                    Console.WriteLine($"The product with Name [{product.Name}] was deleted!");
                }
                else
                    MyLog.Log($"The product with Nmae [{product.Name}] wasn`t deleted in SQL-base", LogLevel.Warning);
            }
        }
        
        public void DeleteProduct(string name)              //Удаление в базе продукта с полем Name по значению name
        {
            if (ReadProduct().Count > 1)
            {
                MyLog.Log($"Start deleting the product with Name [{name}] in SQL-base", LogLevel.Information);
                int rows = connection.Execute($"DELETE FROM [dbo].[Product] WHERE Name='{name}'");
                if (rows > 0)
                {
                    MyLog.Log($"The product with Name [{name}] was deleted in SQL-base successfully", LogLevel.Information);
                    Console.WriteLine($"The product with Name [{name}] was deleted!");
                }
                else
                    MyLog.Log($"The product with Nmae [{name}] wasn`t deleted in SQL-base", LogLevel.Warning);
            }
        }

        public void DeleteProduct(int id)              //Удаление в базе продукта с полем Id по значению id
        {
            if (ReadProduct().Count > 1)
            {
                MyLog.Log($"Start deleting the product with id [{id}] in SQL-base", LogLevel.Information);
                int rows = connection.Execute($"DELETE FROM [dbo].[Product] WHERE Id='{id}'");
                
                if (rows > 0)
                {
                    MyLog.Log($"The product with Name id [{id}] was deleted in SQL-base successfully", LogLevel.Information);
                    Console.WriteLine($"The product with id [{id}] was deleted!");
                }
                else
                    MyLog.Log($"The product with id [{id}] wasn`t deleted in SQL-base", LogLevel.Warning);
            }
        }

        public void DeleteProduct(string col, string name)              //Удаление в базе продукта с полем col, котoрое имеет значение value типа string
        {
            if (ReadProduct().Count > 1)
            {
                MyLog.Log($"Start deleting the product with {col} [{name}] in SQL-base", LogLevel.Information);
                int rows = connection.Execute($"DELETE FROM [dbo].[Product] WHERE '{col}'='{name}'");
                if (rows > 0) 
                {
                    MyLog.Log($"The product with {col} [{name}] was deleted in SQL-base successfully", LogLevel.Information);
                    Console.WriteLine($"The product with {col} [{name}] was deleted!"); 
                }
                else
                    MyLog.Log($"The product with {col} [{name}] wasn`t deleted in SQL-base", LogLevel.Warning);
            }
        }

        public void DeleteProduct(string colWithId, int id)             //Удаление в базе продукта с полем colWithId, котoрое имеет значение id типа int
        {
            if (ReadProduct().Count > 1)
            {
                MyLog.Log($"Start deleting the product with {colWithId}=[{id}] in SQL-base", LogLevel.Information);
                int rows = connection.Execute($"DELETE FROM [dbo].[Product] WHERE '{colWithId}'='{id}'");
                if (rows > 0)
                {
                    MyLog.Log($"The product with {colWithId}=[{id}] was deleted in SQL-base successfully", LogLevel.Information);
                    Console.WriteLine($"The product with {colWithId}=[{id}] was deleted!");
                }
                else
                    MyLog.Log($"The product with {colWithId}=[{id}] wasn`t deleted in SQL-base", LogLevel.Warning);
            }
        }
    }
}
