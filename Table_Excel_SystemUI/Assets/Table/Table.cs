using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using static XP.TableModel.Cell;

namespace XP.TableModel
{

    /// <summary>
    /// 表格
    /// </summary>
    public class Table : MonoBehaviour
    {
        /// <summary>
        /// 当表格被点击时触发
        /// </summary>
        public event _CellClickDelegate _OnCellClickEvent;
        /// <summary>
        /// 触发单元格被点击事件
        /// </summary>
        /// <param name="cellClickData"></param>
        public virtual void _Invoke_OnCellClickEvent(CellClickData cellClickData)
        {
            _OnCellClickEvent?.Invoke(cellClickData);
        }
        public CellData this[int x,int y]
        {
            get {
                return _GetCellData(x,y);
            } 
        }
        /// <summary>
        /// 单元格数据
        /// </summary>
        public readonly CellDataCollection _CellDatas = new CellDataCollection();
 
        /// <summary>
        /// 表头控制器
        /// </summary>
        public HeaderColumn _HeaderColumn;
        /// <summary>
        /// 表头控制器
        /// </summary>
        public HeaderRow _HeaderRow;
        /// <summary>
        /// 允许多选发生变化事件
        /// </summary>
        public event System.EventHandler<bool> _MultiSelectChangedEvent;
        [SerializeField]
        bool multiSelect;
        /// <summary>
        /// 允许多选
        /// </summary>
        public bool _MultiSelect
        {
            get
            {
                return multiSelect;
            }
            set
            {
                if (multiSelect == value) return;
                multiSelect = value;
                _MultiSelectChangedEvent?.Invoke(this, value);
            }
        } 
        /// <summary>
        /// 当前选中单元格数据，可以监听该字段里的<see cref="ObservableCollection.CollectionChanged"/>事件
        /// </summary>
        public readonly ObservableCollection<CellData> _CurrentSelectedCellDatas = new ObservableCollection<CellData>();

        /// <summary>
        /// 刷新表格事件
        /// </summary>
        public event EventHandler<Table> _OnRefreshEvent;
        ScrollRect scrollRect;
        /// <summary>
        /// 滚动容器组件
        /// </summary>
        public ScrollRect _ScrollRect
        {
            get
            {
                if (!scrollRect)
                {
                    scrollRect = GetComponent<ScrollRect>();
                }
                return scrollRect;
            }
        }
        /// <summary>
        /// 单元格滚动容器
        /// </summary>
        public RectTransform _CellContent
        {
            get
            {
                return _ScrollRect.content;
            }
        }
        /// <summary>
        /// 单元格预制体
        /// </summary>
        public Cell _CellPrefab;
        /// <summary>
        /// 单元格容器
        /// </summary>
        public CellView _CellView;
 
        /// <summary>
        /// 绑定列
        /// </summary>
        private void _BindColumns(List<ColumnAttributeData>  columnAttributeDatas) { 
            for (int i = 0; i < columnAttributeDatas.Count; i++)
            { 
               var _column=   _AddColumn();
                var columnData= columnAttributeDatas.FirstOrDefault(p=>p._ColumnAttribute._Index==i); 
                _column._ColumnAttributeData = columnData; 
            } 
        }
        /// <summary>
        /// 绑定行数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cellData"></param>
        /// <param name="rowAttributeDatas"></param>
        /// <param name="item"></param>
        private void _BindRowData<T>(CellData cellData, List<ColumnAttributeData> rowAttributeDatas,T item)
        {
            if (item == null) return;
            //找到当前列特性数据
            var _attributeData = rowAttributeDatas.FirstOrDefault(p => p._ColumnAttribute._Index == cellData._Column); 
            if (_attributeData == null) return;
     
            var _data=  _attributeData._PropertyInfo.GetValue(item);
            cellData._ShowData = _data;
            cellData._Data = item;
        }
        /// <summary>
        /// 绑定行数据
        /// </summary>
        private void _BindRows<T>(List<ColumnAttributeData> rowAttributeDatas, ICollection<T> arrayData)
        {
            int rowIndex = 0;
                foreach (var item in arrayData)
                {
                    var _row = _AddRow(); 
                    var _cellDatas = _CellDatas._GetRowCellDatas(_row._Index);
                foreach (var cellDataItem in _cellDatas)
                    {
                        _BindRowData(cellDataItem, rowAttributeDatas, item);
                      
                     } 
                rowIndex++;
            } 
        }
      
        /// <summary>
        /// 绑定数组数据
        /// </summary>
        /// <param name="arrayData"></param>
        public void _BindArray<T>(ICollection<T> arrayData)
        { 
            _ClearTable();
            if (arrayData == null) return;
            List<ColumnAttributeData> _ColumnAttributeDatas =  ColumnAttribute.GetColumnAttributeDatas<T>();
            if (_ColumnAttributeDatas == null || _ColumnAttributeDatas.Count <= 0) return;
            _BindColumns(_ColumnAttributeDatas);
            _BindRows(_ColumnAttributeDatas, arrayData);
           
        }


        /// <summary>
        /// 删除选中行
        /// </summary>
        public void RemoveSelectedRow()
        {
            List<int> _removeIndexBuffer = new List<int>();
            foreach (var item in _CurrentSelectedCellDatas)
            {
                if (!_removeIndexBuffer.Contains(item._Row))
                {
                    _removeIndexBuffer.Add(item._Row);
                }
            }
            _removeIndexBuffer = _removeIndexBuffer.OrderByDescending(p => p).ToList();
            foreach (var item in _removeIndexBuffer)
            {
                _RemoveRow(item);
            }
        }

