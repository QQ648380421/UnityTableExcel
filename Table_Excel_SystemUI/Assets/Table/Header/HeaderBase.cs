using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.ObjectModel;
namespace XP.TableModel
{
    /// <summary>
    /// ��ͷ����
    /// </summary>
    public abstract class HeaderBase : MonoBehaviour
    {
     
        Table _table;
        /// <summary>
        /// ��������
        /// </summary>
        public Table _Table { get {
                if (!_table)
                {
                    _table = GetComponentInParent<Table>();
                }
                return _table;
            }  }
        RectTransform rectTransform;
        /// <summary>
        /// ����任���
        /// </summary>
       public  RectTransform _RectTransform {
            get {
                if (!rectTransform)
                {
                    rectTransform = GetComponent<RectTransform>();
                }
                return rectTransform;
            }
        }

        /// <summary>
        /// ��ͷ��Ԫ��Ԥ����
        /// </summary>
        public HeaderCellBase _HeaderCellPrefab;

        /// <summary>
        /// Ԥ��������
        /// </summary>
        public Transform _CreatePrefabView;
        /// <summary>
        /// �任�����С�����仯�¼�
        /// </summary>
        public event Action _OnRectSizeChangedEvent;
        /// <summary>
        /// ���б�ͷ��Ԫ��ʵ��
        /// </summary> 
        public readonly ObservableCollection<HeaderCellBase> _HeaderCells = new ObservableCollection<HeaderCellBase>();
        /// <summary>
        /// ��ǰѡ�б�ͷ��Ԫ�񼯺�
        /// </summary>
        public readonly ObservableCollection<HeaderCellBase> _CurrentSelectHeaderCells = new ObservableCollection<HeaderCellBase>();

        /// <summary>
        /// �߽��ڵĵ�Ԫ������
        /// </summary>
        /// <returns></returns>
        public virtual int InsideBoundaryCellCount() {
         return InsideBoundaryCell().Count(); 
        }
        /// <summary>
        /// �߽��ڵĵ�Ԫ��
        /// </summary>
        /// <returns></returns>
        public virtual IEnumerable<HeaderCellBase> InsideBoundaryCell()
        {
            return _HeaderCells.Where(p => p._IsInsideBoundary);
        }

        /// <summary>
        /// ��ͷ��Ԫ������
        /// </summary>
        public int _HeaderCellsCount
        {
            get
            {
                return _HeaderCells.Count;
            } 
        }
        HorizontalOrVerticalLayoutGroup layoutGroup;
        /// <summary>
        /// ����
        /// </summary>
        public HorizontalOrVerticalLayoutGroup _LayoutGroup
        {
            get
            {
                if (!layoutGroup)
                {
                    layoutGroup = GetComponent<HorizontalOrVerticalLayoutGroup>();
                }
                return layoutGroup;
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

        protected virtual void Start()
        {
          
            _ResetCellContentSize();
            _Table._ScrollRect.onValueChanged.AddListener(_ScrollRectValueChanged); 
        }
        protected virtual  void Reset()
        { 
            Unit._AddComponent<ContentSizeFitter>(this.gameObject); 
            Unit._AddComponent<ToggleGroup>(this.gameObject).allowSwitchOff=true;
        }
        public abstract void _ScrollRectValueChanged(Vector2 vector2);
        /// <summary>
        /// ����ӱ�ͷ��Ԫʱ������Ԫ������
        /// ���ǵ�������ͷ��ʱ��Ҫ����Ԫ�����ݻ���Ҳһ�𴴽���
        /// </summary>
        public abstract void _OnAddHeaderCellCreateCellData(HeaderCellData headerCellData);
        /// <summary>
        /// ������Ԫ������
        /// </summary>
        protected virtual Cell.CellData _CreateCellData( Vector2Int indexV2) { 
            var _findCellData = _Table._CellDatas[indexV2];
            if (_findCellData == null)
            {
                _findCellData = new Cell.CellData()
                {
                    _Column = indexV2.x,
                    _Row = indexV2.y,
                    _Table=this._Table
                };
                
                _Table._CellDatas.Add(_findCellData);
            }
            return _findCellData;
        }
        /// <summary>
        /// ��ӵ�Ԫ��
        /// </summary>
        /// <param name="columnCellData"></param>
        public virtual HeaderCellBase _Add(HeaderCellData cellData)
        {
            if (cellData == null) return null;
            var _newObj= Instantiate(_HeaderCellPrefab, _CreatePrefabView); 
            _newObj._CellData = cellData; 
            _HeaderCells.Add(_newObj);
            _OnAddHeaderCellCreateCellData(cellData); 
            _ResetCellContentSize(); 
            return _newObj;
        }

        /// <summary>
        /// ˢ�µ�Ԫ��������С
        /// </summary>
        public abstract void _ResetCellContentSize();

        /// <summary>
        /// ����<see cref="_OnRectSizeChangedEvent"/>�¼�
        /// </summary>
        protected virtual void _Invoke_RectSizeChangedEvent() {
            _OnRectSizeChangedEvent?.Invoke();
        }

        /// <summary>
        /// ����������任�����������Ѱ�ҵ�Ԫ��
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public virtual HeaderCellBase _TransformIndexFindCell(int index) {
              var _child= transform.GetChild(index);
            HeaderCellBase cell = _child.GetComponent<HeaderCellBase>() ;
            return cell;
        }
        /// <summary>
        /// ����������Ѱ�ҵ�Ԫ��
        /// </summary>
        /// <returns></returns>
        public virtual HeaderCellBase _FindCellOfIndex(int index) { 
        return    _HeaderCells.FirstOrDefault(p=>p._CellData._Index==index);
        }
        /// <summary>
        /// ɾ����ͷ
        /// </summary>
        /// <param name="row"></param>
        public virtual void _Remove(int index)
        {
            var _cell= _FindCellOfIndex(index);
            if (!_cell) return;
         
           
            Destroy(_cell.gameObject);
            foreach (var item in _HeaderCells)
            {
                if (item._CellData._Index>=index)
                {
                    item._CellData._Index--;
                }
            }
            _HeaderCells.Remove(_cell);
        }
    }
}