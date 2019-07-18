using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Xp_MouseRigthMenu_V1;
using MenuItemData = Xp_MouseRigthMenu_V1.MouseRigthMenuController.MenuItemData;
using System.Collections.Generic;

namespace Xp_Table_V1
{
    /// <summary>
    /// 该类描述：暂无描述
    /// 负责人:夏鹏
    /// 联系方式(QQ):648380421
    /// 时间：
    /// </summary>
    public partial class TableCell : MonoBehaviour
    {
 
        [Header("点击遮罩")]
        public Button Marsk;

        [Header("选中颜色")]
        public Color SelectColor = Color.yellow;
        [Header("输入框")]
        public InputField InputField;
  
        /// <summary>
        /// 默认颜色
        /// </summary>
        private Color Color;
        private Image image;

        public Image Image
        {
            get
            {
                if (this)
                {
                    if (!image) image = GetComponent<Image>();
                } 
                return image;
            }
        }

        private RectTransform rectTransform;
        /// <summary>
        /// 本对象的Rect
        /// </summary>
        public RectTransform RectTransform
        {
            get
            {
                if (!this) return null;
                if (!rectTransform) rectTransform = GetComponent<RectTransform>();
                return rectTransform;
            } 
        }
        /// <summary>
        /// 重置默认颜色
        /// </summary>
        internal void RestColor()
        { 
            Image.color = Color;
        }

        /// <summary>
        /// 上次点击时间
        /// </summary>
        float lastClickTime;

        private CellData data;
        /// <summary>
        /// 这个单元格绑定的数据
        /// </summary>
        public CellData Data
        {
            get { return data; }
            set
            {
                data = value;
                CellData_RefreshEvent(value);
                 //还要计算坐标，x根据合并单元格的起点，y根据单元格合并的起点,size根据单元格合并来计算 
            }
        }
        /// <summary>
        /// 刷新单元格
        /// </summary>
        private void CellData_RefreshEvent(CellData value)
        {
            CellData_ContentChange(value.Content, value);
            value.ContentChange -= CellData_ContentChange;
            value.ContentChange += CellData_ContentChange;
            value.SizeChange -= CellData_SizeChange;
            value.SizeChange += CellData_SizeChange;
            CellData_MergeChange(value, value.HandlerMergeCells());
            value.MergeDataEvent -= CellData_MergeChange;
            value.MergeDataEvent += CellData_MergeChange;
            value.IsNullChangeEvent -= CellData_IsNullChangeEvent;
            value.IsNullChangeEvent += CellData_IsNullChangeEvent;
            value.RefreshEvent -= CellData_RefreshEvent;
            value.RefreshEvent += CellData_RefreshEvent;
            value.IndexChange -= Value_IndexChange;
            value.IndexChange += Value_IndexChange;
            UpdateCellPosition(value);
        }

    

        /// <summary>
        /// 索引被改变
        /// </summary>
        /// <param name="position"></param>
        private void Value_IndexChange(CellData data, Vector2 position)
        {
            RectTransform.anchoredPosition = new Vector2(position.x, -position.y);
        }

        /// <summary>
        /// 如果空状态发生变化
        /// </summary>
        /// <param name="value"></param>
        private void CellData_IsNullChangeEvent(CellData data, bool value)
        { 
            this.gameObject.SetActive(!value);
        }

        /// <summary>
        /// 如果宽高发生变化
        /// </summary>
        /// <param name="value"></param>
        private void CellData_SizeChange(CellData data, Vector2 value)
        {
            if (RectTransform)
            {
                RectTransform.sizeDelta = value;
            } 
        }

        /// <summary>
        /// 如果合并了单元格
        /// </summary>
        /// <param name="value"></param>
        private void CellData_MergeChange(CellData data, Vector2 value)
        {
            CellData_SizeChange(data,value);
        }
        /// <summary>
        /// 改变单元格的xy
        /// </summary>
        /// <param name="value"></param>
        public void UpdateCellPosition(CellData value)
        {
            if (value.IsNull) return;
            if (this == null) return;
            Vector2 xy = Vector2.zero;
            if (value.RowData!=null)
            {
                for (int i = 0; i < value.ColumnIndex; i++)
                {
                    var row = value.RowData[i];
                    if (row == null) continue;
                    xy.x += row.ColumnWidth;
                }
            }
            if (value.ColumnData!=null)
            {
                for (int i = 0; i < value.RowIndex; i++)
                {
                   var column = value.ColumnData[i];
                    if (column == null) continue;
                    xy.y -= column.RowHeigth;
                }
            }
          
            RectTransform.anchoredPosition = xy; 
        }


        /// <summary>
        /// 单元格内容发生变化
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        private bool CellData_ContentChange(string content, CellData value)
        {
            InputField.text = content;
            return true;
        }

        void Start()
        {
            Color = Image.color;
            //Marsk.onClick.AddListener(OnClick);
            InputField.onValueChanged.AddListener(OnValueChange);
        
        } 
        private void OnClick() {
            
            if (Time.time - lastClickTime < 0.4f)
            {//双击
                Marsk.gameObject.SetActive(false);
                InputField.image.enabled = true;
            }
            else
            {//单击
                if (Input.GetKey(KeyCode.LeftControl))
                {
                    Data.TableController.SelectCells.Add(this);
                }
                else
                {
                    Data.TableController.SelectCells.Select(this);
                }
            }
            lastClickTime = Time.time;
        }
        private void OnValueChange( string str) {
            Data.Content = str;
        }

        public void OnPointerClick(PointerEventData eventData)
        { 
            if (eventData.currentInputModule.input.GetMouseButtonUp(1))
            {//右键
                Data.TableController.ShowRigthMenu();
            }
            else
            {
                OnClick();
            } 
        }

       
    }
}