using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Newtonsoft.Json;
using PetaPocoDemo.Test.Entities;
using PetaPocoDemo.Test.Repositories;

namespace PetaPocoDemo.Test
{
    [TestFixture]
    public class PetaPocoMySqlTest
    {
        private readonly UserInfoSqlServerRepository _userInfoSqlServerRepository = new UserInfoSqlServerRepository();

        private readonly UserInfoMySqlRepository _userInfoMySqlRepository = new UserInfoMySqlRepository();

        [Test]
        public void MySql_Add_Test()
        {
            var userInfo = new UserInfo
                {
                    CreateDate = DateTime.Now,
                    UserName = "TestName3"
                };

            _userInfoMySqlRepository.Add(userInfo);
            var addUserInfo = _userInfoMySqlRepository.GetByUserName(userInfo.UserName);
            Console.WriteLine(JsonConvert.SerializeObject(addUserInfo));
        }

        [Test]
        public void MySql_FirstOrDefault_Test()
        {
            const string userName = "TestName";
            var userInfo = _userInfoMySqlRepository.GetByUserName(userName);

            if (userInfo != null)
            {
                Console.WriteLine(JsonConvert.SerializeObject(userInfo));
            }
        }

        [Test]
        public void MySql_Update_Test()
        {
            const string userName = "TestName";
            var userInfo = _userInfoMySqlRepository.GetByUserName(userName);

            if (userInfo != null)
            {
                Console.WriteLine("Before update:\r\n" + JsonConvert.SerializeObject(userInfo));

                userInfo.CreateDate = DateTime.Now.AddDays(1);
                _userInfoMySqlRepository.Update(userInfo);

                var updateAfterUserInfo = _userInfoMySqlRepository.GetByUserName(userName);
                Console.WriteLine("After update:\r\n" + JsonConvert.SerializeObject(updateAfterUserInfo));
            }
        }

        [Test]
        public void MySql_List_Test()
        {
            var userList = _userInfoMySqlRepository.GetAll();
            Console.WriteLine(JsonConvert.SerializeObject(userList));
        }

        [Test]
        public void MySql_Delete_Test()
        {
            const string userName = "TestName";

            _userInfoMySqlRepository.DeleteByUserName(userName);

            var list = _userInfoMySqlRepository.GetAll();
            Console.WriteLine(JsonConvert.SerializeObject(list));
        }

        [Test]
        public void MySql_PagedList_Test()
        {
            const int pageIndex = 1;
            const int pageSize = 10;
            var pagedList = _userInfoMySqlRepository.GetPagedList(pageIndex, pageSize);

            Console.WriteLine("总记录数:{0}，总页数:{1}，页大小：{2}，当前页：{3}"
                              , pagedList.TotalItemCount
                              , pagedList.TotalPageCount
                              , pagedList.PageSize
                              , pagedList.CurrentPageIndex);
            Console.WriteLine(JsonConvert.SerializeObject(pagedList));
        }
    }
}
