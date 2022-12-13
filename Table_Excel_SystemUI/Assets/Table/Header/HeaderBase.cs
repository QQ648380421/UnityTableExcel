using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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
        /// ���е�Ԫ��ʵ��
        /// </summary>

        public readonly List<HeaderCellBase> _HeaderCells = new List<HeaderCellBase>();

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

       

    }
}