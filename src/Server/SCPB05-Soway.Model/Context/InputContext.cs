using Soway.Model.SqlServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Soway.Model.Context
{
    /// <summary>
    /// 输入选择的上下文提示
    /// </summary>
    public class InputContext
    {
        public List<InputQueryResult> Query(Soway.Model.Model model, string text, ModelBindingList source, string Fileter = null,int Size = 10)
        {
            this.Size = Size;
            if (source == null)
                return QueryFromSql(model, text, Fileter);
            else
                return QueryFromSource(source, text, Fileter);

        }

        public List<InputQueryResult> Query(dynamic model, string text, ModelBindingList source, string Fileter = null, int Size = 10)
        {
            this.Size = Size;
            if (source == null)
                return QueryFromSql(model, text, Fileter);
            else
                return QueryFromSource(source, text, Fileter);

        }




        public SqlCon Con { get; set; }
        public int Size { get; private set; }

        public InputContext(Soway.Model.SqlCon con)
        {
            Con = con;
        }

        private List<InputQueryResult>QueryFromSource(ModelBindingList source,string text,string Filter){

            var arry = source.Where(p => (p[p.Model.ShowProperty] ?? "").ToString().Trim().ToUpper().IndexOf((text??"").Trim().ToUpper()) >= 0).Select(p =>
                new InputQueryResult()
                {
                    Text = (p[p.Model.ShowProperty] ?? "").ToString(),
                    id = p.ID
                });
            List<InputQueryResult> result = new List<InputQueryResult>();
            result.AddRange(arry.ToArray());
            return result;

        }

        private   List<InputQueryResult> QueryFromSql(Soway.Model.Model model, string text, string Fileter)
        {


 

            System.Data.SqlClient.SqlCommand command = new System.Data.SqlClient.SqlCommand();
            command.Connection = new System.Data.SqlClient.SqlConnection(this.Con.ToString());

            string IdCol = "SysId";
            string paramCol = "";
            if (model.IdProperty != null)
                IdCol = model.IdProperty.DBName;
            command.CommandText = string.Format("SELECT TOP {0} {1} ", this.Size, IdCol);
            string ShowCol = IdCol;
            paramCol = IdCol;
            if (model.ShowProperty != model.IdProperty &&model.ShowProperty != null)
            {
                ShowCol = model.ShowProperty.DBName;
                paramCol = ShowCol;
            }
            //else
            //    ShowCol = "";
            if (string.IsNullOrEmpty(ShowCol) == false && ShowCol !=IdCol)
                command.CommandText += string.Format(",{0} ", ShowCol);

            command.CommandText += string.Format("FROM {0} WHERE ", model.DataTableName);
            if (String.IsNullOrEmpty(Fileter) == false)
            {
                command.CommandText += string.Format(" {0} AND ", Fileter);

            }
            command.CommandText += string.Format("{0} like '%'+@{0}+'%'", paramCol);
            command.Parameters.Add(new System.Data.SqlClient.SqlParameter("@" + paramCol, text));

            command.CommandText += string.Format(" ORDER BY {0} DESC", IdCol);

            var table = SqlDataLoader.GetSqlData(command);
            List<InputQueryResult> result = new List<InputQueryResult>();
            for (int i = 0; i < table.Rows.Count; i++)
            {
                result.Add(new InputQueryResult() { Text = table.Rows[i][ShowCol].ToString(), id = table.Rows[i][IdCol] });
            }
            command.Connection.Close();
            return result;
        }



        private List<InputQueryResult> QueryFromSql(dynamic model, string text, string Fileter)
        {




            System.Data.SqlClient.SqlCommand command = new System.Data.SqlClient.SqlCommand();
            command.Connection = new System.Data.SqlClient.SqlConnection(this.Con.ToString());

            string IdCol = "SysId";
            string paramCol = "";
            if (model.IdProperty != null)
                IdCol = model.IdProperty.DBName;
            command.CommandText = string.Format("SELECT TOP {0} {1} ", this.Size, IdCol);
            string ShowCol = IdCol;
            paramCol = IdCol;
            if (model.ShowProperty != model.IdProperty && model.ShowProperty != null)
            {
                ShowCol = model.ShowProperty.DBName;
                paramCol = ShowCol;
            }
            //else
            //    ShowCol = "";
            if (string.IsNullOrEmpty(ShowCol) == false && ShowCol != IdCol)
                command.CommandText += string.Format(",{0} ", ShowCol);

            command.CommandText += string.Format("FROM {0} WHERE ", model.DataTableName);
            if (String.IsNullOrEmpty(Fileter) == false)
            {
                command.CommandText += string.Format(" {0} AND ", Fileter);

            }
            command.CommandText += string.Format("{0} like '%'+@{0}+'%'", paramCol);
            command.Parameters.Add(new System.Data.SqlClient.SqlParameter("@" + paramCol, text));

            command.CommandText += string.Format(" ORDER BY {0} DESC", IdCol);

            var table = SqlDataLoader.GetSqlData(command);
            List<InputQueryResult> result = new List<InputQueryResult>();
            for (int i = 0; i < table.Rows.Count; i++)
            {
                result.Add(new InputQueryResult() { Text = table.Rows[i][ShowCol].ToString(), id = table.Rows[i][IdCol] });
            }
            command.Connection.Close();
            return result;
        }
    }
}
