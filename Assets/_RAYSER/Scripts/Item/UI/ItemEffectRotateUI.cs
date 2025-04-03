using DG.Tweening;
using UnityEngine;

namespace _RAYSER.Scripts.Item.Camera
{
    /// <summary>
    /// アイテムUIの回転
    /// </summary>
    public class ItemEffectRotateUI : MonoBehaviour
    {
        [SerializeField] private float rotationDuration = 1f; // 一回転にかかる時間（秒）

        private RectTransform rectTransform;

        private void Awake()
        {
            // RectTransformの取得
            rectTransform = GetComponent<RectTransform>();

            if (rectTransform == null)
            {
                Debug.LogError("RectTransformが見つかりません。スクリプトをUIオブジェクトにアタッチしてください。");
            }
        }

        private void Start()
        {
            StartRotation();
        }

        private void StartRotation()
        {
            if (rectTransform == null) return;

            // 初期値の回転を取得
            Vector3 initialRotation = rectTransform.localEulerAngles;

            float y = Random.Range(0, 360);
            float z = Random.Range(0, 360);

            // DOTweenでX軸を360度回転
            rectTransform
                .DOLocalRotate(new Vector3(360f, y, z), rotationDuration, RotateMode.FastBeyond360)
                .SetEase(Ease.Linear) // 一定速度で回転
                .Pause()
                .OnComplete(() =>
                {
                    // 回転完了後に初期位置に戻して再度回転
                    rectTransform.localEulerAngles = initialRotation;
                    StartRotation();
                })
                .Restart();
        }

        private void OnDestroy()
        {
            // このオブジェクトに関連付けられたTweenを停止
            DOTween.Kill(this);
        }
    }
}
