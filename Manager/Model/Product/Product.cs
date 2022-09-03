using Dapper_BDSQL.Controller;
using Manager.Model;
using System.Collections.Generic;

namespace Dapper_BDSQL
{
    class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int CategoryId { get; set; }
        public int ProviderId { get; set; }
        public string Price { get; set; }

        public Product()
        {
            this.Id = 0;
            this.Name = string.Empty;
            this.CategoryId = 0;
            this.ProviderId = 0;
            this.Price = string.Empty;
        }
        public Product(int Id=0, string Name="", int CategoryId=0, int ProviderId=0, string Price="")
        {
            this.Id = Id;
            this.Name = Name;
            this.CategoryId = CategoryId;
            this.ProviderId = ProviderId;
            this.Price = Price;
        }

        public override string ToString()
        {
            return $"Name [{Name}], Category [{CategoryId}], Provider [{ProviderId}], Price [{Price}]";
        }

        public string ToString(List<Category> categories, List<Provider> providers)
        {
            return $"Name [{Name}], Category [{categories[CategoryId-1].CategoryName}], Provider [{providers[ProviderId-1].ProviderShortName}], Price [{Price}]";
        }

    }
}
