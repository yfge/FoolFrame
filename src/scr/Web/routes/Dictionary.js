//var dic = new Dictionary();
//dic.Add('11', '1');
//if (dic.ContainsKey('22')) {
//    alert(dic.Item('22'));
//}
//dic.Clear();


// JScript 文件
/*
作者：无涯 2007年3月27日 xrwang@126.com
许可：在保留作者信息的前提下，本文件可以随意修改、传播、使用，但对可能由此造成的损失作者不负担任何责任。
Dictionary类：本类实现了字典功能，所有方法、属性都模仿System..Collection.Generic.Dictionary类
构造函数：
	Dictionary()
属性：
	CompareMode：比较模式，0——二进制   1——文本
	Count：字典中的项目数
	ThrowException：遇到错误时，是否抛出异常
方法：
	Item(key)：获取指定键对应的值
	Keys()：获取键数组
	Values()：获取值数组
	Add(key,value)：将指定的键和值添加到字典中
	BatchAdd(keys,values)：尝试将指定的键和值数组添加到字典中，如果全部添加成功，返回true；否则返回false。
	Clear()：清除字典中的所有项
	ContainsKey(key)：字典中是否包含指定的键
	ContainsValue(value)：字典中是否包含指定的值
	Remove(key)：删除字典中指定的键
	TryGetValue(key,defaultValue)：尝试获取字典中指定键对应的值，如果键不存在，返回默认值
	ToString()：返回字典中所有键和值组成的字符串，格式为“逗号分隔的键列表  分号  逗号分隔的值列表”
*/

function Dictionary() {
    var me = this;            //将this指针保存到变量me中
    
    this.CompareMode = 1;        //比较关键字是否相等的模式，0——二进制；1——文本
    
    this.Count = 0;            //字典中的项目数
    
    this.arrKeys = new Array();    //关键字数组
    
    this.arrValues = new Array();    //值数组
    
    this.ThrowException = true;    //遇到错误时，是否用throw语句抛出异常
    
    this.Item = function (key)        //Item方法，获取指定键对应的值。如果键不存在，引发异常
    {
        var idx = GetElementIndexInArray(me.arrKeys, key);
        if (idx != -1) {
            return me.arrValues[idx];
        }
        else {
            if (me.ThrowException)
                throw "在获取键对应的值时发生错误，键不存在。";
        }
    }
    
    this.Keys = function ()        //获取包含所有键的数组
    {
        return me.arrKeys;
    }
    
    this.Values = function ()        //获取包含所有值的数组
    {
        return me.arrValues;
    }
    
    this.Add = function (key, value)    //将指定的键和值添加到字典中
    {
        if (CheckKey(key)) {
            me.arrKeys[me.Count] = key;
            me.arrValues[me.Count] = value;
            me.Count++;
        }
        else {
            if (me.ThrowException)
                throw "在将键和值添加到字典时发生错误，可能是键无效或者键已经存在。";
        }
    }
    
    this.BatchAdd = function (keys, values)        //批量增加键和值数组项，如果成功，增加所有的项，返回true；否则，不增加任何项，返回false。
    {
        var bSuccessed = false;
        if (keys != null && keys != undefined && values != null && values != undefined) {
            if (keys.length == values.length && keys.length > 0)    //键和值数组的元素数目必须相同
            {
                var allKeys = me.arrKeys.concat(keys);    //组合字典中原有的键和新键到一个新数组
                if (!IsArrayElementRepeat(allKeys))    //检验新数组是否存在重复的键
                {
                    me.arrKeys = allKeys;
                    me.arrValues = me.arrValues.concat(values);
                    me.Count = me.arrKeys.length;
                    bSuccessed = true;
                }
            }
        }
        return bSuccessed;
    }
    
    this.Clear = function ()            //清除字典中的所有键和值
    {
        if (me.Count != 0) {
            me.arrKeys.splice(0, me.Count);
            me.arrValues.splice(0, me.Count);
            me.Count = 0;
        }
    }
    
    this.ContainsKey = function (key)    //确定字典中是否包含指定的键
    {
        return GetElementIndexInArray(me.arrKeys, key) != -1;
    }
    
    this.ContainsValue = function (value)    //确定字典中是否包含指定的值
    {
        return GetElementIndexInArray(me.arrValues, value) != -1;
    }
    
    this.Remove = function (key)        //从字典中移除指定键的值
    {
        var idx = GetElementIndexInArray(me.arrKeys, key);
        if (idx != -1) {
            me.arrKeys.splice(idx, 1);
            me.arrValues.splice(idx, 1);
            me.Count--;
            return true;
        }
        else
            return false;
    }
    
    this.TryGetValue = function (key, defaultValue)    //尝试从字典中获取指定键对应的值，如果指定键不存在，返回默认值defaultValue
    {
        var idx = GetElementIndexInArray(me.arrKeys, key);
        if (idx != -1) {
            return me.arrValues[idx];
        }
        else
            return defaultValue;
    }
    
    this.ToString = function ()        //返回字典的字符串值，排列为： 逗号分隔的键列表  分号  逗号分隔的值列表
    {
        if (me.Count == 0)
            return "";
        else
            return me.arrKeys.toString() + ";" + me.arrValues.toString();
    }
    
    function CheckKey(key)            //检查key是否合格，是否与已有的键重复
    {
        if (key == null || key == undefined || key == "" || key == NaN)
            return false;
        return !me.ContainsKey(key);
    }
    
    function GetElementIndexInArray(arr, e)    //得到指定元素在数组中的索引，如果元素存在于数组中，返回所处的索引；否则返回-1。
    {
        var idx = -1;    //得到的索引
        var i;        //用于循环的变量
        if (!(arr == null || arr == undefined || typeof (arr) != "object")) {
            try {
                for (i = 0; i < arr.length; i++) {
                    var bEqual;
                    if (me.CompareMode == 0)
                        bEqual = (arr[i] === e);    //二进制比较
                    else
                        bEqual = (arr[i] == e);        //文本比较
                    if (bEqual) {
                        idx = i;
                        break;
                    }
                }
            }
            catch (err) {
            }
        }
        return idx;
    }
    
    function IsArrayElementRepeat(arr)    //判断一个数组中的元素是否存在重复的情况，如果存在重复的元素，返回true，否则返回false。
    {
        var bRepeat = false;
        if (arr != null && arr != undefined && typeof (arr) == "object") {
            var i;
            for (i = 0; i < arr.length - 1; i++) {
                var bEqual;
                if (me.CompareMode == 0)
                    bEqual = (arr[i] === arr[i + 1]);    //二进制比较
                else
                    bEqual = (arr[i] == arr[i + 1]);        //文本比较
                if (bEqual) {
                    bRepeat = true;
                    break;
                }
            }
        }
        return bRepeat;
    }
}