using System.Collections;
using System.Collections.Generic;

using UnityEditor;

using UnityEngine;

namespace Tools.EditorTools.Window
{
    public abstract class ExtendedEditorWindow : EditorWindow
    {
#region Public Variables

#endregion

#region Protected Variables
        protected SerializedObject serializedObject;
        protected SerializedProperty currentProperty;

        protected SerializedProperty selectedProperty;

#endregion

#region Private Variables

        private string selectedPropertyPath;

#endregion

#region Properties

#endregion

#region Unity Methods

#endregion

#region Public Methods

#endregion

#region Protected Methods
        protected void DrawProperties(SerializedProperty propertys, bool drawChildren)
        {
            string lastPropertyPath = string.Empty;

            foreach (SerializedProperty property in propertys)
            {
                if (property.isArray && property.propertyType == SerializedPropertyType.Generic)
                {
                    EditorGUILayout.BeginHorizontal();
                    property.isExpanded = EditorGUILayout.Foldout(property.isExpanded, property.displayName);
                    EditorGUILayout.EndHorizontal();

                    if (property.isExpanded)
                    {
                        EditorGUI.indentLevel++;
                        DrawProperties(propertys, drawChildren);
                        EditorGUI.indentLevel--;
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(lastPropertyPath) && property.propertyPath.Contains(lastPropertyPath))
                    {
                        continue;
                    }
                    lastPropertyPath = property.propertyPath;
                    EditorGUILayout.PropertyField(property, drawChildren);
                }
            }
        }

        protected void DrawSideBar(SerializedProperty propertys)
        {
            foreach (SerializedProperty property in propertys)
            {
                if (GUILayout.Button(property.displayName))
                {
                    selectedPropertyPath = property.propertyPath;
                }
            }
            if (!string.IsNullOrEmpty(selectedPropertyPath))
            {
                selectedProperty = serializedObject.FindProperty(selectedPropertyPath);
            }
        }

        protected void DrawField(string propertyName, bool relative)
        {
            if (relative && currentProperty != null)
            {
                EditorGUILayout.PropertyField(currentProperty.FindPropertyRelative(propertyName), true);
            }
            else if (serializedObject != null)
            {
                EditorGUILayout.PropertyField(serializedObject.FindProperty(propertyName), true);
            }
        }

        protected void Apply()
        {
            serializedObject.ApplyModifiedProperties();
        }

#endregion

#region Private Methods

#endregion
    }
}