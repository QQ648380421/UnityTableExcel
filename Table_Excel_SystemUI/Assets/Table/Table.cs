using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections.Specialized;
using System.Linq;
using UnityEngine.UI;
using static XP.TableModel.Cell;
using System;

namespace XP.TableModel
{
    //备忘录：添加一列或者添加一行，要同步向cellData里添加同样多的数据缓冲区
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
                return _CellDatas._MaxRowIndex;
            }
        }
        /// <summary>
        /// 最大列索引
        /// </summary>
        int _MaxColumnIndex
        {
            get
            {
                return _CellDatas._MaxColumnIndex;
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

        private void Start()
        {
            for (int i = 0; i < 10; i++)
            {
                _AddColumn(new HeaderCellData() { 
                 _Data="Column"+i,
                  _Index=i,
                   Width=300,
                    Higth=50
                });
            }
            for (int i = 0; i < 20; i++)
            {
                _AddRow(new HeaderCellData() { 
                 _Data="Row"+i,
                  _Index=i,
                   Width=300,
                   Higth=50
                });
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
        public void _AddColumn(HeaderCellData   headerCellData) {
            _HeaderColumn._Add(headerCellData);  
        }
        /// <summary>
        /// 添加一行
        /// </summary>
        /// <param name="columnCellData"></param>
        public void _AddRow(HeaderCellData headerCellData)
        {
            _HeaderRow._Add(headerCellData);
        }

        /// <summary>
        /// 删除行
        /// </summary>
        /// <param name="row"></param>
        public void _RemoveRow(int row)
        {
            _CellDatas._RemoveRow(row);
        }

        /// <summary>
        /// 删除列
        /// </summary>
        /// <param name="row"></param>
        public void _RemoveColum(int colum)
        {
            _CellDatas._RemoveColumn(colum);
        }
    
        /// <summary>
        /// 刷新表
        /// </summary>
        public virtual void _Refresh() {
            _ScrollRect.verticalScrollbar.value = 0;
            _ScrollRect.horizontalScrollbar.value = 0;
            _OnRefreshEvent?.Invoke(this,this);
        }


        //private void Update()
        //{
        //    if (Input.GetKeyDown( KeyCode.Space))
        //    {
        //        _Refresh();
        //    }
        //}
    }
}