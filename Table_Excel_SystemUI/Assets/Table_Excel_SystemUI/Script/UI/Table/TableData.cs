using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;
using System.Collections.ObjectModel;
using UnityEngine.Events;
using CellData = Xp_Table_V1.TableCell.CellData;
using RowData = Xp_Table_V1.TableRowController.RowData;
using ColumnData = Xp_Table_V1.TableColumnController.ColumnData;
using Xp_ListEvent;

namespace Xp_Table_V1
{

    /// <summary>
    /// 该类描述：表格数据类
    /// 负责人:夏鹏
    /// 联系方式(QQ):648380421
    /// 时间：
    /// </summary>  
    public class TableData: DataBase
    {
        public CellData this[int columnIndex, int rowIndex] { 
            get {
                return CellDatas.FirstOrDefault(p=>p.ColumnIndex==columnIndex && p.RowIndex==rowIndex); 
            } 
        }

        RectTransform rect_Content;
        ListEvent<CellData> cellDatas=new ListEvent<CellData>();
        ListEvent<RowData> rowDatas= new ListEvent<RowData>();
        ListEvent<ColumnData> columnDatas= new ListEvent<ColumnData>();
        /// <summary>
        /// 这张表的大小要刷新
        /// </summary>
        public event UnityAction  SizeChangeEvent;
        /// <summary>
        /// 结束拖拽事件
        /// </summary>
        public event UnityAction<ColumnData, float> ColumnEndDrag;
        /// <summary>
        /// 结束拖拽事件
        /// </summary>
        public event UnityAction<RowData, float> RowEndDrag;
        /// <summary>
        ///合并单元格事件
        /// </summary>
        public event UnityAction<CellData, Vector2> MergeEvent;

        /// <summary>
        /// 一张表中的所有的单元格
        /// </summary>
        public ListEvent<CellData> CellDatas
        {
            get
            {
                return cellDatas;
            }

            set
            {
                cellDatas = value; 
            }
        }
        /// <summary>
        /// 一张表中的所有行
        /// </summary>
        public ListEvent<RowData> RowDatas
        {
            get
            {
                return rowDatas;
            }

            set
            {
                rowDatas = value;
            }
        }
        /// <summary>
        /// 一张表中的所有列
        /// </summary>
        public ListEvent<ColumnData> ColumnDatas
        {
            get
            {
                return columnDatas;
            }

            set
            {
                columnDatas = value;
            }
        }
        /// <summary>
        /// 这张表的单元格容器，用来刷新宽高
        /// </summary>
        public RectTransform Rect_Content
        {
            get
            {
                return rect_Content;
            }

            set
            {
                rect_Content = value;
            }
        }
        /// <summary>
        /// 滚动容器的父对象
        /// </summary>
        private RectTransform ScrollContentParent;
        public TableData(TableController tableController, RectTransform rect_Content) : base(tableController)
        {
            Rect_Content = rect_Content;
            CellDatas.ClearEvent += CellDatas_ClearEvent;
            CellDatas.InsertEvent += CellDatas_InsertEvent;
            CellDatas.RemoveEvent += CellDatas_RemoveEvent;
            CellDatas.ValueChange += CellDatas_ValueChange; 
            RowDatas.InsertEvent += RowDatas_InsertEvent; 
            ColumnDatas.InsertEvent += ColumnDatas_InsertEvent; 
        }
      

        /// <summary>
        /// 强制刷新所有单元格
        /// </summary>
        internal void UpdateAllCell()
        {
            foreach (var item in CellDatas)
            {
                item.Refresh();
            }
        }

        #region ColumnData事件 
     
        private bool ColumnDatas_InsertEvent(int index, ColumnData value)
        {
            value.WidthChange -= Value_WidthChange;
            value.WidthChange += Value_WidthChange;
            value.EndDragEvent -= Column_EndDragEvent;
            value.EndDragEvent += Column_EndDragEvent;
            return true; 
        }
        /// <summary>
        /// 列结束拖拽
        /// </summary>
        /// <param name="arg1"></param>
        /// <param name="arg2"></param>
        private void Column_EndDragEvent(ColumnData arg1, float arg2)
        {
            if (ColumnEndDrag!=null)
            {
                ColumnEndDrag.Invoke(arg1, arg2);
            }
        }

