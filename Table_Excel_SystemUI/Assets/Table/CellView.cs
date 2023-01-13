using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace XP.TableModel
{
    /// <summary>
    /// 单元格容器，用于刷新单元格位置
    /// </summary>
    public class CellView : MonoBehaviour
    {
        Table _table;
        /// <summary>
        /// 表格控制器
        /// </summary>
        public Table _Table
        {
            get
            {
                if (!_table)
                {
                    _table = GetComponentInParent<Table>();
                }
                return _table;
            }
        }

        Mask mask;
        ToggleGroup toggleGroup;
        public ToggleGroup _ToggleGroup
        {
            get
            {
                if (!toggleGroup)
                {
                    toggleGroup = GetComponent<ToggleGroup>();
                }
                return toggleGroup;
            } 
        }
        /// <summary>
        /// 所有的单元格
        /// </summary>
        public readonly ObservableCollection<Cell> _Cells = new ObservableCollection<Cell>();
        /// <summary>
        /// 单元格预制体
        /// </summary>
        public Cell _CellPrefab {
            get {
                return _Table._CellPrefab;
            }
        }
        /// <summary>
        /// 父级遮罩
        /// </summary>
        public Mask _Mask { get {
                if (!mask)
                {
                    mask = GetComponentInParent<Mask>();
                }
                return mask;
            }  }

        /// <summary>
        /// 注册事件
        /// </summary>
        private void _RegisterEvents() {
            _Table._OnRefreshEvent -= _Table__OnRefreshEvent;
            _Table._OnRefreshEvent += _Table__OnRefreshEvent;
        }

        /// <summary>
        /// 刷新
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _Table__OnRefreshEvent(object sender, Table e)
        {
            //for (int i = 0; i < _Cells.Count; i++)
            //{
            //    Destroy(_Cells[0].gameObject);
            //} 
        }

        /// <summary>
        /// 清理事件
        /// </summary>
        private void _ClearEvents()
        {
            if (_Table == null) return;
            _Table._OnRefreshEvent -= _Table__OnRefreshEvent;
        }
        private void Awake()
        {
            _RegisterEvents();
        }
        private void Reset()
        {
            toggleGroup=  Unit._AddComponent<ToggleGroup>(this.gameObject);
        }
        private void OnDestroy()
        {
            _ClearEvents();
        }

        /// <summary>
        /// 实例化一个新预制体
        /// </summary>
        private Cell CreatePrefab(HeaderCellData column, HeaderCellData row) { 
            var _newCell = Instantiate(_CellPrefab, transform);
            _newCell._CellView = this;
            _newCell._ColumnCellData = column;
            _newCell._RowCellData = row;
            _Cells.Add(_newCell);
            return _newCell;
        }
     
        private void Update()
        {
            var _xCellDatas = _Table._HeaderColumn._CurrentViewCellDatas;
            var _yCellDatas = _Table._HeaderRow._CurrentViewCellDatas;
            Vector2Int _min=Vector2Int.zero;
            Vector2Int _max = Vector2Int.zero;
            _min.x= _xCellDatas==null || _xCellDatas.Count<=0 ? 0: _xCellDatas.Min(p=>p._Index);
            _min.y = _yCellDatas == null || _yCellDatas.Count <= 0 ? 0 : _yCellDatas.Min(p => p._Index);
            _max.x = _xCellDatas == null || _xCellDatas.Count <= 0 ? 0 : _xCellDatas.Max(p => p._Index);
            _max.y = _yCellDatas == null || _yCellDatas.Count <= 0 ? 0 : _yCellDatas.Max(p => p._Index);
          
             var _removeCells=    _Cells.Where(
                p=>p!=null && p._CellData==null ||
               !(
                p._CellData._Column >= _min.x && p._CellData._Column <= _max.x
                &&
                 p._CellData._Row >= _min.y && p._CellData._Row <= _max.y
               )
                ).ToList(); 

            for (int x = 0; x < _xCellDatas.Count; x++)
            {
                for (int y = 0; y < _yCellDatas.Count; y++)
                {
                    _SetCellData(_removeCells, _xCellDatas[x], _yCellDatas[y]);
                }
            } 
            for (int i = 0; i < _removeCells.Count; i++)
            {
                var _removeCell= _removeCells[i]; 
                if (_removeCell)
                {
                    Destroy(_removeCell.gameObject);
                } 
            }


        }


        /// <summary>
        /// 设置单元格数据
        /// </summary>
        /// <param name="column"></param>
        /// <param name="row"></param>
        private void _SetCellData(List<Cell> removeCells,HeaderCellData column, HeaderCellData row)
        {
            var _cell= _Cells.FirstOrDefault(p=>p._ColumnCellData== column && p._RowCellData==row);
            //缓存中还在，不用管
            if (_cell) {
                bool flag = false;
                if (_cell._ColumnCellData != column)
                {
                    _cell._ColumnCellData = column; 
                    flag = true;
                }
                if (_cell._RowCellData != row)
                {
                    _cell._RowCellData = row;
                    flag = true;
                } 
                var _data= _Table._CellDatas[column._Index,row._Index];
                if (_data!= _cell._CellData)
                {
                    flag = true; 
                }
                if (flag)
                {
                    _cell._Initialization();
                }
                removeCells.Remove(_cell); 
                return;
            }
            Cell cell=null;
            if (removeCells.Count>0)
            {
                cell = removeCells.FirstOrDefault();
                removeCells.Remove(cell);
                cell._ColumnCellData = column;
                cell._RowCellData = row;
                cell._Initialization();
            }
            else
            {
                cell = CreatePrefab(column, row);
            }
           

        }


    }
}