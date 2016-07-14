using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PetaPoco;

namespace PetaPocoDemo.Test.Entities
{
    [TableName("UserInfo")]
    [PrimaryKey("UserId")]
    public class UserInfo
    {
        public int UserId { get; set; }

        public string UserName { get; set; }

        public DateTime CreateDate { get; set; }
    }
}
