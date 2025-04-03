using DG.Tweening;
using UnityEngine;

namespace _RAYSER.Scripts.Item.Move
{
    /// <summary>
    /// アイテムの回転
    /// </summary>
    public class ItemRotate : MonoBehaviour
    {
        [SerializeField] private float rotationDuration = 1f; // 一回転にかかる時間（秒）
        [SerializeField] private bool loop = true; // 回転をループさせるか

        private Transform targetTransform;

        private void Awake()
        {
            // Transformの取得
            targetTransform = GetComponent<Transform>();

            if (targetTransform == null)
            {
                Debug.LogError("Transformが見つかりません。スクリプトを3Dオブジェクトにアタッチしてください。");
            }
        }

        private void Start()
        {
            StartRotation();
        }

        private void StartRotation()
        {
            if (targetTransform == null) return;

            // DOTweenでY軸を360度回転
            targetTransform
                .DOLocalRotate(new Vector3(0, 360f, 0), rotationDuration, RotateMode.FastBeyond360)
                .SetEase(Ease.Linear) // 一定速度で回転
                .SetLoops(loop ? -1 : 0, LoopType.Restart) // ループの設定
                .OnKill(() =>
                {
                    Debug.Log("Rotation animation stopped.");
                });
        }

        private void OnDestroy()
        {
            // このオブジェクトに関連付けられたTweenを停止
            DOTween.Kill(targetTransform);
        }
    }
}
