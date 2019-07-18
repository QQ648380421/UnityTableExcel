using System;
using System.Linq;
using Xp_ListEvent;
using CellData = Xp_Table_V1.TableCell.CellData;

namespace Xp_Table_V1
{
    public partial class TableColumnController
    {
        /// <summary>
        /// 列数据类
        /// </summary> 
        public class ColumnData : DataBase
        {
            public CellData this[int index]
            {
                get
                {
                    return CellDatas.FirstOrDefault(p => p.RowIndex == index);
                }
                set
                {
                    for (int i = 0; i < CellDatas.Count; i++)
                    {
                        if (CellDatas[i].RowIndex == index)
                        {
                            CellDatas[i] = value;
                            return;
                        }
                    }
                }
            }

            int columnIndex;
            ListEvent<CellData> cellDatas = new ListEvent<CellData>();
            TableColumnButton columnButton;
            float width; 
            /// <summary>
            /// 当列宽发生变化
            /// </summary>
            public event Action<float> WidthChange;
            /// <summary>
            /// 当列索引发生变化
            /// </summary>
            public event Action<int> IndexChange;


            /// <summary>
            /// 当结束拖拽
            /// </summary>
            public event Action<ColumnData,float> EndDragEvent;

            /// <summary>
            /// 触发结束拖拽事件
            /// </summary>
            public void EndDrag(UnityEngine.Vector2 v2)
            {
                Width = v2.x;
                if (EndDragEvent!=null)
                {
                    EndDragEvent.Invoke(this,Width);
                }
            }

            public ColumnData(TableController tableController, TableColumnButton columnButton, int columnIndex) : base(tableController)
            {
                ColumnButton = columnButton;
                columnButton.Data = this;
                ColumnIndex = columnIndex;
            }

            private bool CellDatas_InsertEvent(int index, CellData value)
            {
                if (value.RowIndex == 0 && value.ColumnIndex == ColumnIndex)
                {//如果是第一行
                    value.ContentChange -= Value_ContentChange;
                    value.ContentChange += Value_ContentChange;
                    Width = value.ColumnWidth;
                }
                return true;
            }

            /// <summary>
            /// 如果列名称发生变化
            /// </summary>
            /// <param name="content"></param>
            /// <returns></returns>
            private bool Value_ContentChange(string content, CellData value)
            {
                ColumnButton.ChangeButtonText(content);
                return true;
            }

            /// <summary>
            /// 一列中的单元格
            /// </summary>
            public ListEvent<CellData> CellDatas
            {
                get
                {
                    return cellDatas;
                }

                set
                {
                    cellDatas = value;
                }
            }
            /// <summary>
            /// 列索引
            /// </summary>
            public int ColumnIndex
            {
                get
                {
                    return columnIndex;
                }

                set
                {
                    columnIndex = value;
                    if (IndexChange!=null)
                    {
                        IndexChange.Invoke(value);
                    }
                }
            }
            /// <summary>
            /// 关联的行按钮
            /// </summary>
            public TableColumnButton ColumnButton
            {
                get
                {
                    return columnButton;
                }

                set
                {
                    columnButton = value;
                }
            }
            /// <summary>
            /// 列宽
            /// </summary>
            public float Width
            {
                get
                {
                    return width;
                }

                set
                {
                    width = value;
                    if (WidthChange != null)
                    {
                        WidthChange.Invoke(value);
                    }
                    foreach (var item in TableController.Data.ColumnDatas)
                    {//改变了一行，那么后面的所有单元格都得刷新
                        if (item.ColumnIndex >= ColumnIndex)
                        {
                            foreach (var itemCell in item.CellDatas)
                            {
                                if (itemCell!=null && itemCell.TableCell)
                                {
                                    itemCell.TableCell.UpdateCellPosition(itemCell);
                                }
                              
                            }
                        }
                    } 
                }
            }

           
          
        }

    }
}