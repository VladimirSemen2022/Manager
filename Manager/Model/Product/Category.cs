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
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }

        public Category( int CategoryId=0, string CategoryName="")
        {
            this.CategoryId = CategoryId;
            this.CategoryName = CategoryName;
        }

        public override string ToString()
        {
            return $"Category Id [{CategoryId}] Name [{CategoryName}]";
        }
    }
}
