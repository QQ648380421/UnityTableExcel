using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.ObjectModel;
using UnityEngine.EventSystems;
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
        RectTransform parent;
        /// <summary>
        /// 本身变换组件
        /// </summary>
        public RectTransform _Parent
        {
            get
            {
                if (!parent)
                {
                    parent = transform.parent.GetComponent<RectTransform>();
                }
                return parent;
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
        /// 所有表头单元格数据实例
        /// </summary> 
        public readonly ObservableCollection<HeaderCellData> _HeaderCellDatas = new ObservableCollection<HeaderCellData>();
        
        /// <summary>
        /// 当前选中表头单元格集合
        /// </summary>
        public readonly ObservableCollection<HeaderCellData> _CurrentSelectHeaderCells = new ObservableCollection<HeaderCellData>();

        /// <summary>
        /// 当前视图范围内显示的单元格缓存
        /// </summary>
        public List<HeaderCellData> _CurrentViewCellDatas=new List<HeaderCellData>();
     
        /// <summary>
        /// 计算表头宽高大小
        /// </summary>
        /// <returns></returns>
        public float _GetSize() {
            float size = 0; 
            for (int i = 0; i < _HeaderCellDatas.Count; i++)
            {
                var item = _HeaderCellDatas[i];
                size += item._Size;
            }
            return size;
        }

        /// <summary>
        ///  获取范围内单元格
        /// </summary>
        /// <param name="position">开始位置</param>
        /// <param name="viewSize">开始位置+视图大小</param>
        /// <returns></returns>
        public virtual IEnumerable<HeaderCellData> _GetViewCellDatas(float position,float viewSize)
        { 

            return _HeaderCellDatas.Where(
                p=>
                (p._Position+p._Size) >=position 
                &&
              (position + viewSize ) >=(p._Position )

                );
        }

    
         

        /// <summary>
        /// 表头单元格数量
        /// </summary>
        public int _HeaderCellsCount
        {
            get
            {
                return _HeaderCellDatas.Count;
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
        /// 更新所有单元格位置坐标索引
        /// </summary>
        public virtual void _ResetCellDatasPosition() {
          var _cellDatas=  _HeaderCellDatas.OrderBy(p=>p._Index);
            float pos =0;
            foreach (var item in _cellDatas)
            {
                item._Position = pos; 
                pos += item._Size; 
            }
        }
        /// <summary>
        /// 添加单元格
        /// </summary>
        /// <param name="columnCellData"></param>
        public virtual HeaderCellData _Add(HeaderCellData cellData)
        {
            if (cellData == null) return null;
            cellData._Index = this._HeaderCellDatas.Count;
            _HeaderCellDatas.Add(cellData); 
            _ResetCellDatasPosition();
            cellData.PropertyChanged -= _HeaderCellData_PropertyChanged;
            cellData.PropertyChanged += _HeaderCellData_PropertyChanged; 
            _OnAddHeaderCellCreateCellData(cellData); 
            _ResetCellContentSize(); 
            return cellData;
        }
        /// <summary>
        /// 重置表头单元格位置
        /// </summary>
        public virtual void _ResetHeaderCellPosition() {
            float pos = 0;
            var _buffer = _HeaderCellDatas.OrderBy(p => p._Index).ToList();
            for (int i = 0; i < _buffer.Count; i++)
            {
                var _item = _buffer[i];
                _item._Position = pos;
                pos += _item._Size;
                if (_item._CellObj)
                {
                    _item._CellObj.OnCellDataChanged(_item);
                }
            }
            foreach (var item in _HeaderCells)
            {
                if (!item) continue;
                item._ResetPosition(item._CellData);
            }
        }

        
        /// <summary>
        /// 表头单元格数据内容发生变化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _HeaderCellData_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(HeaderCellData._Size) 
                || e.PropertyName == nameof(HeaderCellData._Index)
                )
            {//任何一个单元格大小发生了变化
                //重新计算位置 
                _ResetHeaderCellPosition(); 
            }
           
          
        }

        /// <summary>
        /// 创建表头单元格
        /// </summary>
        /// <returns></returns>
        protected virtual HeaderCellBase _CreateHeaderCell() {
            var _newObj = Instantiate(_HeaderCellPrefab, _CreatePrefabView); 
            _HeaderCells.Add(_newObj);
            return _newObj;
        }
    
        /// <summary>
        /// 更新单元格
        /// </summary>
        protected abstract void _UpdateCells();
        /// <summary>
        /// 刷新单元格
        /// </summary>
        /// <param name="pos"></param>
        /// <param name="viewSize"></param>
        protected virtual void _UpdateCells(float pos, float viewSize)
        {
          
            //新的视图内单元格
            var _ViewCellDatasBuffer = _GetViewCellDatas(pos, viewSize).ToList(); 
            var _removeCells = _CurrentViewCellDatas.Where(p => _ViewCellDatasBuffer.Contains(p) == false || p._CellObj==null);
             
            //移除单元格缓存
            Queue<HeaderCellData> _removeCellsQueue = new Queue<HeaderCellData>(_removeCells);
         
            //添加新数据
            for (int i = 0; i < _ViewCellDatasBuffer.Count; i++)
            {
                //新单元格
                var _newCellData = _ViewCellDatasBuffer[i];
                var _cell = _CurrentViewCellDatas.FirstOrDefault(p => p == _newCellData);
                if (_cell != null && _cell._CellObj)
                {//还在显示，不用管  
                    _cell._CellObj._CellData = _newCellData; 
                    continue;
                }
                else
                {//没有显示出来的单元格数据
                    if (_removeCellsQueue.Count > 0)
                    {//还有剩下的没有移除的单元格数据可以用
                        var _oldCell = _removeCellsQueue.Dequeue();
                        if (_oldCell._CellObj)
                        {//有关联物体  
                            _oldCell._CellObj._CellData = _newCellData;//直接更改 
                            continue;
                        } 
                    }
                    //没有关联物体，或者单元格不够
                    var _newCell = _CreateHeaderCell();
                    _newCell._CellData = _newCellData;
                }
            }
            for (int i = 0; i < _removeCellsQueue.Count; i++)
            {
                var item = _removeCellsQueue.Dequeue();
                //还有剩下的多余的
                if (item._CellObj)
                { 
                    item._CellObj._CellData = null;
                } 
            }
          
            _CurrentViewCellDatas = _ViewCellDatasBuffer;


        }
        protected virtual void Update()
        {
            _UpdateCells();
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
        return    _HeaderCells.FirstOrDefault(p=>p._CellData!=null && p._CellData._Index==index);
        }
        /// <summary>
        /// 删除表头
        /// </summary>
        /// <param name="row"></param>
        public virtual void _Remove(int index)
        {
           var _findCellData= _HeaderCellDatas.FirstOrDefault(p=>p._Index==index);
            if (_findCellData == null) return;
            var _cell= _HeaderCells.FirstOrDefault(p=>p._CellData== _findCellData);
            if (_cell)
            {//删除表头关联单元格
                _cell._CellData = null;
            } 
            _HeaderCellDatas.Remove(_findCellData);
            foreach (var item in _HeaderCellDatas)
            {
                if (item._Index>=index)
                {
                    item._Index--;
                }
            }
        
        }

        
    }
}