using System;
using System.ComponentModel;
using UnityEngine.EventSystems;
namespace XP.TableModel
{
    
    /// <summary>
    /// 列表头特性，可以更快速的使用该表格
    /// 在属性上标记后，会自动将该属性的名称作为列名
    /// </summary>
    [AttributeUsage( AttributeTargets.Property)]
    public class ColumnAttribute : Attribute, INotifyPropertyChanged
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