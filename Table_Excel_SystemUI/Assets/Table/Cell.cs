using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using static XP.TableModel.Cell;
using UnityEngine.Events;
using System;
using System.Linq;
using UnityEngine.EventSystems;

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
        /// �����е����ݷ����仯
        /// </summary>
        private void _CellData_DataPropertyChanged() {
            string dataStr = string.Empty;
            if (cellData != null && cellData._Data != null)
            {
                dataStr = cellData._Data.ToString();
            }
            _CellDataChangedEvents_String?.Invoke(dataStr);
        }
        /// <summary>
        /// ��Ԫ������
        /// </summary>
        public CellData _CellData { get => cellData; set {
                if (cellData == value) return;
                if (cellData!=null)
                {
                    cellData.PropertyChanged -= Value_PropertyChanged;
                }
                cellData = value;
                _ClearIsInsideBoundaryChangedEvent(); 
                _Invoke__CellDataChangeEvent(this, value); 
                _CellDataChangedEvents?.Invoke(value);
                _CellData_DataPropertyChanged();
                if (value == null) return;
                value._Cell = this;   
                SetIsOnWithoutNotify(value._Selected);//����ѡ���״̬
                value.PropertyChanged -= Value_PropertyChanged;
                value.PropertyChanged += Value_PropertyChanged;
                name = value._Column+","+value._Row;

            }  }

        /// <summary>
        /// �����ڲ����Է����仯ʱ����
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Value_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (cellData == null) return;
            if (e.PropertyName==nameof(CellData._Selected))
            {//ѡ��״̬�����仯
                SetIsOnWithoutNotify(cellData._Selected);//����ѡ���״̬ 
            }else
            if (e.PropertyName == nameof(CellData._Data))
            {//���ݷ����仯
                _CellData_DataPropertyChanged();
            }
        }
        
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
        public override void OnPointerClick(PointerEventData eventData)
        {
            base.OnPointerClick(eventData);
            if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightCurlyBracket)) return;
            foreach (var item in _Table._CellDatas)
            {
                if (item._Cell!=this)
                {
                    item._Selected = false;
                }
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
  

        /// <summary>
        /// �е�Ԫ�����仯
        /// </summary>
        public event _CellBaseChangedDelegate _RowCellChangedEvent;

        HeaderBase column;
        /// <summary>
        /// ������
        /// </summary>
        public HeaderBase _Column
        {
            get
            {
                if (columnCell)
                {
                    return columnCell._HeaderBase;
                }
                return null;
            } 
        }


        HeaderBase row;
        /// <summary>
        /// ������
        /// </summary>
        public HeaderBase _Row
        {
            get
            {
                if (rowCell)
                {
                    return rowCell._HeaderBase;
                }
                return null;
            } 
        }

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
                _RowCellChangedEvent?.Invoke(this, value);
                if (rowCell)
                {
                    rowCell._IsInsideBoundaryChangedEvent += _ColumnAndRowCell__IsInsideBoundaryChangedEvent;
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
            {//û����*********************** 
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
                _Table._MultiSelectChangedEvent += _Table__MultiSelectChangedEvent;

            }
            this.onValueChanged.AddListener(_IsOnValueChanged); 
        }
        /// <summary>
        /// ѡ��״̬�����仯
        /// </summary>
        private void _IsOnValueChanged(bool value) { 
            if (cellData == null) return;
            cellData._Selected = value;
            if (value)
            {//ѡ�б�ͷ��Ԫ��
                if (_ColumnCell)
                {
                    _ColumnCell.SetIsOnWithoutNotify(value);
                }
                if (_RowCell)
                {
                    _RowCell.SetIsOnWithoutNotify(value);
                }
              
            }
         
        }
        /// <summary>
        /// �����ѡ�����仯
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _Table__MultiSelectChangedEvent(object sender, bool e)
        {
            if (e)
            {
                group = null;
            }
            else
            {
                if (_CellView)
                {
                    group = _CellView._ToggleGroup;
                } 
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
                _Table._OnRefreshEvent -= _Table__OnRefreshEvent;
                _Table._MultiSelectChangedEvent -= _Table__MultiSelectChangedEvent;
            }
            this.onValueChanged.RemoveListener(_IsOnValueChanged); 
      
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
            _Initialization();
        }

        /// <summary>
        /// ˢ�¸õ�Ԫ������
        /// </summary>
        private void _UpdateData() { 
             var _index= _Index();
            if (_Table)
            { 
                _CellData = _Table._CellDatas[_index];
                if (_CellData==null)
                {
                    Destroy(this.gameObject);
                    return;
                }
                _CellData._ColumnCell = _ColumnCell;
                _CellData._RowCell = _RowCell;
           
            }
        
        }
        /// <summary>
        /// ��ʼ��
        /// </summary>
        public virtual  void _Initialization() {
            StartCoroutine(_YieldUpdatePos());
            if (_Table) _Table__MultiSelectChangedEvent(_Table, _Table._MultiSelect);
            _UpdateData();
        }
        protected override void Start()
        {
            base.Start();
            _RegisterEvents();
            _Initialization();
        }

        private void Update()
        {
            if (_ColumnCell)
            {
                _ColumnAndRowCell__IsInsideBoundaryChangedEvent(_ColumnCell, _ColumnCell._IsInsideBoundary);
            }
            else
            {
                Destroy(this.gameObject);
            }
            if (_RowCell)
            {
                _ColumnAndRowCell__IsInsideBoundaryChangedEvent(_RowCell, _RowCell._IsInsideBoundary);
            }
            else
            {
                Destroy(this.gameObject);
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
            if (cellData != null)
            {
                cellData.PropertyChanged -= Value_PropertyChanged;
            }
            
        }

    }
}