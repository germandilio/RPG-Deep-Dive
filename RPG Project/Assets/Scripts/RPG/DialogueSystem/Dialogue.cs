using System.Collections.Generic;
using UnityEngine;

namespace RPG.DialogueSystem
{
    [CreateAssetMenu(fileName = "New Dialogue", menuName = "RPG Project/Dialogue/New Dialogue", order = 0)]
    public class Dialogue : ScriptableObject
    {
        [SerializeField]
        private List<DialogueNode> nodes = new List<DialogueNode>();

        private readonly Dictionary<string, DialogueNode> _nodeLookup = new Dictionary<string, DialogueNode>();

        public IReadOnlyList<DialogueNode> Nodes => nodes;
        
#if UNITY_EDITOR
        private void Awake()
        {
            if (nodes.Count == 0)
            {
                nodes.Add(new DialogueNode());
            }
        }
#endif

        private void OnValidate()
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

        public IEnumerable<DialogueNode> GetAllChildNodes(DialogueNode parent)
        {
            foreach (string childNodeID in parent.ChildNodes)
            {
                if (_nodeLookup.ContainsKey(childNodeID)) 
                    yield return _nodeLookup[childNodeID]; 
            }
        }
    }
}