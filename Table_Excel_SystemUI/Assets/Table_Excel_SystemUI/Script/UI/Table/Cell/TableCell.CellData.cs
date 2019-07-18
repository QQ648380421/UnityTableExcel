using UnityEngine;
using UnityEngine.Events;
using System.Linq;
using ColumnData = Xp_Table_V1.TableColumnController.ColumnData;
using RowData = Xp_Table_V1.TableRowController.RowData;
using System;

namespace Xp_Table_V1
{
    public partial class TableCell
    { 
        /// <summary>
        /// 单元格数据类
        /// </summary> 
        public class CellData : DataBase
        {
            public delegate bool ChangeContentAction(string content, CellData data);
            /// <summary>
            /// 当内容发生变化
            /// </summary>
            public event ChangeContentAction ContentChange;
            /// <summary>
            /// 当宽高发生变化
            /// </summary>
            public event UnityAction<CellData, Vector2> SizeChange;
            /// <summary>
            /// 重新刷新单元格
            /// </summary>
            public event UnityAction<CellData> RefreshEvent;
            /// <summary>
            /// 当单元格为空状态改变时
            /// </summary>
            public event UnityAction<CellData, bool> IsNullChangeEvent;
            /// <summary>
            /// 合并数据事件
            /// </summary>
            public event UnityAction<CellData,Vector2> MergeDataEvent;
            /// <summary>
            /// 绑定行发生变化
            /// </summary>
            public event UnityAction<RowData> BindRowChangeEvent;
            /// <summary>
            /// 绑定列发生变化
            /// </summary>
            public event UnityAction<ColumnData> BindColumnChangeEvent;
            /// <summary>
            /// 索引值被改变
            /// </summary>
            public event UnityAction<CellData, Vector2>  IndexChange;

            TableCell tableCell;
            RowData rowData;
             ColumnData columnData;
            string content;
            int rowIndex;
            int columnIndex;
            float rowHeigth=35;
            float columnWidth=100;
            int rowMerge;
            int columnMerge;
            bool isNull=false;
            object tag;
            CellData mergeCell;
            public CellData(TableController tableController, TableCell tableCell,int columnIndex,int rowIndex,string content) : base(tableController)
            {
                TableCell = tableCell;
                tableCell.Data = this;
                ColumnIndex = columnIndex;
                RowIndex = rowIndex;
                Content = content;
            }

            public CellData(TableController tableController, TableCell tableCell, int columnIndex, int rowIndex, string content,float rowHeigth, float columnWidth, int rowMerge, int columnMerge, bool isNull):this(
                 tableController,  tableCell,  columnIndex,  rowIndex,  content
                )
            {
                RowHeigth = rowHeigth;
                ColumnWidth = columnWidth;
                RowMerge = rowMerge;
                ColumnMerge = columnMerge;
                IsNull = isNull;
            }


            /// <summary>
            /// 单元格的内容
            /// </summary>
            public string Content
            {
                get
                {
                    return content;
                }

                set
                {
                    if (value == content) return;
                    if (ContentChange!=null)
                    { 
                        if (ContentChange.Invoke(value, this))
                            content = value;
                    } 
                }
            }
            /// <summary>
            /// 单元格的行索引
            /// </summary>
            public int RowIndex
            {
                get
                {
                    return rowIndex;
                }

                set
                {
                    if (RowData!=null)
                    {
                        RowData.CellDatas.Remove(this);
                        var row = TableController.Data.RowDatas[value];
                        RowData = row;
                    } 
                    rowIndex = value;
                    if (IndexChange!=null)
                    {
                        IndexChange.Invoke(this,GetCellPosition());
                    }
             
                }
            }
            /// <summary>
            /// 单元格的列索引
            /// </summary>
            public int ColumnIndex
            {
                get
                {
                    return columnIndex;
                }

                set
                {
                    if (ColumnData!=null)
                    {
                        ColumnData.CellDatas.Remove(this);
                        var columnn = TableController.Data.ColumnDatas[value];
                        ColumnData = columnn;
                    } 
                
                    columnIndex = value;

                    if (IndexChange != null)
                    {
                        IndexChange.Invoke(this,GetCellPosition());
                    }
                }
            }
            /// <summary>
            /// 获取单元格的XY
            /// </summary>
            /// <returns></returns>
            private Vector2 GetCellPosition()
            {
                Vector2 v2 = Vector2.zero;
                var rows = TableController.Data.RowDatas.Where(p => p.RowIndex<RowIndex).ToArray();
                var columns  = TableController.Data.ColumnDatas.Where(p => p.ColumnIndex < ColumnIndex).ToArray();
              
                foreach (var item in columns)
                {
                    v2.x += item.Width;
                }
                foreach (var item in rows)
                {
                    v2.y += item.Heigth;
                }
                return v2;
            }

            /// <summary>
            /// 单元格的行高度
            /// </summary>
            public float RowHeigth
            {
                get
                {
                    return rowHeigth;
                }

                set
                {
                    rowHeigth = value;
                    if (SizeChange != null)
                    {
                        SizeChange.Invoke(this,HandlerMergeCells());
                    }
                }
            }
            /// <summary>
            /// 单元格的列宽度
            /// </summary>
            public float ColumnWidth
            {
                get
                {
                    return columnWidth;
                }

                set
                {
                    columnWidth = value;
                    if (SizeChange != null)
                    {
                        SizeChange.Invoke(this,HandlerMergeCells());
                    }
                }
            }
            /// <summary>
            /// 单元格的合并行
            /// </summary>
            public int RowMerge
            {
                get
                {
                    return rowMerge;
                }

                set
                {
                    if (value < 0) return; 
                    rowMerge = value;
                    if (MergeDataEvent != null)
                    {
                        MergeDataEvent.Invoke(this,HandlerMergeCells());
                    } 
                }
            }
          
