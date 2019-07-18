using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Xp_MouseRigthMenu_V1;
using CellData = Xp_Table_V1.TableCell.CellData;
using MenuItemData = Xp_MouseRigthMenu_V1.MouseRigthMenuController.MenuItemData;
using UnityEngine.EventSystems;
namespace Xp_Table_V1
{
    /// <summary>
    /// 该类描述：暂无描述
    /// 负责人:夏鹏
    /// 联系方式(QQ):648380421
    /// 时间：
    /// </summary>
    public partial class TableController : MonoBehaviour,IPointerClickHandler
    {
        private TableData data;
        /// <summary>
        /// 这张表的数据
        /// </summary>
        public TableData Data
        {
            get
            {
                if (data==null)
                {
                    data = new TableData(this, CellController.GetComponent<RectTransform>());
                }
             
                return data;
            }

            set
            {
                data = value;
            }
        }

        private TableCellController cellController;
        private TableRowController rowController;
        private TableColumnController columnController;

        /// <summary>
        /// Cell的Content容器
        /// </summary>
        public TableCellController CellController
        {
            get
            {
                if (!cellController) cellController = GetComponentInChildren<TableCellController>();
                return cellController;
            }
        }
        /// <summary>
        /// Row的Content容器
        /// </summary>
        public TableRowController RowController
        {
            get
            {
                if (!rowController) rowController = GetComponentInChildren<TableRowController>();
                return rowController;
            } 
        }
        /// <summary>
        /// Column的Content容器
        /// </summary>
        public TableColumnController ColumnController
        {
            get
            {
                if (!columnController) columnController = GetComponentInChildren<TableColumnController>();
                return columnController;
            } 
        }

        [Header("滚动内容区域容器")]
        public RectTransform ScrollContent;

        [Space(20)]
        [Header("以下数值为Debug调试数值，不要修改！")]
        [Header("当前选择的单元格")]
        public SelectCellsColliction SelectCells = new SelectCellsColliction();
        [Header("是否为测试模式")]
        public bool IsTestMode;
        /// <summary>
        /// 右键菜单列表
        /// </summary>
        public enum RigthMenu
        {
            添加行, 添加列, 删除行, 删除列, 合并单元格 
        }
        [Header("右键菜单按钮列表")]
        public List<MenuItemData> RigthMenuItems = new List<MenuItemData>();

        private void OnValidate()
        {
            AddMenuItems();
           
        }
        /// <summary>
        /// 添加菜单
        /// </summary>
        private void AddMenuItems()
        {
            foreach (var item in Enum.GetNames(typeof(RigthMenu)))
            {
                var menu = RigthMenuItems.FirstOrDefault(p => p.Content == item);
                if (menu == null)
                {
                    menu=    new MenuItemData(item); 
                    RigthMenuItems.Add(menu);
                }
                menu.OnClick -= MenuItemClick;
                menu.OnClick += MenuItemClick;
            }
        }

        private void Start()
        {
            AddMenuItems(); 
            //Data = new TableData(this, CellController.GetComponent<RectTransform>());
            if (IsTestMode)
            {
                for (int i = 0; i < 30; i++)
                {
                    for (int ii = 0; ii < 10; ii++)
                    {
                        Data.AddCell(ii, i, "行" + i + "列" + ii);
                    }
                }
            } 
        }
         
        /// <summary>
        /// 显示右键菜单
        /// </summary>
        public void ShowRigthMenu() {
             MouseRigthMenuController.Show(RigthMenuItems);
        }

