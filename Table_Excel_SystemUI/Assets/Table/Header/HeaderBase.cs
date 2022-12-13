using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace XP.TableModel
{
    /// <summary>
    /// 表头基类
    /// </summary>
    public abstract class HeaderBase : Selectable
    {
        Table _table;
        /// <summary>
        /// 表格控制器
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
        /// 本身变换组件
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
        /// 表头单元格预制体
        /// </summary>
        public HeaderCellBase _HeaderCellPrefab;

        /// <summary>
        /// 预制体容器
        /// </summary>
        public Transform _CreatePrefabView;

        /// <summary>
        /// 所有单元格实例
        /// </summary>

        public readonly List<HeaderCellBase> _HeaderCells = new List<HeaderCellBase>();

        HorizontalOrVerticalLayoutGroup layoutGroup;
        /// <summary>
        /// 布局
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
        /// 添加单元格
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
        /// 刷新单元格容器大小
        /// </summary>
        public abstract void _ResetCellContentSize();

       

    }
}