using System;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

namespace RPG.DialogueSystem.Editor
{
    public class DialogueEditor : EditorWindow
    {
        private const string NoDialogueSelected = "To start creating a Dialogue, create an asset";
        private const string FileNameLabel = "Dialogue Asset: ";

        private const float BackgroundImageSize = 50f;

        private readonly Vector2 _viewportSize = new Vector2(6000, 6000);

        [SerializeField]
        private Dialogue selectedDialogue;

        [NonSerialized]
        private readonly Vector2 _offsetForNewNode = new Vector2(350, 50);

        [NonSerialized]
        private Vector2 _scrollPosition;

        [NonSerialized]
        private Vector2 _scrollingCanvasOffset;

        [NonSerialized]
        private BezierLinesStyle _linesStyle;

        [NonSerialized]
        private GUIStyle _nodeStyle;

        [NonSerialized]
        private DialogueNode _draggingNode;

        [NonSerialized]
        private Vector2 _draggingNodeOffset;

        [NonSerialized]
        private DialogueNode _parentForCreatingNode;

        [NonSerialized]
        private DialogueNode _deletingNode;

        [NonSerialized]
        private DialogueNode _linkingParentNode;

        [NonSerialized]
        private Texture2D _background;


        [MenuItem("Window/Dialogue Editor")]
        private static void ShowWindow()
        {
            var window = GetWindow<DialogueEditor>();
            window.titleContent = new GUIContent("Dialogue Editor");
            window.Show();
        }

        [OnOpenAsset]
        public static bool OnOpenDialogueAsset(int instanceID, int line)
        {
            var dialogueAsset = EditorUtility.InstanceIDToObject(instanceID) as Dialogue;
            if (dialogueAsset == null) return false;

            ShowWindow();
            return true;
        }

        private void OnGUI()
        {
            if (selectedDialogue == null)
            {
                var style = EditorStyles.centeredGreyMiniLabel;
                style.alignment = TextAnchor.MiddleCenter;
                style.fontSize = 12;
                style.margin = new RectOffset(10, 10, 30, 10);
                
                EditorGUILayout.LabelField(NoDialogueSelected, style);
                return;
            }

            ProcessEvents();

            EditorGUILayout.LabelField(FileNameLabel + selectedDialogue.name, EditorStyles.whiteLargeLabel);

            _scrollPosition = GUILayout.BeginScrollView(_scrollPosition);

            var canvas = GUILayoutUtility.GetRect(_viewportSize.x, _viewportSize.y);
            var texCoords = new Rect(0f, 0f, _viewportSize.x / BackgroundImageSize,
                _viewportSize.y / BackgroundImageSize);

            GUI.DrawTextureWithTexCoords(canvas, _background, texCoords);

            foreach (var dialogueNode in selectedDialogue.Nodes)
            {
                DrawConnections(dialogueNode);
            }

            foreach (var dialogueNode in selectedDialogue.Nodes)
            {
                OnGUINode(dialogueNode);
            }

            EditorGUILayout.EndScrollView();

            if (_parentForCreatingNode != null)
            {
                var newNode = selectedDialogue.CreateNode(_parentForCreatingNode);

                newNode.SetPosition(_parentForCreatingNode.Rect.position + _offsetForNewNode);
                _parentForCreatingNode = null;
            }

            if (_deletingNode != null)
            {
                selectedDialogue.DeleteNode(_deletingNode);
                _deletingNode = null;
            }
        }

        private void DrawConnections(DialogueNode node)
        {
            Vector3 startPos = _linesStyle.GetStartPos(node.Rect);

            foreach (var childNode in selectedDialogue.GetAllChildNodes(node))
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
                _scrollingCanvasOffset = Event.current.mousePosition + _scrollPosition;
                _draggingNode = GetNodeUnderCursor(_scrollingCanvasOffset);
                Selection.activeObject = _draggingNode;

                if (_draggingNode != null)
                    _draggingNodeOffset = Event.current.mousePosition - _draggingNode.Rect.position;
            }
            else if (Event.current.type == EventType.MouseUp && _draggingNode != null)
            {
                _draggingNode = null;
            }
            else if (Event.current.type == EventType.MouseDrag)
            {
                if (_draggingNode != null)
                {
                    _draggingNode.SetPosition(Event.current.mousePosition - _draggingNodeOffset);

                    ScrollCanvasOnDraggingNode();
                }
                else
                {
                    _scrollPosition = _scrollingCanvasOffset - Event.current.mousePosition;
                }

                Repaint();
            }
        }

