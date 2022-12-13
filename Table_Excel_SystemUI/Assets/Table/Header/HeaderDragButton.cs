using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
namespace XP.TableModel {
    /// <summary>
    /// 表头拖拽按钮
    /// </summary>
    public class HeaderDragButton : Button, IBeginDragHandler, IEndDragHandler, IDragHandler
    {
        /// <summary>
        /// 开始拖拽和结束拖拽事件
        /// </summary>
        public event EventHandler<PointerEventData> _OnBeginDragEvent,_OnEndDragEvent;
        /// <summary>
        /// 拖拽委托
        /// </summary>
        /// <param name="originPos">原坐标</param>
        /// <param name="newPos">新坐标</param>
        public delegate void _DragDelegate(HeaderDragButton headerDragButton, Vector2 buttonSize);
        /// <summary>
        /// 拖拽事件
        /// </summary>
        public event _DragDelegate _OnDragEvent;
 
        /// <summary>
        /// 控制大小的变换组件
        /// </summary>
        public RectTransform _ControllerSizeRect;
        /// <summary>
        /// 拖拽方向
        /// </summary>
        public enum DragDirectionEnum {
            x,
            y
        }
        [SerializeField]
        /// <summary>
        /// 拖拽方向
        /// </summary>
        public DragDirectionEnum _DragDirection;

        public virtual void OnBeginDrag(PointerEventData eventData)
        {
            _OnBeginDragEvent?.Invoke(this, eventData);
        }

        public virtual void OnDrag(PointerEventData eventData)
        {
         
           var size= _ControllerSizeRect.sizeDelta;
            switch (_DragDirection)
            {
                case DragDirectionEnum.x:
                    size.x += eventData.delta.x;
                    break;
                case DragDirectionEnum.y:
                    size.y -= eventData.delta.y;
                    break;
                default:
                    break;
            }
            _ControllerSizeRect.sizeDelta = size; 
            _OnDragEvent?.Invoke(this, _ControllerSizeRect.sizeDelta);
        }

        public virtual void OnEndDrag(PointerEventData eventData)
        {
            OnDrag(eventData);
            _OnEndDragEvent?.Invoke(this, eventData);
        }
    }
    }
