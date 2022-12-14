using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static XP.TableModel.HeaderColumnCell;

namespace XP.TableModel
{
    public delegate void _CellDataChangeDelegate(HeaderCellBase cell, HeaderCellData columnCellData);
    /// <summary>
    /// 是否处于边界内委托
    /// </summary>
    /// <param name="cell">单元格</param>
    /// <param name="isInsideBoundary">是否处于边界内</param>
    public delegate void _IsInsideBoundaryChangeDelegate(HeaderCellBase cell, bool isInsideBoundary);
    /// <summary>
    /// 表头单元格基类
    /// </summary>
    public abstract  class HeaderCellBase : Toggle
    {
        [Header("当列名发生变化时触发")]
        public InputField.OnChangeEvent _OnCellNameChanged;
        /// <summary>
        /// 当数据发生变化时触发
        /// </summary>
        public event _CellDataChangeDelegate _OnCellDataChangeEvent;
        /// <summary>
        /// 拖拽按钮
        /// </summary>
        public HeaderDragButton _DragButton;

        HeaderCellData cellData;
        /// <summary>
        /// 单元格数据
        /// </summary>
        public HeaderCellData _CellData
        {
            get => cellData; set
            {
                if (cellData == value) return;
                if (cellData != null)
                {
                    cellData.PropertyChanged -= _CellData_PropertyChanged;
                }
                cellData = value;
                _OnCellDataChangeEvent?.Invoke(this, value);
                if (value == null) return;

                value.PropertyChanged -= _CellData_PropertyChanged;
                value.PropertyChanged += _CellData_PropertyChanged;
                transform.SetSiblingIndex(value._Index);
                 
                if (value._Data==null)
                {
                   
                    _OnCellNameChanged?.Invoke(string.Empty);
                }
                else
                {
                    name = value._Index.ToString();
                   _OnCellNameChanged?.Invoke(value._Data.ToString());
                }

            }
        }
        /// <summary>
        /// 父级表头基类
        /// </summary>
        public HeaderBase _HeaderBase { get {
                if (!headerBase)
                {
                    headerBase = GetComponentInParent<HeaderBase>();
                }
                return headerBase;
            } }

        HeaderBase headerBase;


        Table table;
        public Table _Table
        {
            get
            {
                if (!table)
                {
                    table = _HeaderBase._Table;
                }
                return table;
            }
          
        }

        RectTransform rectTransform;
        public RectTransform _RectTransform
        {
            get
            {
                if (!rectTransform)
                {
                    rectTransform = transform as RectTransform;
                }
                return rectTransform;
            } 
        }


        Mask parentMask;
        /// <summary>
        /// 父对象遮罩
        /// </summary>
        protected Mask _ParentMask
        {
            get
            {
                if (!parentMask)
                {
                    parentMask = GetComponentInParent<Mask>();
                }
                return parentMask;
            } 
        }


        ToggleGroup  toggleGroup;
        public ToggleGroup _ToggleGroup
        {
            get
            {
                if (!toggleGroup&&  _HeaderBase)
                {
                    toggleGroup = _HeaderBase._ToggleGroup;
                }
                return  toggleGroup;
            } 
        }

        /// <summary>
        /// 是否在显示范围边界内状态改变时触发
        /// </summary>
        public event _IsInsideBoundaryChangeDelegate _IsInsideBoundaryChangedEvent;

        bool isInsideBoundary;
        /// <summary>
        /// 该单元格是否在边界内，每一帧刷新一次
        /// </summary>
        public bool _IsInsideBoundary
        {
            get
            {
                return isInsideBoundary;
            }
            set
            {
                if (isInsideBoundary == value) return;
                isInsideBoundary = value;
                _IsInsideBoundaryChangedEvent?.Invoke(this,value);
            }
        }

        /// <summary>
        /// 该单元格是否处于边界内
        /// </summary>
        public abstract bool InsideBoundary();
         
        /// <summary>
        /// 数据发生变化时触发
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _CellData_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (_CellData == null) return;

            if (e.PropertyName == nameof(HeaderCellData._Data))
            {
                if (_CellData._Data==null)
                {
                    _OnCellNameChanged?.Invoke(string.Empty);
                }
                else
                {
                    _OnCellNameChanged?.Invoke(_CellData._Data.ToString());
                }
               
            }
            else if (e.PropertyName == nameof(HeaderCellData._Index))
            {
                transform.SetSiblingIndex(_CellData._Index);
            }
        }
        protected override void Awake()
        {
            base.Awake();
            this.group = _ToggleGroup;
        }
        protected override void Start()
        {
            base.Start();
            _DragButton.transform.SetSiblingIndex(transform.childCount);
            _DragButton._OnEndDragEvent -= _DragButton__OnEndDragEvent;
            _DragButton._OnEndDragEvent += _DragButton__OnEndDragEvent;
            this.onValueChanged.AddListener(_IsOnChangedListener);
            _Table._OnRefreshEvent += _Table__OnRefreshEvent;

        }
        
        /// <summary>
        /// 刷新
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _Table__OnRefreshEvent(object sender, Table e)
        {
            _HeaderBase._ResetCellContentSize();
        }

        /// <summary>
        /// 选择框发生变化监听事件
        /// </summary>
        /// <param name="value"></param>
        protected virtual void _IsOnChangedListener(bool value) {
            if (_HeaderBase)
            {
                if (value)
                {
                    if (!_HeaderBase._CurrentSelectHeaderCells.Contains(this))
                    {
                        _HeaderBase._CurrentSelectHeaderCells.Add(this);
                    }
                }
                else
                {
                    if (_HeaderBase._CurrentSelectHeaderCells.Contains(this))
                    {
                        _HeaderBase._CurrentSelectHeaderCells.Remove(this);
                    }
                }
            }
         
            _IsOnChanged(value);
        }
        /// <summary>
        /// 选择框发生变化
        /// </summary>
        /// <param name="value"></param>
        protected abstract void _IsOnChanged(bool value);

        protected override void OnDestroy()
        {
            base.OnDestroy();
            _IsInsideBoundary = false;
            if (cellData != null)
            {
                cellData.PropertyChanged -= _CellData_PropertyChanged;
            }
            if (_DragButton)
            {
                _DragButton._OnEndDragEvent -= _DragButton__OnEndDragEvent;
            }
            if (_HeaderBase)
            {
                if (_HeaderBase._HeaderCells.Contains(this))
                {
                    _HeaderBase._HeaderCells.Remove(this);
                } 
            }
            if (_Table)
            {
                _Table._OnRefreshEvent -= _Table__OnRefreshEvent;
            }
       
        }

        /// <summary>
        /// 拖拽按钮结束
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void _DragButton__OnEndDragEvent(object sender, UnityEngine.EventSystems.PointerEventData e) {
            _HeaderBase._ResetCellContentSize();
        }

        private void Update()
        {
            _IsInsideBoundary = InsideBoundary();

        }

    }
}