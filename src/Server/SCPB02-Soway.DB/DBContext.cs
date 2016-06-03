using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Data.SqlClient;
using System.Data;
using System.Reflection;
using Soway.Data.Discription.ORM;
using Soway.Data;

namespace Soway.DB
{
    public class DBContext : Soway.DB.IDBContext,IDisposable,System.Linq.IQueryProvider
    {




        　


        private static MethodInfo GetMethod { get; set; }
        static DBContext()
        {
            foreach (var methodinfo in typeof(DBContext).GetMethods())
            {
                if (methodinfo.IsGenericMethod && methodinfo.GetGenericArguments().Length == 2 && methodinfo.Name == "GetDetail")
                {
                    GetMethod = methodinfo;

                    return;
                }
            }
        }



       
        /// <summary>
        /// 表名前缀
        /// </summary>
        public string TablePreStr { get; set; } 
        public string ConnectionString { get; set; }
        private static string constr;
     
        public DBContext()  {    this.TransmitCommands = new Queue<SqlTransAutoMic>();}
        public DBContext(string ConStr,string TableStr="")
        {

            
            ConnectionString = ConStr;
            this.TransmitCommands = new Queue<SqlTransAutoMic>();
            this.TablePreStr = TableStr;
            constr = ConStr;
            GlobalSqlContext.ConStr = ConStr;
        }

        /// <summary>
        /// 新建 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ob"></param>
        private void Create<T>(T ob,bool Submit)
        { 
               var sqlCon = ConnectionString;
            if (string.IsNullOrEmpty(sqlCon))
                sqlCon = GlobalSqlContext.GetConStr(typeof(T), "");


            ConnectionString
                 = sqlCon;
            var gernerater = new MSSQL.MSSSqlCommandGenerater();
            var command = gernerater.GenerateCreateSql<T>(ob, this);
        //    var UpdateSqlCommand = gernerater.GenerateSeletedAutoGernerateSql<T>(ob);
            this.TransmitCommands.Enqueue(new SqlTransAutoMic() { ob = ob, command = command, Operation = SqlOperation.insert });
            var UpdateSqlCommand = gernerater.GenerateSeletedAutoGernerateSql<T>(ob);

            if (UpdateSqlCommand != null)
            {
                this.TransmitCommands.Enqueue(new SqlTransAutoMic()
                {
                    ob = ob,
                    command = UpdateSqlCommand,
                    Operation = SqlOperation.updateafterinsert
                });
            }

            if(Submit )
            this.SubmitChanges();
        }
        public void Create<P,T>(P ob)
            where P:Soway.Data.ObjectWithSubItem<T>
            where T : IItemInterface<P>,new() 
        {

                this.Create<P>(ob, false);
                foreach (var item in ob.Items)
                    this.Create<T>(item, false);
                this.SubmitChanges(); 
        }

        public P GetWithItems<P, T>(object key)
            where P : Soway.Data.ObjectWithSubItem<T>,new()
            where T : IItemInterface<P>, new()
        {
 
            P item = GetDetail<P>(key);
            if (item != null)
            {
                item.Items.RaiseListChangedEvents = false;
                var list = this.GetItems<T>(item, "Parent");
                foreach (var i in list)
                    item.Items.Add(i);
                item.Items.RaiseListChangedEvents = true;
            }
            return item;

        }




        public void Save<T>(T ob)
        {

            Save<T>(ob, true);
        }

        public void Delete<T>(T ob)
        {
            Delete<T>(ob, true);
        }


