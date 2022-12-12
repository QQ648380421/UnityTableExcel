using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static XP.TableModel.HeaderColumnCell;

namespace XP.TableModel
{
    public delegate void _ColumnCellDataChangeDelegate(HeaderColumnCell cell, ColumnCellData columnCellData);

    /// <summary>
    /// 表头单元格
    /// </summary>
    public partial class HeaderColumnCell : Selectable
    {
        [Header("当列名发生变化时触发")]
        public InputField.OnChangeEvent _OnColumnCellNameChanged;
        /// <summary>
        /// 当列数据发生变化时触发
        /// </summary>
        public event _ColumnCellDataChangeDelegate _ColumnCellDataChangeEvent;

        ColumnCellData columnCellData;
        /// <summary>
        /// 列数据
        /// </summary>
        public ColumnCellData _ColumnCellData
        {
            get => columnCellData; set
            {
                if (columnCellData == value) return;
                if (columnCellData!=null)
                {
                    columnCellData.PropertyChanged -= ColumnCellData_PropertyChanged;
                } 
                columnCellData = value;
                columnCellData.PropertyChanged -= ColumnCellData_PropertyChanged;
                columnCellData.PropertyChanged += ColumnCellData_PropertyChanged;
                _ColumnCellDataChangeEvent?.Invoke(this,value);

            }
        }
        protected override void OnDestroy()
        {
            base.OnDestroy();
            if (columnCellData != null)
            {
                columnCellData.PropertyChanged -= ColumnCellData_PropertyChanged;
            }
        }
        /// <summary>
        /// 列数据发生变化时触发
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ColumnCellData_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (_ColumnCellData!=null &&  e.PropertyName==nameof(ColumnCellData._Name))
            {
                _OnColumnCellNameChanged?.Invoke(_ColumnCellData._Name);
            } 
        }
    }
}