        /// <summary>
        /// 右键菜单点击事件
        /// </summary>
        /// <param name="itemData"></param>
        public void MenuItemClick(MenuItemData itemData)
        {
          var menu =  (RigthMenu)Enum.Parse( typeof(RigthMenu) , itemData.Content);

            switch (menu)
            {
                case RigthMenu.添加行:
                    AddNewRow();
                    break;
                case RigthMenu.添加列:
                    AddNewColumn();
                    break;
                case RigthMenu.删除行:
                    foreach (var item in SelectCells)
                    {
                        RemoveRow(item.Data);
                    } 
                    break;
                case RigthMenu.删除列:
                    foreach (var item in SelectCells)
                    {
                        RemoveColumn(item.Data);
                    }
                    break;
                case RigthMenu.合并单元格:
                    MergeSelectCell();
                    break;
                default:
                    break;
            }
        }
        /// <summary>
        /// 合并单元格
        /// </summary>
        public void MergeSelectCell()
        {
            CellData leftTop, rigthDown;

             Get_Select_LeftTop_And_RigthDown_Cell(out leftTop, out rigthDown);
            if (leftTop == null || rigthDown == null) return;
            leftTop.IsNull = false;
            leftTop.RowMerge = rigthDown.RowIndex - leftTop.RowIndex; 
            leftTop.ColumnMerge = rigthDown.ColumnIndex - leftTop.ColumnIndex;
      
        }
        /// <summary>
        /// 获取选中单元格中左上角的单元格
        /// </summary>
        private  void Get_Select_LeftTop_And_RigthDown_Cell(out CellData leftTop, out CellData rigthDown)
        {
              leftTop = null;
            rigthDown = null;
            foreach (var item in SelectCells)
            {
                if (leftTop == null)
                {
                    leftTop = item.Data;
                }
                else
                {
                    if (item.Data.ColumnIndex < leftTop.ColumnIndex || item.Data.RowIndex < leftTop.RowIndex)
                    {
                        leftTop = item.Data;
                    }
                }
                if (rigthDown == null)
                {
                    rigthDown = item.Data;
                }
                else
                {
                    if (item.Data.ColumnIndex > rigthDown.ColumnIndex || item.Data.RowIndex > rigthDown.RowIndex)
                    {
                        rigthDown = item.Data;
                    }
                }

            } 
        }
 

        /// <summary>
        /// 删除列
        /// </summary>
        /// <param name="cellData"></param>
        public void RemoveColumn(CellData cellData)
        {
            int columnIndex = cellData.ColumnIndex;
            var columns = Data.CellDatas.Where(p=>p.ColumnIndex== cellData.ColumnIndex).ToArray();
            foreach (var item in columns)
            {
                Data.CellDatas.Remove(item);
            }
            var cells = Data.CellDatas.Where(p => p.ColumnIndex > columnIndex);
            foreach (var item in cells)
            {
                item.ColumnIndex--;
            }
        }
        /// <summary>
        /// 删除行
        /// </summary>
        /// <param name="cellData"></param>
        public void RemoveRow(CellData cellData)
        {
           int rowIndex=   cellData.RowIndex;
            var rows = Data.CellDatas.Where(p => p.RowIndex == cellData.RowIndex).ToArray();
            foreach (var item in rows)
            {
                Data.CellDatas.Remove(item);
            }
           var cells= Data.CellDatas.Where(p=>p.RowIndex> rowIndex);
            foreach (var item in cells)
            {
                item.RowIndex --;
            }
        }
        /// <summary>
        /// 添加新列
        /// </summary>
        public void AddNewColumn()
        {
            int rowCount= Data.RowDatas.Count;
            int columnCount = Data.ColumnDatas.Count;
            if (rowCount < 1)
            {
                Data.AddCell(columnCount, rowCount, "");
            }
            else
            {
                for (int i = 0; i < Data.RowDatas.Count; i++)
                {
                    var item = Data.RowDatas[i];
                    Data.AddCell(columnCount, item.RowIndex, "");
                }
            }
        }

        /// <summary>
        /// 添加新行
        /// </summary>
        public void AddNewRow()
        {
            int rowCount = Data.RowDatas.Count;
            int columnCount = Data.ColumnDatas.Count;
            if (columnCount < 1)
            {
                Data.AddCell(columnCount, rowCount, "");
            }
            else
            {
                for (int i = 0; i < Data.ColumnDatas.Count; i++)
                {
                    var item = Data.ColumnDatas[i];
                    Data.AddCell(item.ColumnIndex, rowCount, "");
                }
            }
           
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.currentInputModule.input.GetMouseButtonUp(1))
            {//右键
                ShowRigthMenu();
            }
       
        }

        /// <summary>
        /// 滚动容器滑动
        /// </summary>
        /// <param name="v2"></param>
        public void ScrollDragChange(Vector2 v2) {
            Data.ScrollDragChange(ScrollContent, v2);
        }
    }
}