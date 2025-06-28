using UnityEditor;
using UnityEngine;

namespace Electrigger
{

    [CustomEditor(typeof(AutoFullScreenSettings))]
    public class AutoFullScreenSettingsEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            AutoFullScreenSettings settings = (AutoFullScreenSettings)target;

            EditorGUILayout.LabelField("ゲーム再生時にフルスクリーンにするか", EditorStyles.boldLabel);
            settings.enableFullScreen = EditorGUILayout.Toggle("フルスクリーン有効", settings.enableFullScreen);

            if (GUI.changed)
            {
                EditorUtility.SetDirty(settings);
            }
        }
    }
}