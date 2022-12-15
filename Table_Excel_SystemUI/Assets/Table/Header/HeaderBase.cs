using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.ObjectModel;
namespace XP.TableModel
{
    /// <summary>
    /// 表头基类
    /// </summary>
    public abstract class HeaderBase : MonoBehaviour
    {
     
        Table _table;
        /// <summary>
        /// 表格控制器
        /// </summary>
        public Table _Table { get {
                if (!_table)
                {
                    _table = GetComponentInParent<Table>();
                }
                return _table;
            }  }
        RectTransform rectTransform;
        /// <summary>
        /// 本身变换组件
        /// </summary>
       public  RectTransform _RectTransform {
            get {
                if (!rectTransform)
                {
                    rectTransform = GetComponent<RectTransform>();
                }
                return rectTransform;
            }
        }

        /// <summary>
        /// 表头单元格预制体
        /// </summary>
        public HeaderCellBase _HeaderCellPrefab;

        /// <summary>
        /// 预制体容器
        /// </summary>
        public Transform _CreatePrefabView;
        /// <summary>
        /// 变换组件大小发生变化事件
        /// </summary>
        public event Action _OnRectSizeChangedEvent;
        /// <summary>
        /// 所有表头单元格实例
        /// </summary> 
        public readonly ObservableCollection<HeaderCellBase> _HeaderCells = new ObservableCollection<HeaderCellBase>();
        /// <summary>
        /// 当前选中表头单元格集合
        /// </summary>
        public readonly ObservableCollection<HeaderCellBase> _CurrentSelectHeaderCells = new ObservableCollection<HeaderCellBase>();

        /// <summary>
        /// 边界内的单元格数量
        /// </summary>
        /// <returns></returns>
        public virtual int InsideBoundaryCellCount() {
         return InsideBoundaryCell().Count(); 
        }
        /// <summary>
        /// 边界内的单元格
        /// </summary>
        /// <returns></returns>
        public virtual IEnumerable<HeaderCellBase> InsideBoundaryCell()
        {
            return _HeaderCells.Where(p => p._IsInsideBoundary);
        }

        /// <summary>
        /// 表头单元格数量
        /// </summary>
        public int _HeaderCellsCount
        {
            get
            {
                return _HeaderCells.Count;
            } 
        }
        HorizontalOrVerticalLayoutGroup layoutGroup;
        /// <summary>
        /// 布局
        /// </summary>
        public HorizontalOrVerticalLayoutGroup _LayoutGroup
        {
            get
            {
                if (!layoutGroup)
                {
                    layoutGroup = GetComponent<HorizontalOrVerticalLayoutGroup>();
                }
                return layoutGroup;
            } 
        }

        ToggleGroup toggleGroup;
        public ToggleGroup _ToggleGroup
        {
            get
            {
                if (!toggleGroup)
                {
                    toggleGroup = GetComponent<ToggleGroup>();
                }
                return toggleGroup;
            } 
        }

        protected virtual void Start()
        {
          
            _ResetCellContentSize();
            _Table._ScrollRect.onValueChanged.AddListener(_ScrollRectValueChanged); 
        }
        protected virtual  void Reset()
        { 
            Unit._AddComponent<ContentSizeFitter>(this.gameObject); 
            Unit._AddComponent<ToggleGroup>(this.gameObject).allowSwitchOff=true;
        }
        public abstract void _ScrollRectValueChanged(Vector2 vector2);
        /// <summary>
        /// 当添加表头单元时创建单元格数据
        /// 就是当创建表头的时候，要连单元格数据缓存也一起创建好
        /// </summary>
        public abstract void _OnAddHeaderCellCreateCellData(HeaderCellData headerCellData);
        /// <summary>
        /// 创建单元格数据
        /// </summary>
        protected virtual Cell.CellData _CreateCellData( Vector2Int indexV2) { 
            var _findCellData = _Table._CellDatas[indexV2];
            if (_findCellData == null)
            {
                _findCellData = new Cell.CellData()
                {
                    _Column = indexV2.x,
                    _Row = indexV2.y,
                    _Table=this._Table
                };
                
                _Table._CellDatas.Add(_findCellData);
            }
            return _findCellData;
        }
        /// <summary>
        /// 添加单元格
        /// </summary>
        /// <param name="columnCellData"></param>
        public virtual HeaderCellBase _Add(HeaderCellData cellData)
        {
            if (cellData == null) return null;
            var _newObj= Instantiate(_HeaderCellPrefab, _CreatePrefabView); 
            _newObj._CellData = cellData; 
            _HeaderCells.Add(_newObj);
            _OnAddHeaderCellCreateCellData(cellData); 
            _ResetCellContentSize(); 
            return _newObj;
        }

        /// <summary>
        /// 刷新单元格容器大小
        /// </summary>
        public abstract void _ResetCellContentSize();

        /// <summary>
        /// 触发<see cref="_OnRectSizeChangedEvent"/>事件
        /// </summary>
        protected virtual void _Invoke_RectSizeChangedEvent() {
            _OnRectSizeChangedEvent?.Invoke();
        }

        /// <summary>
        /// 根据子物体变换组件的索引来寻找单元格
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public virtual HeaderCellBase _TransformIndexFindCell(int index) {
              var _child= transform.GetChild(index);
            HeaderCellBase cell = _child.GetComponent<HeaderCellBase>() ;
            return cell;
        }
        /// <summary>
        /// 根据索引来寻找单元格
        /// </summary>
        /// <returns></returns>
        public virtual HeaderCellBase _FindCellOfIndex(int index) { 
        return    _HeaderCells.FirstOrDefault(p=>p._CellData._Index==index);
        }
        /// <summary>
        /// 删除表头
        /// </summary>
        /// <param name="row"></param>
        public virtual void _Remove(int index)
        {
            var _cell= _FindCellOfIndex(index);
            if (!_cell) return;
         
           
            Destroy(_cell.gameObject);
            foreach (var item in _HeaderCells)
            {
                if (item._CellData._Index>=index)
                {
                    item._CellData._Index--;
                }
            }
            _HeaderCells.Remove(_cell);
        }
    }
}