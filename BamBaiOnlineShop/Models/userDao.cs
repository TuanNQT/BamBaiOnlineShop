using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BamBaiOnlineShop.Models;
namespace BamBaiOnlineShop.Models
{
    public class userDao
    {
        quangtuanEntities db = null;
        public userDao()
        {
            db = new quangtuanEntities();
        }
        public long Insert(Customer entity)
        {
            db.Customers.Add(entity);
            db.SaveChanges();
            return 2;
        }
        public Customer GetById(string email)
        {
            return db.Customers.SingleOrDefault(x => x.EmailAddress == email);
        }
        public bool Login(string UserName, string PassWord)
        {

            var res = db.Customers.Count(x =>x.EmailAddress == UserName && x.Password == PassWord);

            if (res > 0)
            {

                return true;
            }
            else
            {
                return false;
            }
        }

    }
}