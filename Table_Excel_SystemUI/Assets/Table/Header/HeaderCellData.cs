using System.ComponentModel;
namespace XP.TableModel
{

    /// <summary>
    /// 表头单元格数据
    /// </summary>
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
        /// <summary>
        /// 宽度
        /// </summary>
        public float Width
        {
            get => width; set
            {
                if (width == value) return;
                width = value;
                _InvokePropertyChanged(nameof(Width));
            }
        }

        private float width=200;
        /// <summary>
        /// 高度
        /// </summary>
        public float Higth
        {
            get => higth; set
            {
                if (higth == value) return;
                higth = value;
                _InvokePropertyChanged(nameof(Higth));
            }
        }

        private float higth=50;
          

    }
}