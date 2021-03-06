using System.Collections.Generic;

using UnityEditor;

namespace Tools.EditorTools.Attributes
{
    internal static class SerializedPropertyUtility
    {

        public static IEnumerable<SerializedProperty>
            EnumerateChildProperties(this SerializedObject serializedObject)
            {
                var iterator = serializedObject.GetIterator();
                if (iterator.NextVisible(enterChildren: true))
                {
                    // yield return property; // skip "m_Script"
                    while (iterator.NextVisible(enterChildren: false))
                    {
                        yield return iterator;
                    }
                }
            }

        public static IEnumerable<SerializedProperty>
            EnumerateChildProperties(this SerializedProperty parentProperty)
            {
                var iterator = parentProperty.Copy();
                var end = iterator.GetEndProperty();
                if (iterator.NextVisible(enterChildren: true))
                {
                    do
                    {
                        if (SerializedProperty.EqualContents(iterator, end))
                            yield break;

                        yield return iterator;
                    }
                    while (iterator.NextVisible(enterChildren: false));
                }
            }

    }

}