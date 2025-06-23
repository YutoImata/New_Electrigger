using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

namespace Electrigger
{
    /// <summary>
    /// Hierarchy内のオブジェクト名を検索して、そのTransform.positionをGUIに表示するエディタツール
    /// </summary>
    public class ObjectPositionFinder : EditorWindow
    {
        private string searchName = "";
        private List<Transform> foundObjects = new List<Transform>(); // 検索結果を保持

        [MenuItem("Tools/Object Position Finder")]
        public static void ShowWindow()
        {
            GetWindow<ObjectPositionFinder>("Object Position Finder");
        }

        private void OnGUI()
        {
            GUILayout.Label("【使い方】", EditorStyles.boldLabel);
            EditorGUILayout.HelpBox(
                "Hierarchy 内のオブジェクト名を入力して「検索」ボタンを押すと、\n" +
                "一致するオブジェクトの位置情報（Transform.position）をこの画面に表示します。",
                MessageType.Info);

            GUILayout.Space(10);

            searchName = EditorGUILayout.TextField("オブジェクト名", searchName);

            if (GUILayout.Button("検索"))
            {
                SearchObjectPositions();
            }

            GUILayout.Space(10);

            if (foundObjects.Count > 0)
            {
                GUILayout.Label($"【検索結果】{foundObjects.Count} 件見つかりました：", EditorStyles.boldLabel);

                foreach (var t in foundObjects)
                {
                    GUILayout.Label($"・{t.name} の位置: {t.position}");
                }
            }
        }

        private void SearchObjectPositions()
        {
            foundObjects.Clear(); // 前回の検索結果をクリア

            if (string.IsNullOrEmpty(searchName))
            {
                Debug.LogWarning("検索ワードが空です。オブジェクト名を入力してください。");
                return;
            }

            Transform[] allTransforms = Object.FindObjectsByType<Transform>(
                FindObjectsInactive.Include,
                FindObjectsSortMode.None
            );

            foreach (Transform t in allTransforms)
            {
                if (t.name == searchName)
                {
                    foundObjects.Add(t);
                }
            }

            if (foundObjects.Count == 0)
            {
                Debug.LogWarning($"「{searchName}」という名前のオブジェクトは見つかりませんでした。");
            }
        }
    }
}
