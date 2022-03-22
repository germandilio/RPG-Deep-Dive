using UnityEditor;
using UnityEngine;

namespace Utils
{
    public class ReplaceWithPrefab : EditorWindow
    {
        [SerializeField]
        private GameObject prefabToReplace;

        [MenuItem("Tools/Replace With Prefab")]
        static void CreateReplaceWithPrefab()
        {
            GetWindow<ReplaceWithPrefab>();
        }

        private void OnGUI()
        {
            prefabToReplace = (GameObject)EditorGUILayout.ObjectField("Prefab", prefabToReplace, typeof(GameObject), false);

            if (!GUILayout.Button("Replace")) return;
                Replace();

            GUI.enabled = false;
            EditorGUILayout.LabelField("Selection count: " + Selection.objects.Length);
        }

        private void Replace()
        {
            var selection = Selection.gameObjects;

            for (var i = selection.Length - 1; i >= 0; --i)
            {
                var selected = selection[i];
                var prefabType = PrefabUtility.GetPrefabAssetType(prefabToReplace);
                GameObject newObject;

                if (prefabType != PrefabAssetType.NotAPrefab)
                {
                    newObject = (GameObject) PrefabUtility.InstantiatePrefab(prefabToReplace);
                }
                else
                {
                    newObject = Instantiate(prefabToReplace);
                    newObject.name = prefabToReplace.name;
                }

                if (newObject == null)
                {
                    Debug.LogError("Error instantiating prefab");
                    break;
                }

                Undo.RegisterCreatedObjectUndo(newObject, "Replace With Prefabs");
                newObject.transform.parent = selected.transform.parent;
                newObject.transform.localPosition = selected.transform.localPosition;
                newObject.transform.localRotation = selected.transform.localRotation;
                newObject.transform.localScale = selected.transform.localScale;
                newObject.transform.SetSiblingIndex(selected.transform.GetSiblingIndex());
                Undo.DestroyObjectImmediate(selected);
            }
        }
    }
}