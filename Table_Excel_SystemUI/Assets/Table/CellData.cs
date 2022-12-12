using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System;

namespace XP.TableModel
{
    [Serializable]
    public class CellDataEvent : UnityEvent<object> { }
    public delegate void _CellDataChanged(Cell _cell, CellData _cellData);

  
    /// <summary>
    /// 单元格数据
    /// </summary>
    [Serializable]
    public class CellData 
    {
        /// <summary>
        /// 单元格中的<see cref="_Data"/>和<see cref="_Row"/>还有<see cref="_Colum"/>等数据发生变化时触发
        /// </summary>
        public event _CellDataChanged _CellDataChangeEvent;

        private Cell cell;
        object data;
        
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
              
            }
        }

        [SerializeField]
        int row, colum;
        /// <summary>
        /// 行
        /// </summary>
        public int _Row
        {
            get => row; set
            {
                if (row == value) return;
                row = value;
                _CellDataChangeEvent?.Invoke(this._Cell, this); 
            }
        }
        /// <summary>
        /// 列
        /// </summary>
        public int _Colum
        {
            get => colum; set
            {
                if (colum == value) return;
                colum = value;
                _CellDataChangeEvent?.Invoke(this._Cell, this);
            }
        }
        /// <summary>
        /// 关联单元格
        /// </summary>
        public Cell _Cell { get => cell; set => cell = value; }


      


    }
}