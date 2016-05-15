using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Soway.Service.Login.V1
{
    public enum ViewType
    {
        //新建视图
        NewCreate=0,
        //流式，朋友圈
        Flow =1,
        //有一张图片的
        ImageList=2,
        //List 无图片的
        List=3,
        //按钮列表
        ButtonList=4,
        //有图片的button列表
        ImagButtonList=5,
        //九宫格
        Table =6
    }
}