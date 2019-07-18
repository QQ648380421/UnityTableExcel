using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Xp_ListEvent;

namespace Xp_MouseRigthMenu_V1
{
    public partial class MouseRigthMenuController
    {
        [Serializable]
        public class MenuItemData
        {
            [Header("菜单内容")]
            [SerializeField]
            string content;
            [Header("菜单图标")]
            [SerializeField]
            Sprite ico;
            [Header("子菜单")]
            [SerializeField]
            ListEvent<MenuItemData> menuItems = new ListEvent<MenuItemData>();
            MouseRigthMenuButton itemButton;
            MenuItemData parent;
            /// <summary>
            /// 内容发生变化
            /// </summary>
            public event Action<MenuItemData> ContentChange;
            /// <summary>
            /// 图标发生变化
            /// </summary>
            public event Action<MenuItemData> IcoChange;
            /// <summary>
            /// 当按钮点击
            /// </summary>
            public event Action<MenuItemData> OnClick;
            [Serializable]
            public class ClickEventClass: UnityEvent<MenuItemData> {};

            /// <summary>
            /// 当按钮点击，与OnClick相同
            /// </summary>
            public ClickEventClass ClickEvent;

            /// <summary>
            /// 触发OnClick事件
            /// </summary>
            public void Click() {
                if (OnClick!=null)
                {
                    OnClick.Invoke(this);
                }
                if (ClickEvent!=null)
                {
                    ClickEvent.Invoke(this);
                }
            }
            public MenuItemData(string content, Sprite ico=null)
            {
                MenuItems.InsertEvent += MenuItems_InsertEvent;
                this.Content = content;
                this.Ico = ico;
            }

            /// <summary>
            /// 插入的时候
            /// </summary>
            /// <param name="index"></param>
            /// <param name="value"></param>
            /// <returns></returns>
            private bool MenuItems_InsertEvent(int index, MenuItemData value)
            {
                value.Parent = this;
                return true;
            }



            /// <summary>
            /// 内容
            /// </summary>
            public string Content
            {
                get
                {
                    return content;
                }

                set
                {
                    content = value;
                    if (ContentChange!=null)
                    {
                        ContentChange.Invoke(this);
                    }
                }
            }
            /// <summary>
            /// 图标
            /// </summary>
            public Sprite Ico
            {
                get
                {
                    return ico;
                }

                set
                {
                    ico = value;
                    if (IcoChange != null)
                    {
                        IcoChange.Invoke(this);
                    }
                }
            }

            public ListEvent<MenuItemData> MenuItems
            {
                get
                {
                    return menuItems;
                }

                set
                {
                    menuItems = value;
                }
            }

            public MouseRigthMenuButton ItemButton
            {
                get
                {
                    return itemButton;
                }

                set
                {
                    itemButton = value;
                    itemButton.Data = this;

                }
            }
       
            /// <summary>
            /// 父对象
            /// </summary>
            public MenuItemData Parent
            {
                get
                {
                    return parent;
                }

                set
                {
                    parent = value;
                }
            }
     
        }
    }
}