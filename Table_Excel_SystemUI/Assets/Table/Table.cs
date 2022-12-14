using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using UnityEngine;
using System.Collections.Specialized;
using System.Linq;
using UnityEngine.UI;
using static XP.TableModel.Cell;
using System;

namespace XP.TableModel
{
    /// <summary>
    /// 表格
    /// </summary>
    public class Table : MonoBehaviour
    {
        /// <summary>
        /// 单元格数据
        /// </summary>
        public readonly CellDataCollection _CellDatas = new CellDataCollection();
        /// <summary>
        /// 最大行索引
        /// </summary>
        int _MaxRowIndex {
            get {
                return _HeaderRow._HeaderCellsCount;
            }
        }
        /// <summary>
        /// 最大列索引
        /// </summary>
        int _MaxColumnIndex
        {
            get
            {
                return _HeaderColumn._HeaderCellsCount;
            }
        }

        ScrollRect scrollRect;
        /// <summary>
        /// 滚动容器组件
        /// </summary>
        public ScrollRect _ScrollRect { get {
                if (!scrollRect)
                {
                    scrollRect = GetComponent<ScrollRect>();
                }
                return scrollRect;
            }   }
        /// <summary>
        /// 单元格滚动容器
        /// </summary>
        public RectTransform _CellContent {
            get {
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
        /// 所有实例单元格
        /// </summary>
        public  List<Cell> _Cells {
            get {
                return _CellView._Cells;
            }
        }
        [SerializeField]
        private bool lockHeaderColumnCells = true;
        /// <summary>
        /// 锁定表头列
        /// </summary>
        public bool _LockHeaderColumnCells { get => lockHeaderColumnCells; set => lockHeaderColumnCells = value; }

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
            _removeIndexBuffer = _removeIndexBuffer.OrderByDescending(p=>p).ToList();
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
        [SerializeField]
        private bool lockHeaderRowCells = true;
        /// <summary>
        /// 锁定表头行
        /// </summary>
        public bool _LockHeaderRowCells { get => lockHeaderRowCells; set => lockHeaderRowCells = value; }
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
        Cell this[int column,int row] {
            get {
                return _Cells.FirstOrDefault(p=>p!=null &&p._CellData._Column==column && p._CellData._Row==row);
            }
        }
        Cell this[CellData cellData]
        {
            get
            {
                return _Cells.FirstOrDefault(p => p != null && p._CellData._Column == cellData._Column && p._CellData._Row == cellData._Row);
            }
        }
        /// <summary>
        /// 当前选中单元格数据，可以监听该字段里的<see cref="ObservableCollection.CollectionChanged"/>事件
        /// </summary>
        public readonly ObservableCollection<CellData> _CurrentSelectedCellDatas=new ObservableCollection<CellData>();

        /// <summary>
        /// 刷新表格事件
        /// </summary>
        public event EventHandler<Table> _OnRefreshEvent;
        /// <summary>
        /// 获取行所有单元格
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        public IEnumerable<Cell> _GetRowCells(int row) {
          return  _Cells.Where(p=>p._CellData._Row== row);
        }
        /// <summary>
        /// 获取列所有单元格
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        public IEnumerable<Cell> _GetColumCells(int colum)
        {
            return _Cells.Where(p => p._CellData._Column == colum);
        }



        private void Awake()
        { 
            _CellDatas.CollectionChanged -= _CellDatas_CollectionChanged;
            _CellDatas.CollectionChanged += _CellDatas_CollectionChanged; 
        }
         
        private IEnumerator Start()
        {
            for (int i = 0; i < 10; i++)
            {
                yield return null;
                _AddColumn();
            }
            for (int i = 0; i < 40; i++)
            {
                yield return null;
                _AddRow();
            }
            int index=0; 
            foreach (var item in _CellDatas)
            {
                item._Data = index; 
                index++;
            }
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
        public HeaderCellBase _AddColumn(HeaderCellData   headerCellData) {
          return  _HeaderColumn._Add(headerCellData);  
        }
        /// <summary>
        /// 添加一行
        /// </summary>
        /// <param name="columnCellData"></param>
        public HeaderCellBase _AddRow(HeaderCellData headerCellData)
        {
         return   _HeaderRow._Add(headerCellData);
        }

        /// <summary>
        /// 添加一列
        /// </summary>
        /// <param name="columnCellData"></param>
        public HeaderCellBase _AddColumn()
        {
            HeaderCellData headerCellData = new HeaderCellData();
            headerCellData._Index = _MaxColumnIndex;
            headerCellData._Data = "Column" + headerCellData._Index; 
           return    _AddColumn(headerCellData);
        }
        /// <summary>
        /// 添加一行
        /// </summary>
        /// <param name="columnCellData"></param>
        public HeaderCellBase _AddRow()
        {
            HeaderCellData headerCellData = new HeaderCellData();
            headerCellData._Index = _MaxRowIndex;
            headerCellData._Data = "Row" + headerCellData._Index;
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
        public void _ResetScroll() {
            _ScrollRect.verticalScrollbar.value = 1;
            _ScrollRect.horizontalScrollbar.value = 0;
        }
        /// <summary>
        /// 刷新表
        /// </summary>
        public virtual void _Refresh() {
           
            _OnRefreshEvent?.Invoke(this,this);
        }


        //private void Update()
        //{
        //    if (Input.GetKeyDown(KeyCode.Space))
        //    {
        //        _CellDatas[1, 1]._Data = "Hello!";
        //    }
        //}
    }
}