            /// <summary>
            /// 单元格的合并列
            /// </summary>
            public int ColumnMerge
            {
                get
                {
                    return columnMerge;
                }

                set
                {
                    if (value < 0) return; 
                    columnMerge = value;
                    if (MergeDataEvent != null)
                    {
                        MergeDataEvent.Invoke(this,HandlerMergeCells());
                    } 

                }
            }
          
            /// <summary>
            /// 绑定的表格
            /// </summary>
            public TableCell TableCell
            {
                get
                {
                    return tableCell;
                }

                set
                {
                    tableCell = value;
                }
            }
            /// <summary>
            /// 关联的行
            /// </summary>
            public  RowData RowData
            {
                get
                {
                    return rowData;
                }

                set
                {
                    if (BindRowChangeEvent != null)
                    {
                        BindRowChangeEvent.Invoke(value);
                    }
                    if (rowData!=null)
                    {
                        rowData.HeigthChange += RowData_HeigthChange;
                    }
          
                    rowData = value;
                    rowData.HeigthChange -= RowData_HeigthChange;
                    rowData.HeigthChange += RowData_HeigthChange;
                }
            }
            /// <summary>
            /// 关联的列
            /// </summary>
            public  ColumnData ColumnData
            {
                get
                {
                    return columnData;
                }

                set
                {
                    if (BindColumnChangeEvent!=null)
                    {
                        BindColumnChangeEvent.Invoke(value);
                    }
                    if (columnData!=null)
                    {
                        columnData.WidthChange -= ColumnData_WidthChange;
                    } 
                    columnData = value;
                    columnData.WidthChange -= ColumnData_WidthChange;
                    columnData.WidthChange += ColumnData_WidthChange;
                }
            }
            /// <summary>
            /// 是否为空的单元格
            /// </summary>
            public bool IsNull
            {
                get
                {
                    return isNull;
                } 
                set
                {
                    if (value == isNull) return;
                    isNull = value;
                    if (IsNullChangeEvent!=null)
                    {
                        IsNullChangeEvent.Invoke(this,value);
                    }
                }
            }
            /// <summary>
            /// 是谁合并了这个单元格？
            /// </summary>
            public CellData MergeCell
            {
                get
                {
                    return mergeCell;
                }

                set
                {
                    mergeCell = value;
                }
            }
            /// <summary>
            /// 存储该单元格对应的数据
            /// </summary>
            public object Tag
            {
                get
                {
                    return tag;
                }

                set
                {
                    tag = value;
                }
            }



            /// <summary>
            /// 当列宽发生变化
            /// </summary>
            /// <param name="obj"></param>
            private void ColumnData_WidthChange(float width)
            {
                ColumnWidth = width;
            }
            /// <summary>
            /// 当行高发生变化
            /// </summary>
            /// <param name="obj"></param>
            private void RowData_HeigthChange(float heigth)
            {
                RowHeigth = heigth;
            }
            /// <summary>
            /// 刷新单元格
            /// </summary>
            public void Refresh() { 
                if (MergeDataEvent != null)
                {
                    MergeDataEvent.Invoke(this, HandlerMergeCells());
                }
                if (RefreshEvent != null)
                {
                    RefreshEvent.Invoke(this);
                }
            }

           

            #region 合并  
            /// <summary>
            /// 处理合并单元格
            /// </summary>
            /// <param name="isNull"></param>
            public Vector2 HandlerMergeCells() {
                
               var cells = TableController.Data.CellDatas.Where(  p => p.RowIndex >= RowIndex && p.RowIndex <= RowIndex + RowMerge  && p.ColumnIndex >= ColumnIndex && p.ColumnIndex <= ColumnIndex + ColumnMerge  );
                Vector2 v2 = Vector2.zero;
                if (RowData==null || ColumnData==null)
                {
                    v2.x = ColumnWidth;
                    v2.y = RowHeigth;
                    return v2;
                }
                //遍历计算宽高*****************************************
                var columns = RowData.CellDatas.Where(p => p.ColumnIndex >= ColumnIndex && p.ColumnIndex <= ColumnIndex + ColumnMerge);
                var rows = ColumnData.CellDatas.Where(p => p.RowIndex >= RowIndex && p.RowIndex <= RowIndex + RowMerge);

                foreach (var item in columns)
                {
                    v2.x += item.ColumnWidth;
                }
                foreach (var item in rows)
                {
                    v2.y += item.RowHeigth;
                }
                //如果本身已经被隐藏了
                if (IsNull) {
                    //那么还得去刷新合并了的单元格
                    if (MergeCell!=null)
                    { 
                        MergeCell.TableCell.UpdateCellPosition(MergeCell);
                       var MergeCellV2=  MergeCell.HandlerMergeCells();
                        if (MergeCell.MergeDataEvent!=null)
                        {
                            MergeCell.MergeDataEvent.Invoke(MergeCell, MergeCellV2);
                        }
                    
                    }
                    return v2;
                }
                foreach (var item in cells)
                {
                    if (item == this)
                    {
                        item.IsNull = false;
                    }
                    else
                    {
                        item.IsNull = true;
                        item.MergeCell = this;
                    }
              
                }
                return v2; 
            }
        
            #endregion
        }
    }
}