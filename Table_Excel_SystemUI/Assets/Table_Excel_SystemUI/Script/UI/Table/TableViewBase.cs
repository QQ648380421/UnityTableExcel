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
    public class TableViewBase : MonoBehaviour
    {
        private TableController tableController;
        /// <summary>
        /// 表格核心控制器
        /// </summary>
        public TableController TableController
        {
            get
            {
                if (!tableController) tableController = GetComponentInParent<TableController>();
                return tableController;
            } 
        }
        [Header("创建的预制体")]
        public GameObject CreatePrefab;
        [Header("创建预制体的容器")]
        public RectTransform CreatePrefabView;

        /// <summary>
        /// 创建一个预制体
        /// </summary>
        /// <returns></returns>
        public  T Create<T>() where T:Component
        {
          var newObj =   Instantiate(CreatePrefab, CreatePrefabView); 
          return  newObj.GetComponent<T>(); 
        }
    }
}