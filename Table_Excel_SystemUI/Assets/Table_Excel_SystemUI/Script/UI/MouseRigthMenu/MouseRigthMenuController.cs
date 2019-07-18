using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
namespace Xp_MouseRigthMenu_V1
{
    /// <summary>
    /// 该类描述：暂无描述
    /// 负责人:夏鹏
    /// 联系方式(QQ):648380421
    /// 时间：
    /// </summary>
    public partial class MouseRigthMenuController : MonoBehaviour,IPointerClickHandler
    {
        public static MouseRigthMenuController Controller;

        [Header("MouseRigthMenuView预制体")]
        public MouseRigthMenuView MouseRigthMenuViewPrefab;

        private MouseRigthMenuView createView;
        /// <summary>
        /// 创建容器
        /// </summary>
        private MouseRigthMenuView CreateView
        {
            get
            {
                if (!createView)
                {
                  var obj =  Instantiate(MouseRigthMenuViewPrefab,this.transform);
                    createView = obj.GetComponent<MouseRigthMenuView>();
                   var mousePoint = Input.mousePosition;
                    createView.GetComponent<RectTransform>().anchoredPosition=new Vector2(mousePoint.x,-(Screen.height- mousePoint.y));
                }
                return createView;
            }
            set {
                createView = value;
            }
        }
  
        public void OnPointerClick(PointerEventData eventData)
        {
            if (CreateView)
            {
                Destroy(CreateView.gameObject);
            }
            this.gameObject.SetActive(false);
        }
        private void Start()
        {
            Controller = this;
            Controller.gameObject.SetActive(false);
        }
        /// <summary>
        /// 显示右键菜单
        /// </summary>
        /// <param name="datas"></param>
        public static void Show(List<MenuItemData>  datas) {
            Controller.gameObject.SetActive(true);
            if (Controller.CreateView!=null)
            {
                Destroy(Controller.CreateView.gameObject);
                Controller.CreateView = null;
            }
            foreach (var item in datas)
            {
                Controller.CreateView.CreateMenuItem(item);
            }
            if (!Controller.transform.parent) return; 
            Controller.transform.SetSiblingIndex(Controller.transform.parent.childCount);
        }
    }
}