using UnityEditor;

using UnityEngine;
using System;

namespace Tools.EditorTools.Hierarchy
{
    [InitializeOnLoad]
    public class HierarchyWindowGroupHeader : Editor
    {
        static HierarchyWindowGroupHeader()
        {
            EditorApplication.hierarchyWindowItemOnGUI += HierarchyWindowItemOnGUI;
        }

        private static void HierarchyWindowItemOnGUI(int instanceID, Rect selectionRect)
        {
            var gameObject = EditorUtility.InstanceIDToObject(instanceID)as GameObject;

            if (gameObject?.name.StartsWith("---", StringComparison.Ordinal) == true)
            {
                EditorGUI.DrawRect(selectionRect, Color.gray);
                EditorGUI.DropShadowLabel(selectionRect, gameObject.name.Replace("-", "").ToUpperInvariant());
            }
        }
    }
}