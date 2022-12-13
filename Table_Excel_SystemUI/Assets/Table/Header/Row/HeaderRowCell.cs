using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static XP.TableModel.HeaderColumnCell;

namespace XP.TableModel
{

    /// <summary>
    /// ��ͷ��Ԫ��
    /// </summary>
    public class HeaderRowCell : HeaderCellBase
    {

        public override bool InsideBoundary()
        {
            //������x����λ��
            float parentPos_y = Mathf.Abs(_HeaderBase._RectTransform.anchoredPosition.y);
            float _addHeith = 0;//�ۼӵĸ߶� 
            float _parentSize = Mathf.Abs(_ParentMask.rectTransform.rect.height);
            float _Pos_y = Mathf.Abs(_RectTransform.anchoredPosition.y);

            for (int i = 0; i < _HeaderBase._HeaderCellsCount; i++)
            {
                var _cell = _HeaderBase._TransformIndexFindCell(i);
                if (!_cell) continue;
                _addHeith += _cell._RectTransform.sizeDelta.y;
                if (_cell.gameObject == this.gameObject)
                {//һֱ�ۼӵ��Լ�
                    break;
                }
            }
            if (parentPos_y < _addHeith && (parentPos_y + _parentSize) > _Pos_y)
            {
                return true;
            }
            return false;
        }
      
    }
}