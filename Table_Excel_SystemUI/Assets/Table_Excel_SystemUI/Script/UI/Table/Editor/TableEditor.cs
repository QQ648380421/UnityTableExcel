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
    public class TableEditor : Editor
    {
        [MenuItem("设置/UI/全部UI取消按键导航")]
        public static void SetAllUiNavagetion()
        {
            var uis = GameObject.FindObjectsOfType<Selectable>();
            Undo.RecordObjects(uis, "change");
            foreach (var item in uis)
            {
                item.navigation = new Navigation() { mode = Navigation.Mode.None };
            }
            EditorUtility.DisplayDialog("温馨提示：", "全部取消完成！", "我知道了！");
        }

        [MenuItem("GameObject/UI/Create Table")]
        public static void CreateTable() {
          var obj =  AssetDatabase.LoadAssetAtPath<GameObject>(@"Assets/Script\UI\Table\Prefab\TableViewPrefab.prefab");
          var createObj = (GameObject) PrefabUtility.InstantiatePrefab(obj);
            createObj.transform.SetParent(Selection.activeTransform);
            Undo.RegisterCreatedObjectUndo(createObj, "create");
        }
    }
}