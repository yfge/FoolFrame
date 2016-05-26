using System;
using System.Collections.Generic;
namespace Soway.Model
{
   public interface IObjectProxy
    {
        System.Collections.Generic.IEnumerable<string> GetDynamicMemberNames();



        /// <summary>
        /// 旧ID
        /// </summary>
        object OldId { get;  }
        /// <summary>
        /// ID
        /// </summary>
        object ID { get; set; }
        /// <summary>
        /// 是否已经加载
        /// </summary>
        LoadType IsLoad { get; set; }
        /// <summary>
        /// 模型定义
        /// </summary>
        Model Model { get; set; }
        event EventHandler<ObjectPropertyCanSetChanged> OjbectPropertyCanSetChangedEventHandler;
        /// <summary>
        /// 父对象
        /// </summary>
        IObjectProxy Owner { get; set; }
        event EventHandler<ObjectValueChangedEventArgs> PropertyChangedEventHandler;
        object this[Property index] { get; set; }
        object this[string exp] { get; set; }


        object GetOld(String exp);
        object GetOld(Property exp);
     
     //   object GetOldValue(string exp);
        string ToString();

        void NotifyPropertyCanSet(Property proerty, bool canSet);


        /// <summary>
        /// 是否已经保存
        /// </summary>
          SaveType IsSave { get; set; }
          void UpdateToNew(Property exp);

        LoadType GetLoadType(Property exp);
    }
}
