using System.IO;
using UnityEditor;

namespace RPG.DialogueSystem.Editor
{
    public class DialogueModificationProcessor : UnityEditor.AssetModificationProcessor
    {
        private static AssetMoveResult OnWillMoveAsset(string sourcePath, string destinationPath)
        {
            var dialogue = AssetDatabase.LoadMainAssetAtPath(sourcePath) as Dialogue;
            if (dialogue == null)
                return AssetMoveResult.DidNotMove;

            // moving asset database
            if (Path.GetDirectoryName(sourcePath) != Path.GetDirectoryName(destinationPath))
                return AssetMoveResult.DidNotMove;

            // renaming
            dialogue.name = Path.GetFileNameWithoutExtension(destinationPath);
            return AssetMoveResult.DidNotMove;
        }
    }
}