        public List<T> Get<T>() where T : class,new()
        {
            
            return Get<T, T>();
        }
        public List<I> Get<T,I> () where T:class,I,new()
        {


            var sqlCon = ConnectionString;
            if (string.IsNullOrEmpty(sqlCon))
                sqlCon = GlobalSqlContext.GetConStr(typeof(T), "");
            ConnectionString = sqlCon;
            using (System.Data.SqlClient.SqlConnection con = 
                new System.Data.SqlClient.SqlConnection(ConnectionString))
            {
                con.Open();
                var selectCommand = new MSSQL.MSSSqlCommandGenerater().GenerateSelectSql<T>();
                selectCommand.Connection = con;
                var adapter = new System.Data.SqlClient.SqlDataAdapter(selectCommand as SqlCommand);
                DataTable table = new DataTable();
                adapter.Fill(table);
                List<I> result = new List<I>();
                var Type = typeof(T);
                var Properties = Type.GetProperties();
                for (int i = 0; i < table.Rows.Count; i++)
                {

                    var item = new T();
                    SetObject(item, table.Rows[i], Properties);
                    result.Add(item);

                }
                return result;

            }
        }


     
        public List<I> Get<T,I>(String SqlScript) where T:class,I,new() {
             var sqlCon = ConnectionString;
            if (string.IsNullOrEmpty(sqlCon))
                sqlCon = GlobalSqlContext.GetConStr(typeof(T), "");
            ConnectionString = sqlCon;

            using (System.Data.SqlClient.SqlConnection con = new System.Data.SqlClient.SqlConnection(sqlCon)){
                con.Open();
                var selectCommand = new System.Data.SqlClient.SqlCommand();
                selectCommand.CommandText = SqlScript;
                selectCommand.Connection = con;
                var adapter = new System.Data.SqlClient.SqlDataAdapter(selectCommand as SqlCommand);
                DataTable table = new DataTable();
                adapter.Fill(table);
                List<I> result = new List<I>();
                var Type = typeof(T);
                var Properties = Type.GetProperties();
                for (int i = 0; i < table.Rows.Count; i++)
                {

                    var item = new T();
                    SetObject(item, table.Rows[i], Properties);
                    result.Add((T)item);

                }
                return result;

            }

    }

        public List<T> Get<T>(String SqlScript) where T:class,new()
        {
            return Get<T, T>(SqlScript);
        }
        public List<I> Get<T, I>(System.Data.SqlClient.SqlCommand selectCommand) where T : class,I, new()
        {
            var sqlCon = ConnectionString;
            if (string.IsNullOrEmpty(sqlCon))
                sqlCon = GlobalSqlContext.GetConStr(typeof(T), "");
            ConnectionString = sqlCon;
            using (System.Data.SqlClient.SqlConnection con = new System.Data.SqlClient.SqlConnection(sqlCon))
            {
                con.Open();
                selectCommand.Connection = con;
                var adapter = new System.Data.SqlClient.SqlDataAdapter(selectCommand as SqlCommand);
                DataTable table = new DataTable();
                adapter.Fill(table);
                List<I> result = new List<I>();
                var Type = typeof(T);
                var Properties = Type.GetProperties();
                for (int i = 0; i < table.Rows.Count; i++)
                {

                    I item = new T();
                    SetObject(item, table.Rows[i], Properties);
                    result.Add((T)item);

                }
                return result;

            }

        }
  
