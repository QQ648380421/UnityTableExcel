using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace XP.TableModel {
    /// <summary>
    /// ��Ԫ���������¼�
    /// </summary>
    public class CellClickData {
        Selectable selectable;
        PointerEventData eventData; 
        /// <summary>
        /// ���������������
        /// </summary>
        public Selectable _Selectable { get => selectable; set => selectable = value; }
        /// <summary>
        /// ��������������
        /// </summary>
        public PointerEventData _EventData { get => eventData; set => eventData = value; }
        /// <summary>
        /// ������ĵ�Ԫ��
        /// </summary>
        public Cell _Cell { get => cell; set => cell = value; }

        Cell cell ;
         
        HeaderCellBase headerCell;
        /// <summary>
        /// ������ı�ͷ
        /// </summary>
        public HeaderCellBase _HeaderCell
        {
            get
            {
                return headerCell;
            }
            set
            {
                if (headerCell == value) return;
                headerCell = value;
            }
        }

    }
    /// <summary>
    /// ��Ԫ����ί��
    /// </summary>
    /// <param name="selectable"></param>
    /// <param name="eventData"></param>
    public delegate void _CellClickDelegate(CellClickData cellClickData);
    public class CellDataBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        /// <summary>
        /// ����Ԫ�񱻵��ʱ����
        /// </summary>
        public event _CellClickDelegate _OnCellDataClickEvent;

        /// <summary>
        /// ������Ԫ�����¼�
        /// </summary>
        public void _Invoke_OnCellDataClickEvent(CellClickData cellClickData) {
            _OnCellDataClickEvent?.Invoke(cellClickData);
        }

        /// <summary>
        /// �����¼�<see cref="PropertyChanged"/>
        /// </summary>
        /// <param name="name"></param>
        protected void _InvokePropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        object showData;
        /// <summary>
        /// ��Ԫ����ʾ���ݣ���ʾ�㴫������ݣ�����ת��������Ҫ�ģ�������д<see cref="object.ToString()"/>
        /// </summary>
        public virtual object _ShowData
        {
            get => showData; set
            {
                if (showData == value) return;
                showData = value;
                _InvokePropertyChanged(nameof(_ShowData));
            }
        }
        object data;
        /// <summary>
        /// ��õ�Ԫ�������������
        /// </summary>
        public object _Data { get => data; set {
                if (data == value) return;
                data = value;
                _InvokePropertyChanged(nameof(_Data));
            } }

        bool selected;
        /// <summary>
        /// ѡ�е�Ԫ��
        /// </summary>
        public virtual bool _Selected
        {
            get
            {
                return selected;
            }
            set
            {
                if (selected == value) return;
                selected = value;
                _InvokePropertyChanged(nameof(_Selected)); 
            }
        }


        Table table;
        /// <summary>
        /// ������
        /// </summary>
        public Table _Table
        {
            get
            {
                return table;
            }
            set
            {
                if (table == value) return;
                table = value;
                _InvokePropertyChanged(nameof(_Selected));
            }
        }

  
    }
}