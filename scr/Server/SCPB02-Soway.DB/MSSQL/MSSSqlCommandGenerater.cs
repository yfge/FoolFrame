using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Data.SqlClient;
using Soway.Data.Discription.ORM;
using System.Reflection;
using System.Linq;

namespace Soway.DB.MSSQL
{
    class MSSSqlCommandGenerater :IDbCommandGenerater 
    {

        private static Hashtable insertCommand;
        private static Hashtable selectedCommand;
        private static Hashtable selectedItemsCommad;
        private static Hashtable updateCommand;
        private static Hashtable deleteCommand;
        private static ORMHelper helper;
        private static string tableStr = "";
       
 
        static MSSSqlCommandGenerater()
        {
            insertCommand = new Hashtable();
            SeletctAutoGernertateCommands = new Hashtable();
            selectedCommand = new Hashtable();
            selectedItemsCommad = new Hashtable();
            updateCommand = new Hashtable();
            deleteCommand = new Hashtable();
            helper = new ORMHelper();

        }
         


        
        public System.Data.IDbCommand GenerateCreateSql<T>(T ob,IDBContext cxt)
        { 
            return GenerateCreateSql(ob, cxt, typeof(T));
        }
        public System.Data.IDbCommand GenerateCreateSql(object ob, IDBContext cxt)
        {
            return GenerateCreateSql(ob, cxt, ob.GetType());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ob"></param>
        /// <param name="cxt"></param>
        /// <param name="TargetType"></param>
        /// <returns></returns>
        /// <remarks>
        /// 2013.8.7
        /// 将MSSQLScriptSqlPatternFactory改为可以得到列信息和名称信息
        /// </remarks>
        private  System.Data.IDbCommand GenerateCreateSql(object ob, IDBContext cxt, Type TargetType)
        {
            System.Data.SqlClient.SqlCommand command = null;

            var patternFactory = new MSSQLScriptSqlPatternFactory();
            if (insertCommand.ContainsKey(TargetType) == false)
            {
                //得到表名
                command = new System.Data.SqlClient.SqlCommand();
                String TableName = helper.GetTable(TargetType);
                var Properties = TargetType.GetProperties();
                String Cols = "";
                String Values = "";
                for (int i = 0; i < Properties.Length; i++)
                {
                    if (Properties[i].PropertyType.IsArray)
                        continue;
                    var attribute = helper.GetColNameAttributes(Properties[i]);
                    String paramName = "@" + Properties[i].Name;
                    string ColName = "";
                    if (attribute.Count > 0)
                    {
                        for (int k = 0; k < attribute.Count; k++)
                        {
                            var Col =attribute[k];
                            if (Col.NoMap == true)
                            {
                                continue;
                            }
                            else if (Col.IsAutoGenerate == GenerationType.OnInSert)
                            {
                                var auto = Properties[i].GetCustomAttributes(typeof(SerialNoObject), true);
                                if (auto.Length > 0)
                                {
                                    var seriSetting = auto[0] as SerialNoObject;
                                    Properties[i].SetValue(ob,
                                        cxt.GetSerialNo(seriSetting.SerialPre, seriSetting.Len, seriSetting.DateFormateStr,""), null);
                                }
                                else
                                    continue;
                            }

                            ColName = helper.GetColName(Col, Properties[i]);
                            paramName = "@" + ColName;
                            var valueObject = new ORMHelper().GetDbObject(Properties[i], ob,Col.PropertyName);
                            if (valueObject is DateTime &&valueObject != null && String.IsNullOrEmpty(Col.FormatStr) == false)
                                valueObject = ((DateTime)valueObject).ToString(Col.FormatStr);
                            Cols += "[" + ColName + "],";
                            Values += paramName + ",";
                            command.Parameters.Add(new System.Data.SqlClient.SqlParameter(paramName, valueObject ?? ""));
                        }
                    }
                  
                  
                  
                }
                command.CommandText = "INSERT " + TableName + "(" + Cols.Substring(0, Cols.Length - 1) + ") VALUES(" + Values.Substring(0, Values.Length - 1) + ")";
                insertCommand.Add(TargetType, command);
                //return command;
            }
          
            command =
                   (insertCommand[TargetType] as System.Data.SqlClient.SqlCommand).Clone();
            SetValues(ob, command, TargetType, "",true,cxt);
            return command;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ob"></param>
        /// <returns></returns>
        /// <remarks>
        /// 2013.8.7</remarks>
        public System.Data.IDbCommand GenerateSaveSql<T>(T ob)
        {

            if (!updateCommand.ContainsKey(typeof(T)))
            {
                //初始化一个Command 
                System.Data.SqlClient.SqlCommand command = new System.Data.SqlClient.SqlCommand();
                var TargetType = typeof(T);
                //得到表名
                var factory = new MSSQLScriptSqlPatternFactory();//.GetTable(TargetType);
                var table = helper.GetTableAttribute(TargetType);
                String TableName = helper.GetTable(TargetType);
                var Properties = TargetType.GetProperties();
                String Cols = "";


                for (int i = 0; i < Properties.Length; i++)
                {
                    var attribute = helper.GetColNameAttributes(Properties[i]);
                   
                    string ColName = "";
                    if (attribute.Count > 0)
                    {
                        for (int j = 0; j < attribute.Count; j++)
                        {
                            
                            var Col = (ColumnAttribute)attribute[j];
                            if (Col.IsAutoGenerate != GenerationType.Never || Col.NoMap == true||Col.IsIdentify)
                                continue; //如果自动生成
                            else
                                ColName = helper.GetColName(Col, Properties[i]);
                            String paramName ="@"+ ColName;
                           // ColName = factory.GetColName(Col, Properties[i]);
                            var dbValue = new ORMHelper().GetDbObject(Properties[i], ob, Col.PropertyName);
                      //      if (dbValue != null)
                          //  {
                                Cols += "[" + ColName + "]=" + paramName + ",";
                                command.Parameters.Add(new System.Data.SqlClient.SqlParameter(paramName, dbValue??""));
                             
                        }
                    }
                }

                var parity = new MSSQLScriptSqlPatternFactory().GetExistSql<T>(Properties, ob, true, "Pre");
                command.Parameters.AddRange(parity.Params.ToArray());
                command.CommandText = "Update " + TableName + " SET " + Cols.Substring(0, Cols.Length - 1) + " WHERE " + parity.SqlScript;
                updateCommand.Add(typeof(T), command);
            }

            var upatecommand = (updateCommand[typeof(T)] as SqlCommand).Clone();
            SetValues<T>(ob, upatecommand);
            SetValues<T>(ob, upatecommand, "Pre");
            // + " SET " + Cols.Substring(0, Cols.Length - 1) + " WHERE " + parity.SqlScript;
            return upatecommand;

        }
        public System.Data.IDbCommand GenerateDeleteSql<T>(T ob)
        {
            var TargetType = typeof(T);
            if (deleteCommand.ContainsKey(TargetType) == false)
            {
                String TableName = helper.GetTable(TargetType);
                var Properties = TargetType.GetProperties();
                MSSQLScriptSqlPatternFactory factory = new MSSQLScriptSqlPatternFactory();
                System.Data.SqlClient.SqlCommand command = new System.Data.SqlClient.SqlCommand();
                String sql = "DELETE  " + TableName + " Where  ";
                var parity = factory.GetExistSql<T>(Properties, ob);
                command.Parameters.AddRange(parity.Params.ToArray());
                command.CommandText = sql + parity.SqlScript;
                deleteCommand.Add(TargetType, command);

            }
            var deletedcommand = (deleteCommand[TargetType] as SqlCommand).Clone();
            SetValues<T>(ob, deletedcommand);
            return deletedcommand;
        }

        public System.Data.IDbCommand GenerateCreateTableSql<T>()
        {

            return null;
            

          

        }

        public System.Data.IDbCommand GenerateGetItem<T>(object key)
        { //初始化一个Command 
            System.Data.SqlClient.SqlCommand command = null;
            var TargetType = typeof(T);


            bool HaveKey = false;

           
                command = new System.Data.SqlClient.SqlCommand();
                var factory = new MSSQLScriptSqlPatternFactory();
                String TableName = helper.GetTable(TargetType);
                var Properties = TargetType.GetProperties();
                String Cols = "";

                String SelectedSql = "SELECT ";

                String WhereSql = " WHERE ";

              
                for (int i = 0; i < Properties.Length; i++)
                {
                    var attribute = helper.GetColNameAttributes(Properties[i]);
                    for(int aI=0;aI<attribute.Count;aI++){
                        var Col=attribute[aI];
                       
                   
                        if (Col.NoMap)
                            continue;
                

                        

                        var ColName = helper.GetColName(Col, Properties[i]);
                        String paramName = "@" + ColName;
                          if (String.IsNullOrEmpty(ColName))
                                        continue;
                        if (Col.IsKey && String.IsNullOrEmpty(Col.KeyGroupName))
                        {
                            
                            if(HaveKey ==false )
                                HaveKey =true;
                            else 
                                throw new Exception("没有默认的主键!");
                            command.Parameters.Add(new SqlParameter(paramName,key));
                            WhereSql += "["+ColName +"]="+paramName;
                        
                            

                        }
                         Cols += TableName + ".[" + ColName + "],";
                }
                     


                   


                   

                   
                }

                command.CommandText = "SELECT " + Cols.Substring(0, Cols.Length - 1) + " FROM " + TableName + WhereSql ;//+ "(" + Cols.Substring(0, Cols.Length - 1) + ") VALUES(" + Values.Substring(0, Values.Length - 1) + ")";
               // selectedCommand.Add(TargetType, command);
                return command;
         
        }
        public System.Data.IDbCommand GenerateGetPreKey<T>(object key)
        { //初始化一个Command 
            System.Data.SqlClient.SqlCommand command = null;
            var TargetType = typeof(T);


            bool HaveKey = false;


            command = new System.Data.SqlClient.SqlCommand();
            var factory = new MSSQLScriptSqlPatternFactory();
            String TableName = helper.GetTable(TargetType);
            var Properties = TargetType.GetProperties();
            String Cols = "";

            String SelectedSql = "SELECT ";

            String WhereSql = " WHERE ";


            for (int i = 0; i < Properties.Length; i++)
            {
                var attribute = helper.GetColNameAttributes(Properties[i]);
                for (int aI = 0; aI < attribute.Count; aI++)
                {
                    var Col = attribute[aI];
                    var ColName = helper.GetColName(Col, Properties[i]);
                    String paramName = "@" + ColName;


                    if (Col.NoMap)
                        continue;


                    if (String.IsNullOrEmpty(ColName))
                        continue;


                    if (Col.IsKey && String.IsNullOrEmpty(Col.KeyGroupName))
                    {

                        if (HaveKey == false)
                            HaveKey = true;
                        else
                            throw new Exception("没有默认的主键!");
                        command.Parameters.Add(new SqlParameter(paramName,Properties[i].GetValue( key,new object[]{})));
                        WhereSql += "[" + ColName + "]<" + paramName + " Order by " + ColName + " DESC";
                        Cols = TableName + ".[" + ColName + "],";
                        break;
                    }
                    Cols += TableName + ".[" + ColName + "],";
                }
                if (HaveKey)
                    break;
            }

            command.CommandText = "SELECT Top 1 " + Cols.Substring(0, Cols.Length - 1) + " FROM " + TableName + WhereSql;//+ "(" + Cols.Substring(0, Cols.Length - 1) + ") VALUES(" + Values.Substring(0, Values.Length - 1) + ")";
            // selectedCommand.Add(TargetType, command);
            return command;

        }
        public System.Data.IDbCommand GenerateGetNextKey<T>(object key)
        { //初始化一个Command 
            System.Data.SqlClient.SqlCommand command = null;
            var TargetType = typeof(T);


            bool HaveKey = false;


            command = new System.Data.SqlClient.SqlCommand();
            var factory = new MSSQLScriptSqlPatternFactory();
            String TableName = helper.GetTable(TargetType);
            var Properties = TargetType.GetProperties();
            String Cols = "";

            String SelectedSql = "SELECT ";

            String WhereSql = " WHERE ";


            for (int i = 0; i < Properties.Length; i++)
            {
                var attribute = helper.GetColNameAttributes(Properties[i]);
                for (int aI = 0; aI < attribute.Count; aI++)
                {
                    var Col = attribute[aI];
                    var ColName = helper.GetColName(Col, Properties[i]);
                    String paramName = "@" + ColName;


                    if (Col.NoMap)
                        continue;


                    if (String.IsNullOrEmpty(ColName))
                        continue;


                    if (Col.IsKey && String.IsNullOrEmpty(Col.KeyGroupName))
                    {

                        if (HaveKey == false)
                            HaveKey = true;
                        else
                            throw new Exception("没有默认的主键!");
                        command.Parameters.Add(new SqlParameter(paramName, Properties[i].GetValue(key, new object[] { })));
                        WhereSql += "[" + ColName + "]>" + paramName + " Order by " + ColName + " ASC";
                        Cols = TableName + ".[" + ColName + "],";
                        break;
                    }
                    Cols += TableName + ".[" + ColName + "],";
                }
                if (HaveKey)
                    break;
            }

            command.CommandText = "SELECT Top 1 " + Cols.Substring(0, Cols.Length - 1) + " FROM " + TableName + WhereSql;//+ "(" + Cols.Substring(0, Cols.Length - 1) + ") VALUES(" + Values.Substring(0, Values.Length - 1) + ")";
            // selectedCommand.Add(TargetType, command);
            return command;

        }

        private static Hashtable SeletctAutoGernertateCommands;
        public System.Data.IDbCommand GenerateSeletedAutoGernerateSql<T>(T ob)
        {


            //初始化一个Command 
            System.Data.SqlClient.SqlCommand command = null;
            bool IsAuto = false;
            var TargetType = typeof(T);

            if (SeletctAutoGernertateCommands.ContainsKey(TargetType) == false)
            {
                //得到表名
                command = new System.Data.SqlClient.SqlCommand();
                var factory = new MSSQLScriptSqlPatternFactory();
                String TableName = helper.GetTable(TargetType);
                var Properties = TargetType.GetProperties();
                String Cols = null ;
                String Values = "";
                String SqlInsert = "SELECT " + TableName + "(" + Cols + ") VALUES(";
                String Condition = null;
                for (int i = 0; i < Properties.Length; i++)
                {
                    if (Properties[i].PropertyType.IsArray)
                        continue;



                    var attributes = helper.GetColNameAttributes(Properties[i]);//.GetCustomAttributes(typeof(ColumnAttribute), e);

                    foreach (var attr in attributes)
                    {


                        if (attr.NoMap)
                            continue;
                        String paramName ="@"+ helper.GetColName(attr, Properties[i]);
                        string ColName = helper.GetColName(attr, Properties[i]);
                        bool IsSelected = false;

                        IsSelected = attr.IsAutoGenerate != GenerationType.Never;





                        if (IsSelected)
                        {

                            if (Properties[i].GetCustomAttributes(typeof(SerialNoObject), true).Length
                                == 0)
                            {

                                if (Cols != null)
                                    Cols += ",";
                                Cols += ColName;

                                IsAuto = true;
                            }
                        }
                        else
                        {
                            if (Condition != null)
                                Condition += " AND ";
                            Condition += "("+ColName + "=" + paramName+" OR "+ paramName +" IS NULL )";
                            command.Parameters.Add(new System.Data.SqlClient.SqlParameter(paramName, Properties[i].GetValue(ob, null) ?? ""));
                        }
                    }
                }
                if (IsAuto)
                    command.CommandText = "SELECT  " + Cols + " FROM " + TableName + " WHERE " + Condition;
                else
                    command = null;
                SeletctAutoGernertateCommands.Add(TargetType, command);
          
            }
            else
            {
                if (SeletctAutoGernertateCommands[TargetType] == null)
                    return null;

                command =
                    (SeletctAutoGernertateCommands[TargetType] as System.Data.SqlClient.SqlCommand).Clone();

               
            }
            if(command != null)
            SetValues<T>(ob, command);
            return command;
        }





  
        /// <summary>
        /// 生成一条记录是否在数据库中存在
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ob"></param>
        /// <returns></returns>
        public System.Data.IDbCommand GenerateExitsSql<T>(T ob)
        {
            var TargetType = typeof(T);
            String TableName = helper.GetTable(TargetType);
            var Properties = TargetType.GetProperties();
            MSSQLScriptSqlPatternFactory factory = new MSSQLScriptSqlPatternFactory();
            System.Data.SqlClient.SqlCommand command = new System.Data.SqlClient.SqlCommand();
            String sql = "Select  * From " + TableName + " Where  ";
            var parity = factory.GetExistSql<T>(Properties, ob,false);
            command.Parameters.AddRange(parity.Params.ToArray());
            command.CommandText = sql + parity.SqlScript;
            return command;
             
        }
        /// <summary>
        /// 设置参数
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ob"></param>
        /// <param name="command"></param>
        private void SetValues<T>(T ob, System.Data.SqlClient.SqlCommand command,string ParamPre="")
        {
            SetValues(ob, command, typeof(T),ParamPre);
        }




        /// <summary>
        /// 设置一个命令
        /// </summary>
        /// <param name="ob"></param>
        /// <param name="command"></param>
        /// <param name="TagetType"></param>
        /// <param name="cxt"></param>
        public void SetValues(object ob, System.Data.SqlClient.SqlCommand command, Type TagetType, String ParamPre="",bool GernerateSerialNo=false, IDBContext cxt = null)
        {

            var Propertitest = TagetType.GetProperties();
            var factory = new MSSQLScriptSqlPatternFactory();
            foreach (var property in Propertitest)
            {

                foreach (var colattr in helper.GetColNameAttributes(property))
                {
                    if (colattr.NoMap)
                        continue;
                    var paramName = "@" + ParamPre + helper.GetColName(colattr, property);
                    object propertyob = new ORMHelper().GetDbObject(property, ob, colattr.PropertyName) ?? "";

           
                    if (command.Parameters.Contains( paramName))
                    {
                        if (GernerateSerialNo && cxt != null
                            &&
                            String.IsNullOrEmpty(
                            //(command.Parameters[paramName].Value ?? ""
                            (new ORMHelper().GetDbObject(property, ob, colattr.PropertyName) ?? ""

                            ).ToString()
                            )
                            == true

                            )
                        {
                            var auto = property.GetCustomAttributes(typeof(SerialNoObject), true);
                            if (auto.Length > 0)
                            {
                                var seriSetting = auto[0] as SerialNoObject;
                                command.Parameters[paramName].Value = cxt.GetSerialNo(seriSetting.SerialPre, seriSetting.Len, seriSetting.DateFormateStr, "");
                                property.SetValue(ob,
                                  command.Parameters[paramName].Value, null);

                            }
                            else
                                command.Parameters[paramName].Value = propertyob;

                            //else if(String.IsNullOrEmpty( (command.Parameters[paramName].Value ?? "").ToString())==false)
                            //command.Parameters[paramName].Value = new ORMHelper().GetDbObject(property, ob, colattr.PropertyName) ?? "";
                        }
                        else
                        {

                            var insertDb =  new ORMHelper().GetDbObject(property, ob, colattr.PropertyName);
                            if(insertDb is DateTime &&insertDb != null && String.IsNullOrEmpty(colattr.FormatStr)==false )
                                insertDb = ((DateTime)insertDb).ToString(colattr.FormatStr);
                            command.Parameters[paramName].Value =insertDb ?? "";

                        }


                    }
                }
            }


        }





        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        /// <remarks>
        /// 2016.4.16
        /// 加了一个简单的限定，对加密的东西不做处理。。 
        /// 有待完善
        /// </remarks>
        public System.Data.IDbCommand GenerateSelectSql<T>()
        {
            
            

            //初始化一个Command 
            System.Data.SqlClient.SqlCommand command = null;
            var TargetType = typeof(T);
            var patternfactory = new MSSQLScriptSqlPatternFactory();
            if (selectedCommand.ContainsKey(TargetType) == false)
            {
                command = new System.Data.SqlClient.SqlCommand();
                String TableName = helper.GetTable(TargetType);
                var Properties = TargetType.GetProperties();
                String Cols = "";

                String SelectedSql = "SELECT ";

                for (int i = 0; i < Properties.Length; i++)
                {
                    var attributes = helper.GetColNameAttributes(Properties[i]);
                    foreach (var attribute in attributes.Where(p=>p.EncrpytType == EncryptType.NoEncrpty))
                    {
                        if (attribute.NoMap)
                            continue;



                        string ColName = helper.GetColName(attribute, Properties[i]);
                        String paramName = "@" + ColName;
                        Cols += TableName + ".[" + ColName + "],";
                    }
                }
                command.CommandText = "SELECT " + Cols.Substring(0, Cols.Length - 1) + " FROM " + TableName;    
                selectedCommand.Add(TargetType, command);
                return command;
            }
            else
                return selectedCommand[TargetType] as System.Data.IDbCommand;

        }






        private class ParentAttr 
        {
            public Type DataType;
            public Type ParentType;
        }
        public System.Data.IDbCommand GenerateSelectItemsSql<T>(object ob,PropertyInfo ParentInfo)
        {
            //初始化一个Command 
            System.Data.SqlClient.SqlCommand command = null;
            var TargetType = typeof(T);
            var ParentType = ob.GetType();
   ORMHelper helper = new ORMHelper();
                command = new System.Data.SqlClient.SqlCommand();
                String TableName = helper.GetTable(TargetType);

                var Properties = TargetType.GetProperties();
     
            
            String Cols = "";


            String WhereSql = "";
            PropertyInfo ParentKey = null;
            String ColumnName ="";

          
                var parentAttribute = ParentInfo.GetCustomAttributes(typeof(ParentRelationAttribute), true);
                 if (parentAttribute.Length > 0)
                {

                    ColumnName = (parentAttribute[0] as ParentRelationAttribute).ColumnName;
                    //  ParentKey = ParentType.GetProperty(parentInfo.PropertyName);


                }
                else
                {
                    var columAttribute = ParentInfo.GetCustomAttributes(typeof(ColumnAttribute), true);

                    if (columAttribute.Length > 0)
                        ColumnName = (columAttribute[0] as ColumnAttribute).ColumnName;
                }


                 ParentKey = new ORMHelper().GetKeyPropertyInfo(ParentInfo.PropertyType);
                if (ParentKey != null)
                {

                    command.Parameters.Add(new SqlParameter("@" + ColumnName,
                        ParentKey.GetValue(ob, null) ?? ""));
                    WhereSql = "where  [" + ColumnName + "]=@" + ColumnName
                       ;
                }
               
                for (int i = 0; i < Properties.Length; i++)
                {
                    
                   if(Properties[i]!= ParentInfo)
                    {

                        foreach(var Col in helper .GetColNameAttributes(Properties[i])){
                       
                        String paramName = "@" + helper.GetColName(Col,Properties[i]);
                        string ColName = helper.GetColName(Col,Properties[i]);
                 
                            if (Col.NoMap)
                                continue;
                            ColName = Col.ColumnName;
                   
                        if (String.IsNullOrEmpty(ColName))
                            ColName = Properties[i].Name;
                        Cols += TableName + ".[" + ColName + "],";
                     //   }
                    }
                   }
                  
                }
                
                command.CommandText = "SELECT " + Cols.Substring(0, Cols.Length - 1)+ " FROM " + TableName +WhereSql ;//+ "(" + 

                return command;
      

        }

        public System.Data.IDbCommand GenerateDynamicUpateSql<T>(T ob)
        {


            //初始化一个Command 
            System.Data.SqlClient.SqlCommand command = null;
            var TargetType = typeof(T);
            var patternfactory = new MSSQLScriptSqlPatternFactory();
            var helper = new ORMHelper();
         
                command = new System.Data.SqlClient.SqlCommand();
                String TableName = helper.GetTable(TargetType);
                var Properties = TargetType.GetProperties();
                String Cols = "";

                String update = "SELECT ";

                String WhereSql = "";

                for (int i = 0; i < Properties.Length; i++)
                {

                     var attribute = Properties[i].GetCustomAttributes(typeof(DynamicColumnAttribute), true);
                    if(attribute.Length >0){
                        var dynamicAttr = attribute[0] as DynamicColumnAttribute;
                        var proerpty = TargetType.GetProperty(dynamicAttr.SourcePropertyName );
                        if(proerpty != null )
                        {

                              var colattributes = helper.GetColNameAttributes(Properties[i]);

                              if(colattributes.Count > 0)
                              {
                                  if(colattributes[0].NoMap)
                                     continue;
                                  else
                                  {
                                       string ColName = helper.GetColName(colattributes[0], Properties[i]);
                                       String paramName = "@" + dynamicAttr.SourcePropertyName;
                                      ColName = TableName+".["+ColName+"]";
                                      command.Parameters.Add(paramName,proerpty.GetValue(ob,new object[]{}));
                                      Cols += ColName+"="+ ColName +(dynamicAttr.Operation == DanymicOperationType.Add?"+":"-")+paramName +",";
                                     
                                  
                                  }
                              }
                                 
                        }
                    }else {
                        var cols = helper.GetColNameAttributes(Properties[i]);
                        if(cols.Count >0)
                        foreach(var col in cols ){
                            if(col.IsKey == true &&String.IsNullOrEmpty(col.KeyGroupName)){
                                
                                var colname = helper.GetColName(col,Properties[i]);
                                var paramName = "@Pre"+colname;
                                WhereSql = " WHERE "+TableName+".["+colname +"]="+paramName;
                                command.Parameters.Add(paramName,Properties[i].GetValue(ob,new object[]{}));
                            }
                        }
                    }
                  
                } 

                command.CommandText = "UPDATE " +  TableName +" SET "+Cols.Substring(0, Cols.Length - 1)  + WhereSql;
                //selectedCommand.Add(TargetType, command);
                return command;
         

        }
    }
}
