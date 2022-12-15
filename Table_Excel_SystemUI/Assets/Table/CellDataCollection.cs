using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using UnityEngine;
using static XP.TableModel.Cell;

namespace XP.TableModel
{
    public class CellDataCollection : ObservableCollection<CellData> {
        
       
         public  CellData this[CellData cellData]
        {
            get
            {
                return this.FirstOrDefault(p => p != null && p._Column == cellData._Column && p._Row == cellData._Row);
            }
        }

        public CellData this[int x ,int y ]
        {
            get
            {
                return this.FirstOrDefault(p => p != null && p._Column == x && p._Row ==y);
            }
        }
        public CellData this[Vector2Int vector2]
        {
            get
            {
                return this[vector2.x, vector2.y];
            }
        }
        /// <summary>
        /// 获取行所有单元格数据
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        public IEnumerable<CellData> _GetRowCellsData(int row)
        {
            return this.Where(p => p._Row == row);
        }
        /// <summary>
        /// 获取列所有单元格数据
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        public IEnumerable<CellData> _GetColumnCellsData(int column)
        {
            return this.Where(p => p._Column == column);
        }

        /// <summary>
        /// 删除行
        /// </summary>
        /// <param name="row"></param>
        public void _RemoveRow(int row)
        { 
            var _rowCellsData =  _GetRowCellsData(row); 
            if (_rowCellsData == null) return; 
            foreach (var item in _rowCellsData.ToArray())
            {
                this.Remove(item);
            }
            var _maxIndex= _MaxRowIndex;
            foreach (var item in this)
            {
                if (item._Row>= row)
                {
                    item._Row--;
                }
            }
           
        }

        /// <summary>
        /// 删除列
        /// </summary>
        /// <param name="row"></param>
        public void _RemoveColumn(int column)
        {
            var _columnCellsData = _GetColumnCellsData(column);
            if (_columnCellsData == null) return;
            foreach (var item in _columnCellsData.ToArray())
            {
                this.Remove(item);
            }
            var _maxIndex = _MaxColumnIndex;
            foreach (var item in this)
            {
                if (item._Column >= column)
                {
                    item._Column--;
                }
            }
        }

        /// <summary>
        /// 最大行索引
        /// </summary>
       public int _MaxRowIndex
        {
            get
            {
                if (this==null || Count<=0) return 0;
                return this.Max(p => p._Row);
            }
        }
        /// <summary>
        /// 最大列索引
        /// </summary>
        public int _MaxColumnIndex
        {
            get
            {
                if (this == null || Count <= 0) return 0;
                return this.Max(p => p._Column);
            }
        }
        /// <summary>
        /// 获取所有行单元格数据
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        internal IEnumerable<CellData> _GetRowCellDatas(int row)
        {
            return this.Where(p=>p._Row==row);
        }
        /// <summary>
        /// 获取所有列单元格数据
        /// </summary>
        /// <param name="colum"></param>
        /// <returns></returns>
        internal IEnumerable<CellData> _GetColumCellDatas(int colum)
        {
            return this.Where(p => p._Column == colum);
        }
    }
}