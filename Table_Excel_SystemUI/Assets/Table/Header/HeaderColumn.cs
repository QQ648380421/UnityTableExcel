using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace XP.TableModel
{
    /// <summary>
    /// 表头列
    /// </summary>
    public class HeaderColumn : Selectable
    {
        Table _table;

        public Table _Table { get {
                if (!_table)
                {
                    _table = GetComponentInParent<Table>();
                }
                return _table;
            }  }
        RectTransform rectTransform;
        RectTransform _RectTransform {
            get {
                if (!rectTransform)
                {
                    rectTransform = GetComponent<RectTransform>();
                }
                return rectTransform;
            }
        }
        /// <summary>
        /// 表头列单元格预制体
        /// </summary>
        public HeaderColumnCell _HeaderColumnCellPrefab;

        /// <summary>
        /// 预制体容器
        /// </summary>
        public Transform _CreatePrefabView;
        /// <summary>
        /// 所有列单元格
        /// </summary>

        public readonly List<HeaderColumnCell> _ColumnCells = new List<HeaderColumnCell>();

        protected override void Start()
        {
            base.Start();

            StartCoroutine(_ResetCellContentSize());
           
             
            _Table._ScrollRect.onValueChanged.AddListener(_ScrollRectValueChanged); 
        }
        private void _ScrollRectValueChanged(Vector2 vector2) { 
            var _v2= _RectTransform.anchoredPosition;
            _v2.x = _Table._CellContent.anchoredPosition.x;
            _RectTransform.anchoredPosition = _v2; 
        }

        /// <summary>
        /// 重置单元格画布大小
        /// </summary>
        /// <returns></returns>
        private IEnumerator _ResetCellContentSize() {
            yield return new WaitForEndOfFrame();
            var __cellContentSize = _Table._CellContent.sizeDelta;
            __cellContentSize.x = _RectTransform.sizeDelta.x;
            _Table._CellContent.sizeDelta = __cellContentSize; 
        }
        /// <summary>
        /// 添加列
        /// </summary>
        /// <param name="columnCellData"></param>
        public void _AddColumn(HeaderColumnCell.ColumnCellData columnCellData)
        {
            if (columnCellData == null) return;
           var _newColumn= Instantiate(_HeaderColumnCellPrefab, _CreatePrefabView);
            _newColumn._ColumnCellData = columnCellData;
            _ColumnCells.Add(_newColumn);
            StartCoroutine(_ResetCellContentSize());
        }
        
        

    }
}