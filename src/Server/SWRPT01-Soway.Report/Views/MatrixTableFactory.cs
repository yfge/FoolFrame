using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Soway.Report.Views
{
    using Soway.Data.DS.Tree;
    public class MatrixTableFactory
    {
         
        public MatrixTable CreateMatrixTable(TableFormat format, System.Data.DataTable source) {
            Soway.Data.DS.Tree.ITreeFactory<MatrixHeader, MatrixHeader> fac =
                new Soway.Data.DS.Tree.ITreeFactory<MatrixHeader, MatrixHeader>(

                          (ob1, ob2) =>
                          {
                              return Soway.Data.DS.Tree.TeeNodeCompareResult.Child;
                          });
            Soway.Data.DS.Tree.Tree<MatrixHeader> colHeaders = new Soway.Data.DS.Tree.Tree<MatrixHeader>();
            Soway.Data.DS.Tree.Tree<MatrixHeader> rowHeaders = new Soway.Data.DS.Tree.Tree<MatrixHeader>();
            MatrixTable table = new MatrixTable();
            //生成列头
            MakeTableHeader(format.Colums, source,  table.ColHeaders, colHeaders);
            MakeTableHeader(format.Rows, source, table.RowHeaders, rowHeaders);

      
            foreach (DataRow row in source.Rows)
            {
                DataRect rect = new DataRect();
                //1 得到列头
                List<object> cols = new List<object>();
                foreach (var colfmt in format.Colums)
                {
                    cols.Add(row[colfmt.SourceColumn]);

                }
                //2 得到行头
                List<object> rows= new List<object>();
                foreach (var colfmt in format.Rows)
                {

                    rows.Add(row[colfmt.SourceColumn]);

                }
                //3 得到数据
                foreach (var dtfmt in format.ValueCell)
                {
                    rect.Cells.Add(new Cell() { Value = row[dtfmt.SourceColumn] });
                }
                rect.ColHeaderIndex = fac.AddArrayToLeaf(colHeaders, GetFmtArry(format.Colums, cols.ToArray()))-1;
                rect.RowHeaderIndex = fac.AddArrayToLeaf(rowHeaders, GetFmtArry(format.Rows, rows.ToArray()))-1;
                table.Cells.Add(rect);
            }

            //计算汇总 
            var colBottom = fac.GetBottomNodes(colHeaders);
            var rowBottom = fac.GetBottomNodes(rowHeaders);
            foreach (var node in rowHeaders)
            {
                if (node.Data.StaticCell != null)
                {

                    int colIndex = 0;
                    int RowHeaderIndex = GetTreeNodeIndex(node,rowHeaders);// fac.AddArrayToLeaf(rowHeaders, GetFmtArry(format.Colums, GetObjsToParent(node))) - 1;
                    string CalScope =  GetCalScope(node,rowBottom,RowHeaderIndex);
                   
                    
                    for (colIndex = 0;colIndex < colBottom.Count; colIndex++)
                    {
                        DataRect staticRect = new DataRect();
                        staticRect.ColHeaderIndex = colIndex;
                        staticRect.RowHeaderIndex = RowHeaderIndex;

                        foreach (var staticCell in node.Data.StaticCell.StaticsCells)
                        {
                            staticRect.Cells.Add(new Cell()
                            {
                                Value = node.Data.StaticCell.Name + colIndex,
                                IsCalculate = true,
                                CalDirection = CalDirection.Column,
                                CalScope = CalScope,
                                Expression = staticCell.StaticType


                            });
                        }

                        table.Cells.Add(staticRect);
                    }

                }
            }
            foreach (var node in colHeaders)
            {
                if (node.Data.StaticCell != null)
                {
                    int RowHeaderIndex = 0;
                    int ColIndex = GetTreeNodeIndex(node, colHeaders);// fac.AddArrayToLeaf(rowHeaders, GetFmtArry(format.Colums, GetObjsToParent(node))) - 1;
                    string CalScope = GetCalScope(node,colBottom,ColIndex);
                    for (RowHeaderIndex = 0; RowHeaderIndex < rowBottom.Count; RowHeaderIndex++)
                    {
                        DataRect staticRect = new DataRect();
                        staticRect.ColHeaderIndex = ColIndex;
                        staticRect.RowHeaderIndex = RowHeaderIndex;
                        foreach (var staticCell in node.Data.StaticCell.StaticsCells)
                        {
                            staticRect.Cells.Add(new Cell()
                            {
                                Value = node.Data.StaticCell.Name + RowHeaderIndex,
                                CalDirection = CalDirection.Row,
                                CalScope = CalScope,
                                IsCalculate = true,
                                Expression = staticCell.StaticType

                            });
                        }

                        table.Cells.Add(staticRect);
                    }

                }
            }
            foreach(var colnode in colBottom )
            {
             

            }

            if (format.Colums.Count == 0 && format.Rows.Count == 0)
            {

            }
            return table;
        
        
        }

        private static void MakeTableHeader(List<CellFormat> fomatcells,
            System.Data.DataTable source, 
            List<List<SingleCell>> headers,
            Soway.Data.DS.Tree.Tree<MatrixHeader> tableHeaders)
        {
     
            Soway.Data.DS.Tree.ITreeFactory<MatrixHeader, MatrixHeader> fac = new Soway.Data.DS.Tree.ITreeFactory<MatrixHeader, MatrixHeader>(

                          (ob1, ob2) =>
                          {
                              return Soway.Data.DS.Tree.TeeNodeCompareResult.Child;
                          });
            if (fomatcells.Count > 0)
            {

                //按列区分出表格的列
                var sourceTable = source.DefaultView.ToTable(true, fomatcells.Select(p => p.SourceColumn).ToArray());
                //生成列表的树
                foreach (DataRow row in sourceTable.Rows)
                {
                    fac.AddArrayToLeaf(tableHeaders, GetFmtArry(fomatcells,row.ItemArray));
                }

                foreach (var node in tableHeaders)
                {
                    var objs = GetObjsToParent(node);
                    if (fomatcells[node.Level].StaticFormats.Count > 0)
                    {
                        foreach(var staticcell in fomatcells[node.Level].StaticFormats){
                            objs[node.Level] = staticcell.Name;
                            fac.AddArrayToLeaf(tableHeaders, GetFmtArry(fomatcells, objs,staticcell));
                        }
                    }
                }
        
                int level = -1;
                List<SingleCell> cells = null;
                foreach (var node in tableHeaders)
                {
                    if (node.Level > level)
                    {
                        level = node.Level;
                        cells = new List<SingleCell>();
                        headers.Add(cells);
                    }
                    var cell = new SingleCell()
                    {
                        Value = node.Data.Value.ToString(),
                        Span = node.Width
                    };
                    if (node.Data.Value == ReportEmptyValue.Value)
                    {
                        cell.MegerToParent = true;
                    }
                    cells.Add(cell);
                }
            }
        }


        public List<Cell> GetCells(MatrixTable table,int colStart = 0,int rowStart = 0) {

            List<Cell> result = new List<Cell>();
            int currentcol = colStart ;
            int currentrow=rowStart ;
            int lastCol = colStart;
            int lastRow=rowStart;
       
            for (int i =0;i <table.ColHeaders.Count;i++)//cols in table.ColHeaders)
            {
                var cols = table.ColHeaders[i];
                currentcol = colStart+ table.RowHeaders.Count;
                foreach(var col in cols){
                    var cell = new Cell()
                    {
                        ColSpan = col.Span,
                        Column = currentcol,
                        RowSpan = 1,
                        Row = currentrow,
                        Value = col.Value
                    };
                    if(col.MegerToParent ==false )
                        result.Add(cell);
                    else
                    {
                        var parent = result.FirstOrDefault(p => p.Column == currentcol && p.ColSpan == col.Span && p.Row < currentrow);
                        if(parent != null)
                        {
                            parent.RowSpan++;
                        }                         
                    }
                    currentcol += col.Span;
                }
                currentrow++;
            }

            currentcol = colStart;
            foreach (var rows in table.RowHeaders)
            {
                currentrow = rowStart + table.ColHeaders.Count;
                foreach (var row in rows)
                {

                    var cell = new Cell()
                    {
                        ColSpan = 1,
                        Column = currentcol,
                        Row = currentrow,
                        RowSpan = row.Span,
                        Value = row.Value
                    };
                    if(row.MegerToParent ==false )

                    result.Add(cell
                        );
                    else
                    {
                        var parent = result.FirstOrDefault(p => p.Row == currentrow && p.RowSpan == row.Span
                            && p.Column < currentcol);
                        if (parent != null)
                        {
                            parent.ColSpan++;
                        }
                    }

                    currentrow += row.Span;
                }
                currentcol++;
            }
            currentrow = rowStart + table.ColHeaders.Count;
            currentcol = colStart + table.RowHeaders.Count;
            foreach (var cells in table.Cells)
            {
                
                foreach (var cell in cells.Cells)
                {
                    result.Add(new Cell()
                    {
                        ColSpan = 1,
                        RowSpan = 1,
                        Column = cell.Column+currentcol+cells.ColHeaderIndex,
                        IsCalculate = cell.IsCalculate,
                        CalScope  = cell.CalDirection ==  CalDirection.Column ?
                        cell.GetScopeFromOffSet(currentrow )
                        :(cell.CalDirection == CalDirection.Row?
                            cell.GetScopeFromOffSet(currentcol ) :""),
                        Expression = cell.Expression,

                        CalDirection = cell.CalDirection,
                        Row = cell.Row+currentrow+cells.RowHeaderIndex,

                        Value = cell.Value
                    });
                }
            }
            return result;
        
        }
         
 

        private static MatrixHeader[] GetFmtArry(List<CellFormat> headers, object[] arrarys, StaticFormat staticFmt = null)
        {
            List<MatrixHeader> result = new List<MatrixHeader>();


            bool isStaticAdded = false; ;
            for (int i = 0; i < headers.Count; i++)
            {
                var item = new MatrixHeader()
                {
                    FormatCell = headers[i],
                    Value = i >= arrarys.Length ? ReportEmptyValue.Value : arrarys[i],
                   // StaticCell = staticFmt
                };
                if ((item.Value == ReportEmptyValue.Value
                    ||(staticFmt != null && item.Value == staticFmt.Name))&& !isStaticAdded)
                {
                    item.StaticCell = staticFmt;
                    isStaticAdded = true;
                }

                
              
                result.Add(item);

            }
            return result.ToArray();
        }


        private static object[] GetObjsToParent(Soway.Data.DS.Tree.TreeNode<MatrixHeader> node)
        {
            List<object> result = new List<object>();
            while (node != null) {
                result.Insert(0, node.Data.Value);
                node = node.Parent;
               
            }
            return result.ToArray();
        }

        private static int GetTreeNodeIndex (Soway.Data.DS.Tree.TreeNode<MatrixHeader> node,
            Soway.Data.DS.Tree.Tree<MatrixHeader> tree)
        {



            List<TreeNode<MatrixHeader>> items = null;
            if (node.Parent == null)
                items = tree.Nodes;
            else
                items = node.Parent.Children;
                int nodeIndex = items.IndexOf(node);

                int i = 0;
                int index = 0;
                while (i < nodeIndex)
                {
                    index += items[i].Width;

                i++;

                }

            if (node.Parent != null)
                return index + GetTreeNodeIndex(node.Parent, tree);
            else
                return index;
                
            
        }



   
        private string GetCalScope(TreeNode<MatrixHeader> node, List<TreeNode<MatrixHeader>> items,int EndWidh)
        {

            string result = "";
            int i = 0;
            int start = 0;
            while(i < EndWidh)
            {
                if (IsSameParent(items[i], node) ==false ){
                    start = i + 1;
                  

                }else
                {
                 
                    if (IsStatic(items[i]))
                    {

                        result += string.Format(",{0}-{1}", start, i-1);
                        start = i + 1;
                    }else
                    {
                         
                    }
                }
                i++;

            }
            if(start < i )
            result += string.Format(",{0}-{1}", start, i-1);



            return result.Substring(1);
        }
        private bool IsSameParent(TreeNode<MatrixHeader> nodeA, TreeNode<MatrixHeader> nodeB)
        {



            var rootA = GetRouteToParent(nodeA);
            var rootB = GetRouteToParent(nodeB);
            if (rootA.Count == 0 || rootB.Count == 0)
                return true;
            foreach (var item in rootA)
                foreach (var itemB in rootB)
                    if (item == itemB)
                        return true;
            return false;

        }
        private bool IsStatic(TreeNode<MatrixHeader> nodeA)
        {
            var node = nodeA;
            while(node != null)
            {
                if (node.Data.StaticCell != null)
                    return true;
                else
                    node = node.Parent;
            }
            return false;
        }
        private static  List<TreeNode<MatrixHeader>> GetRouteToParent(
            TreeNode<MatrixHeader> node)
    {
        List<TreeNode<MatrixHeader>> result = new List<TreeNode<MatrixHeader>>();
        var temp = node.Parent;
        while(temp != null)
        {
            result.Add(temp);
            temp = temp.Parent;
        }
        return result;
    }
 
    }
}
