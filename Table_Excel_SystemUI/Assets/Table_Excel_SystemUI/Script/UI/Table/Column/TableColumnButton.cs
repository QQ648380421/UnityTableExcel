using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RowData = Xp_Table_V1.TableRowController.RowData;
using CellData=  Xp_Table_V1.TableCell.CellData;
using Xp_ListEvent;
using System;
using UnityEngine.UI;
using ColumnData = Xp_Table_V1.TableColumnController.ColumnData;
using System.Linq;

namespace Xp_Table_V1
{
    /// <summary>
    /// 该类描述：暂无描述
    /// 负责人:夏鹏
    /// 联系方式(QQ):648380421
    /// 时间：
    /// </summary>
    public class TableColumnButton : MonoBehaviour
    {
       


        [Header("显示的文本")]
        public Text ButtonContentText;
        [Header("控制按钮")]
        public Button Button;
        private ColumnData data;
        /// <summary>
        /// 列数据
        /// </summary>
        public  ColumnData Data
        {
            get
            {
                return data;
            }

            set
            {
                data = value;
                ValueChange(value);
            }
        }
        RectTransform rectTransform;
        public RectTransform RectTransform
        {
            get
            {
                if (!rectTransform) rectTransform = GetComponent<RectTransform>();
                return rectTransform;
            }


        }
        private void ValueChange(ColumnData value)
        {
            Button.onClick.RemoveAllListeners();
            Button.onClick.AddListener(() => {
                value.TableController.SelectCells.Clear();
                 
                var _cells = value.TableController.Data.CellDatas.Where(p => p.ColumnIndex == value.ColumnIndex);
                foreach (var item in _cells)
                {//如果选择了这个按钮，那么关联的所有单元格都被选中  
                    value.TableController.SelectCells.Add(item.TableCell);
                }
            });
            value.IndexChange -= ColumnValue_IndexChange; 
            value.IndexChange += ColumnValue_IndexChange;
            value.WidthChange -= Value_WidthChange;
            value.WidthChange += Value_WidthChange;
        }
        /// <summary>
        /// 列宽被改变
        /// </summary>
        /// <param name=""></param>
        private void Value_WidthChange(float obj)
        {
            RectTransform.sizeDelta = new Vector2(obj, RectTransform.sizeDelta.y);
        }

        /// <summary>
        /// 列的索引被改变
        /// </summary>
        /// <param name="obj"></param>
        private void ColumnValue_IndexChange(int obj)
        {
            ButtonContentText.text = (obj+1).ToString();
        }


        /// <summary>
        /// 改变按钮名字
        /// </summary>
        /// <param name="content"></param>
        internal void ChangeButtonText(string content)
        {
            ButtonContentText.text = content;
        }

        /// <summary>
        /// 改变列宽
        /// </summary>
        /// <param name=""></param>
        public void ChangeColumnWidth(Vector2 v2) {
            Data.Width = v2.x;
           
        }

        /// <summary>
        /// 触发结束拖拽事件
        /// </summary>
        public void EndDrag(Vector2 vector2)
        {
            Data.EndDrag(vector2);
        }

    }
}