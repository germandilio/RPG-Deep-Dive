using System.Collections.Generic;
using UnityEngine;

namespace RPG.DialogueSystem
{
    [CreateAssetMenu(fileName = "New Dialogue", menuName = "RPG Project/Dialogue/New Dialogue", order = 0)]
    public class Dialogue : ScriptableObject
    {
        [SerializeField]
        private List<DialogueNode> nodes = new List<DialogueNode>();
        
#if UNITY_EDITOR
        private void Awake()
        {
            if (nodes.Count == 0)
            {
                nodes.Add(new DialogueNode());
            }
        }
#endif

        public IReadOnlyCollection<DialogueNode> Nodes => nodes;
    }
}