        /// <summary>
        /// 宽高发生变化
        /// </summary>
        /// <param name="obj"></param>
        private void Value_WidthChange(float obj)
        {
            RefreshSize();
        }
        
        #endregion

        #region RowData事件
   
        private bool RowDatas_InsertEvent(int index, RowData value)
        {
            value.HeigthChange -= Value_HeigthChange;
            value.HeigthChange += Value_HeigthChange;
            value.EndDragEvent -= Row_EndDragEvent;
            value.EndDragEvent += Row_EndDragEvent;
            return true;
        }

        private void Row_EndDragEvent(RowData arg1, float arg2)
        {
            if (RowEndDrag!=null)
            {
                RowEndDrag.Invoke(arg1,arg2);
            }
        }

        /// <summary>
        /// 改变宽高
        /// </summary>
        /// <param name="obj"></param>
        private void Value_HeigthChange(float obj)
        {
            RefreshSize();
        }

   
        #endregion

        #region CellData事件 
        private bool CellDatas_ValueChange(int index, CellData origingValue, CellData value)
        {//修改数据的话，直接把之前的删掉，换一个新的 
            origingValue.RowData.CellDatas.Remove(origingValue);
            origingValue.ColumnData.CellDatas.Remove(origingValue);
            CellDatas_InsertEvent(index, value);
            if (origingValue == value) return false;//一样的数据直接不给修改
            GameObject.Destroy(origingValue.TableCell.gameObject);
            return true;
        }

        private bool CellDatas_RemoveEvent(int index, CellData value)
        {
            value.RowData.CellDatas.Remove(value);
            value.ColumnData.CellDatas.Remove(value);
            GameObject.Destroy(value.TableCell.gameObject);
            RefreshSize(); 
            return true;
        }

        private bool CellDatas_InsertEvent(int index, CellData value)
        {
            value.MergeDataEvent -= Value_MergeDataEvent;
            value.MergeDataEvent += Value_MergeDataEvent;

            #region 处理行
            var row = RowDatas.FirstOrDefault(p=>p.RowIndex==value.RowIndex);
            if (row==null)
            {
                var rowButton = TableController.RowController.Create<TableRowButton>();
                row = new RowData(TableController, rowButton, value.RowIndex)
                {  
                    RowIndex= value.RowIndex
                }; 
                value.RowData = row;
                row.Heigth = value.RowHeigth;
                RowDatas.Add(row);
            }
            row.CellDatas.Add(value);
            value.RowData = row;
            #endregion
            #region 处理列
            var column = ColumnDatas.FirstOrDefault(p => p.ColumnIndex == value.ColumnIndex);
            if (column == null)
            {
                var columnButton = TableController.ColumnController.Create<TableColumnButton>();
                column = new ColumnData(TableController, columnButton, value.ColumnIndex)
                { 
                      ColumnIndex=value.ColumnIndex
                }; 
                value.ColumnData = column;
                column.Width = value.ColumnWidth;
                ColumnDatas.Add(column);
            }
            column.CellDatas.Add(value);
            value.ColumnData = column;
            #endregion 
            RefreshSize();
            value.Refresh(); 
            return true;
        }

        /// <summary>
        /// 触发合并单元格事件
        /// </summary>
        /// <param name="arg0"></param>
        /// <param name="arg1"></param>
        private void Value_MergeDataEvent(CellData arg0, Vector2 arg1)
        {
            if (MergeEvent!=null)
            {
                MergeEvent.Invoke(arg0, arg1);
            }
        }


        /// <summary>
        /// 清空单元格
        /// </summary>
        /// <returns></returns>
        private bool CellDatas_ClearEvent()
        {
            foreach (var item in CellDatas)
            {
                if (item!=null && item.TableCell)
                {
                    GameObject.Destroy(item.TableCell.gameObject);
                } 
            }
            RowDatas.Clear();
            ColumnDatas.Clear();
            RefreshSize();
            
            return true;
        }
        #endregion

