using System;
using System.ComponentModel;

namespace XP.TableModel
{ 
    //表格特性基类
    public class TableAttributeBase: Attribute, INotifyPropertyChanged
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
        string name;
        /// <summary>
        /// 名称，如果为空，则使用属性名称
        /// </summary>
        public string _Name
        {
            get
            {
                return name;
            }
            set
            {
                if (name == value) return;
                name = value;
                _InvokePropertyChanged(nameof(_Name));
            }
        } 
        int index;

        public TableAttributeBase(int index, string name)
        {
            _Index = index;
            _Name = name;
        }

        public TableAttributeBase()
        {
        }

        /// <summary>
        /// 索引标记
        /// </summary>
        public int _Index
        {
            get
            {
                return index;
            }
            set
            {
                if (index == value) return;
                index = value;
                _InvokePropertyChanged(nameof(_Index));
            }
        }

       
    }
}