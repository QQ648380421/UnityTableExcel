using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.UI;
namespace XP.TableModel
{
    [CustomEditor(typeof(HeaderDragButton),true)]
    [CanEditMultipleObjects]
    public class HeaderDragButtonEditor :ButtonEditor
    {
        SerializedProperty _DragDirectionProperty;
        SerializedProperty _ControllerSizeRectProperty;

        protected override void OnEnable()
        {
            base.OnEnable();
            _DragDirectionProperty = serializedObject.FindProperty(nameof(HeaderDragButton._DragDirection));
            _ControllerSizeRectProperty = serializedObject.FindProperty(nameof(HeaderDragButton._ControllerSizeRect));
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            EditorGUILayout.Space();

            serializedObject.Update();
            EditorGUILayout.PropertyField(_DragDirectionProperty);
            EditorGUILayout.PropertyField(_ControllerSizeRectProperty);
            serializedObject.ApplyModifiedProperties();
        }
    }
}