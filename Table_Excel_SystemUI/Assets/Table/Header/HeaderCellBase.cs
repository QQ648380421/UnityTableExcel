using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static XP.TableModel.HeaderColumnCell;
using UnityEngine.EventSystems;
using static XP.TableModel.Cell;

namespace XP.TableModel
{
    public delegate void _CellDataChangeDelegate(HeaderCellBase cell, HeaderCellData columnCellData);
    /// <summary>
    /// �Ƿ��ڱ߽���ί��
    /// </summary>
    /// <param name="cell">��Ԫ��</param>
    /// <param name="isInsideBoundary">�Ƿ��ڱ߽���</param>
    public delegate void _IsInsideBoundaryChangeDelegate(HeaderCellBase cell, bool isInsideBoundary);
    /// <summary>
    /// ��ͷ��Ԫ�����
    /// </summary>
    public abstract class HeaderCellBase : Toggle 
    {
        /// <summary>
        /// ����Ԫ�񱻵��ʱ����
        /// </summary>
        public event _CellClickDelegate _OnHeaderCellClickEvent;
        [Header("�����������仯ʱ����")]
        public InputField.OnChangeEvent _OnCellNameChanged;
        /// <summary>
        /// �����ݷ����仯ʱ����
        /// </summary>
        public event _CellDataChangeDelegate _OnCellDataChangeEvent;
        /// <summary>
        /// ��ק��ť
        /// </summary>
        public HeaderDragButton _DragButton;

        /// <summary>
        /// <see cref="_CellData"/>�����仯ʱ����
        /// </summary>
        /// <param name="data"></param>
        public abstract void OnCellDataChanged(HeaderCellData data);
        /// <summary>
        /// ����ˢ�µ�Ԫ������
        /// </summary>
        public abstract void _ResetPosition(HeaderCellData data);
        HeaderCellData cellData;
        /// <summary>
        /// ��Ԫ������
        /// </summary>
        public virtual HeaderCellData _CellData
        {
            get => cellData; set
            {
                if (cellData == value) return;
                if (cellData != null)
                {
                    cellData.PropertyChanged -= _CellData_PropertyChanged;
                }
                cellData = value; 
                _OnCellDataChangeEvent?.Invoke(this, value);
             
                if (value == null || value._CellObj) { 
                    Destroy(this.gameObject); 
                    return;
                }
               
                value._CellObj = this;
                value.PropertyChanged -= _CellData_PropertyChanged;
                value.PropertyChanged += _CellData_PropertyChanged;
                SetIsOnWithoutNotify(value._Selected); 
                OnCellDataChanged(value);
                if (value._ShowData == null)
                { 
                    _OnCellNameChanged?.Invoke(string.Empty);
                }
                else
                {
                    name = value._Index.ToString();
                    _OnCellNameChanged?.Invoke(value._ShowData.ToString());
                }

            }
        }
        /// <summary>
        /// ������ͷ����
        /// </summary>
        public HeaderBase _HeaderBase
        {
            get
            {
                if (!headerBase)
                {
                    headerBase = GetComponentInParent<HeaderBase>();
                }
                return headerBase;
            }
        }

        HeaderBase headerBase;


        Table table;
        public Table _Table
        {
            get
            {
                if (!table)
                {
                    if (_HeaderBase==null)
                    {
                        return null;
                    }
                    table = _HeaderBase._Table;
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
                    if (!this) return null;
                    rectTransform = transform as RectTransform;
                }
                return rectTransform;
            }
        }


        Mask parentMask;
        /// <summary>
        /// ����������
        /// </summary>
        protected Mask _ParentMask
        {
            get
            {
                if (!parentMask)
                {
                    parentMask = GetComponentInParent<Mask>();
                }
                return parentMask;
            }
        }
    

        ToggleGroup toggleGroup;
        public ToggleGroup _ToggleGroup
        {
            get
            {
                if (!toggleGroup && _HeaderBase)
                {
                    toggleGroup = _HeaderBase._ToggleGroup;
                }
                return toggleGroup;
            }
        }
         
        
        /// <summary>
        /// ���ݷ����仯ʱ����
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _CellData_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (_CellData == null) return;
       