        private void ScrollCanvasOnDraggingNode()
        {
            if (_draggingNode == null || OutOfCanvas(_draggingNode)) return;

            float offset = 50f * Time.smoothDeltaTime;

            if (_draggingNode.Rect.xMax >= position.width + _scrollPosition.x)
            {
                _scrollPosition.x += offset;
                _draggingNodeOffset.x -= offset;
            }

            if (_draggingNode.Rect.yMax >= position.height + _scrollPosition.y)
            {
                _scrollPosition.y += offset;
                _draggingNodeOffset.y -= offset;
            }

            if (_draggingNode.Rect.xMin <= _scrollPosition.x)
            {
                _scrollPosition.x -= offset;
                _draggingNodeOffset.x += offset;
            }

            if (_draggingNode.Rect.yMin <= _scrollPosition.y)
            {
                _scrollPosition.y -= offset;
                _draggingNodeOffset.y += offset;
            }
        }

        private bool OutOfCanvas(DialogueNode node)
        {
            if (node.Rect.xMax >= _viewportSize.x || node.Rect.yMax >= _viewportSize.y) return true;
            if (node.Rect.xMin <= 0f || node.Rect.yMin <= 0f) return true;
            return false;
        }

        private DialogueNode GetNodeUnderCursor(Vector2 cursorPosition)
        {
            DialogueNode topFoundNode = null;
            foreach (var dialogueNode in selectedDialogue.Nodes)
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

            EditorGUILayout.LabelField("Text:", EditorStyles.whiteLabel);
            var newText = EditorGUILayout.TextArea(dialogueNode.Text, EditorStyles.textArea);

            if (EditorGUI.EndChangeCheck())
            {
                dialogueNode.Text = newText;
            }

            GUILayout.Space(15);

            GUILayout.BeginHorizontal();

            if (GUILayout.Button("Delete", EditorStyles.miniButtonLeft))
                _deletingNode = dialogueNode;

            OnGUILinkingButtons(dialogueNode);

            if (GUILayout.Button("Add child", EditorStyles.miniButtonRight))
                _parentForCreatingNode = dialogueNode;

            GUILayout.EndHorizontal();
            GUILayout.EndArea();
        }

        private void OnGUILinkingButtons(DialogueNode dialogueNode)
        {
            if (_linkingParentNode == null)
            {
                if (GUILayout.Button("Link", EditorStyles.miniButtonMid))
                    _linkingParentNode = dialogueNode;

                return;
            }

            // _linkingParentNode != null
            if (ReferenceEquals(dialogueNode, _linkingParentNode))
            {
                if (GUILayout.Button("Cancel", EditorStyles.miniButtonMid))
                    _linkingParentNode = null;

                return;
            }

            // _linkingParentNode != null && dialogueNode != _linkingParentNode
            if (_linkingParentNode.IsParentFor(dialogueNode.ID))
            {
                if (GUILayout.Button("Unlink", EditorStyles.miniButtonMid))
                {
                    _linkingParentNode.RemoveChild(dialogueNode.ID);
                    _linkingParentNode = null;
                }

                return;
            }

            // _linkingParentNode != null && dialogueNode is a child of _linkingParentNode
            if (GUILayout.Button("Link", EditorStyles.miniButtonMid))
            {
                _linkingParentNode.AddChild(dialogueNode.ID);
                _linkingParentNode = null;
            }
        }

        private void OnSelectionChange()
        {
            var dialog = Selection.activeObject as Dialogue;

            // allow switching only between Dialog ScriptableObjects
            if (dialog != null)
            {
                selectedDialogue = dialog;
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

            _background = Resources.Load<Texture2D>("background");
        }
    }
}