        public List<T> Get<T>(System.Data.SqlClient.SqlCommand selectCommand) where T:class,new()
        {
            return Get<T, T>(selectCommand);
           
        }
        
         
        private void SetObject(object ob, DataRow Row)
        {
            var Propertyies = ob.GetType().GetProperties();
            SetObject(ob, Row, Propertyies);

        }
        private void SetObject(object ob, DataRow Row, PropertyInfo[] properties,bool GetDetail=false)
        {
                MSSQL.MSSQLScriptSqlPatternFactory patternFactory = new MSSQL.MSSQLScriptSqlPatternFactory();

                if (ob is Soway.Data.IObjectFromDataBase)
                {
                    (ob as Soway.Data.IObjectFromDataBase).Row = Row;
                }
            ORMHelper helper = new ORMHelper();
            for (int j = 0; j < properties.Length; j++)
            {
                try
                {
                    var attribute = helper.GetColNameAttributes(properties[j]);


                    object propertyob = new object();
                    var isBO = new ORMHelper().IsBusinessType(properties[j].PropertyType);
                    var propertyType = properties[j].PropertyType;



                    for (int i = 0; i < attribute.Count; i++)
                    {

                        var attr = attribute[i];
                        if (attr.NoMap)
                            continue;
                        String ColName = helper.GetColName(attr, properties[j]);
                        if (String.IsNullOrEmpty(ColName))
                            continue;

                        if (Row.Table.Columns.Contains(ColName))
                        {
                            var dbObject = Row[ColName];




                            if ((dbObject is DBNull))
                                continue;
                            if (propertyType == typeof(DateTime) && String.IsNullOrEmpty(attr.FormatStr) == false&&dbObject !=null)
                                dbObject = DateTime.ParseExact(dbObject.ToString(), attr.FormatStr, null);
                            if (attribute.Count == 1)
                            {
                                if (isBO)
                                {

                                    if (GetDetail)
                                    {
                                        if (!(dbObject is DBNull))
                                        {
                                         

                                            properties[j].SetValue(ob, DBContext.GetMethod.MakeGenericMethod(
                                                new Type[] { properties[j].PropertyType, properties[j].PropertyType }).Invoke(
                                            this, new object[] { Row[ColName] }), new object[] { });
                                        }
                                    }
                                    else
                                    {
                                        propertyob = propertyType.Assembly.CreateInstance(propertyType.FullName);
                                        properties[j].SetValue(ob, propertyob
                                         , new object[] { });

                                        PropertyInfo key;
                                        if (String.IsNullOrEmpty(attr.PropertyName))
                                            key = new ORMHelper().GetKeyPropertyInfo(propertyType);
                                        else
                                            key = propertyType.GetProperty(attr.PropertyName);
                                        if (!(dbObject is DBNull))
                                            key.SetValue(propertyob, dbObject, new object[] { });


                                    }
                                }
                                else
                                    if (String.IsNullOrEmpty(attr.PropertyName))
                                    {

                                        if (!(dbObject is DBNull))
                                            properties[j].SetValue(ob, dbObject is DBNull ? null : dbObject, new object[] { });
                                    }
                                    else if (GetDetail && isBO)
                                    {
                                        if (!(dbObject is DBNull))
                                            properties[j].SetValue(ob, DBContext.GetMethod.MakeGenericMethod(new Type[] { properties[j].PropertyType, properties[j].PropertyType }).Invoke(
                                           new Soway.DB.DBContext(), new object[] { Row[ColName] }), new object[] { });
                                    }
                            }
                            else
                            {

                                if (i == 0)//{
                                {
                                    propertyob = propertyType.Assembly.CreateInstance(propertyType.FullName);
                                    properties[j].SetValue(ob, propertyob, new object[] { });
                                }

                                if (String.IsNullOrEmpty(attr.PropertyName) == false)
                                {
                                    if (!(dbObject is DBNull))
                                    {
                                        if (propertyType.GetProperty(attr.PropertyName).PropertyType
                                            == typeof(DateTime) && String.IsNullOrEmpty(attr.FormatStr) == false && dbObject != null)
                                            dbObject = DateTime.ParseExact(dbObject.ToString(), attr.FormatStr, null);
                                        propertyType.GetProperty(attr.PropertyName).SetValue(propertyob, dbObject, new object[] { });
                                    }
                                }

                                //  }


                            }
                        }

                    }
                }
                catch (Exception ex)
                {
                }
                
            }

        }
        public void Attatch<T>(T ob, OperationType SubmitType)
        {
                 var gernerater = new MSSQL.MSSSqlCommandGenerater();
            switch(SubmitType){
                case OperationType.Create:
                    this.TransmitCommands.Enqueue(new SqlTransAutoMic()
                    {
                        command =
                            gernerater.GenerateCreateSql<T>(ob, this) as SqlCommand,
                        ob = ob,
                        Operation =  SqlOperation.insert
                    });
                      var UpdateSqlCommand = gernerater.GenerateSeletedAutoGernerateSql<T>(ob);

                    if (UpdateSqlCommand != null)
                    {
                        this.TransmitCommands.Enqueue(new SqlTransAutoMic()
                        {
                            ob = ob,
                            command = UpdateSqlCommand,
                            Operation = SqlOperation.updateafterinsert
                        });
                    }

                    break;
                case OperationType.Save:
                    this.TransmitCommands.Enqueue(new SqlTransAutoMic()
                    {
                        command =
                            gernerater.GenerateSaveSql<T>(ob) as SqlCommand,
                        ob = ob,
                        Operation = SqlOperation.update
                    });
                    break;
                case OperationType.Delete:
                    this.TransmitCommands.Enqueue(new SqlTransAutoMic()
                    {
                        command =
                            gernerater.GenerateDeleteSql<T>(ob) as SqlCommand,
                        ob = ob,
                        Operation =  SqlOperation.delete
                    });
                    break;
                case OperationType.DynamicUpdate:
                    this.TransmitCommands.Enqueue(new SqlTransAutoMic()
                    {
                        command = gernerater.GenerateDynamicUpateSql<T>(ob),
                        ob = ob,
                        Operation = SqlOperation.updateafterupdate
                    });;
                    break;
                default:
                    break;
            }
        }
        public void Attatch(String sql )
        {
            SqlCommand command = new SqlCommand();
            command.CommandText = sql;
            Attatch(command);
        }

