using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Xp_ListEvent;
using ColumnData= Xp_Table_V1.TableColumnController.ColumnData;
using CellData= Xp_Table_V1.TableCell.CellData;
using System.Linq;

namespace Xp_Table_V1
{
    /// <summary>
    /// 该类描述：暂无描述
    /// 负责人:夏鹏
    /// 联系方式(QQ):648380421
    /// 时间：
    /// </summary>
    public class TableRowController : TableViewBase
    {
        /// <summary>
        /// 行数据类
        /// </summary> 
        public class RowData : DataBase
        {
            public CellData this[int index] {
                get {
                    return CellDatas.FirstOrDefault(p=>p.ColumnIndex==index);
                }
                set {
                    for (int i = 0; i < CellDatas.Count; i++)
                    {
                        if (CellDatas[i].ColumnIndex==index)
                        {
                            CellDatas[i] = value;
                            return;
                        }
                    }
                }
            }
            int rowIndex;
            ListEvent<CellData> cellDatas = new ListEvent<CellData>();
            float heigth;
            TableRowButton rowButton;
            /// <summary>
            /// 当行高发生变化
            /// </summary>
            public event Action<float> HeigthChange;
            /// <summary>
            /// 当索引值发生变化
            /// </summary>
            public event Action<int> IndexChange;
            /// <summary>
            /// 当结束拖拽
            /// </summary>
            public event Action<RowData, float> EndDragEvent;

            /// <summary>
            /// 触发结束拖拽事件
            /// </summary>
            public void EndDrag(UnityEngine.Vector2 v2)
            {
                Heigth = v2.y;
                if (EndDragEvent != null)
                {
                    EndDragEvent.Invoke(this, heigth);
                }
            }

            public RowData(TableController tableController, TableRowButton tableRowButton,int rowIndex) : base(tableController)
            {
                RowButton = tableRowButton;
                tableRowButton.Data = this;
                RowIndex = rowIndex; 
            }

            private bool CellDatas_InsertEvent(int index, CellData value)
            {
                if (cellDatas.Count < 1 && value.RowIndex == RowIndex)
                {//如果是这一行，且子物体没有
                    Heigth = value.RowHeigth;
                }
                return true;
            }

            /// <summary>
            /// 一行中的单元格
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
            /// 行索引
            /// </summary>
            public int RowIndex
            {
                get
                {
                    return rowIndex;
                }

                set
                {
                    rowIndex = value; 
                    if (IndexChange != null)
                    {
                        IndexChange.Invoke(value);
                    }
                }
            }
            /// <summary>
            /// 行高
            /// </summary>
            public float Heigth
            {
                get
                {
                    return heigth;
                }

                set
                {
                    heigth = value;
                    if (HeigthChange != null)
                    {
                        HeigthChange.Invoke(value);
                    }
                    foreach (var item in TableController.Data.RowDatas)
                    {//改变了一行，那么后面的所有单元格都得刷新
                        if (item.RowIndex >= RowIndex)
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
            /// <summary>
            /// 行控制器
            /// </summary>
            public TableRowButton RowButton
            {
                get
                {
                    return rowButton;
                }

                set
                {
                    rowButton = value;
                }
            }
        }

    }
}