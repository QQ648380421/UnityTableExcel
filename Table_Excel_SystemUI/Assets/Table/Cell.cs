using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using static XP.TableModel.Cell;
using UnityEngine.Events;
using System;
using System.Linq;

namespace XP.TableModel
{
    [Serializable]
    public class CellDataEvent : UnityEvent<object> { }
    public delegate void _CellDataChanged(Cell _cell, CellData _cellData);
    public delegate void _CellBaseChangedDelegate(Cell cell, HeaderCellBase headerCellBase);
    /// <summary>
    /// ��Ԫ��
    /// </summary>
    public partial class Cell : Toggle
    {
        /// <summary>
        /// ����������Բ鿴������
        /// </summary>
        [Header("DebugInfoData")]
        [SerializeField]
        private CellData cellData; 
        /// <summary>
        /// ��Ԫ������
        /// </summary>
        public CellData _CellData { get => cellData; set {
                if (cellData == value) return;
                cellData = value;
                _ClearIsInsideBoundaryChangedEvent(); 
                _Invoke__CellDataChangeEvent(this, value); 
                _CellDataChangedEvents?.Invoke(value);
                string dataStr = string.Empty;
                if (value!=null && value._Data!=null)
                {
                    dataStr = value._Data.ToString();
                }
                _CellDataChangedEvents_String?.Invoke(dataStr);
                if (value == null) return;
                value._Cell = this;
                isOn = value._Selected;
            }  }
          
        CellView cellView;
        /// <summary>
        /// ��Ԫ������
        /// </summary>
        public CellView _CellView
        {
            get
            {
                if (!cellView)
                {
                    GetComponentInParent<CellView>();
                }
                return cellView;
            }
            set
            {
                if (cellView == value) return;
                cellView = value;
            }
        }


        Table table;
        public Table _Table
        {
            get
            {
                if (!table)
                {
                    if (_CellView)
                    {
                        table = _CellView._Table;
                    } 
                }
                return table;
            } 
        }

        RectTransform rectTransform;

        public RectTransform _RectTransform
        {
            get
            {
                if (!rectTransform)
                {
                    rectTransform = GetComponent<RectTransform>();
                }
                return rectTransform;
            }
           
        }
  
        HeaderCellBase rowCell;
        /// <summary>
        /// �����е�Ԫ��
        /// </summary>
        public HeaderCellBase _RowCell
        {
            get
            {
                return rowCell;
            }
            set
            {
                if (rowCell == value) return;
                if (rowCell)
                {
                    rowCell._IsInsideBoundaryChangedEvent -= _ColumnAndRowCell__IsInsideBoundaryChangedEvent;
                }
                rowCell = value;
                _UpdatePos();
                _RowCellChangedEvent?.Invoke(this,value);
                if (rowCell)
                {
                    rowCell._IsInsideBoundaryChangedEvent += _ColumnAndRowCell__IsInsideBoundaryChangedEvent;
                }
            }
        }
        /// <summary>
        /// �е�Ԫ�����仯
        /// </summary>
        public event _CellBaseChangedDelegate _RowCellChangedEvent;

