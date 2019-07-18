using System.Collections.ObjectModel;
using System.Linq;
using UnityEngine.Events;
namespace Xp_Table_V1
{
    public partial class TableController
    {
        /// <summary>
        /// 当前选择单元格集合类
        /// </summary>
        public class SelectCellsColliction : Collection<TableCell>
        {
            /// <summary>
            /// 选中事件
            /// </summary>
            public event UnityAction<TableCell> SelectEvent;
            /// <summary>
            /// 清理的时候把颜色都还原
            /// </summary>
            protected override void ClearItems()
            {
                // 重置选中单元格的默认颜色
                foreach (var item in this)
                {
                    if (item)
                    {
                        item.RestColor();
                    } 
                }
                base.ClearItems();
            }
            /// <summary>
            /// 添加完后修改颜色
            /// </summary>
            /// <param name="index"></param>
            /// <param name="item"></param>
            protected override void InsertItem(int index, TableCell item)
            {
                base.InsertItem(index, item);
                item.Image.color = item.SelectColor;
                if (item.Data.ColumnMerge>0 || item.Data.RowMerge>0)
                {
                 var cells =   item.Data.TableController.Data.CellDatas.Where(p=>
                    p.ColumnIndex > item.Data.ColumnIndex && p.ColumnIndex <= item.Data.ColumnIndex + item.Data.ColumnMerge
                    &&
                     p.RowIndex > item.Data.RowIndex && p.RowIndex <= item.Data.RowIndex + item.Data.RowMerge
                    ); 
                    foreach (var cell in cells)
                    {
                        if (item!= cell.TableCell)
                        {
                            Add(cell.TableCell);
                        } 
                    }
                }
                if (SelectEvent != null) SelectEvent.Invoke(item);
            }

            /// <summary>
            /// 设置选中单元格
            /// </summary>
            /// <param name="tableCell"></param>
            internal void Select(TableCell tableCell)
            {
                ClearItems();
                Add(tableCell);
            }

        }
    }
}