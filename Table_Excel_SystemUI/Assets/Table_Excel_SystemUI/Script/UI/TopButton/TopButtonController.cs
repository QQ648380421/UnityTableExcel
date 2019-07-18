using System.Collections.Generic;
using System.Windows.Forms;
using UnityEngine;
namespace Xp_TopButton_V1
{

    /// <summary>
    /// 该类描述：暂无描述
    /// 负责人:夏鹏
    /// 联系方式(QQ):648380421
    /// 时间：
    /// </summary>
    public class TopButtonController : MonoBehaviour
    {
        public List<TopButton> Buttons = new List<TopButton>();

        /// <summary>
        /// 显示按钮
        /// </summary>
        /// <param name="button"></param>
        public void ShowButton(TopButton button)
        {

            foreach (var item in Buttons)
            {
                if (!item.ShowObj)
                {
                    Debug.Log(item.Text + "按钮没有绑定显示对象！", item);
                    
                    continue;
                }
                item.ShowObj.SetActive(item == button);
            }
        } 
    }

}