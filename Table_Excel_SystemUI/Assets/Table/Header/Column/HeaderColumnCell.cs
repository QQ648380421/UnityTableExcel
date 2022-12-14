using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static XP.TableModel.HeaderColumnCell;

namespace XP.TableModel
{

    /// <summary>
    /// 表头单元格
    /// </summary>
    public class HeaderColumnCell : HeaderCellBase
    {
        public override bool InsideBoundary()
        {
            if (_HeaderBase==null) return false;
            //父物体x对象位置
            float parentPos_x = Mathf.Abs(_HeaderBase._RectTransform.anchoredPosition.x);
            float _addWidth = 0;//累加的宽度 
            float _parentSize = Mathf.Abs(_ParentMask.rectTransform.rect.width);
            float _Pos_x = _RectTransform.anchoredPosition.x;

            for (int i = 0; i < _HeaderBase._HeaderCellsCount; i++)
            {
                var _cell = _HeaderBase._TransformIndexFindCell(i);
                if (!_cell) continue;
                _addWidth += _cell._RectTransform.sizeDelta.x;
                if (_cell.gameObject == this.gameObject)
                {//一直累加到自己
                    break;
                }
            }

            if (_addWidth > parentPos_x && (parentPos_x + _parentSize) > _Pos_x)
            {
                return true;
            }
            return false;
        }

        protected override void _IsOnChanged(bool value)
        { 
            if (_Table)
            {
                foreach (var item in _Table._CellDatas)
                {
                    item._Selected = false;
                }
                var _cellDatas = _Table._CellDatas._GetColumnCellsData(_CellData._Index);

                if (_Table._MultiSelect)
                {

                    foreach (var item in _cellDatas)
                    { 
                        if (item._RowCell==null || item._RowCell.isOn == true && value == false)
                        {//如果有一边选中了，不能取消
                            continue;
                        }
                        item._Selected = value;
                    }
                }
                else
                {
                    var _cell = _cellDatas.OrderBy(p => p._Row).FirstOrDefault();
                    if (_cell != null)
                    {
                        _cell._Selected = value;
                    }
                }
            }
        }

       
    }
}