using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.ObjectModel;
using UnityEngine.EventSystems;
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
        RectTransform parent;
        /// <summary>
        /// ����任���
        /// </summary>
        public RectTransform _Parent
        {
            get
            {
                if (!parent)
                {
                    parent = transform.parent.GetComponent<RectTransform>();
                }
                return parent;
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
        /// ���б�ͷ��Ԫ������ʵ��
        /// </summary> 
        public readonly ObservableCollection<HeaderCellData> _HeaderCellDatas = new ObservableCollection<HeaderCellData>();
        
        /// <summary>
        /// ��ǰѡ�б�ͷ��Ԫ�񼯺�
        /// </summary>
        public readonly ObservableCollection<HeaderCellData> _CurrentSelectHeaderCells = new ObservableCollection<HeaderCellData>();

        /// <summary>
        /// ��ǰ��ͼ��Χ����ʾ�ĵ�Ԫ�񻺴�
        /// </summary>
        public List<HeaderCellData> _CurrentViewCellDatas=new List<HeaderCellData>();
     
        /// <summary>
        /// �����ͷ��ߴ�С
        /// </summary>
        /// <returns></returns>
        public float _GetSize() {
            float size = 0; 
            for (int i = 0; i < _HeaderCellDatas.Count; i++)
            {
                var item = _HeaderCellDatas[i];
                size += item._Size;
            }
            return size;
        }

        /// <summary>
        ///  ��ȡ��Χ�ڵ�Ԫ��
        /// </summary>
        /// <param name="position">��ʼλ��</param>
        /// <param name="viewSize">��ʼλ��+��ͼ��С</param>
        /// <returns></returns>
        public virtual IEnumerable<HeaderCellData> _GetViewCellDatas(float position,float viewSize)
        { 

            return _HeaderCellDatas.Where(
                p=>
                (p._Position+p._Size) >=position 
                &&
              (position + viewSize ) >=(p._Position )

                );
        }

    
         

        /// <summary>
        /// ��ͷ��Ԫ������
        /// </summary>
        public int _HeaderCellsCount
        {
            get
            {
                return _HeaderCellDatas.Count;
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
        /// �������е�Ԫ��λ����������
        /// </summary>
        public virtual void _ResetCellDatasPosition() {
          var _cellDatas=  _HeaderCellDatas.OrderBy(p=>p._Index);
            float pos =0;
            foreach (var item in _cellDatas)
            {
                item._Position = pos; 
                pos += item._Size; 
            }
        }
        /// <summary>
        /// ��ӵ�Ԫ��
        /// </summary>
        /// <param name="columnCellData"></param>
        public virtual HeaderCellData _Add(HeaderCellData cellData)
        {
            if (cellData == null) return null;
            cellData._Index = this._HeaderCellDatas.Count;
            _HeaderCellDatas.Add(cellData); 
            _ResetCellDatasPosition();
            cellData.PropertyChanged -= _HeaderCellData_PropertyChanged;
            cellData.PropertyChanged += _HeaderCellData_PropertyChanged; 
            _OnAddHeaderCellCreateCellData(cellData); 
            _ResetCellContentSize(); 
            return cellData;
        }
        /// <summary>
        /// ���ñ�ͷ��Ԫ��λ��
        /// </summary>
        public virtual void _ResetHeaderCellPosition() {
            float pos = 0;
            var _buffer = _HeaderCellDatas.OrderBy(p => p._Index).ToList();
            for (int i = 0; i < _buffer.Count; i++)
            {
                var _item = _buffer[i];
                _item._Position = pos;
                pos += _item._Size;
                if (_item._CellObj)
                {
                    _item._CellObj.OnCellDataChanged(_item);
                }
            }
            foreach (var item in _HeaderCells)
            {
                if (!item) continue;
                item._ResetPosition(item._CellData);
            }
        }

        
        /// <summary>
        /// ��ͷ��Ԫ���������ݷ����仯
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _HeaderCellData_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(HeaderCellData._Size) 
                || e.PropertyName == nameof(HeaderCellData._Index)
                )
            {//�κ�һ����Ԫ���С�����˱仯
                //���¼���λ�� 
                _ResetHeaderCellPosition(); 
            }
           
          
        }

        /// <summary>
        /// ������ͷ��Ԫ��
        /// </summary>
        /// <returns></returns>
        protected virtual HeaderCellBase _CreateHeaderCell() {
            var _newObj = Instantiate(_HeaderCellPrefab, _CreatePrefabView); 
            _HeaderCells.Add(_newObj);
            return _newObj;
        }
    
        /// <summary>
        /// ���µ�Ԫ��
        /// </summary>
        protected abstract void _UpdateCells();
        /// <summary>
        /// ˢ�µ�Ԫ��
        /// </summary>
        /// <param name="pos"></param>
        /// <param name="viewSize"></param>
        protected virtual void _UpdateCells(float pos, float viewSize)
        {
          
            //�µ���ͼ�ڵ�Ԫ��
            var _ViewCellDatasBuffer = _GetViewCellDatas(pos, viewSize).ToList(); 
            var _removeCells = _CurrentViewCellDatas.Where(p => _ViewCellDatasBuffer.Contains(p) == false || p._CellObj==null);
             
            //�Ƴ���Ԫ�񻺴�
            Queue<HeaderCellData> _removeCellsQueue = new Queue<HeaderCellData>(_removeCells);
         
            //���������
            for (int i = 0; i < _ViewCellDatasBuffer.Count; i++)
            {
                //�µ�Ԫ��
                var _newCellData = _ViewCellDatasBuffer[i];
                var _cell = _CurrentViewCellDatas.FirstOrDefault(p => p == _newCellData);
                if (_cell != null && _cell._CellObj)
                {//������ʾ�����ù�  
                    _cell._CellObj._CellData = _newCellData; 
                    continue;
                }
                else
                {//û����ʾ�����ĵ�Ԫ������
                    if (_removeCellsQueue.Count > 0)
                    {//����ʣ�µ�û���Ƴ��ĵ�Ԫ�����ݿ�����
                        var _oldCell = _removeCellsQueue.Dequeue();
                        if (_oldCell._CellObj)
                        {//�й�������  
                            _oldCell._CellObj._CellData = _newCellData;//ֱ�Ӹ��� 
                            continue;
                        } 
                    }
                    //û�й������壬���ߵ�Ԫ�񲻹�
                    var _newCell = _CreateHeaderCell();
                    _newCell._CellData = _newCellData;
                }
            }
            for (int i = 0; i < _removeCellsQueue.Count; i++)
            {
                var item = _removeCellsQueue.Dequeue();
                //����ʣ�µĶ����
                if (item._CellObj)
                { 
                    item._CellObj._CellData = null;
                } 
            }
          
            _CurrentViewCellDatas = _ViewCellDatasBuffer;


        }
        protected virtual void Update()
        {
            _UpdateCells();
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
        return    _HeaderCells.FirstOrDefault(p=>p._CellData!=null && p._CellData._Index==index);
        }
        /// <summary>
        /// ɾ����ͷ
        /// </summary>
        /// <param name="row"></param>
        public virtual void _Remove(int index)
        {
           var _findCellData= _HeaderCellDatas.FirstOrDefault(p=>p._Index==index);
            if (_findCellData == null) return;
            var _cell= _HeaderCells.FirstOrDefault(p=>p._CellData== _findCellData);
            if (_cell)
            {//ɾ����ͷ������Ԫ��
                _cell._CellData = null;
            } 
            _HeaderCellDatas.Remove(_findCellData);
            foreach (var item in _HeaderCellDatas)
            {
                if (item._Index>=index)
                {
                    item._Index--;
                }
            }
        
        }

        
    }
}