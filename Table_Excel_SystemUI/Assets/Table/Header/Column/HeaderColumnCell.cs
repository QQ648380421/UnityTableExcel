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
    public class HeaderColumnCell : HeaderCellBase
    {
        private ColumnAttributeData columnAttributeData;
        /// <summary>
        /// 关联列特性数据
        /// </summary>
        public virtual ColumnAttributeData _ColumnAttributeData
        {
            get => columnAttributeData; set
            {
                if (columnAttributeData == value) return;
                columnAttributeData = value;
                if (value == null)
                {
                    _CellData._Data = string.Empty;
                    return;
                }
                _CellData._Data = value._ColumnAttribute._Name;
                _SetRectSize_X(value._ColumnAttribute._Width);
            }
        }
        public override HeaderCellData _CellData { get => base._CellData; set {
                base._CellData = value;
                if (value == null) return;
                _SetRectSize_X(value.Width);
            } }

        public override IEnumerable<CellData> GetCells()
        {
            return _Table._CellDatas._GetColumCellDatas(_CellData._Index);
        }
    
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