        public void Attatch(SqlCommand command)
        {
            this.TransmitCommands.Enqueue(new SqlTransAutoMic()
            {
                command =
                  command,
                ob = null,
                Operation =  SqlOperation.excute
            });
        }
        private Queue<SqlTransAutoMic> TransmitCommands;
        public bool SubmitChanges() { return this.SubmitChanges(ConnectionString); }
        public bool SubmitChanges(String sqlCon)
        {
            var con1 = sqlCon;
            if (string.IsNullOrEmpty(con1))
                con1 = ConnectionString;
            using (System.Data.SqlClient.SqlConnection con = new System.Data.SqlClient.SqlConnection(con1))
            {
                con.Open();



                if (TransmitCommands.Count > 1)
                {
                    var trans = con.BeginTransaction();

                    while (TransmitCommands.Count > 0)
                    {
                        var command = TransmitCommands.Dequeue();
                        
                        command.command.Transaction = trans;
                        command.command.Connection = con;
                        if (command.ob != null  )
                        new MSSQL.MSSSqlCommandGenerater().SetValues(command.ob, command.command as SqlCommand, command.ob.GetType(),"",true, this);

                    
  

                        if (command.Operation == SqlOperation.delete
                            || command.Operation == SqlOperation.excute
                            || command.Operation == SqlOperation.insert
                            || command.Operation == SqlOperation.update)
                        {
                            command.command.ExecuteNonQuery();

                                                 }
                        else
                        {
                            var table = new DataTable();
                            new SqlDataAdapter(command.command as SqlCommand).Fill(table);
                            if (command.ob != null&&table.Rows.Count > 0)
                            this.SetObject(command.ob, table.Rows[0]);

                        }

                    }
                    try
                    {
                        trans.Commit();

                        return true;
                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();
                        return false;
                    }
                }
                else if (TransmitCommands.Count == 1)
                {
                    var command = TransmitCommands.Dequeue();

                    command.command.Connection = con;
                    if (command.Operation == SqlOperation.delete
                        || command.Operation == SqlOperation.excute
                        || command.Operation == SqlOperation.insert
                        || command.Operation == SqlOperation.update)
                    {

                        command.command.ExecuteNonQuery();
                    }
                    else
                    {
                        var table = new DataTable();
                        new SqlDataAdapter(command.command as SqlCommand).Fill(table);

                        if (command.ob != null)
                        this.SetObject(command.ob, table.Rows[0]);
                    }
                    return true;
                }
                else
                    return true;

            }
            
        }

