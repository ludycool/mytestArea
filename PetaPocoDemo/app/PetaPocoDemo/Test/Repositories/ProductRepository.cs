using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PetaPocoDemo.ORM;
using PetaPocoDemo.Test.Entities;

namespace PetaPocoDemo.Test.Repositories
{
    [SingleDbFactory("SqlServerDB")]
    public class ProductRepository :SingleRepository<Product>
    {
    }
}
