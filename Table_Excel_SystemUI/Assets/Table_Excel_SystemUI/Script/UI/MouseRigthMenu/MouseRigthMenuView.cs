using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
namespace Xp_MouseRigthMenu_V1
{


    /// <summary>
    /// 该类描述：暂无描述
    /// 负责人:夏鹏
    /// 联系方式(QQ):648380421
    /// 时间：
    /// </summary>
    public class MouseRigthMenuView : MonoBehaviour 
    {
        [Header("MenuItem预制体")]
        public GameObject MenuItemPrefab;
        [Header("本物体的组件")]
        public ContentSizeFitter SizeFitter;

        /// <summary>
        /// 创建出来的按钮列表
        /// </summary>
        public List<MouseRigthMenuButton> CreateMenuItems=new List<MouseRigthMenuButton>();

        private MouseRigthMenuView currentView;
        /// <summary>
        /// 当前选中视图
        /// </summary>
        public MouseRigthMenuView CurrentView
        {
            get
            {
                return currentView;
            }

            set
            {
                if (currentView && currentView != value)
                {
                    Destroy(currentView.gameObject);
                }
                currentView = value;
            }
        }

        /// <summary>
        /// 创建一个子菜单按钮
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        internal MouseRigthMenuButton CreateMenuItem(MouseRigthMenuController.MenuItemData item)
        {
            var obj = Instantiate(MenuItemPrefab, this.transform);
            var script = obj.GetComponent<MouseRigthMenuButton>();
            script.CreateMenuItemView = this;
            CreateMenuItems.Add(script);
            script.Data = item; 
            SizeFitter.SetLayoutHorizontal();
            SizeFitter.SetLayoutVertical();
            return script;
        }
    }
}
