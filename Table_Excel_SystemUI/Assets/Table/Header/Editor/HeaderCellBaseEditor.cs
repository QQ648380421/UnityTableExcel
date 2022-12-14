using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.UI;
namespace XP.TableModel
{
    [CustomEditor(typeof(HeaderCellBase),true)]
    [CanEditMultipleObjects]
    public class HeaderCellBaseEditor : ToggleEditor
    {
        SerializedProperty _DragButtonProperty;
        SerializedProperty _OnCellNameChangedProperty;
        protected override void OnEnable()
        {
            base.OnEnable();
            _DragButtonProperty = serializedObject.FindProperty(nameof(HeaderCellBase._DragButton));
            _OnCellNameChangedProperty = serializedObject.FindProperty(nameof(HeaderCellBase._OnCellNameChanged));
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            EditorGUILayout.Space();

            serializedObject.Update();
            EditorGUILayout.PropertyField(_DragButtonProperty);
            EditorGUILayout.PropertyField(_OnCellNameChangedProperty);
            serializedObject.ApplyModifiedProperties();
        }
    }
}