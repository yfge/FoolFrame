using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Soway.Model
{
    public enum ModelTriggerType
    {
        Create = 0,
        Save = 1,
        Delete = 2,
        BeforeCreate=3,
        BeforeSave=4,
        BeforeDelete=5
    }
}