        #region 刷新这张表的宽高
        /// <summary>
        /// 刷新这张表的宽高
        /// </summary>
        private void RefreshSize() {
            Vector2 size = new Vector2(20,20);
            foreach (var item in ColumnDatas)
            {
                size.x += item.Width;
            }
            foreach (var item in RowDatas)
            {
                size.y += item.Heigth;
            }
            
            Rect_Content.sizeDelta = size;
             
            if (SizeChangeEvent!=null)
            {
                SizeChangeEvent.Invoke();
            }
        }
        /// <summary>
        /// 刷新表
        /// </summary>
        public void RefreshAllCell() {
            foreach (var item in CellDatas)
            {
                item.Refresh();
            }
        }


      
        /// <summary>
        /// 滚动容器滑动
        /// </summary>
        /// <param name="v2"></param>
        internal void ScrollDragChange(RectTransform scrollContent,Vector2 v2)
        {
            if (!ScrollContentParent)
            {
                ScrollContentParent = scrollContent.parent.GetComponent<RectTransform>();
            }

            //滚动区域的最小边界
            var rect_min = scrollContent.anchoredPosition.y-0.1f;
            //滚动区域的最大边界
            var rect_max = scrollContent.anchoredPosition.y + ScrollContentParent.rect.height+0.1f;

            for (int i = 0; i < CellDatas.Count; i++)
            {

                var item = CellDatas[i];
                if (item.IsNull)
                {//如果已经是不显示的
                    continue;
                }
                var item_rect = item.TableCell.RectTransform;
                float cellMaxSize_y =(-item_rect.anchoredPosition.y) + item_rect.sizeDelta.y;
                bool show =! (cellMaxSize_y < rect_min || (-item_rect.anchoredPosition.y) > rect_max);
              
                item.TableCell.gameObject.SetActive(show);
            
            }
        }
        #endregion

        /// <summary>
        /// 添加一个单元格
        /// </summary>
        /// <param name="columnIndex"></param>
        /// <param name="rowIndex"></param>
        /// <param name="content"></param>
        public CellData AddCell(int columnIndex ,int rowIndex,string content) {
           var cell = TableController.CellController.Create<TableCell>();
            var data = CellDatas.FirstOrDefault(p=>p.ColumnIndex==columnIndex && p.RowIndex==rowIndex);
            if (data!=null)
            {
                data.Content = content;
                return data;
            }
          
            data =   new CellData(TableController, cell, columnIndex, rowIndex, content);
            if (columnIndex< ColumnDatas.Count)
            {
                var column = ColumnDatas[columnIndex];
                data.ColumnWidth = column.Width;
            }
            if (rowIndex < RowDatas.Count)
            {
                var row = RowDatas[rowIndex];
                data.RowHeigth = row.Heigth;
            } 
            CellDatas.Add(data);
            return data;
        }
        /// <summary>
        /// 添加一个单元格
        /// </summary>
        /// <param name="columnIndex"></param>
        /// <param name="rowIndex"></param>
        /// <param name="content"></param>
        public CellData AddCell(int columnIndex, int rowIndex, string content, float rowHeigth, float columnWidth, int rowMerge, int columnMerge, bool isNull)
        {
            var cell = TableController.CellController.Create<TableCell>();
            var data = CellDatas.FirstOrDefault(p => p.ColumnIndex == columnIndex && p.RowIndex == rowIndex);
            if (data != null)
            {
                data.Content = content;
                return data;
            }

            data = new CellData(TableController, cell, columnIndex, rowIndex, content ,  rowHeigth,  columnWidth,  rowMerge,  columnMerge,  isNull);
            if (columnIndex < ColumnDatas.Count)
            {
                var column = ColumnDatas[columnIndex];
                data.ColumnWidth = column.Width;
            }
            if (rowIndex < RowDatas.Count)
            {
                var row = RowDatas[rowIndex];
                data.RowHeigth = row.Heigth;
            }
            CellDatas.Add(data);
            return data;
        }

    }
}