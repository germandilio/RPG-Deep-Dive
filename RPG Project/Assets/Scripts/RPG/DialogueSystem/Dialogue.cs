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

        private Vector2 _offsetForNewNode = new Vector2(350, 50);

        public IReadOnlyList<DialogueNode> Nodes => nodes;

        public DialogueNode RootNode
        {
            get
            {
                if (nodes.Count > 0)
                    return nodes[0];

                return null;
            }
        }

        public IEnumerable<DialogueNode> GetAllChildNodes(DialogueNode parent)
        {
            if (parent == null)
                yield break;

            foreach (string childNodeID in parent.ChildNodes)
            {
                if (_nodeLookup.ContainsKey(childNodeID))
                    yield return _nodeLookup[childNodeID];
            }
        }

        public IEnumerable<DialogueNode> GetPlayerChildren(DialogueNode parent)
        {
            if (parent == null)
                yield break;

            foreach (string childNodeID in parent.ChildNodes)
            {
                if (_nodeLookup.ContainsKey(childNodeID) && _nodeLookup[childNodeID].Speaker == Speaker.Player)
                    yield return _nodeLookup[childNodeID];
            }
        }

        public IEnumerable<DialogueNode> GetAIChildren(DialogueNode parent)
        {
            if (parent == null)
                yield break;

            foreach (string childNodeID in parent.ChildNodes)
            {
                if (_nodeLookup.ContainsKey(childNodeID) && _nodeLookup[childNodeID].Speaker != Speaker.Player)
                    yield return _nodeLookup[childNodeID];
            }
        }

        private void Awake()
        {
            UpdateLookup();
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

        #region Editor code

#if UNITY_EDITOR

        private const string RootNodeText =
            "*System Root Node* Do not place any content, triggers and conditions on it.";

        private void OnValidate()
        {
            UpdateLookup();
        }

        public void AddNode(DialogueNode parentNode)
        {
            var newNode = CreateNode(parentNode);
            Undo.RegisterCreatedObjectUndo(newNode, "Create new node");
            Undo.RecordObject(this, "Added dialog node");
            AddToDialogue(newNode);
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

        private DialogueNode CreateNode(DialogueNode parentNode)
        {
            var newNode = CreateInstance<DialogueNode>();

            newNode.name = $"Node {nodes.Count}";
            if (parentNode != null)
            {
                parentNode.AddChild(newNode.ID);
                newNode.SetPosition(parentNode.Rect.position + _offsetForNewNode);
                _offsetForNewNode += new Vector2(0, 10);
                newNode.SetAlternativeSpeaker(parentNode.Speaker);
            }
            else
            {
                newNode.SetDefaultSpeaker();
            }

            return newNode;
        }

        private void AddToDialogue(DialogueNode newNode)
        {
            nodes.Add(newNode);
            UpdateLookup();
        }
#endif

        void ISerializationCallbackReceiver.OnBeforeSerialize()
        {
#if UNITY_EDITOR
            if (nodes.Count == 0)
            {
                var newNode = CreateNode(null);
                newNode.Text = RootNodeText;
                AddToDialogue(newNode);
            }

            if (AssetDatabase.GetAssetPath(this) == String.Empty) return;

            foreach (var node in nodes)
            {
                if (AssetDatabase.GetAssetPath(node) == String.Empty)
                {
                    AssetDatabase.AddObjectToAsset(node, this);
                }
            }
#endif
        }

        void ISerializationCallbackReceiver.OnAfterDeserialize()
        {
        }

        #endregion
    }
}