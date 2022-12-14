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
    public class HeaderRowCell : HeaderCellBase
    {

        public override bool InsideBoundary()
        {
            //父物体x对象位置
            float parentPos_y = Mathf.Abs(_HeaderBase._RectTransform.anchoredPosition.y);
            float _addHeith = 0;//累加的高度 
            float _parentSize = Mathf.Abs(_ParentMask.rectTransform.rect.height);
            float _Pos_y = Mathf.Abs(_RectTransform.anchoredPosition.y);

            for (int i = 0; i < _HeaderBase._HeaderCellsCount; i++)
            {
                var _cell = _HeaderBase._TransformIndexFindCell(i);
                if (!_cell) continue;
                _addHeith += _cell._RectTransform.sizeDelta.y;
                if (_cell.gameObject == this.gameObject)
                {//一直累加到自己
                    break;
                }
            }
            if (parentPos_y < _addHeith && (parentPos_y + _parentSize) > _Pos_y)
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
                var _cellDatas = _Table._CellDatas._GetRowCellsData(_CellData._Index);
                if (_Table._MultiSelect)
                {
                    foreach (var item in _cellDatas)
                    {
                        if (item._ColumnCell.isOn == true && value == false)
                        {//如果有一边选中了，不能取消
                            continue;
                        }
                        item._Selected = value;
                    }
                }
                else
                {
                    var _cell = _cellDatas.OrderBy(p => p._Column).FirstOrDefault();
                    if (_cell != null)
                    {
                        _cell._Selected = value;
                    }
                }
            }
        }
    }
}