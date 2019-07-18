using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 该类描述：暂无描述
/// 负责人:夏鹏
/// 联系方式(QQ):648380421
/// 时间：
/// </summary>
public class CreateObjData : MonoBehaviour
{
    [Serializable]
    public class ObjData
    {
     
        [Header("预制体")]
        public GameObject Prefab;
        [Header("预制体对应的名字")]
        public string Name;
        [Header("类型分组")]
        public string Type;
    }

    [Header("数据列表")]
    public List<ObjData> Datas=new List<ObjData>();

 
}
