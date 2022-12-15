using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace XP.TableModel
{
    /// <summary>
    /// ��Ԫ������������ˢ�µ�Ԫ��λ��
    /// </summary>
    public class CellView : MonoBehaviour
    {
        Table _table;
        /// <summary>
        /// ��������
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
        /// ���еĵ�Ԫ��
        /// </summary>
        public readonly ObservableCollection<Cell> _Cells = new ObservableCollection<Cell>();
        /// <summary>
        /// ��Ԫ��Ԥ����
        /// </summary>
        public Cell _CellPrefab;
   
        /// <summary>
        /// ע���¼�
        /// </summary>
        private void _RegisterEvents() {
            _Table._HeaderRow._HeaderCells.CollectionChanged += _HeaderCells_CollectionChanged;
            _Table._HeaderColumn._HeaderCells.CollectionChanged += _HeaderCells_CollectionChanged;
        }
        /// <summary>
        /// �����¼�
        /// </summary>
        private void _ClearEvents()
        {
            if (_Table == null) return;
            if (  _Table._HeaderRow)
            {
                _Table._HeaderRow._HeaderCells.CollectionChanged -= _HeaderCells_CollectionChanged;
                foreach (var item in _Table._HeaderRow._HeaderCells)
                {
                    item._IsInsideBoundaryChangedEvent -= _Cell__IsInsideBoundaryChangedEvent;
                }
            }
            if (_Table._HeaderColumn)
            {
                _Table._HeaderColumn._HeaderCells.CollectionChanged -= _HeaderCells_CollectionChanged;
                foreach (var item in _Table._HeaderColumn._HeaderCells)
                {
                    item._IsInsideBoundaryChangedEvent -= _Cell__IsInsideBoundaryChangedEvent;
                }
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
        /// ʵ����һ����Ԥ����
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
        /// �к��б�ͷ��Ԫ�񼯺Ϸ����仯
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
                        //������ʾ�������¼�
                     var _addCell=   e.NewItems[0] as HeaderCellBase;
                        _addCell._IsInsideBoundaryChangedEvent -= _Cell__IsInsideBoundaryChangedEvent;
                        _addCell._IsInsideBoundaryChangedEvent += _Cell__IsInsideBoundaryChangedEvent;

                        break;
                    case System.Collections.Specialized.NotifyCollectionChangedAction.Move:
                        break;
                    case System.Collections.Specialized.NotifyCollectionChangedAction.Remove:
                        var _removeCell = e.OldItems[0] as HeaderCellBase;
                        _removeCell._IsInsideBoundaryChangedEvent -= _Cell__IsInsideBoundaryChangedEvent; 
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

        /// <summary>
        /// ��Ԫ�����ʾ�����ط����仯
        /// </summary>
        /// <param name="cell"></param>
        /// <param name="isInsideBoundary"></param>
        private void _Cell__IsInsideBoundaryChangedEvent(HeaderCellBase cell, bool isInsideBoundary)
        {
            if (cell is HeaderColumnCell _headerColumnCell)
            {//������� 
                var _rowCells= _Table._HeaderRow.InsideBoundaryCell();
                foreach (var rowItem in _rowCells)
                {
                    CreatePrefab(_headerColumnCell, rowItem);
                }
            }
            else if(cell is HeaderRowCell _headerRowCell)
            {//��
                var _columnCells = _Table._HeaderColumn.InsideBoundaryCell();
                foreach (var columnItem in _columnCells)
                {
                    CreatePrefab(columnItem, _headerRowCell);
                }
            } 
        }


       
         
    }
}