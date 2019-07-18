using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Xp_Table_V1
{
    /// <summary>
    /// 该类描述：暂无描述
    /// 负责人:夏鹏
    /// 联系方式(QQ):648380421
    /// 时间：
    /// </summary>
    public class TableSliderBind : MonoBehaviour
    {
        [Header("跟随目标容器")]
        public RectTransform TargetTransform;
        private RectTransform rectTransform;
        [Header("跟随方向")]
        public Axis TargetAxis;
        public enum Axis
        {
            Top,
            Left
        }

        public RectTransform RectTransform
        {
            get
            {
                if (!rectTransform) rectTransform = GetComponent<RectTransform>();
                return rectTransform;
            }
        }

        public void Change()
        {
            Vector2 v2 = Vector2.zero;
            switch (TargetAxis)
            {
                case Axis.Top:
                    v2 = new Vector2(TargetTransform.anchoredPosition.x, RectTransform.anchoredPosition.y);
                    break;
                case Axis.Left:
                    v2 = new Vector2(RectTransform.anchoredPosition.x, TargetTransform.anchoredPosition.y);
                    break;
                default:
                    break;
            }
            RectTransform.anchoredPosition = v2;

        }
    }
}