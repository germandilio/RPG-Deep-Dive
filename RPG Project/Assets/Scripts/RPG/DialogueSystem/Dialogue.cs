using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace RPG.DialogueSystem
{
    [CreateAssetMenu(fileName = "New Dialogue", menuName = "RPG Project/Dialogue/New Dialogue", order = 0)]
    public class Dialogue : ScriptableObject, ISerializationCallbackReceiver
    {
        [SerializeField]
        private List<DialogueNode> nodes = new List<DialogueNode>();

        private readonly Dictionary<string, DialogueNode> _nodeLookup = new Dictionary<string, DialogueNode>();
 
        public IReadOnlyList<DialogueNode> Nodes => nodes;

        private void OnValidate()
        {
            UpdateLookup();
        }

        public IEnumerable<DialogueNode> GetAllChildNodes(DialogueNode parent)
        {
            foreach (string childNodeID in parent.ChildNodes)
            {
                if (_nodeLookup.ContainsKey(childNodeID))
                    yield return _nodeLookup[childNodeID];
            }
        }

        public DialogueNode CreateNode(DialogueNode parentNode)
        {
            var newNode = CreateInstance<DialogueNode>();
            Undo.RegisterCreatedObjectUndo(newNode, "Create new node");
            
            if (parentNode != null)
                parentNode.AddChild(newNode.ID);
            
            Undo.RecordObject(this, "Added dialog node");
            nodes.Add(newNode);
            UpdateLookup();
            
            return newNode;
        }

        public void DeleteNode(DialogueNode node)
        {
            Undo.RecordObject(this, "Deleted Dialogue Node");
            nodes.Remove(node);
            UpdateLookup();

            foreach (var dialogueNode in nodes)
            {
                dialogueNode.RemoveChild(node.ID); 
            }
            
            Undo.DestroyObjectImmediate(node);
        }

        private void UpdateLookup()
        {
            _nodeLookup.Clear();
            foreach (var dialogueNode in nodes)
            {
                if (_nodeLookup.ContainsKey(dialogueNode.ID))
                {
                    Debug.LogError("Node IDs should be unique");
                }

                _nodeLookup[dialogueNode.ID] = dialogueNode;
            }
        }

        public void OnBeforeSerialize()
        {
#if UNITY_EDITOR
            if (nodes.Count == 0)
                CreateNode(null);
            
            if (AssetDatabase.GetAssetPath(this) != String.Empty)
            {
                foreach (var node in nodes)
                {
                    if (AssetDatabase.GetAssetPath(node) == String.Empty)
                    {
                        AssetDatabase.AddObjectToAsset(node, this);
                    }
                }
            }      
#endif
        }

        public void OnAfterDeserialize()
        {
        }
    }
}