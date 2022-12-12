using System.ComponentModel;
namespace XP.TableModel
{
    public partial class HeaderColumnCell
    {
        /// <summary>
        /// 列单元格数据
        /// </summary>
        public class ColumnCellData: INotifyPropertyChanged
        {
            private int column;
            /// <summary>
            /// 列索引
            /// </summary>
            public int _Column { get => column; set {
                    if (column == value) return;
                    column = value;
                    _InvokePropertyChanged(nameof(_Column));
                } }
            /// <summary>
            /// 列宽度
            /// </summary>
            public float Width { get => width; set {
                    if (width == value) return;
                    width = value;
                    _InvokePropertyChanged(nameof(Width)); 
                }
            }

            private float width;
            /// <summary>
            /// 列名称
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
}