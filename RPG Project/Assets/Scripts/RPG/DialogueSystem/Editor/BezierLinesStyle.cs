using UnityEngine;

namespace RPG.DialogueSystem.Editor
{
    public class BezierLinesStyle
    {
        private float _coefficientOfBezierCurvature = 0.6f;

        public float Width { get; } = 3.5f;

        public Color Color { get; } = new Color(245, 251, 239);

        public Vector3 GetStartPos(Rect nodeRect)
        {
            return new Vector3(nodeRect.xMax, nodeRect.yMin + nodeRect.height / 2, 0);
        }

        public Vector3 GetEndPos(Rect nodeRect)
        {
            return new Vector3(nodeRect.xMin, nodeRect.yMin + nodeRect.height / 2, 0);
        }

        public Vector3 GetStartTangent(Vector3 startPos, Vector3 endPos)
        {
            Vector3 bezierOffset = endPos - startPos;
            bezierOffset.y = 0;
            bezierOffset.x *= _coefficientOfBezierCurvature;

            if (startPos.x > endPos.x)
                return startPos - bezierOffset;
            else
                return startPos + bezierOffset;
        }

        public Vector3 GetEndTangent(Vector3 startPos, Vector3 endPos)
        {
            Vector3 bezierOffset = endPos - startPos;
            bezierOffset.y = 0;
            bezierOffset.x *= _coefficientOfBezierCurvature;

            if (startPos.x > endPos.x)
                return endPos + bezierOffset;
            else
                return endPos - bezierOffset;
        }
    }
}