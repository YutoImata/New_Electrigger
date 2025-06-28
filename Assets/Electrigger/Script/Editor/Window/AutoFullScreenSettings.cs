using UnityEngine;

namespace Electrigger
{
    [CreateAssetMenu(fileName = "AutoFullScreenSettings", menuName = "Tools/Window/Auto FullScreen Settings")]
    public class AutoFullScreenSettings : ScriptableObject
    {
        public bool enableFullScreen = true;
    }
}