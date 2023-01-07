using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static XP.TableModel.Cell;
using static XP.TableModel.HeaderColumnCell;

namespace XP.TableModel
{

    /// <summary>
    /// 表头单元格
    /// </summary>
    public class HeaderRowCell : HeaderCellBase
    {
   

        public override IEnumerable<CellData> GetCells()
        { 
          return  _Table._CellDatas._GetRowCellDatas(_CellData._Index) ;
        }

        public override void _ResetDataSize()
        {
            if (_CellData == null) return;
            _CellData._Size = _RectTransform.sizeDelta.y;
        }

        public override void OnCellDataChanged(HeaderCellData data)
        { 
            _ResetPosition(data);
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

        public override void _ResetPosition(HeaderCellData data)
        {
            if (data == null) return;
            var _size = _RectTransform.sizeDelta;
            _size.y = data._Size;
            _RectTransform.sizeDelta = _size;

            var _pos = _RectTransform.anchoredPosition;
            _pos.y = -data._Position;
            _RectTransform.anchoredPosition = _pos;
        }
    }
}