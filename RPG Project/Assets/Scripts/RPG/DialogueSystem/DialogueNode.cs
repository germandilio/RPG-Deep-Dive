using System;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.DialogueSystem
{
    [Serializable]
    public class DialogueNode
    {
        [SerializeField]
        private string id;
        
        [Header("Configuration:")]
        [TextArea]
        [SerializeField]
        private string text;

        [SerializeField]
        private List<DialogueNode> childNodes;

        [SerializeField]
        private Rect position;
        
        public string Text
        {
            get => text;
            set => text = value;
        }
        
        public string ID
        {
            get => id;
            set => id = value;
        }

        public Rect Position
        {
            get => position;
            set => position = value;
        }

        public DialogueNode()
        {
            text = String.Empty;
            id = String.Empty;

            position = new Rect(0, 0, 200, 200);
        }
    }
}