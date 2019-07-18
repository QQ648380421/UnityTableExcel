using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ColumnData =  Xp_Table_V1.TableController;
using CellData = Xp_Table_V1.TableCell.CellData;
using Xp_ListEvent;
using System;
using UnityEngine.UI;
using RowData= Xp_Table_V1.TableRowController.RowData;

namespace Xp_Table_V1
{
    /// <summary>
    /// 该类描述：暂无描述
    /// 负责人:夏鹏
    /// 联系方式(QQ):648380421
    /// 时间：
    /// </summary>
    public class TableRowButton : MonoBehaviour
    {
     
        [Header("显示的文本")]
        public Text ButtonContentText;
        [Header("控制按钮")]
        public Button Button;

        RectTransform rectTransform;
        public RectTransform RectTransform
        {
            get
            {
                if (!rectTransform) rectTransform = GetComponent<RectTransform>();
                return rectTransform;
            }

           
        }
         RowData data;
        /// <summary>
        /// 行数据
        /// </summary>
        public  RowData Data
        {
            get
            {
                return data;
            }

            set
            {
                data = value;
                ValueChange(value);
                //Data_IndexChange(Data.RowIndex);
                //Data.IndexChange += Data_IndexChange; 
            }
        }

        private void ValueChange(RowData value)
        {
            Data_HeigthChange(value.Heigth);
            value.HeigthChange += Data_HeigthChange;
            value.IndexChange += Value_IndexChange;
            Button.onClick.RemoveAllListeners();
            Button.onClick.AddListener(() => {
                value.TableController.SelectCells.Clear();
                foreach (var item in value.CellDatas)
                {//如果选择了这个按钮，那么关联的所有单元格都被选中 
                    value.TableController.SelectCells.Add(item.TableCell);
                   
                }
            }); 
        }

        /// <summary>
        /// 行的索引被改变
        /// </summary>
        /// <param name="obj"></param>
        private void Value_IndexChange(int obj)
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
        /// 行高发生变化
        /// </summary>
        /// <param name="obj"></param>
        private void Data_HeigthChange(float obj)
        {
            RectTransform.sizeDelta = new Vector2(RectTransform.sizeDelta.x,obj);
        }

      
        /// <summary>
        /// 改变行高
        /// </summary>
        /// <param name="v2"></param>
        public void ChangeRowHeigth(Vector2 v2) {
            Data.Heigth = v2.y;
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