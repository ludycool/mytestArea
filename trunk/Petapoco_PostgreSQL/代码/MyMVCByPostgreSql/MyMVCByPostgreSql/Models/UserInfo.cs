using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PetaPoco
{
    [TableName("userinfo")]
    [PrimaryKey("id")]
    [ExplicitColumns]
    public class UserInfo
    {
        [Column("id")]
        public int Id { get; set; }

        [Column("name")]
        public string Name { get; set; }

        [Column("age")]
        public int Age { get; set; }

        [Column("qq")]
        public int Qq { get; set; }
    }
}