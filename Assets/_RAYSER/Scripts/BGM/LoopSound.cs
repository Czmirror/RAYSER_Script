using UnityEngine;

namespace BGM
{
    /// <summary>
    /// 音源ループScriptableObject
    /// </summary>
    [CreateAssetMenu(fileName = "LoopSound", menuName = "ScriptableObjects/LoopSound", order = 1)]
    public class LoopSound :ScriptableObject
    {
        /// <summary>
        /// 音源ファイル
        /// </summary>
        public AudioClip clip;

        /// <summary>
        /// イントロ部分のスキップ時間帯（秒）
        /// </summary>
        public float skipTime;
    }
}