        /// <summary>
        /// 删除选中列
        /// </summary>
        public void RemoveSelectedColumn()
        {
            List<int> _removeIndexBuffer = new List<int>();
            foreach (var item in _CurrentSelectedCellDatas)
            {
                if (!_removeIndexBuffer.Contains(item._Column))
                {
                    _removeIndexBuffer.Add(item._Column);
                }
            }
            _removeIndexBuffer = _removeIndexBuffer.OrderByDescending(p => p).ToList();
            foreach (var item in _removeIndexBuffer)
            {
                _RemoveColum(item);
            }
        }
 
       
        /// <summary>
        /// 获取行所有单元格数据
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        public IEnumerable<CellData> _GetRowCellDatas(int row)
        {
            return _CellDatas._GetRowCellDatas(row);
        }
        /// <summary>
        /// 获取单元格数据
        /// </summary>
        /// <param name="cellIndex"></param>
        /// <returns></returns>
        public CellData _GetCellData(Vector2Int cellIndex) {
         return   _CellDatas[cellIndex];
        }

        /// <summary>
        /// 获取单元格数据
        /// </summary>
        /// <param name="cellIndex"></param>
        /// <returns></returns>
        public CellData _GetCellData(int x  ,int y)
        {
            return _CellDatas[x,y];
        }


        /// <summary>
        /// 获取列所有单元格数据
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        public IEnumerable<CellData> _GetColumCellDatas(int colum)
        {
            return _CellDatas._GetColumCellDatas(colum);
        }
         
        private void Awake()
        { 
            _CellDatas.CollectionChanged -= _CellDatas_CollectionChanged;
            _CellDatas.CollectionChanged += _CellDatas_CollectionChanged;
        }
 

        /// <summary>
        /// 当单元格列表发生变化时触发
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _CellDatas_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    break;
                case NotifyCollectionChangedAction.Move:
                    break;
                case NotifyCollectionChangedAction.Remove:
                    var _removeCell = e.OldItems[0] as CellData;
                    if (_removeCell != null && _CurrentSelectedCellDatas.Contains(_removeCell))
                    {
                        _CurrentSelectedCellDatas.Remove(_removeCell);
                    }
                    break;
                case NotifyCollectionChangedAction.Replace:
                    break;
                case NotifyCollectionChangedAction.Reset:
                    break;
                default:
                    break;
            }

        }

        /// <summary>
        /// 添加一列
        /// </summary>
        /// <param name="columnCellData"></param>
        public HeaderCellData _AddColumn(HeaderCellData headerCellData)
        {
            return _HeaderColumn._Add(headerCellData);
        }
        /// <summary>
        /// 添加一行
        /// </summary>
        /// <param name="columnCellData"></param>
        public HeaderCellData _AddRow(HeaderCellData headerCellData)
        {
            return  _HeaderRow._Add(headerCellData);
        }

        /// <summary>
        /// 添加一列
        /// </summary>
        /// <param name="columnCellData"></param>
        public HeaderCellData _AddColumn()
        {
            HeaderCellData headerCellData = new HeaderCellData();  
            headerCellData._ShowData = "Column" +  _HeaderColumn._HeaderCellDatas.Count;
            headerCellData._Size = 200;
            return _AddColumn(headerCellData);
        }
        /// <summary>
        /// 添加一行
        /// </summary>
        /// <param name="columnCellData"></param>
        public HeaderCellData _AddRow()
        {
            HeaderCellData headerCellData = new HeaderCellData(); 
            headerCellData._ShowData =  _HeaderRow._HeaderCellDatas.Count+1;
            headerCellData._Size = 50;
            return _AddRow(headerCellData);
        }

        /// <summary>
        /// 删除行
        /// </summary>
        /// <param name="row"></param>
        public void _RemoveRow(int row)
        {
            _HeaderRow._Remove(row);
        }

        /// <summary>
        /// 删除列
        /// </summary>
        /// <param name="row"></param>
        public void _RemoveColum(int colum)
        {
            _HeaderColumn._Remove(colum);
        }
        /// <summary>
        /// 重置滑动条
        /// </summary>
        public void _ResetScroll()
        {
            _ScrollRect.verticalScrollbar.value = 1;
            _ScrollRect.horizontalScrollbar.value = 0;
        }
        /// <summary>
        /// 刷新表
        /// </summary>
        public virtual void _Refresh()
        {

            _OnRefreshEvent?.Invoke(this, this);
        }
         
        /// <summary>
        /// 清空所有行
        /// </summary>
        public virtual void _ClearRows()
        {
            while (_HeaderRow._HeaderCellsCount > 0)
            { 
                _RemoveRow(_HeaderRow._HeaderCellsCount - 1);
            }
        }
        /// <summary>
        /// 清空所有列
        /// </summary>
        public virtual void _ClearColumns()
        {
            while (_HeaderColumn._HeaderCellsCount > 0)
            { 
                _RemoveColum(_HeaderColumn._HeaderCellsCount - 1);
            }
        }
        /// <summary>
        /// 清空整个表格
        /// </summary>
        public virtual void _ClearTable() {
            _ClearRows();
            _ClearColumns(); 
        }

        /// <summary>
        /// 给单元格赋值
        /// </summary>
        /// <param name="column">单元格列</param>
        /// <param name="row">单元格行</param>
        /// <param name="showData">要显示的数据</param>
        ///        /// <param name="data">该单元格关联的数据</param>
        public virtual void _SetCellData(int column,int row,object showData,object data=null) {
            var _cellData= this[column, row];
            if (_cellData == null) return; 
            _cellData._ShowData=showData;
            _cellData._Data = data;
        }
    }
}