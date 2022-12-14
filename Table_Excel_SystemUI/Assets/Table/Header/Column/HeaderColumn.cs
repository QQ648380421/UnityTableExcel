using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace XP.TableModel
{
    /// <summary>
    /// ��ͷ��
    /// </summary>
    public class HeaderColumn : HeaderBase
    {  
        public override void _ScrollRectValueChanged(Vector2 vector2) {  
            var _v2= _RectTransform.anchoredPosition;
            _v2.x = _Table._CellContent.anchoredPosition.x;
            _RectTransform.anchoredPosition = _v2; 
        }

        /// <summary>
        /// ���õ�Ԫ�񻭲���С
        /// </summary>
        /// <returns></returns>
        private IEnumerator _ResetCellContentSize_Async() {
            yield return new WaitForEndOfFrame();
            var __cellContentSize = _Table._CellContent.sizeDelta;
            __cellContentSize.x = _RectTransform.sizeDelta.x;
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
    }
}