        public object GetPreKey<T>(T ob)
        {
             if (String.IsNullOrEmpty(ConnectionString))
                ConnectionString = GlobalSqlContext.GetConStr(typeof(T), "");
             using (System.Data.SqlClient.SqlConnection con = new System.Data.SqlClient.SqlConnection(ConnectionString))
             {


                 con.Open();
                 var gernerater = new MSSQL.MSSSqlCommandGenerater();

                 var command = gernerater.GenerateGetPreKey<T>(ob);
                 command.Connection = con;
                 SqlDataAdapter adapter = new SqlDataAdapter(command as SqlCommand);
                 DataTable table = new DataTable();
                 adapter.Fill(table);
                 if (table.Rows.Count > 0)
                     return table.Rows[0][0];
                 else
                     return null;
             }
        }
        public object GetNextKey<T>(T ob)
        {
            if (String.IsNullOrEmpty(ConnectionString))
                ConnectionString = GlobalSqlContext.GetConStr(typeof(T), "");
            using (System.Data.SqlClient.SqlConnection con = new System.Data.SqlClient.SqlConnection(ConnectionString))
            {


                con.Open();
                var gernerater = new MSSQL.MSSSqlCommandGenerater();

                var command = gernerater.GenerateGetNextKey<T>(ob);
                command.Connection = con;
                SqlDataAdapter adapter = new SqlDataAdapter(command as SqlCommand);
                DataTable table = new DataTable();
                adapter.Fill(table);
                if (table.Rows.Count > 0)
                    return table.Rows[0][0];
                else
                    return null;
            }
        }


        public object
            GetExist<T>(T ob)
        {


            if (String.IsNullOrEmpty(ConnectionString))
                ConnectionString = GlobalSqlContext.GetConStr(typeof(T), "");
            using (System.Data.SqlClient.SqlConnection con = new System.Data.SqlClient.SqlConnection(ConnectionString))
            {


                con.Open();
                var gernerater = new MSSQL.MSSSqlCommandGenerater();

                var command = gernerater.GenerateExitsSql<T>(ob);
                command.Connection = con;
                //   int insertRowCount = command.ExecuteNonQuery();


                command.Connection = con;
                SqlDataAdapter adapter = new SqlDataAdapter(command as SqlCommand);
                DataTable table = new DataTable();
                T newob = (T)typeof(T).Assembly.CreateInstance(typeof(T).FullName);
                adapter.Fill(table);
                if (table.Rows.Count > 0)
                {
                    SetObject(newob, table.Rows[0]);
                    return newob;
                }
                else
                    return null;

            }



        }

        public List<T> GetItems<T>(object ob,string ParentProperty=null )
        {
            if (String.IsNullOrEmpty(ConnectionString))
                ConnectionString = GlobalSqlContext.GetConStr(typeof(T), "");
         
            using (System.Data.SqlClient.SqlConnection con = new System.Data.SqlClient.SqlConnection(ConnectionString))
            {
                var Type = typeof(T);
                var Properties = Type.GetProperties();
                PropertyInfo parentInfo = null;

                if (ParentProperty == null)
                {
                    foreach (var property in Properties)
                    {
                        var attrib = property.GetCustomAttributes(typeof(ParentRelationAttribute), true);
                        if (attrib.Length > 0)
                        {
                            parentInfo = property;
                            break;
                        }
                        else if (property.PropertyType == ob.GetType())
                        {
                            parentInfo = property;
                        }
                    }


                }
                else
                {
                    parentInfo = Type.GetProperty(ParentProperty);
                }
                con.Open();
                var selectCommand = new MSSQL.MSSSqlCommandGenerater().GenerateSelectItemsSql<T>(ob,parentInfo);
                selectCommand.Connection = con;
                var adapter = new System.Data.SqlClient.SqlDataAdapter(selectCommand as SqlCommand);
                DataTable table = new DataTable();
                adapter.Fill(table);
                List<T> result = new List<T>();   
                for (int i = 0; i < table.Rows.Count; i++)
                {
                   
                    var item = Type.Assembly.CreateInstance(Type.FullName);
                    SetObject(item, table.Rows[i], Properties,true);
                    parentInfo.SetValue(item, ob, null);
                    result.Add((T)item);

                }
                return result;

            }
        }
        
    
     