        HeaderCellBase columnCell;
        /// <summary>
        /// �����е�Ԫ��
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
                if (columnCell)
                {
                    columnCell._IsInsideBoundaryChangedEvent -= _ColumnAndRowCell__IsInsideBoundaryChangedEvent;
                }
                columnCell = value;
                _UpdatePos();
                _ColumnCellChangedEvent?.Invoke(this,value);
                if (columnCell)
                {
                    columnCell._IsInsideBoundaryChangedEvent += _ColumnAndRowCell__IsInsideBoundaryChangedEvent;
                     
                } 
            }
        }
        /// <summary>
        /// �е�Ԫ�����仯
        /// </summary>
        public event _CellBaseChangedDelegate _ColumnCellChangedEvent;

        /// <summary>
        /// ˢ������λ��
        /// </summary>
        private void _UpdatePos()
        {
            Vector2 pos= _RectTransform.anchoredPosition
                        , size= _RectTransform.sizeDelta;

            if (rowCell)
            {//��
                pos.y = rowCell._RectTransform.anchoredPosition.y; 
                
                size.y = rowCell._RectTransform.sizeDelta.y; 
              
            }
            if (columnCell)
            {
                pos.x = columnCell._RectTransform.anchoredPosition.x;  
                size.x= columnCell._RectTransform.sizeDelta.x;  
            }
            _RectTransform.anchoredPosition = pos;
            _RectTransform.sizeDelta = size;
        }

        /// <summary>
        /// �ȴ�һ֡ˢ��λ��
        /// </summary>
        /// <returns></returns>
        private IEnumerator _YieldUpdatePos()
        {
            yield return null;
            _UpdatePos();
        }


        /// <summary>
        /// �к��е���ʾ״̬�����仯
        /// </summary>
        /// <param name="cell"></param>
        /// <param name="isInsideBoundary"></param>
        private void _ColumnAndRowCell__IsInsideBoundaryChangedEvent(HeaderCellBase cell, bool isInsideBoundary)
        {
            if (isInsideBoundary==false)
            {
                //�����Լ� 
                if (this)
                {
                    Destroy(this.gameObject);
                } 
            }
        }

        /// <summary>
        /// <see cref="_Data"/>�����仯ʱ����
        /// </summary>
        public event _CellDataChanged _CellDataChangeEvent;

        /// <summary>
        /// <see cref="_Data"/>�����仯ʱ����
        /// </summary>
        public CellDataEvent _CellDataChangedEvents;

        /// <summary>
        /// <see cref="_Data"/>�����仯ʱ����,������崫ֵ
        /// </summary>
        public InputField.SubmitEvent _CellDataChangedEvents_String;
        /// <summary>
        /// ������Ԫ�����ݷ����仯�¼�
        /// </summary>
        /// <param name="_cell"></param>
        /// <param name="_cellData"></param>
        public void _Invoke__CellDataChangeEvent(Cell _cell, CellData _cellData) {
            _CellDataChangeEvent?.Invoke(_cell, _cellData);
        }
        /// <summary>
        /// ע���¼�
        /// </summary>
        private void _RegisterEvents() { 
            if (_Table)
            {
                _Table._HeaderColumn._OnRectSizeChangedEvent += _Header__OnRectSizeChangedEvent;
                _Table._HeaderRow._OnRectSizeChangedEvent += _Header__OnRectSizeChangedEvent;
                _Table._OnRefreshEvent += _Table__OnRefreshEvent;
            }
           
        }
        /// <summary>
        /// ����¼�
        /// </summary>
        private void _ClearEvents()
        {  
            if (_Table)
            {
                _Table._HeaderRow._OnRectSizeChangedEvent -= _Header__OnRectSizeChangedEvent;
                _Table._HeaderColumn._OnRectSizeChangedEvent -= _Header__OnRectSizeChangedEvent;
                _Table._OnRefreshEvent += _Table__OnRefreshEvent;
            }
        }
        /// <summary>
        /// ����߽��¼�
        /// </summary>
        private void _ClearIsInsideBoundaryChangedEvent() {
            if (columnCell)
            {
                columnCell._IsInsideBoundaryChangedEvent -= _ColumnAndRowCell__IsInsideBoundaryChangedEvent;
            }
        }
        private void _Header__OnRectSizeChangedEvent()
        {
            _UpdatePos();
        }

        /// <summary>
        /// ��Ԫ������
        /// </summary>
        public Vector2Int _Index(){
            
                var buffer= Vector2Int.zero;
                if (_ColumnCell && _ColumnCell._CellData != null)
                {
                    buffer.x = _ColumnCell._CellData._Index;
                }
                if (_RowCell && _RowCell._CellData!=null)
                {
                    buffer.y = _RowCell._CellData._Index;
                }
                return buffer; 
        }

        protected override void Awake()
        {
            base.Awake();
        }
        /// <summary>
        /// �����ñ��ˢ��ʱ����
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _Table__OnRefreshEvent(object sender, Table e)
        {
            _UpdateData();
        }

        /// <summary>
        /// ˢ�¸õ�Ԫ������
        /// </summary>
        private void _UpdateData() { 
             var _index= _Index();
            if (_Table)
            {
                _CellData = _Table._CellDatas[_index];
            }
        
        }
        protected override void Start()
        {
            base.Start();
            _RegisterEvents();
            StartCoroutine(_YieldUpdatePos());
            _UpdateData();
            if (_CellView)
            {
                this.group = _CellView._ToggleGroup; 
            }

        }


        protected override void OnDestroy()
        {
            base.OnDestroy();
            _ClearEvents();
            _ClearIsInsideBoundaryChangedEvent();
            if (_CellView)
            {
                _CellView._Cells.Remove(this);
            }
         
        }

    }
}