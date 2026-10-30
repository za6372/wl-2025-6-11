using Assets.HotFix.MyUtils.src.common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.HotFix.MyUtils.src
{
    public class ScreenConsole : MonoBehaviour
    {
        const int maxLines = 50;
        const int maxLineLength = 60;
        private string _logStr = "";

        private readonly List<string> _lines = new List<string>();

        public int fontSize = 24;

        void OnEnable() { Application.logMessageReceived += Log; }
        void OnDisable() { Application.logMessageReceived -= Log; }


        public void Log(string logString, string stackTrace, LogType type)
        {
            foreach (var line in logString.Split('\n'))
            {
                if (line.Length <= maxLineLength)
                {
                    _lines.Add(line);
                    continue;
                }
                var lineCount = line.Length / maxLineLength + 1;
                for (int i = 0; i < lineCount; i++)
                {
                    if ((i + 1) * maxLineLength <= line.Length)
                    {
                        _lines.Add(line.Substring(i * maxLineLength, maxLineLength));
                    }
                    else
                    {
                        _lines.Add(line.Substring(i * maxLineLength, line.Length - i * maxLineLength));
                    }
                }
            }
            if (_lines.Count > maxLines)
            {
                _lines.RemoveRange(0, _lines.Count - maxLines);
            }
            //_lines.AddRange(logString.Split('\n'));
            _logStr = string.Join("\n", _lines);
        }

        void OnGUI()
        {
            if (sysUtils.isOutLogToScreen)
            {
                GUI.matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity,
                new Vector3(1.2f, Screen.height / 800.0f, 1.0f));
                GUIStyle fontStyle = new GUIStyle();
                fontStyle.normal.textColor = Color.red;
                fontStyle.fontSize = Math.Max(10, fontSize);

                GUI.Label(new Rect(10, 10, Screen.width - 20, 370), _logStr, fontStyle);
            }

        }

        public void clear()
        {
            _lines.Clear();
            _logStr = "";
        }
        public string getLogStr()
        {
            return _logStr;
        }
    }
}
