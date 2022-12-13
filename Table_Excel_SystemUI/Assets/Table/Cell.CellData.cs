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
        public class CellData : INotifyPropertyChanged
        {
         
            /// <summary>
            /// 单元格中的<see cref="_Data"/>和<see cref="_Row"/>还有<see cref="_Column"/>等数据发生变化时触发
            /// </summary>
            public event _CellDataChanged _CellDataChangeEvent;
            public event PropertyChangedEventHandler PropertyChanged;

            private Cell cell;
            object data;
            /// <summary>
            /// 触发事件<see cref="PropertyChanged"/>
            /// </summary>
            /// <param name="name"></param>
            private void _InvokePropertyChanged(string name)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
            }
            /// <summary>
            /// 单元格数据，你传入的数据，可以转化成你想要的
            /// </summary>
            public object _Data
            {
                get => data; set
                {
                    if (data == value) return;
                    data = value;
                    _CellDataChangeEvent?.Invoke(this._Cell, this);
                    string valueStr = string.Empty;

                    if (value != null)
                    {
                        valueStr = value.ToString();
                    }
                    if (cell)
                    {
                        cell._Invoke__CellDataChangeEvent(cell, this);
                        cell._CellDataChangedEvents_String?.Invoke(valueStr);
                    }
                    _InvokePropertyChanged(nameof(_Data));
                }
            }

            [SerializeField]
            int row, column;

            public CellData(object data, int row, int colum)
            {
                this.row = row;
                this.column = colum;
                this.data = data;
            }

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
                    _CellDataChangeEvent?.Invoke(this._Cell, this);
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
                    _CellDataChangeEvent?.Invoke(this._Cell, this);
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



        }
    }
}