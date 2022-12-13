using System.ComponentModel;
namespace XP.TableModel
{

    /// <summary>
    /// 表头单元格数据
    /// </summary>
    public class HeaderCellData : INotifyPropertyChanged
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

        private float width;
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

        private float higth;

        /// <summary>
        /// 单元格名称
        /// </summary>
        public string _Name
        {
            get => name; set
            {
                if (name == value) return;
                name = value;
                _InvokePropertyChanged(nameof(_Name));
            }
        }

        private string name;


        bool selectd;
        /// <summary>
        /// 选中单元格
        /// </summary>
        public bool _Selectd
        {
            get
            {
                return selectd;
            }
            set
            {
                if (selectd == value) return;
                selectd = value;
                _InvokePropertyChanged(nameof(_Selectd));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        /// <summary>
        /// 触发事件<see cref="PropertyChanged"/>
        /// </summary>
        /// <param name="name"></param>
        private void _InvokePropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }


    }
}