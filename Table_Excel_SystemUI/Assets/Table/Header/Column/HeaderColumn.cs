using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
namespace XP.TableModel
{
    /// <summary>
    /// 表头列
    /// </summary>
    public class HeaderColumn : HeaderBase
    {  
        public override void _ScrollRectValueChanged(Vector2 vector2) {  
            var _v2= _RectTransform.anchoredPosition;
            _v2.x = _Table._CellContent.anchoredPosition.x;
            _RectTransform.anchoredPosition = _v2; 
        }

        /// <summary>
        /// 重置单元格画布大小
        /// </summary>
        /// <returns></returns>
        private IEnumerator _ResetCellContentSize_Async() {
            yield return new WaitForEndOfFrame();
            var __cellContentSize = _Table._CellContent.sizeDelta;
            float _addOffsetSize = 0;
            if (_Table._ScrollRect.verticalScrollbar)
            {
                var _verticalScrollbarRect = (RectTransform)_Table._ScrollRect.verticalScrollbar.transform; 
                var _cellViewRect = _Table._CellView._Mask.rectTransform;
                if (_RectTransform.sizeDelta.x >= _cellViewRect.rect.width)
                {
                    _addOffsetSize = _verticalScrollbarRect.sizeDelta.x;
                }
            }
    
            __cellContentSize.x = _RectTransform.sizeDelta.x + _addOffsetSize; 
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

          int rowCount=  _Table._HeaderRow._HeaderCellsCount;
            for (int i = 0; i < rowCount; i++)
            {
                Vector2Int indexV2 = new Vector2Int(headerCellData._Index, i );
               this._CreateCellData(indexV2); 
            }
         
        }


        public override void _Remove(int index)
        {
            base._Remove(index);
          var _cellDatas=  _Table._CellDatas.Where(p => p != null && p._Column==index);
            if (_cellDatas == null || _cellDatas.Count() <= 0) return;
            var _removeArr = _cellDatas.ToArray();
            foreach (var item in _removeArr)
            {
                _Table._CellDatas.Remove(item);
            }
            foreach (var item in _Table._CellDatas)
            {
                if (item._Column>=index)
                {
                    item._Column--;
                }
            }
            _Table._Refresh();
        }

        protected override void _UpdateCells()
        {
            float pos =- _RectTransform.anchoredPosition.x;
            float viewSize = _Parent.rect.width;
            //计算宽高
            float size = _GetSize();
            if (size != _RectTransform.sizeDelta.x)
            {//大小发生变化
                _RectTransform.sizeDelta = new Vector2(size,_RectTransform.sizeDelta.y );
                _ResetCellContentSize();
            }
            _UpdateCells(pos, viewSize);

        }
    }
}