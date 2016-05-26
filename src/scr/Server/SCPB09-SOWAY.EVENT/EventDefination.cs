using Soway.Model;
using Soway.Model.View;
using SOWAY.ORM.AUTH;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Soway.Data.Discription.ORM;

namespace Soway.Event
{
    [Table(ColPreStr ="EVTDEF_",Name ="SW_EVT_DEF")]
    public class EventDefination
    {


        [Column(ColumnName ="ID",IsIdentify =true,IsAutoGenerate = GenerationType.OnInSert)]
        public Guid DefId { get; set; }

        /// <summary>
        /// 条件
        /// </summary>
        /// 
        [Column(ColumnName ="FILTER")]
        public string Filter
        {
            get; set;
        }

        /// <summary>
        /// 通知的部门
        /// </summary>
        /// 
        [MultiType]
        public List<Department> NotifyDeps
        {
            get;set;
        }

        /// <summary>
        /// 通知的角色
        /// </summary>
        /// 
        [MultiType]
        public List<Role> NotifyRoles
        {

            get;set;
        }

        /// <summary>
        /// 通知的用户
        /// </summary>
        /// 
        [MultiType]
        public List<SOWAY.ORM.AUTH.AuthorizedUser> NotifyUsers
        {
            get;set;
        }

        /// <summary>
        /// 视图
        /// </summary>
        /// 
        [Column(ColumnName ="VIEW")]
        public View View 
        {
            get;set;
        }

        /// <summary>
        /// 默认操作
        /// </summary>
        /// 
        [Column(ColumnName ="OPERATION")]
        public  Operation Operation 
        {
            get;set;
        }

        /// <summary>
        /// 通知格式
        /// </summary>
        /// 
        [Column(ColumnName ="MSGFMT")]
        public String MsgFmt
        {
            get;set;
        }

        /// <summary>
        /// 超时时间
        /// </summary>
        /// 
        [Column(ColumnName ="TIMEOUTSECS")]
        public int TimeOutSeconds
        {
            get;set;
        }

        /// <summary>
        /// 监听的模型
        /// </summary>
        /// 
        [Column(ColumnName ="MODEL")]
        public Model.Model DefModel
        {
            get;set;
        }
        [Column(ColumnName ="MODELREF")]
        public Soway.Model.ModelRefType ModelType
        {
            get;set;
        }

        [MultiType]
        public List<Company> NotifyCompanies { get; internal set; }

        [Column(ColumnName ="STATE")]
        public EventState State { get; set; }
    }
}