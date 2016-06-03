using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Soway.Model;
using Soway.Model.Context;

namespace SOWAY.ORM.AUTH
{
    public class MenuItemFactory
    {
        public MenuItemFactory(Soway.Model.SqlCon con,Soway.Model.Context.ICurrentContextFactory conFac)
        {
            this.Con = con;
            this.ConFac = conFac;

        }

    
        public List<MenuItem> GetTopMenus(SOWAY.ORM.AUTH.AuthorizedUser user)
        {


            List<MenuItem> temp = new List<MenuItem>();
            List<MenuItem> result = new List<MenuItem>();
            foreach(var role in user.Roles)
            {
                foreach(var item in role.Items)
                {
                    
                    if(temp.Count(p=>p.ID == item.ID) == 0)
                    {
                        temp.Add(item);
                    }
                        
                }
            }
            foreach(var item in temp.OrderBy(p=>p.Index))
            {
                if (temp.Count(p => p.SubItems.Count(q => q.ID == item.ID) > 0)==0)

                    result.Add(item);
            }
            return result;
        }
        public List<MenuItem > GetMenus(SOWAY.ORM.AUTH.AuthorizedUser user,long topid)
        {

            List<MenuItem> temp = new List<MenuItem>();
            List<MenuItem> result = new List<MenuItem>();
            foreach (var role in user.Roles)
            {
                foreach (var item in role.Items)
                {
                    if (temp.Count(p => p.ID == item.ID) == 0)
                    {
                        temp.Add(item);
                    }

                }
            }
            foreach (var item in temp.OrderBy(p=>p.Index))
            {

                if (temp.Count(p => p.ID == topid && p.SubItems.Count(q => q.ID == item.ID) > 0) > 0)
                    result.Add(item);
            }
          
            return result;
        }

        public SqlCon Con { get; private set; }
        public ICurrentContextFactory ConFac { get; private set; }
    }
}