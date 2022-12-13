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
            __cellContentSize.x = _RectTransform.sizeDelta.x;
            _Table._CellContent.sizeDelta = __cellContentSize; 
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

    }
}