using Soway.DB.Manage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SOWAY.ORM.AUTH
{
    public class UserFactory
    {
        private Application app;

        public UserFactory(Application app)
        {

            this.app = app;

        }
        public List<User> GetUsers()
        {
            return new List<User>();
        }
    }
}