        public I GetDetail<I, T>(object key) where T : class,I, new()
        {
            return InternalGetDetail<I, T>(key);
        }

        private I InternalGetDetail<I, T>(object key) where T : class, I, new()
        {
            if (String.IsNullOrEmpty(ConnectionString))
                ConnectionString = GlobalSqlContext.GetConStr(typeof(T), "");


            if (key == null)
                return default(I);
            using (System.Data.SqlClient.SqlConnection con = new System.Data.SqlClient.SqlConnection(ConnectionString))
            {
                con.Open();
                var selectCommand = new MSSQL.MSSSqlCommandGenerater().GenerateGetItem<T>(key);

                selectCommand.Connection = con;
                var adapter = new System.Data.SqlClient.SqlDataAdapter(selectCommand as SqlCommand);
                DataTable table = new DataTable();
                adapter.Fill(table);

                var Type = typeof(T);
                var Properties = Type.GetProperties();


                if (table.Rows.Count > 0)
                {
                    var item = new T();
                    SetObject(item, table.Rows[0], Properties, true);

                    return item;

                }
                return default(I);

            }
        }

        public T GetDetail<T>(object key) where T : class ,new()
        {
            return GetDetail<T, T>(key);
        }


       
       

         

        public List<I> GenerualQuery<I, T>(string str) where T : class ,I, new()
        {
            return new List<I>();
        }

        public List<T> GenerualQuery< T>(string str) where T : class ,new()
        {
            return GenerualQuery<T, T>(str);
        }






        public DateTime GetServerDateTime()
        {
            using (System.Data.SqlClient.SqlConnection con = new System.Data.SqlClient.SqlConnection(ConnectionString))
            {


                con.Open();
                var command = con.CreateCommand();
                command.CommandText = "Select getdate()";
                return (DateTime) command.ExecuteScalar() ;
            }
        }
        public string GetSerialNo(string Pre, int Len, string DateFormatStr = null)
        {
            return GetSerialNo(Pre, Len, DateFormatStr, "");
        }
        public string GetSerialNo(string Pre, int Len,string DateFormatStr = null,string source="")
        {

            var date = this.GetServerDateTime();
            String TableName = Pre + "_Serial_Table";
            String SerialNoPre = Pre;
            String OldTable = "";
 

            // System.Diagnostics.Trace.WriteLine(DateFormatStr);
            TableName += "_" + date.ToString(DateFormatStr);
            SerialNoPre += date.ToString(DateFormatStr);
            OldTable = Pre + "_Serial_Table" + "_" + date.Subtract(new TimeSpan(1, 0, 0, 0)).ToString(  
                DateFormatStr);


              string newsql = String.Format(@"

declare @count int;
set @count=0
if(exists(select * from sys.tables where name ='{4}'))
begin
select @count =   max(KeyIndex) from [{4}]
    insert SerialTable ([Pre]
      ,[Source]
      ,[GenerateTime]
      ,[GenerateCount]) values('{0}','{3}',getdate(),@count)
drop table [{4}]
end
insert SerialTable ([Pre]
      ,[Source]
      ,[GenerateTime] ) values('{0}','{3}',getdate())

update SerialTable
 set [GenerateCode]=Pre+
right('{1}'+ CONVERT(varchar(max),isnull(A,0)+1),{2}),GenerateCount=A+1 from SerialTable ,
(select  isnull(max(GenerateCount),0)  A  from SerialTable where SerialId < @@IDENTITY 
and Source = '{3}' and Pre = '{0}')B
where  SerialId =  @@IDENTITY 
delete SerialTable 
 where SerialId < @@IDENTITY 
and Source = '{3}' and Pre = '{0}'

select GenerateCode from SerialTable 
 where SerialId = @@IDENTITY",
                             Pre + (String.IsNullOrEmpty(DateFormatStr) == false ? date.ToString(DateFormatStr) : ""),
                             "".PadLeft(Len, '0'),
                             Len, 
                             source, 
                             TableName);


            
            using (System.Data.SqlClient.SqlConnection con = new System.Data.SqlClient.SqlConnection(ConnectionString))
            {

             
                con.Open();
                var command = con.CreateCommand();
                command.CommandText = newsql;

                // System.Diagnostics.Trace.WriteLine(newsql);

                return command.ExecuteScalar().ToString();
            ////    long indexKey = System.Convert.ToInt64(command.ExecuteScalar());
             //   return SerialNoPre + indexKey.ToString().PadLeft(Len, '0');
            }



        }


 
        public void Create<T>(T ob)
        {

            Create<T>(ob, true);
        }

