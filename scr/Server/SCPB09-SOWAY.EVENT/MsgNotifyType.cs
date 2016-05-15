using Soway.Data.Discription.ORM;

namespace Soway.Event
{
    public enum MsgNotifyType
    {
        [Enum("用户")]
        User=0,
        [Enum("角色")]
        Role = 1,
        [Enum("部门")]
        Dep = 2,
        [Enum ("公司")]
        Company = 3,
        [Enum("权限")]
        Auth=4,
        [Enum("所有")]
        All =5
    }
}