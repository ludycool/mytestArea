using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PetaPocoDemo.ORM;

namespace PetaPocoDemo.Test.Entities
{
    [TableName("Product")]
    [PrimaryKey("ProductId")]
    public class Product
    {
        public int ProductId { get; set; }

        public string ProductName { get; set; }
    }
}