        public void Delete<P, T>(P ob)
            where P : ObjectWithSubItem<T>
            where T : IItemInterface<P>, new()
        {
        

        }

        public void Save<P, T>(P ob)
            where P : ObjectWithSubItem<T>
            where T : IItemInterface<P>, new()
        {

            this.Save<P>(ob, false);
            foreach (var item in ob.Items)
                this.Save<T>(item,false);
            foreach (var item in ob.Items.DeleteList)
              this.Delete<T>(item, false);
            foreach (var item in ob.Items.AddedList)
                this.Create<T>(item, false);
            this.SubmitChanges();

        }

 

        /// <summary>
        /// 新建 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ob"></param>
        private void Save<T>(T ob, bool Submit)
        {
            if (String.IsNullOrEmpty(ConnectionString))
                ConnectionString = GlobalSqlContext.GetConStr(typeof(T), "");
         
            var gernerater = new MSSQL.MSSSqlCommandGenerater();
         //   var command = gernerater<T>(ob, this);
            var UpdateSqlCommand = gernerater.GenerateSaveSql<T>(ob);
            this.TransmitCommands.Enqueue(new SqlTransAutoMic() { ob = ob, command = UpdateSqlCommand, Operation = Soway.DB.SqlOperation.update });

                
            if (Submit)
                this.SubmitChanges();
        }


        private void Delete<T>(T ob, bool Submit)
        {
            if (String.IsNullOrEmpty(ConnectionString))
                ConnectionString = GlobalSqlContext.GetConStr(typeof(T), "");
         
            var gernerater = new MSSQL.MSSSqlCommandGenerater();
           // var command = gernerater.GenerateDeleteSql<T>(ob);
            var UpdateSqlCommand = gernerater.GenerateDeleteSql<T>(ob);
            this.TransmitCommands.Enqueue(new SqlTransAutoMic() { ob = ob, command = UpdateSqlCommand, Operation = Soway.DB.SqlOperation.delete });

            if (Submit)
                this.SubmitChanges();
        }

        public object GetExcute(String sql)
        {
            using (var con = new SqlConnection(this.ConnectionString))
            {

                var command = new SqlCommand();
                command.Connection = con;
                command.CommandText = sql;
                con.Open();
                return command.ExecuteScalar();
            }
       
        }


        public void Dispose()
        {
            
        }



        public DataTable GetDataTable(String sql)
        {
            using (System.Data.SqlClient.SqlConnection con = new System.Data.SqlClient.SqlConnection(ConnectionString))
            {


                con.Open();




                var command = new System.Data.SqlClient.SqlCommand();
                command.Connection = con;
                command.CommandText = sql;
                //   int insertRowCount = command.ExecuteNonQuery();


                command.Connection = con;
                SqlDataAdapter adapter = new SqlDataAdapter(command as SqlCommand);
                DataTable table = new DataTable();
               
                adapter.Fill(table);
                return table;


            }
        }

      

        public IQueryable<TElement> CreateQuery<TElement>(System.Linq.Expressions.Expression expression)
        {
            throw new NotImplementedException();
        }

        public IQueryable CreateQuery(System.Linq.Expressions.Expression expression)
        {
            throw new NotImplementedException();
        }

        public TResult Execute<TResult>(System.Linq.Expressions.Expression expression)
        {
            throw new NotImplementedException();
        }

        public object Execute(System.Linq.Expressions.Expression expression)
        {
            throw new NotImplementedException();
        }
    }
}
