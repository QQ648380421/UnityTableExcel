using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
namespace Xp_Table_V1
{
    /// <summary>
    /// 该类描述：暂无描述
    /// 负责人:夏鹏
    /// 联系方式(QQ):648380421
    /// 时间：
    /// </summary>
    public class MouseRigthMenuEditor : Editor
    { 

        [MenuItem("GameObject/UI/Create MouseRigthMenu")]
        public static void CreateMouseRigthMenu() {
          var obj =  AssetDatabase.LoadAssetAtPath<GameObject>(@"Assets/Script\UI\MouseRigthMenu\Prefab\MouseRightMask.prefab");
          var createObj = (GameObject) PrefabUtility.InstantiatePrefab(obj);
            createObj.transform.SetParent(Selection.activeTransform);
            Undo.RegisterCreatedObjectUndo(createObj, "create");
        }
    }
}