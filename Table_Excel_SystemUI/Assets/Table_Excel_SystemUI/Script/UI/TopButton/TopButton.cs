using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace Xp_TopButton_V1
{

/// <summary>
/// 该类描述：暂无描述
/// 负责人:夏鹏
/// 联系方式(QQ):648380421
/// 时间：
/// </summary>
public class TopButton : MonoBehaviour
{ 
    /// <summary>
    /// 控制显示或隐藏的对象
    /// </summary>
    [Header("控制显示或隐藏的对象")]
    public GameObject ShowObj;
    [Header("按钮图片")]
    [SerializeField]
    private Sprite ico;
    [Header("按钮文本")]
    [SerializeField]
    private string text;

    [Space(20)]
    [Header("下面的参数不要管")]
    public Image ButtonImage;
    public Text ButtonText;
     
    public Sprite Ico
    {
        get
        {
            return ico;
        }

        set
        {
            ico = value;
            UpdateChild();
        }
    } 
    public string Text
    {
        get
        {
            return text;
        }

        set
        {
            text = value;
            UpdateChild();
        }
    }

    /// <summary>
    /// 父容器控制器
    /// </summary>
    private TopButtonController Controller
    {
        get
        {
            if (!controller)
            {
                controller = this.GetComponentInParent<TopButtonController>();
            }
            return controller;
        }

        set
        {
            controller = value;
        }
    }
    private TopButtonController controller;

    private void OnValidate()
    {
        UpdateChild();
    }

    private void Start()
    {
        Controller.Buttons.Add(this);
        this.GetComponent<Button>().onClick.AddListener(()=> {
            Controller.ShowButton(this);
        }); ;
    }
    /// <summary>
    /// 刷新子对象的值
    /// </summary>
    private void UpdateChild() {
        ButtonImage.sprite = Ico;
        ButtonText.text = text;
        this.gameObject.name = text;
    }
 
}
}