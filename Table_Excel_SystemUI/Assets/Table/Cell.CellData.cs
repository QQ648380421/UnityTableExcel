using UnityEngine;
using System;
using System.ComponentModel;

namespace XP.TableModel
{
    public partial class Cell
    {
        /// <summary>
        /// 单元格数据
        /// </summary>
        [Serializable]
        public class CellData : CellDataBase
        { 
       
            private Cell cell;
              
            [SerializeField]
            int row, column;
             
            /// <summary>
            /// 行
            /// </summary>
            public int _Row
            {
                get => row;
                set
                {
                    if (row == value) return;
                    row = value; 
                    _InvokePropertyChanged(nameof(_Row));
                }
            }
            /// <summary>
            /// 列
            /// </summary>
            public int _Column
            {
                get => column;
                set
                {
                    if (column == value) return;
                    column = value; 
                    _InvokePropertyChanged(nameof(_Column)); 
                }
            }
            /// <summary>
            /// 关联单元格
            /// </summary>
            public Cell _Cell
            {
                get => cell; set
                {
                    if (cell == value) return;
                    cell = value; 
                    _InvokePropertyChanged(nameof(_Cell));
                }
            }


            HeaderCellBase columnCell;
            /// <summary>
            ///关联列单元格
            /// </summary>
            public HeaderCellBase _ColumnCell
            {
                get
                {
                    return columnCell;
                }
                set
                {
                    if (columnCell == value) return;
                    columnCell = value;
                    _InvokePropertyChanged(nameof(_Cell));
                }
            }

            HeaderCellBase rowCell;
            public HeaderCellBase _RowCell
            {
                get
                {
                    return rowCell;
                }
                set
                {
                    if (rowCell == value) return;
                    rowCell = value;
                    _InvokePropertyChanged(nameof(_Cell));
                }
            }

            public override bool _Selected
            {
                get => base._Selected; set
                {
                    base._Selected = value;
                    if (!_Table) return; 
                    if (value)
                    {
                        if (!_Table._CurrentSelectedCellDatas.Contains(this))
                        {
                            _Table._CurrentSelectedCellDatas.Add(this);
                        }
                    }
                    else
                    {
                        if (_Table._CurrentSelectedCellDatas.Contains(this))
                        {
                            _Table._CurrentSelectedCellDatas.Remove(this);
                        }
                    } 
                }
            }
            ~CellData() {
                if (_Table)
                {
                    if (_Table._CellDatas.Contains(this))
                    {
                        _Table._CellDatas.Remove(this);
                    } 
                } 
            }
        }
    }
}