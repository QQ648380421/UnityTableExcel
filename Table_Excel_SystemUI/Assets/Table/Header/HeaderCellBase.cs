using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static XP.TableModel.HeaderColumnCell;

namespace XP.TableModel
{
    public delegate void _CellDataChangeDelegate(HeaderCellBase cell, HeaderCellData columnCellData);

    /// <summary>
    /// ��ͷ��Ԫ�����
    /// </summary>
    public  partial class HeaderCellBase : Button
    {
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

        HeaderCellData cellData;
        /// <summary>
        /// ��Ԫ������
        /// </summary>
        public HeaderCellData _CellData
        {
            get => cellData; set
            {
                if (cellData == value) return; 
                if (cellData != null)
                {
                    cellData.PropertyChanged -= _CellData_PropertyChanged;
                }
                cellData = value;
                if (cellData!=null)
                {
                    cellData.PropertyChanged -= _CellData_PropertyChanged;
                    cellData.PropertyChanged += _CellData_PropertyChanged;
                    transform.SetSiblingIndex(cellData._Index);
                } 
                _OnCellDataChangeEvent?.Invoke(this, value);
                if (value==null)
                {
                    _OnCellNameChanged?.Invoke(string.Empty);
                }
                else
                {
                    _OnCellNameChanged?.Invoke(value._Name); 
                }
           
            }
        }
        /// <summary>
        /// ������ͷ����
        /// </summary>
        public HeaderBase _HeaderBase { get {
                if (!headerBase)
                {
                    headerBase = GetComponentInParent<HeaderBase>(); 
                }
                return headerBase;
            } }

        HeaderBase headerBase;

        /// <summary>
        /// ���ݷ����仯ʱ����
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _CellData_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (_CellData == null) return;
            
            if ( e.PropertyName == nameof(HeaderCellData._Name))
            {
                _OnCellNameChanged?.Invoke(_CellData._Name);
            }
            else if (e.PropertyName == nameof(HeaderCellData._Index))
            {
                transform.SetSiblingIndex(_CellData._Index);
            }
        }

        protected override void Start()
        {
            base.Start();
            _DragButton.transform.SetSiblingIndex(transform.childCount);
            _DragButton._OnEndDragEvent -= _DragButton__OnEndDragEvent;
            _DragButton._OnEndDragEvent += _DragButton__OnEndDragEvent;
        }
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
        }

        /// <summary>
        /// ��ק��ť����
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void _DragButton__OnEndDragEvent(object sender, UnityEngine.EventSystems.PointerEventData e) {
            _HeaderBase._ResetCellContentSize();
        }
    }
}