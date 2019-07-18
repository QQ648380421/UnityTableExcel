using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.Events;
using System;
namespace Xp_Table_V1
{
    /// <summary>
    /// 该类描述：暂无描述
    /// 负责人:夏鹏
    /// 联系方式(QQ):648380421
    /// 时间：
    /// </summary>
    public class TableCellMouseEvent : MonoBehaviour, IPointerClickHandler
    {
        //[SerializeField]
        //public class ClickData
        //{
        //    PointerEventData EventData; 
        //}
        [Serializable]
        public class ClickEventClass : UnityEvent<PointerEventData> { };

        public ClickEventClass ClickEvent;

        public void OnPointerClick(PointerEventData eventData)
        {
            if (ClickEvent!=null)
            {
                ClickEvent.Invoke(eventData);
            }
      
        }
    }
}