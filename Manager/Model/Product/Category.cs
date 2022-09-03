using Dapper_BDSQL.Controller;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manager.Model
{
    class Category
    {
        public int Id { get; set; } = 0;
        //public int CategoryId { get; set; }
        public string CategoryName { get; set; }

        public Category()
        {
            this.Id = 0;
            //this.CategoryId = 0;
            this.CategoryName = string.Empty;
        }

        public Category(int Id, string CategoryName)
        {
            this.Id = Id;
            //this.CategoryId = CategoryId;
            this.CategoryName = CategoryName;
        }

        public override string ToString()
        {
            return $"Category Id [{Id}] Name [{CategoryName}]";
        }
    }
}
