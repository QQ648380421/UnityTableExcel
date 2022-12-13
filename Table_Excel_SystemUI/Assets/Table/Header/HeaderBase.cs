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
    public abstract class HeaderBase : Selectable
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


        protected override void Start()
        {
            base.Start();
            _ResetCellContentSize();
            _Table._ScrollRect.onValueChanged.AddListener(_ScrollRectValueChanged); 
        }
        protected override void Reset()
        {
            base.Reset();
            Unit._AddComponent<ContentSizeFitter>(this.gameObject);
            Unit._AddComponent<LayoutElement>(this.gameObject);  
        }
        public abstract void _ScrollRectValueChanged(Vector2 vector2);
         
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

    }
}