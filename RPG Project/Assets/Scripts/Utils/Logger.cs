using System.Text;
using UnityEngine;

namespace Utils
{
    /// <summary>
    /// Simple runtime logger.
    /// </summary>
    public class Logger : MonoBehaviour
    {
        private const string UserHint =
            "***begin loging errors***\nЧтобы открыть/закрыть окно логгера нажмите пробел.\nПеред закрытием программы, в случае если здесь появятся ошибки пожалуйста, скопируйте их и вставьте в форму (Раздел: 'Сообщение логгера')\n-----------------------------------------------------------------\n";

        private readonly StringBuilder _output = new StringBuilder();

        private bool _isOpened = true;

        private Vector2 _scrollPosition;

        void OnEnable()
        {
            Application.logMessageReceived += Log;
        }

        void OnDisable()
        {
            Application.logMessageReceived -= Log;
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
                _isOpened = !_isOpened;
        }

        public void Log(string logString, string stackTrace, LogType type)
        {
            // Append log info
            _output.Append(logString);
            _output.Append('\n');
            _output.Append(stackTrace);
            _output.Append("\n\n");
        }

        void OnGUI()
        {
            if (!_isOpened) return;

            GUILayout.BeginArea(new Rect(10, 10, 540, 370));
            _scrollPosition = GUILayout.BeginScrollView(_scrollPosition);
            GUILayout.TextArea(UserHint + _output);
            GUILayout.EndScrollView();
            GUILayout.EndArea();
        }
    }
}