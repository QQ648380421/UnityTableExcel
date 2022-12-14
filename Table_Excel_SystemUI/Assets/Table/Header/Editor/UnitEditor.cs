using UnityEngine;
using UnityEditor;
namespace XP.TableModel
{
    [InitializeOnLoad]
    public static class UnitEditor 
    {
      
        static UnitEditor() {
            EditorApplication.update -= _Update;
            EditorApplication.update += _Update;
            EditorApplication.quitting -= EditorApplication_quitting;
            EditorApplication.quitting += EditorApplication_quitting; 
           
        }
        static void _Update()
        {
            EditorApplication.update -= _Update;
            if (!PlayerPrefs.HasKey(UnitEditorWindow._key))
            {
                PlayerPrefs.SetInt(UnitEditorWindow._key, 1);
                Start();
            } 
        }
        private static void EditorApplication_quitting()
        {
            if (PlayerPrefs.HasKey(UnitEditorWindow._key) &&  PlayerPrefs.GetInt(UnitEditorWindow._key)==2)
            {
                return;
            }
            PlayerPrefs.DeleteKey(UnitEditorWindow._key);
        }

        static void Start() {
      
            UnitEditorWindow.Open();

        } 
    }
}