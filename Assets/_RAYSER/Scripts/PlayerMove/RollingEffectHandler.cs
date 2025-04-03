using System;
using _RAYSER.Scripts.Event.Signal;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Event.Signal;
using UniRx;
using UnityEngine;

    /// <summary>
    /// ローリングエフェクトを管理するクラス（DOTween 使用）
    /// </summary>
public class RollingEffectHandler : MonoBehaviour
{
    private IDisposable rollingSubscription;
    private bool isRolling = false;
    private float duration;
    private float targetRotation;

    /// <summary>
    /// デフォルトのローリング時間（シグナルの `Duration` が `null` の場合に適用）
    /// </summary>
    [SerializeField] private float rollingDuration = 0.5f;

    private void Start()
    {
        // ローリングシグナルを購読
        rollingSubscription = MessageBroker.Default.Receive<RollingSignal>()
            .Subscribe(signal => ApplyRollingEffect(signal))
            .AddTo(this);

        // ゲームクリア時に回転リセット
        MessageBroker.Default.Receive<GameClear>()
            .Subscribe(_ => ResetRolling())
            .AddTo(this);
    }

    private void OnDestroy()
    {
        rollingSubscription?.Dispose();
    }

    private void ApplyRollingEffect(RollingSignal signal)
    {
        if (isRolling) return; // 回転中なら無視

        isRolling = true;
        duration = signal.Duration ?? rollingDuration;

        targetRotation = signal.RotationAngle;

        Debug.Log("targetRotation: " + targetRotation);

        // DOTweenシーケンスセット
        var sequence = DOTween.Sequence();

        sequence
            .Append(transform.DOLocalRotate(new Vector3(targetRotation, 0, 0),
                duration, RotateMode.LocalAxisAdd)
                .SetEase(Ease.OutQuad))
            .OnComplete(() =>
            {
                // **X 軸の回転を完全にリセット**
                transform.localRotation = Quaternion.Euler(0, transform.localEulerAngles.y, transform.localEulerAngles.z);
                isRolling = false;
            })
            .Pause()
            .SetAutoKill(false)
            .SetLink(gameObject);

        sequence.Play();

    }

    private void ResetRolling()
    {
        // DOTweenシーケンスセット
        var sequence = DOTween.Sequence();

        sequence
            .Append(transform.DOLocalRotate(new Vector3(targetRotation, 0, 0),
                    duration, RotateMode.LocalAxisAdd)
                .SetEase(Ease.OutQuad))
            .OnComplete(() =>
            {
                // **X 軸の回転を完全にリセット**
                transform.localRotation = Quaternion.Euler(0, transform.localEulerAngles.y, transform.localEulerAngles.z);
                isRolling = false;
            })
            .Pause()
            .SetAutoKill(false)
            .SetLink(gameObject);

        sequence.Play();
    }
}
