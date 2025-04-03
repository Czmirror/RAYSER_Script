using System;
using UnityEngine;

namespace UI.Game
{
    [Serializable]
    public class TalkSentence
    {
        public TalkCharacter Character;
        [TextArea(2,4)] public string Message;
    }
}
