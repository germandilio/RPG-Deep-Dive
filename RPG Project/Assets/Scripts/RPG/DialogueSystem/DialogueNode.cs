using System;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.DialogueSystem
{
    [Serializable]
    public class DialogueNode
    {
        [Header("Internal properties:")]
        [SerializeField]
        private string id;

        [SerializeField]
        private Rect rect;

        [Header("Dialog Properties:")]
        [TextArea]
        [SerializeField]
        private string text;

        [SerializeField]
        private List<string> childNodes;

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

        public Rect Rect
        {
            get => rect;
            set => rect = value;
        }

        public IEnumerable<string> ChildNodes => childNodes;

        public DialogueNode()
        {
            text = String.Empty;
            id = String.Empty;

            rect = new Rect(0, 0, 200, 120);
        }
    }
}