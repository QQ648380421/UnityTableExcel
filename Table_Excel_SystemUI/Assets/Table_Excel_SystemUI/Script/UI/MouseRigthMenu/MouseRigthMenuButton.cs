using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Xp_MouseRigthMenu_V1;
using UnityEngine.EventSystems;
namespace Xp_MouseRigthMenu_V1
{


/// <summary>
/// 该类描述：暂无描述
/// 负责人:夏鹏
/// 联系方式(QQ):648380421
/// 时间：
/// </summary>
public class MouseRigthMenuButton : MonoBehaviour,IPointerEnterHandler,IPointerClickHandler
{

      MouseRigthMenuController.MenuItemData data;
    [Header("Item预制体")]
    public GameObject ItemPrefab;
    [Header("容器预制体")]
    public GameObject ViewPrefab; 
    [Header("子菜单容器")]
    public RectTransform ChildMenuView;
    [Header("箭头")]
    public GameObject Arrow;


    private MouseRigthMenuView createItemView;
    /// <summary>
    /// 创建Item的容器
    /// </summary>
    public MouseRigthMenuView CreateItemView
    {
        get
        {
            if (!createItemView && Data.MenuItems!=null && Data.MenuItems.Count>0)
            {
                var obj = Instantiate(ViewPrefab, ChildMenuView);
                createItemView = obj.GetComponent<MouseRigthMenuView>();
                createItemView.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
            }
            return createItemView;
        }
    }

    public Text Text;
    public Image IcoImage;

    /// <summary>
    /// 数据
    /// </summary>
    public MouseRigthMenuController.MenuItemData Data
    {
        get
        {
            return data;
        }

        set
        {
            data = value;
            Text.text = value.Content;
            IcoImage.sprite = value.Ico;
            IcoImage.enabled = value.Ico;
            if (value.MenuItems==null || value.MenuItems.Count<1)
            {
                Arrow.SetActive(false);
            }
            else
            {
                Arrow.SetActive(true);
            }
        }
    }

    /// <summary>
    /// 创建该脚本的父对象
    /// </summary>
    public MouseRigthMenuView CreateMenuItemView { get; internal set; }

    public void OnPointerEnter(PointerEventData eventData)
    { 
        CreateMenuItemView.CurrentView = CreateItemView;
            if (Data.MenuItems == null || Data.MenuItems.Count < 1) return;
        foreach (var item in Data.MenuItems)
        {
            if (!item.ItemButton)
            { 
                item.ItemButton = CreateItemView.CreateMenuItem(item);
            }
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Data.Click();
        var controller = GetComponentInParent<MouseRigthMenuController>();
        controller.gameObject.SetActive(false); 
    }
}
}