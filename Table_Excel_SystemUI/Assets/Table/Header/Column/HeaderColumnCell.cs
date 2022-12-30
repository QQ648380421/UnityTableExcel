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
    /// ��ͷ��Ԫ��
    /// </summary>
    public class HeaderColumnCell : HeaderCellBase
    {
        private ColumnAttributeData columnAttributeData;
        /// <summary>
        /// ��������������
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
            } }

        public override IEnumerable<CellData> GetCells()
        {
            return _Table._CellDatas._GetColumCellDatas(_CellData._Index);
        }

        public override void UpdateRectSize()
        { 
            if (_CellData == null) return;
            _CellData._Size = _RectTransform.sizeDelta.x;
        }

        public override void OnCellDataChanged(HeaderCellData data)
        {
         var _size=   _RectTransform.sizeDelta ;
            _size.x=  data._Size;
            _RectTransform.sizeDelta = _size;

            var _pos = _RectTransform.anchoredPosition;
            _pos.x = data._Position;
            _RectTransform.anchoredPosition = _pos; 
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