using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.UI;
namespace XP.TableModel
{
    [CustomEditor(typeof(HeaderButton),true)]
    [CanEditMultipleObjects]
    public class HeaderButtonEditor : ToggleEditor
    {
        SerializedProperty _HeaderColumnMask_Property;
        SerializedProperty _HeaderRowMask_Property;

        SerializedProperty _HeaderColumnScrollbar_Property;
        SerializedProperty _HeaderRowScrollbar_Property;
        protected override void OnEnable()
        {
            base.OnEnable();
            _HeaderColumnMask_Property = serializedObject.FindProperty(nameof(HeaderButton._HeaderColumnMask));
            _HeaderRowMask_Property = serializedObject.FindProperty(nameof(HeaderButton._HeaderRowMask));
            _HeaderColumnScrollbar_Property = serializedObject.FindProperty(nameof(HeaderButton._HeaderColumnScrollbar));
            _HeaderRowScrollbar_Property = serializedObject.FindProperty(nameof(HeaderButton._HeaderRowScrollbar));
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            EditorGUILayout.Space();

            serializedObject.Update();
            EditorGUILayout.PropertyField(_HeaderColumnMask_Property);
            EditorGUILayout.PropertyField(_HeaderRowMask_Property);
            EditorGUILayout.PropertyField(_HeaderColumnScrollbar_Property);
            EditorGUILayout.PropertyField(_HeaderRowScrollbar_Property);
            serializedObject.ApplyModifiedProperties();
        }
    }
}