using System.Collections.Generic;
using UnityEngine;

namespace UI.Game
{

    [CreateAssetMenu(fileName = "TalkConversation", menuName = "Talk/TalkConversation")]
    public class TalkConversation : ScriptableObject
    {
        [SerializeField] private List<TalkSentence> sentences;
        public IReadOnlyList<TalkSentence> Sentences => sentences;

        [SerializeField] private string targetName;
    }
}
