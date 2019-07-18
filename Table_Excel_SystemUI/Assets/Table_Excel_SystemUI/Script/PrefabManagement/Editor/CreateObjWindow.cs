using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using System;
using System.Linq;
using UnityEngine;
/// <summary>
/// 该类描述：暂无描述
/// 负责人:夏鹏
/// 联系方式(QQ):648380421
/// 时间：
/// </summary> 
public class CreateObjWindow : EditorWindow
{
    [MenuItem("工具/创建各种预制体")]
    public static void OpenWindow() {
         GetWindow<CreateObjWindow>().Show();
    }
     
    [Header("存储数据的预制体")]
    public GameObject DataPrefab;

    CreateObjData objData;

    public CreateObjData ObjData
    {
        get
        {
            if (objData==null)
            {
                objData = DataPrefab.GetComponent<CreateObjData>();
            }
            return objData;
        } 
    }
    private void OnEnable()
    {
        titleContent = new GUIContent("创建各种预制体");
    }
    Vector2 ScrollV2;
    CreateObjData.ObjData AddObj=new CreateObjData.ObjData();
    int CurrentPop;
    private void OnGUI()
    { 
        GUILayout.Label("将在选中物体下创建预制体");

       var pops = GetPops();
        GUILayout.BeginHorizontal();
        GUILayout.Label("选择分类：");
        CurrentPop = EditorGUILayout.Popup( CurrentPop, pops.ToArray());
        string currentType_Str = pops[CurrentPop];
        List<CreateObjData.ObjData> _Datas = null;
        if (CurrentPop==0)
        {
            _Datas = ObjData.Datas;
        }
        else
        {
            _Datas = ObjData.Datas.Where(p => p.Type == currentType_Str).ToList();
        }


        GUILayout.EndHorizontal();
        ScrollV2 = GUILayout.BeginScrollView(ScrollV2);
        for (int i = 0; i < _Datas.Count; i++)
        {
            var item = _Datas[i];
            GUILayout.BeginHorizontal();
       
            if (GUILayout.Button(item.Name,GUILayout.MinWidth(100)))
            {
                Create(item.Prefab);
            } 
            if (GUILayout.Button("移除该预制体"))
            {
                ObjData.Datas.Remove(item);
                Save();
            }
            item.Type = EditorGUILayout.TextField("分类：", item.Type );
            EditorGUILayout.ObjectField("预制体：", item.Prefab, typeof(GameObject), true);
            GUILayout.EndHorizontal();
        }
   
        GUILayout.EndScrollView();
        EditorGUILayout.Space(); 
        AddObj.Prefab = (GameObject) EditorGUILayout.ObjectField("添加预制体：", AddObj.Prefab, typeof(GameObject),true);
        AddObj.Name = EditorGUILayout.TextField("预制体名称：", AddObj.Name);
        AddObj.Type = EditorGUILayout.TextField("分类：", AddObj.Type);
        if (GUILayout.Button("添加一个"))
        {
            ObjData.Datas.Add(AddObj);
            Save(); 
            AddObj = new CreateObjData.ObjData();
        }

    }

    private List<string> GetPops()
    {
        List<string> strs = new List<string>();
        strs.Add("请选择：");
        for (int i = 0; i < ObjData.Datas.Count; i++)
        {
            var item = ObjData.Datas[i];
            if (strs.Contains(item.Type)) continue;
            strs.Add(item.Type); 
        }
        return strs;
    }

    private void Save() {
        var assetObj = AssetDatabase.LoadAssetAtPath<GameObject>(AssetDatabase.GetAssetPath(DataPrefab));
        var script = assetObj.GetComponent<CreateObjData>();
        script.Datas = ObjData.Datas;
        EditorUtility.SetDirty(assetObj);
        AssetDatabase.SaveAssets();
    }
    private void Create(GameObject prefab) {
        if (!prefab) return;
        var createObj = (GameObject)PrefabUtility.InstantiatePrefab(prefab);
        createObj.transform.SetParent(Selection.activeTransform);
        Undo.RegisterCreatedObjectUndo(createObj, "create");
        Selection.activeGameObject = createObj;
    }
}
