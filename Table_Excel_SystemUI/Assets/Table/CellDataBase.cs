using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
namespace XP.TableModel {
 
    public class CellDataBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
    
        /// <summary>
        /// 触发事件<see cref="PropertyChanged"/>
        /// </summary>
        /// <param name="name"></param>
        protected void _InvokePropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        object data;
        /// <summary>
        /// 单元格数据，你传入的数据，可以转化成你想要的，可以重写<see cref="object.ToString()"/>
        /// </summary>
        public virtual object _Data
        {
            get => data; set
            {
                if (data == value) return;
                data = value;
                _InvokePropertyChanged(nameof(_Data));
            }
        }
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