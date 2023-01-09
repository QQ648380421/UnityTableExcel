using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
namespace XP.TableModel
{
    /// <summary>
    /// 表头按钮
    /// </summary>
    public class HeaderButton : Toggle
    {
        HeaderDragButton[] headerDragButtons;

        public HeaderDragButton[] HeaderDragButtons { get {
                if (headerDragButtons==null)
                {
                    headerDragButtons = GetComponentsInChildren<HeaderDragButton>();
                }
                return headerDragButtons;
            } }
        private Table table;
        public Table _Table { get {
                if (!table)
                {
                    table = GetComponentInParent<Table>();
                }
                return table;
            } }

        RectTransform rectTransform;
        public RectTransform _RectTransform
        {
            get
            {
                if (!rectTransform)
                {
                    rectTransform = transform as RectTransform ;
                }
                return rectTransform;
            } 
        }

        /// <summary>
        /// 列遮罩
        /// </summary>
        public Mask  _HeaderColumnMask;
        /// <summary>
        /// 行遮罩
        /// </summary>
        public Mask _HeaderRowMask;
        /// <summary>
        /// 列滚动条
        /// </summary>
        public Scrollbar _HeaderColumnScrollbar;
        /// <summary>
        /// 行滚动条
        /// </summary>
        public Scrollbar _HeaderRowScrollbar;

        protected override void Start()
        {
            base.Start();
            if (!Application.isPlaying) return;
            foreach (var item in HeaderDragButtons)
            {
                item._OnBeginDragEvent -= Item__OnBeginDragEvent;
                item._OnBeginDragEvent += Item__OnBeginDragEvent;
                item._OnEndDragEvent -= Item__OnEndDragEvent;
                item._OnEndDragEvent += Item__OnEndDragEvent;
                item._OnDragEvent -= Item__OnDragEvent;
                item._OnDragEvent += Item__OnDragEvent;
            }
            this.onValueChanged.AddListener(_IsOnValueChanged);
        }
        /// <summary>
        /// 选择框发生变化
        /// </summary>
        /// <param name="value"></param>
        private void _IsOnValueChanged(bool value) {
            foreach (var item in _Table._CellDatas)
            {
                if (item!=null)
                {
                    item._Selected = value; 
                }
            } 
        }
       
        private void Item__OnBeginDragEvent(object sender, PointerEventData e)
        {

        }

        private void Item__OnEndDragEvent(object sender, PointerEventData e)
        {
            Item__OnDragEvent(sender,e);
        }

     
        protected override void OnDestroy()
        {
            base.OnDestroy();
            foreach (var item in HeaderDragButtons)
            {
                item._OnBeginDragEvent -= Item__OnBeginDragEvent; 
                item._OnEndDragEvent -= Item__OnEndDragEvent; 
                item._OnDragEvent -= Item__OnDragEvent; 
            }
        } 
        /// <summary>
        /// 拖拽按钮拖拽事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Item__OnDragEvent(object sender, PointerEventData e)
        {
            var _dragButton= sender as HeaderDragButton;
            var buttonSize = _RectTransform.sizeDelta;
            switch (_dragButton._DragDirection)
            {
                case HeaderDragButton.DragDirectionEnum.x:
                    //设置遮罩位置
                 var _columnV2=    _HeaderColumnMask.rectTransform.offsetMin;
                    _columnV2.x = buttonSize.x;
                    _HeaderColumnMask.rectTransform.offsetMin = _columnV2; 
                    _HeaderRowMask.rectTransform.sizeDelta 
                        = new Vector2(buttonSize.x, _HeaderRowMask.rectTransform.sizeDelta.y);

                    //设置滚动容器左边位置
                    var scroll_Column_Rect = _HeaderColumnScrollbar.transform as RectTransform;
                    _columnV2.y = scroll_Column_Rect.offsetMin.y;
                    _columnV2.x = buttonSize.x;
                    scroll_Column_Rect.offsetMin = _columnV2;
                    break;
                case HeaderDragButton.DragDirectionEnum.y:
                    //设置遮罩位置
                    var _rowV2 = _HeaderRowMask.rectTransform.offsetMax;
                    _rowV2.y= -buttonSize.y;
                    _HeaderRowMask.rectTransform.offsetMax = _rowV2; 
                    _HeaderColumnMask.rectTransform.sizeDelta
                 = new Vector2(_HeaderColumnMask.rectTransform.sizeDelta.x, buttonSize.y);

                    //设置滚动容器左边位置
                    var scroll_Row_Rect = _HeaderRowScrollbar.transform as RectTransform;
                    _columnV2.x= scroll_Row_Rect.offsetMax.x;
                    _columnV2.y = -buttonSize.y;
                    scroll_Row_Rect.offsetMax = _columnV2;
                    break;
                default:
                    break;
            }
           
            buttonSize.y = -buttonSize.y;
            _Table._ScrollRect.viewport.offsetMin = new Vector2(buttonSize.x, _Table._ScrollRect.viewport.offsetMin.y);
            _Table._ScrollRect.viewport.offsetMax = new Vector2(_Table._ScrollRect.viewport.offsetMax.x ,buttonSize.y );
     
        }
    }
}