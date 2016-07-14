using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PetaPocoDemo.Test.Entities;
using PetaPoco;
using PetaPocoDemo.ORM;

namespace PetaPocoDemo.Test.Repositories
{
    [SingleDbFactory("SqlServerDB")]
    public class UserInfoSqlServerRepository : SingleRepository<UserInfo>
    {
        public UserInfo GetByUserName(string userName)
        {
            var sql = Sql.Builder.Where("UserName = @0", userName);
            return FirstOrDefault(sql);
        }

        public IEnumerable<UserInfo> GetAll()
        {
            var sql = Sql.Builder.Where("1=1");
            return Query(sql);
        }

        public PagedList<UserInfo> GetPagedList(int pageIndex, int pageSize)
        {
            var sql = Sql.Builder.Where("1=1");
            return PagedList<UserInfo>(pageIndex, pageSize, sql);
        }

        public void DeleteByUserId(int userId)
        {
            var sql = Sql.Builder.Where("UserId = @0", userId);
            Delete(sql);
        }

        public void DeleteByUserName(string userName)
        {
            var sql = Sql.Builder.Where("UserName = @0", userName);
            Delete(sql);
        }
    }
}
