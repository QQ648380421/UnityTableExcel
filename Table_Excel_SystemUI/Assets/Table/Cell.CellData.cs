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



        }
    }
}