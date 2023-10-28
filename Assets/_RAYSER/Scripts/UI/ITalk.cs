using System.Threading;
using _RAYSER.Scripts.Tweening;
using Cysharp.Threading.Tasks;
using TMPro;

namespace _RAYSER.Scripts.UI
{
    public interface ITalk
    {
        TweenExecution TweenExecution { get; set; }
        UniTask Talk(TextMeshProUGUI textMeshProUGUI, Character.Character character, string text, float speed,
            CancellationToken cancellationToken);
    }
}