            if (e.PropertyName == nameof(HeaderCellData._ShowData))
            {
                if (_CellData._ShowData == null)
                {
                    _OnCellNameChanged?.Invoke(string.Empty);
                }
                else
                {
                    _OnCellNameChanged?.Invoke(_CellData._ShowData.ToString());
                }

            }
            else if (e.PropertyName == nameof(HeaderCellData._Position)
                || e.PropertyName == nameof(HeaderCellData._Size))
            {//��ߴ�С�����仯�����Ӽ�����һ�¿�ߴ�С
                _ResetPosition(this._CellData); 
            }
          
        }
        protected override void Awake()
        {
            base.Awake();
            if (!Application.isPlaying) return;
            this.group = _ToggleGroup;
        }
        protected override void Start()
        {
            base.Start();
            if (!Application.isPlaying) return;
            _DragButton._OnBeginDragEvent -= _DragButton__OnBeginDragEvent;
            _DragButton._OnBeginDragEvent += _DragButton__OnBeginDragEvent;
            _DragButton._OnEndDragEvent -= _DragButton__OnEndDragEvent;
            _DragButton._OnEndDragEvent += _DragButton__OnEndDragEvent;
            this.onValueChanged.AddListener(_IsOnChangedListener);
            _Table._OnRefreshEvent += _Table__OnRefreshEvent;

        }

        private void _DragButton__OnBeginDragEvent(object sender, PointerEventData e)
        {
            this.transform.SetAsLastSibling();
        }

        /// <summary>
        /// ��ȡ������Ԫ���б�
        /// </summary>
        public abstract IEnumerable<CellData> GetCells();
        /// <summary>
        /// ˢ��
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _Table__OnRefreshEvent(object sender, Table e)
        {
            _HeaderBase._ResetCellContentSize();
        }

        /// <summary>
        /// ѡ������仯�����¼�
        /// </summary>
        /// <param name="value"></param>
        protected virtual void _IsOnChangedListener(bool value)
        {
            if (cellData == null) return;
            cellData._Selected = value;
            if (value)
            {//�������ѡ�У��������屻ɾ��
                foreach (var item in _HeaderBase._CurrentSelectHeaderCells)
                {
                    item._Selected = false;
                }
                _HeaderBase._CurrentSelectHeaderCells.Clear();
            }
            if (_HeaderBase)
            {
                if (value)
                {
                    if (!_HeaderBase._CurrentSelectHeaderCells.Contains(this._CellData))
                    {
                        _HeaderBase._CurrentSelectHeaderCells.Add(this._CellData);
                    }
                }
                else
                {
                    if (_HeaderBase._CurrentSelectHeaderCells.Contains(this._CellData))
                    {
                        _HeaderBase._CurrentSelectHeaderCells.Remove(this._CellData);
                    }
                }
            }

            _IsOnChanged(value);
        }
        /// <summary>
        /// ѡ������仯
        /// </summary>
        /// <param name="value"></param>
        protected abstract void _IsOnChanged(bool value);

        protected override void OnDestroy()
        {
            base.OnDestroy(); 
            if (cellData != null)
            {
                cellData.PropertyChanged -= _CellData_PropertyChanged;
            }
            if (_DragButton)
            {
                _DragButton._OnEndDragEvent -= _DragButton__OnEndDragEvent;
            }
            if (_HeaderBase)
            {
                if (_HeaderBase._HeaderCells.Contains(this))
                {
                    _HeaderBase._HeaderCells.Remove(this);
                }
            }
            if (_Table)
            {
                _Table._OnRefreshEvent -= _Table__OnRefreshEvent;
            }

        }

        /// <summary>
        /// ���ÿ�ߴ�С
        /// </summary>
        public virtual void _SetRectSize_X(float x)
        {
            Vector2 vector2= _RectTransform.sizeDelta;
            vector2.x = x;
            _SetRectSize(vector2);
        }
        /// <summary>
        /// ���ÿ�ߴ�С
        /// </summary>
        public virtual void _SetRectSize_Y(float y)
        {
            Vector2 vector2 = _RectTransform.sizeDelta;
            vector2.y = y;
            _SetRectSize(vector2);
        }
        /// <summary>
        /// ���ÿ�ߴ�С
        /// </summary>
        public virtual void _SetRectSize(Vector2 rectSize) {
            _RectTransform.sizeDelta = rectSize;
            _DragButton__OnEndDragEvent(null,null);
        }
        
        /// <summary>
        /// ˢ�¿�ߴ�С
        /// </summary>
        public abstract void _ResetDataSize();

        /// <summary>
        /// ��ק��ť����
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void _DragButton__OnEndDragEvent(object sender, UnityEngine.EventSystems.PointerEventData e)
        { 
            _HeaderBase._ResetCellDatasPosition();
            _HeaderBase._ResetCellContentSize();
            _ResetDataSize();
            //ˢ�¸��л��е����е�Ԫ��
            _HeaderBase._ResetHeaderCellPosition();
        }
        public override void OnPointerClick(PointerEventData eventData)
        {
            base.OnPointerClick(eventData);
            _OnHeaderCellClickEvent?.Invoke(new CellClickData()
            {
                _Selectable = this,
                _EventData = eventData,
                 _HeaderCell=this
            });
        }

       
    }
}