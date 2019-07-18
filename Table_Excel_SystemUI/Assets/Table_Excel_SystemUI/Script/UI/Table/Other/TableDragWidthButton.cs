using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using System;
namespace Xp_Table_V1
{
    /// <summary>
    /// 该类描述：按钮拖拽控制宽高
    /// 负责人:夏鹏
    /// 联系方式(QQ):648380421
    /// 时间：
    /// </summary>
    public class TableDragWidthButton : MonoBehaviour, IDragHandler, IEndDragHandler,IPointerEnterHandler,IPointerExitHandler
    {
        [Header("设置方向")]
        public TableSliderBind.Axis TargetAxis;

        [Header("修改目标数值")]
        public RectTransform ChangeTarget;
        [Serializable]
        public class DragData : UnityEvent<Vector2> { }

        [Space(20)]
        [Header("鼠标左右箭头")]
        public Texture2D TopCursor;
        [Header("鼠标上下箭头")]
        public Texture2D LeftCursor;

        /// <summary>
        /// 拖拽事件
        /// </summary>
        [Header("拖拽事件")]
        public DragData DragEvent;
        /// <summary>
        /// 结束拖拽事件
        /// </summary>
        [Header("结束拖拽事件")]
        public DragData EndDragEvent;


        private HorizontalLayoutGroup h_Group;
        private VerticalLayoutGroup v_Group;

        public HorizontalLayoutGroup H_Group
        {
            get
            {
                if (!h_Group) h_Group = this.GetComponentInParent<HorizontalLayoutGroup>();
                return h_Group;
            }
        }
        public VerticalLayoutGroup V_Group
        {
            get
            {
                if (!v_Group) v_Group = this.GetComponentInParent<VerticalLayoutGroup>();
                return v_Group;
            }
        }



        public Vector2 Drag(PointerEventData eventData)
        {
            Vector2 V2 = Vector2.zero;
            switch (TargetAxis)
            {
                case TableSliderBind.Axis.Top:
                    V2 = new Vector2(ChangeTarget.sizeDelta.x + eventData.delta.x, ChangeTarget.sizeDelta.y);
                    break;
                case TableSliderBind.Axis.Left:
                    V2 = new Vector2(ChangeTarget.sizeDelta.x, ChangeTarget.sizeDelta.y - eventData.delta.y);
                    break;
                default:
                    break;
            }
            ChangeTarget.sizeDelta = V2;
            switch (TargetAxis)
            {
                case TableSliderBind.Axis.Top:
                    H_Group.CalculateLayoutInputHorizontal();
                    H_Group.SetLayoutHorizontal();
                    break;
                case TableSliderBind.Axis.Left:
                    V_Group.CalculateLayoutInputHorizontal();
                    V_Group.SetLayoutHorizontal();
                    break;
                default:
                    break;
            }
            if (DragEvent != null)
            {
                DragEvent.Invoke(V2);
            }
            return V2;
        }
        public void OnDrag(PointerEventData eventData)
        {
            Drag(eventData);
        }

        public void OnEndDrag(PointerEventData eventData)
        { 
            //OnDrag(eventData);
            if (EndDragEvent!=null)
            {
                EndDragEvent.Invoke(Drag(eventData));
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            switch (TargetAxis)
            {
                case TableSliderBind.Axis.Top:
                    Cursor.SetCursor(TopCursor,Vector2.zero, CursorMode.Auto);
                    break;
                case TableSliderBind.Axis.Left:
                    Cursor.SetCursor(LeftCursor, Vector2.zero, CursorMode.Auto);
                    break;
                default:
                    break;
            }
       
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            Cursor.SetCursor(null,Vector2.zero, CursorMode.Auto);
        }
    }
}