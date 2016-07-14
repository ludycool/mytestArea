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
    public class PetaPocoSqlServer
    {
        private readonly UserInfoSqlServerRepository _userInfoSqlServerRepository = new UserInfoSqlServerRepository();


        [Test]
        public void SqlServer_Add_Test()
        {
            var userInfo = new UserInfo
                {
                    CreateDate = DateTime.Now,
                    UserName = "TestUser"
                };

            _userInfoSqlServerRepository.Add(userInfo);
            var addUserInfo = _userInfoSqlServerRepository.GetByUserName(userInfo.UserName);
            Console.WriteLine(JsonConvert.SerializeObject(addUserInfo));
        }

        [Test]
        public void SqlServer_FirstOrDefault_Test()
        {
            const string userName = "TestName";
            var userInfo = _userInfoSqlServerRepository.GetByUserName(userName);

            if (userInfo != null)
            {
                Console.WriteLine(JsonConvert.SerializeObject(userInfo));
            }
        }

        [Test]
        public void SqlServer_Update_Test()
        {
            const string userName = "TestName";
            var userInfo = _userInfoSqlServerRepository.GetByUserName(userName);

            if (userInfo != null)
            {
                Console.WriteLine("Before update:\r\n" + JsonConvert.SerializeObject(userInfo));

                userInfo.CreateDate = DateTime.Now.AddDays(1);
                _userInfoSqlServerRepository.Update(userInfo);

                var updateAfterUserInfo = _userInfoSqlServerRepository.GetByUserName(userName);
                Console.WriteLine("After update:\r\n" + JsonConvert.SerializeObject(updateAfterUserInfo));
            }
        }

        [Test]
        public void SqlServer_List_Test()
        {
            var userList = _userInfoSqlServerRepository.GetAll();
            Console.WriteLine(JsonConvert.SerializeObject(userList));
        }

        [Test]
        public void SqlServer_Delete_Test()
        {
            const string userName = "TestName";

            _userInfoSqlServerRepository.DeleteByUserName(userName);

            var list = _userInfoSqlServerRepository.GetAll();
            Console.WriteLine(JsonConvert.SerializeObject(list));
        }

        [Test]
        public void SqlServer_PagedList_Test()
        {
            const int pageIndex = 1;
            const int pageSize = 10;
            var pagedList = _userInfoSqlServerRepository.GetPagedList(pageIndex, pageSize);

            Console.WriteLine("总记录数:{0}，总页数:{1}，页大小：{2}，当前页：{3}"
                              , pagedList.TotalItemCount
                              , pagedList.TotalPageCount
                              , pagedList.PageSize
                              , pagedList.CurrentPageIndex);
            Console.WriteLine(JsonConvert.SerializeObject(pagedList));
        }
    }
}
