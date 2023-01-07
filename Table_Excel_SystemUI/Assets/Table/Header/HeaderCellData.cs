using System.ComponentModel;
namespace XP.TableModel
{

    /// <summary>
    /// 表头单元格数据
    /// </summary>
    [System.Serializable]
    public class HeaderCellData : CellDataBase
    {
        private int index;
        /// <summary>
        /// 表头行或列索引
        /// </summary>
        public int _Index
        {
            get => index; set
            {
                if (index == value) return;
                index = value;
                _InvokePropertyChanged(nameof(_Index));
            }
        }


        float size;
        /// <summary>
        /// 单元格大小宽高
        /// </summary>
        public float _Size
        {
            get
            {
                return size;
            }
            set
            {
                if (size == value) return;
                if (value<5)
                {
                    value = 5;
                }
                size = value;
                _InvokePropertyChanged(nameof(_Size));
            }
        }


        float position;
        /// <summary>
        /// 单元格当前位置
        /// </summary>
        public float _Position
        {
            get
            {
                return position;
            }
            set
            {
                if (position == value) return;
                position = value;
                _InvokePropertyChanged(nameof(_Position));
            }
        }


        ColumnAttributeData columnAttributeData;
        /// <summary>
        /// 关联的特性
        /// </summary>
        public ColumnAttributeData _ColumnAttributeData
        {
            get
            {
                return columnAttributeData;
            }
            set
            {
                if (columnAttributeData == value) return;
                columnAttributeData = value;
                this._Size = value._ColumnAttribute._Width;
                if (!string.IsNullOrEmpty(value._ColumnAttribute._Name))
                {
                    this._ShowData = value._ColumnAttribute._Name;
                }
                else
                {
                    this._ShowData = value._PropertyInfo.Name;
                }
          
            }
        }


        HeaderCellBase cellObj;
        /// <summary>
        /// 关联单元格物体
        /// </summary>
        public HeaderCellBase _CellObj
        {
            get
            {
                return cellObj;
            }
            set
            {
                if (cellObj == value) return;
                cellObj = value;
            }
        }

        /// <summary>
        /// 获取当前显示状态
        /// </summary>
        /// <returns></returns>
        public bool _GetShowState()
        {
            if (cellObj==null)
            {
                return false;
            }
            return true;
        }
     
    }
}