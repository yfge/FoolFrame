using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Soway.Data
{
    public interface IController
    {
        Object Get(object id );
        Array[] GetList(int page, int count);
        void Create(object ob);
        void Update(object ob);
        void Delete(object ob);

         
    }
}
