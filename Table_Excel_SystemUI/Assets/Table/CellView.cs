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
        public Cell _CellPrefab;
   
        /// <summary>
        /// 注册事件
        /// </summary>
        private void _RegisterEvents() {
            _Table._HeaderRow._HeaderCells.CollectionChanged += _HeaderCells_CollectionChanged;
            _Table._HeaderColumn._HeaderCells.CollectionChanged += _HeaderCells_CollectionChanged;
        }
        /// <summary>
        /// 清理事件
        /// </summary>
        private void _ClearEvents()
        {
            if (_Table == null) return;
            if (  _Table._HeaderRow)
            {
                _Table._HeaderRow._HeaderCells.CollectionChanged -= _HeaderCells_CollectionChanged;
              
            }
            if (_Table._HeaderColumn)
            {
                _Table._HeaderColumn._HeaderCells.CollectionChanged -= _HeaderCells_CollectionChanged;
            
            }

      
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
        private Cell CreatePrefab(HeaderCellBase headerColumnCell, HeaderCellBase headerRowCell) { 
            var _newCell = Instantiate(_CellPrefab, transform);
            _newCell._CellView = this;
            _newCell._RowCell = headerRowCell;
            _newCell._ColumnCell = headerColumnCell; 
            _Cells.Add(_newCell);
            return _newCell;
        }
      
        /// <summary>
        /// 行和列表头单元格集合发生变化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _HeaderCells_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        { 
           
            if (sender is ObservableCollection<HeaderCellBase> collection)
            { 
                switch (e.Action)
                {
                    case System.Collections.Specialized.NotifyCollectionChangedAction.Add:
                       
                        break;
                    case System.Collections.Specialized.NotifyCollectionChangedAction.Move:
                        break;
                    case System.Collections.Specialized.NotifyCollectionChangedAction.Remove:
                        var _removeCell = e.OldItems[0] as HeaderCellBase;
                  
                        break;
                    case System.Collections.Specialized.NotifyCollectionChangedAction.Replace:
                        break;
                    case System.Collections.Specialized.NotifyCollectionChangedAction.Reset:
                        break;
                    default:
                        break;
                }
                

            }
           
        }

       

       
         
    }
}