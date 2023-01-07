using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
namespace XP.TableModel
{
    /// <summary>
    /// 表头行
    /// </summary>
    public class HeaderRow : HeaderBase
    {
        public override void _ScrollRectValueChanged(Vector2 vector2)
        {
            var _v2 = _RectTransform.anchoredPosition;
            _v2.y = _Table._CellContent.anchoredPosition.y;
            _RectTransform.anchoredPosition = _v2;
        }

        /// <summary>
        /// 重置单元格画布大小
        /// </summary>
        /// <returns></returns>
        private IEnumerator _ResetCellContentSize_Async()
        {
            yield return new WaitForEndOfFrame();
            var __cellContentSize = _Table._CellContent.sizeDelta;
  
            float _addOffsetSize = 0;
            if (_Table._ScrollRect.horizontalScrollbar)
            {
                var _horizontalScrollbarRect = (RectTransform)_Table._ScrollRect.horizontalScrollbar.transform;
                var _cellViewRect = _Table._CellView._Mask.rectTransform;
                if (_RectTransform.sizeDelta.y >= _cellViewRect.rect.height)
                {
                    _addOffsetSize = _horizontalScrollbarRect.sizeDelta.y;
                }
            }
            __cellContentSize.y = _RectTransform.sizeDelta.y+ _addOffsetSize;
            _Table._CellContent.sizeDelta = __cellContentSize;
            _Invoke_RectSizeChangedEvent();
        }
        public override void _ResetCellContentSize()
        {
            StartCoroutine(_ResetCellContentSize_Async());
        }

        protected override void Reset()
        {
            base.Reset();
            Unit._AddComponent<HorizontalLayoutGroup>(this.gameObject);
        }

        public override void _OnAddHeaderCellCreateCellData(HeaderCellData headerCellData)
        {
            int columnCount = _Table._HeaderColumn._HeaderCellsCount;
            for (int i = 0; i < columnCount; i++)
            {
                Vector2Int indexV2 = new Vector2Int(i,headerCellData._Index);
                this._CreateCellData(indexV2);
            }

        }

        public override void _Remove(int index)
        {
            base._Remove(index);
            var _cellDatas = _Table._CellDatas.Where(p => p != null && p._Row == index);
            if (_cellDatas == null || _cellDatas.Count()<=0) return;
            var _removeArr= _cellDatas.ToArray(); 
            foreach (var item in _removeArr)
            {
                _Table._CellDatas.Remove(item);
            }
            foreach (var item in _Table._CellDatas)
            {
                if (item._Row >= index)
                {
                    item._Row--;
                }
            }
            _Table._Refresh();
        }

        protected override void _UpdateCells()
        {
            float pos = _RectTransform.anchoredPosition.y;
            float viewSize = _Parent.rect.height;
            //计算宽高
            float size = _GetSize();
            if (size != _RectTransform.sizeDelta.y)
            {//大小发生变化
                _RectTransform.sizeDelta = new Vector2(_RectTransform.sizeDelta.x, size); 
                _ResetCellContentSize();
            }
            _UpdateCells(pos, viewSize);
        }
    }
}