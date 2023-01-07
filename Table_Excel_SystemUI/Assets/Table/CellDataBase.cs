using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace XP.TableModel {
    /// <summary>
    /// 单元格点击数据事件
    /// </summary>
    public class CellClickData {
        Selectable selectable;
        PointerEventData eventData; 
        /// <summary>
        /// 被操作的物体对象
        /// </summary>
        public Selectable _Selectable { get => selectable; set => selectable = value; }
        /// <summary>
        /// 操作的类型数据
        /// </summary>
        public PointerEventData _EventData { get => eventData; set => eventData = value; }
        /// <summary>
        /// 被点击的单元格
        /// </summary>
        public Cell _Cell { get => cell; set => cell = value; }

        Cell cell ;
         
        HeaderCellBase headerCell;
        /// <summary>
        /// 被点击的表头
        /// </summary>
        public HeaderCellBase _HeaderCell
        {
            get
            {
                return headerCell;
            }
            set
            {
                if (headerCell == value) return;
                headerCell = value;
            }
        }

    }
    /// <summary>
    /// 单元格点击委托
    /// </summary>
    /// <param name="selectable"></param>
    /// <param name="eventData"></param>
    public delegate void _CellClickDelegate(CellClickData cellClickData);
    public class CellDataBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        /// <summary>
        /// 当单元格被点击时触发
        /// </summary>
        public event _CellClickDelegate _OnCellDataClickEvent;

        /// <summary>
        /// 触发单元格点击事件
        /// </summary>
        public void _Invoke_OnCellDataClickEvent(CellClickData cellClickData) {
            _OnCellDataClickEvent?.Invoke(cellClickData);
        }

        /// <summary>
        /// 触发事件<see cref="PropertyChanged"/>
        /// </summary>
        /// <param name="name"></param>
        protected void _InvokePropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        object showData;
        /// <summary>
        /// 单元格显示数据，显示你传入的数据，可以转化成你想要的，可以重写<see cref="object.ToString()"/>
        /// </summary>
        public virtual object _ShowData
        {
            get => showData; set
            {
                if (showData == value) return;
                showData = value;
                _InvokePropertyChanged(nameof(_ShowData));
            }
        }
        object data;
        /// <summary>
        /// 与该单元格相关联的数据
        /// </summary>
        public object _Data { get => data; set {
                if (data == value) return;
                data = value;
                _InvokePropertyChanged(nameof(_Data));
            } }

        bool selected;
        /// <summary>
        /// 选中单元格
        /// </summary>
        public virtual bool _Selected
        {
            get
            {
                return selected;
            }
            set
            {
                if (selected == value) return;
                selected = value;
                _InvokePropertyChanged(nameof(_Selected)); 
            }
        }


        Table table;
        /// <summary>
        /// 关联表
        /// </summary>
        public Table _Table
        {
            get
            {
                return table;
            }
            set
            {
                if (table == value) return;
                table = value;
                _InvokePropertyChanged(nameof(_Selected));
            }
        }

  
    }
}