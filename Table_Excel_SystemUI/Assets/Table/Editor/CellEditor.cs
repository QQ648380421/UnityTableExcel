using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UI;
namespace XP.TableModel
{
    [CustomEditor(typeof(Cell),true)]
    [CanEditMultipleObjects]
    public class CellEditor : ToggleEditor
    {
        SerializedProperty _CellDataChangedEvents_StringProperty;
 
        protected override void OnEnable()
        {
            base.OnEnable();
            _CellDataChangedEvents_StringProperty = serializedObject.FindProperty(nameof(Cell._CellDataChangedEvents_String));
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            EditorGUILayout.Space();

            serializedObject.Update();
            EditorGUILayout.PropertyField(_CellDataChangedEvents_StringProperty); 
            serializedObject.ApplyModifiedProperties();
        }
    }
}