using System;
using System.Collections;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static XP.TableModel.Cell;

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
        /// ��Ԫ�񱻵���¼�
        /// </summary>
        public event _CellClickDelegate _OnCellClickEvent;
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
            if (cellData != null && cellData._ShowData != null)
            {
                dataStr = cellData._ShowData.ToString();
            } 
            _CellDataChangedEvents_String?.Invoke(dataStr);
            _CellDataChangeEvent?.Invoke(this,cellData);
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
                _Invoke__CellDataChangeEvent(this, value); 
                _CellDataChangedEvents?.Invoke(value);
                _CellData_DataPropertyChanged();
                if (value == null) return;
                value._Cell = this;   
                SetIsOnWithoutNotify(value._Selected);//����ѡ���״̬
                value.PropertyChanged -= Value_PropertyChanged;
                value.PropertyChanged += Value_PropertyChanged;
                StringBuilder str = new StringBuilder();
                str.Append(value._Column.ToString());
                str.Append(",");
                str.Append(value._Row.ToString());
                name = str.ToString();
           
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
            if (e.PropertyName == nameof(CellData._ShowData))
            {//���ݷ����仯
                _CellData_DataPropertyChanged();
            }
            else
            if (e.PropertyName == nameof(CellData._Cell))
            {//���ݷ����仯
                if (cellData._Cell==null &&this!=null)
                {
                    cellData._Cell = this;
                }
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
            var _cellClickData= new CellClickData() { 
             _Selectable=this,
              _EventData= eventData,
               _Cell=this
            }; 
            _OnCellClickEvent?.Invoke(_cellClickData);
           
            if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift) && cellData!=null)
            {//�������shift�����Կ��ж�ѡ
                Vector2Int _minIndex = Vector2Int.zero;
                Vector2Int _maxIndex = Vector2Int.zero;
                var _selectBuffer= _Table._CurrentSelectedCellDatas.Where(p=>p!=null &&p._Cell!=this);
                if (_selectBuffer == null || _selectBuffer.Count() <= 0) return;
                var _firstColumnIndex= _selectBuffer.Min(p=> p._Column); 
                 var _firstRowIndex = _selectBuffer.Min(p => p._Row);
                if (this.cellData._Column> _firstColumnIndex)
                {
                    _maxIndex.x = this.cellData._Column;
                    _minIndex.x = _firstColumnIndex;
                }
                else
                {
                    _maxIndex.x = _firstColumnIndex;
                    _minIndex.x = this.cellData._Column;
                }
                if (this.cellData._Row > _firstRowIndex)
                {
                    _maxIndex.y = this.cellData._Row;
                    _minIndex.y = _firstRowIndex;
                }
                else
                {
                    _maxIndex.y = _firstRowIndex;
                    _minIndex.y = this.cellData._Row;
                } 
                var _selectCells=  _Table._CellDatas.Where(p => 
                p != null 
                && p._Column >= _minIndex.x && p._Column<= _maxIndex.x
                && p._Row >= _minIndex.y && p._Row <= _maxIndex.y
                );

                var _selectedBuffer= _Table._CurrentSelectedCellDatas.ToArray();

                foreach (var item in _selectedBuffer)
                {
                    if (item._Cell == this) continue;
                    item._Selected = false;
                }
                foreach (var item in _selectCells)
                {
                    item._Selected = true;
                }
                return;
            }
            if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightCurlyBracket)) return;//����ctl�����Զ�ѡ��ֱ�ӷ���
            foreach (var item in _Table._CellDatas)
            {
                if ( item._Cell!=this)
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
   
        HeaderBase column;
        /// <summary>
        /// ������
        /// </summary>
        public HeaderBase _Column
        {
            get
            {
                if (_ColumnCell)
                {
                    column = _ColumnCell._HeaderBase; 
                }
                return column;
            } 
        }


        HeaderBase row;


        HeaderCellData columnCellData;
        /// <summary>
        /// �����е�Ԫ������
        /// </summary>
        public HeaderCellData _ColumnCellData
        {
            get
            {
                return columnCellData;
            }
            set
            {
                if (columnCellData == value) return;
                columnCellData = value; 
            }
        }
        HeaderCellData rowCellData;
        /// <summary>
        /// �����е�Ԫ������
        /// </summary>
        public HeaderCellData _RowCellData
        {
            get
            {
                return rowCellData;
            }
            set
            {
                if (rowCellData == value) return;
                rowCellData = value; 
            }
        }
         
        /// <summary>
        /// ������
        /// </summary>
        public HeaderBase _Row
        {
            get
            {
                if (_RowCell)
                {
                    row = _RowCell._HeaderBase; 
                }
                return row;
            } 
        }
         
        /// <summary>
        /// �����е�Ԫ��
        /// </summary>
        public HeaderCellBase _ColumnCell
        {
            get
            {
                if (columnCellData == null)
                {
                    return null;
                }
                return columnCellData._CellObj;
            } 
        }
         
        /// <summary>
        /// �����е�Ԫ��
        /// </summary>
        public HeaderCellBase _RowCell
        {
            get
            {
                if (rowCellData==null)
                {
                    return null;
                }
                return rowCellData._CellObj;
            } 
        }
  
        /// <summary>
        /// ˢ������λ��
        /// </summary>
        public void _UpdatePos()
        {
            Vector2 pos= _RectTransform.anchoredPosition
                        , size= _RectTransform.sizeDelta;

            if (_RowCell)
            {//��
                pos.y = _RowCell._RectTransform.anchoredPosition.y; 
                
                size.y = _RowCell._RectTransform.sizeDelta.y; 
              
            }
            if (_ColumnCell)
            {
                pos.x = _ColumnCell._RectTransform.anchoredPosition.x;  
                size.x= _ColumnCell._RectTransform.sizeDelta.x;  
            }
            if (_RectTransform.anchoredPosition != pos)
            {
                _RectTransform.anchoredPosition = pos;
            }
            if (_RectTransform.sizeDelta != size)
            {
                _RectTransform.sizeDelta = size;
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
            _OnCellClickEvent -= Cell__OnCellClickEvent;
            _OnCellClickEvent += Cell__OnCellClickEvent;
            if (_Table)
            { 
                _Table._OnRefreshEvent += _Table__OnRefreshEvent;
                _Table._MultiSelectChangedEvent += _Table__MultiSelectChangedEvent;
                _Table__MultiSelectChangedEvent(_Table, _Table._MultiSelect);

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
            this._OnCellClickEvent -= Cell__OnCellClickEvent;
            if (_Table)
            { 
                _Table._OnRefreshEvent -= _Table__OnRefreshEvent;
                _Table._MultiSelectChangedEvent -= _Table__MultiSelectChangedEvent;
            }
            this.onValueChanged.RemoveListener(_IsOnValueChanged); 
      
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
        /// ת����Ԫ�񱻵���¼������ⴥ������
        /// </summary>
        /// <param name="cellClickData"></param>
        private void Cell__OnCellClickEvent(CellClickData cellClickData)
        {
            if (cellData != null)
            {
                cellData._Invoke_OnCellDataClickEvent(cellClickData);
            }
            _Table._Invoke_OnCellClickEvent(cellClickData);
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
                if (_CellData._Cell!=this)
                { 
                    _CellData._Cell = this;
                } 
            }
        
        }
        /// <summary>
        /// ��ʼ��
        /// </summary>
        public virtual  void _Initialization() {
            cellData = null;
            //_UpdatePos(); 
            _UpdateData();
            _UpdatePos();
        }
        protected override void Start()
        {
            base.Start();
        
            _RegisterEvents(); 
            _Initialization();
        }

        private void Update()
        {
            _UpdatePos(); 
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            _ClearEvents(); 
            if (_CellView)
            {
                _CellView._Cells.Remove(this);
            }
            if (cellData != null)
            {
                cellData.PropertyChanged -= Value_PropertyChanged;
                cellData._Cell = null;
            } 
        }

    }
}