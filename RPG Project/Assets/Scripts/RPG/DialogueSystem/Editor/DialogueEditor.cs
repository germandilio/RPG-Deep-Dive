using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

namespace RPG.DialogueSystem.Editor
{
    public class DialogueEditor : EditorWindow
    {
        private BezierLinesStyle _linesStyle;

        private Dialogue _selectedDialogue;

        private GUIStyle _nodeStyle;

        private DialogueNode _draggingNode;
        private Vector2 _positionOffset;

        [MenuItem("Window/Dialogue Editor")]
        private static void ShowWindow()
        {
            var window = GetWindow<DialogueEditor>();
            window.titleContent = new GUIContent("Dialogue Editor");
            window.Show();
        }

        private void OnGUI()
        {
            if (_selectedDialogue == null)
            {
                EditorGUILayout.LabelField("No Dialogue selected");
                return;
            }

            ProcessEvents();

            EditorGUILayout.LabelField($"Dialogue file: {_selectedDialogue.name}");
            foreach (var dialogueNode in _selectedDialogue.Nodes)
            {
                DrawConnections(dialogueNode);
            }

            foreach (var dialogueNode in _selectedDialogue.Nodes)
            {
                OnGUINode(dialogueNode);
            }
        }

        private void DrawConnections(DialogueNode node)
        {
            Vector3 startPos = _linesStyle.GetStartPos(node.Rect);

            foreach (var childNode in _selectedDialogue.GetAllChildNodes(node))
            {
                Vector3 endPos = _linesStyle.GetEndPos(childNode.Rect);

                Handles.DrawBezier(startPos, endPos,
                    _linesStyle.GetStartTangent(startPos, endPos), _linesStyle.GetEndTangent(startPos, endPos),
                    _linesStyle.Color, null, _linesStyle.Width);
            }
        }

        private void ProcessEvents()
        {
            if (Event.current.type == EventType.MouseDown && _draggingNode == null)
            {
                _draggingNode = GetNodeUnderCursor(Event.current.mousePosition);
                if (_draggingNode != null)
                    _positionOffset = Event.current.mousePosition - _draggingNode.Rect.position;
            }
            else if (Event.current.type == EventType.MouseUp && _draggingNode != null)
            {
                _draggingNode = null;
            }
            else if (Event.current.type == EventType.MouseDrag && _draggingNode != null)
            {
                Undo.RecordObject(_selectedDialogue, "Update nodes position");

                var newRect = new Rect(Event.current.mousePosition - _positionOffset, _draggingNode.Rect.size);
                _draggingNode.Rect = newRect;
                Repaint();
            }
        }

        private DialogueNode GetNodeUnderCursor(Vector2 cursorPosition)
        {
            DialogueNode topFoundNode = null;
            foreach (var dialogueNode in _selectedDialogue.Nodes)
            {
                if (dialogueNode.Rect.Contains(cursorPosition))
                {
                    topFoundNode = dialogueNode;
                }
            }

            return topFoundNode;
        }

        private void OnGUINode(DialogueNode dialogueNode)
        {
            GUILayout.BeginArea(dialogueNode.Rect, _nodeStyle);
            EditorGUI.BeginChangeCheck();

            EditorGUILayout.LabelField("ID:", EditorStyles.whiteLabel);
            var newID = EditorGUILayout.TextField(dialogueNode.ID, EditorStyles.textField);

            EditorGUILayout.LabelField("Text:", EditorStyles.whiteLabel);
            var newText = EditorGUILayout.TextField(dialogueNode.Text, EditorStyles.textField);

            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(_selectedDialogue, "Modify dialogue node");
                dialogueNode.Text = newText;
                dialogueNode.ID = newID;
            }

            GUILayout.EndArea();
        }

        private void OnSelectionChange()
        {
            var dialog = Selection.activeObject as Dialogue;

            // allow switching only between Dialog ScriptableObjects
            if (dialog != null)
            {
                _selectedDialogue = dialog;
                Repaint();
            }
        }

        private void OnEnable()
        {
            _nodeStyle = new GUIStyle
            {
                normal =
                {
                    background = EditorGUIUtility.Load("node0") as Texture2D
                },
                padding = new RectOffset(15, 15, 15, 15),
                border = new RectOffset(10, 10, 10, 10),
            };

            _linesStyle = new BezierLinesStyle();
        }

        [OnOpenAsset]
        public static bool OnOpenDialogueAsset(int instanceID, int line)
        {
            var dialogueAsset = EditorUtility.InstanceIDToObject(instanceID) as Dialogue;
            if (dialogueAsset == null) return false;

            ShowWindow();
            return true;
        }
    }
}