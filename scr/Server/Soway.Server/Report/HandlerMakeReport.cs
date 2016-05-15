using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Soway.Service.Report
{
    class HandlerMakeReport : Handler
    {


        private ReportResult result;
        public HandlerMakeReport(MakeReportOption opt)
        {
            this.Opt = opt;
            this.PostData = this.Opt;
            IsNeedAuthenticate = true;
            result = new ReportResult();
            Result = result;
        }

        private MakeReportOption Opt { get;  set; }

        protected override void ImplementBusinessLogic()
        {
            var view = new Soway.Model.View.AutoViewFactory(this.Info.AppSqlCon, this).GetView(this.Opt.ViewId);

            var sqlCon = GetViewSql(view);
            var fac = new Soway.Model.Query.QueryFactory(this.Info.AppSqlCon, this);
            var queryModel =fac.GetQueryModel(view);
            var cxt = new Soway.Query.QueryContext(fac, sqlCon.ToString());

            cxt.Add(queryModel.Table);
            cxt.Ins.BoolExp = new BoolExpAdapter(queryModel).GetQueryExp(this.Opt.FilterExp,
                queryModel, this.Info.AppSqlCon,cxt.Ins);
            new Soway.Query.QueryInsFac().RefreshQueryInsReportParam(cxt.Ins);
            foreach (var col in this.Opt.ReportCols)
            {
                var queryCol = queryModel.Columns.First(p => p.ID == col.ColId);
                var selectedType = QueryCache.GetSelectedType(this.Info.AppSqlCon, queryCol.DataType);
                cxt.Ins.SelectedCols.Add(new Query.SelectedCol() {
                    DataCol = queryModel.Columns.First(p => p.ID == col.ColId),
                    SelectType = selectedType.First(p => p.ID.ToString() == col.SelectedTypeId),
                    SelectedIndex = col.Index,
                    SelectedName = col.ColName,
                    OrderType = col.OrderType,
                    SelectedTable = cxt.Ins.SelectedTables.Tables.First()});
                
            }

           
          
            var qresult = cxt.GetResult(sqlCon.ToString(), Opt.PageSize);
            qresult.CurrentPage = this.Opt.CurrentPage;
            var table = qresult.GetData();
      
            Soway.Report.ReportFactory rfac = new Soway.Report.ReportFactory();

            Soway.Report.TableFormat rfmt = new Soway.Report.TableFormat();
            List<ReportCell> cells = new List<ReportCell>();
            int row = 0;
            foreach (DataColumn column in table.Columns)
            {
                rfmt.ValueCell.Add(new Soway.Report.ValueCell
                {
                    SourceColumn = column.ColumnName,
                    Name = column.ColumnName

                });
                cells.Add(new ReportCell() { Col = 0, Row = row, ColSpan = 1, RowSpan = 1, FmtValue = column.ColumnName });
            }
            var matrix = new Soway.Report.Views.MatrixTableFactory().CreateMatrixTable(rfmt, table);
           foreach(var cell in matrix.Cells)
            {
                row++;
                foreach(var vcell in cell.Cells)
                {
                    cells.Add(new ReportCell()
                    {
                        Col = 0,
                        Row = row,
                        ColSpan = 1,
                        RowSpan = 1,
                        FmtValue = vcell.Value.ToString()
                    });
                }
            }


            result.CurrentPage = Opt.CurrentPage;
            result.PageSize = Opt.PageSize;
            result.TotalPages = qresult.TotalPages;
            result.TotalRecords = qresult.TotalRecords;
            result.ViewId = Opt.ViewId;
            result.Cells = cells;

        }
    }
}
