using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;

namespace Electrigger
{
    /// <summary>
    /// エディタ内の複数ツールを一括管理するツールマネージャーウィンドウ
    /// </summary>
    public class ToolManager : EditorWindow
    {
        /// <summary>
        /// 管理するツール名とEditorWindowの型の辞書
        /// 新規ツール追加はここに書き足すだけでOK
        /// </summary>
        private Dictionary<string, Type> tools = new Dictionary<string, Type>()
        {
            { "Missing Script Finder", typeof(MissingScriptFinder) },
            { "Object Position Finder", typeof(ObjectPositionFinder) },
        };

        private Vector2 scrollPos;

        [MenuItem("Tools/Tool Manager")]
        public static void ShowWindow()
        {
            GetWindow<ToolManager>("Tool Manager");
        }

        private void OnGUI()
        {
            EditorGUILayout.LabelField("使いたいツールを選択してください", EditorStyles.boldLabel);

            scrollPos = EditorGUILayout.BeginScrollView(scrollPos);

            foreach (var tool in tools)
            {
                if (GUILayout.Button(tool.Key))
                {
                    EditorWindow.GetWindow(tool.Value).Show();
                    this.Close();
                }
            }

            EditorGUILayout.EndScrollView();
        }
    }
}
