using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using UnityEngine;
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

        float width=200;
        /// <summary>
        /// 列宽
        /// </summary>
        public float _Width
        {
            get
            {
                return width;
            }
            set
            {
                if (width == value) return;
                width = value;
            }
        }
        bool ignore;
        /// <summary>
        /// 忽略该单元格
        /// </summary>
        public bool _Ignore
        {
            get
            {
                return ignore;
            }
            set
            {
                if (ignore == value) return;
                ignore = value;
            }
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

      
        public ColumnAttribute()
        {
        }

        public ColumnAttribute(int index, string name)
        {
            _Index = index;
            _Name = name;
        }

        public ColumnAttribute(int index)
        {
            _Index = index;
        }

        public ColumnAttribute(bool ignore)
        {
            _Ignore = ignore;
        }

        /// <summary>
        /// 将属性转换为数据
        /// </summary>
        /// <param name="propertyInfo"></param>
        /// <returns></returns>
        private static ColumnAttributeData _GetData(PropertyInfo propertyInfo) {
            ColumnAttributeData columnAttributeData = new ColumnAttributeData();
            columnAttributeData._PropertyInfo = propertyInfo; 
           var _columnAttribute=  GetCustomAttribute(propertyInfo,typeof(ColumnAttribute));
            if (_columnAttribute!=null  && _columnAttribute is ColumnAttribute attribute)
            { 
                columnAttributeData._ColumnAttribute = attribute;
            }
            return columnAttributeData;
        }

        /// <summary>
        /// 获取该类型下的所有列的数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public static List<ColumnAttributeData> GetColumnAttributeDatas<T>()
        {
            var _type= typeof(T); 
            var _properties= _type.GetProperties();
            List<ColumnAttributeData> _datas = new List<ColumnAttributeData>();
            List<ColumnAttributeData> _erorrDatas = new List<ColumnAttributeData>();//错误的标记
            foreach (var item in _properties)
            {
                var _data= _GetData(item);
                if (_data._ColumnAttribute == null) {
                    //没有标记，将默认名称加进去
                    _data._ColumnAttribute = new ColumnAttribute();
                    _data._ColumnAttribute._Name = _data._PropertyInfo.Name;
                    _erorrDatas.Add(_data);
                    continue;
                }
                else
                {//有标记
                    if (string.IsNullOrEmpty(_data._ColumnAttribute._Name))
                    {//没有名字给他加上名字
                        _data._ColumnAttribute.name = _data._PropertyInfo.Name;
                    }
                    if (_data._ColumnAttribute._Ignore)
                    {//被忽略的不添加 
                        continue;
                    }
                    if (_datas.Count(p => p._ColumnAttribute._Index == _data._ColumnAttribute._Index) > 0)
                    {//如果已经有相同索引的标记，则警告并自动加到最后面
                        Debug.LogWarning("类型：" + _type.Name +"属性："+ _data ._PropertyInfo.Name+ "的索引[" + _data._ColumnAttribute._Index + "]重复，请检查列特性的索引是否设置正常！如忽略，我将把重复索引添加到最后。");
                        _erorrDatas.Add(_data);
                        continue;
                    }
                } 
                //正常添加
                _datas.Add(_data);
            } 
           var _maxIndex= _datas.Max(p => p._ColumnAttribute._Index);
            foreach (var item in _erorrDatas)
            {
                _maxIndex++;
                item._ColumnAttribute._Index = _maxIndex;
                _datas.Add(item);
            }
            _datas = _datas.OrderBy(p => p._ColumnAttribute._Index).ToList() ; 
            return _datas;
        }
    }

    /// <summary>
    /// 列属性数据
    /// </summary>
    public class ColumnAttributeData {

        ColumnAttribute  columnAttribute;
        /// <summary>
        /// 关联的特性
        /// </summary>
        public ColumnAttribute _ColumnAttribute
        {
            get
            {
                return  columnAttribute;
            }
            set
            {
                if ( columnAttribute == value) return;
                 columnAttribute = value;
            }
        } 
        PropertyInfo  propertyInfo;
        /// <summary>
        /// 关联的属性
        /// </summary>
        public PropertyInfo _PropertyInfo
        {
            get
            {
                return  propertyInfo;
            }
            set
            {
                if ( propertyInfo == value) return;
                 propertyInfo = value;
            }
        }
    }
}