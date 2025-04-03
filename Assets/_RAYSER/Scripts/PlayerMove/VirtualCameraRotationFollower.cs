using System;
using _RAYSER.Scripts.Event.Signal;
using Cinemachine;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UniRx;
using UnityEngine;
using UnityEngine.Serialization;

namespace PlayerMove
{
    public class VirtualCameraRotationFollower : MonoBehaviour
    {
        private IDisposable rollingSubscription;
        [SerializeField] private Transform target; // defaultのTransform
        [SerializeField] private CinemachineVirtualCamera virtualCamera;
        [SerializeField] private Camera rollingCamera;

        /// <summary>
        /// 傾き速度
        /// </summary>
        private float _rotationSpeed = 10f;

        /// <summary>
        /// 回転判定
        /// </summary>
        private bool _isRolling = false;

        /// <summary>
        /// 回転速度
        /// </summary>
        private float _duration;

        /// <summary>
        /// 目標回転角度
        /// </summary>
        private float _targetRotation;

        /// <summary>
        /// デフォルト回転速度
        /// </summary>
        private readonly float defalutDuration = 0.5f;

        private void Start()
        {
            rollingSubscription = MessageBroker.Default.Receive<RollingSignal>()
                .Subscribe(signal => ApplyRollingEffect(signal))
                .AddTo(this);
        }

        private void OnDestroy()
        {
            rollingSubscription?.Dispose();
        }

        private void ApplyRollingEffect(RollingSignal signal)
        {
            if (_isRolling) return; // 回転中なら無視

            _isRolling = true;

            _duration = signal.Duration ?? defalutDuration;

            _targetRotation = signal.RotationAngle;

            var sequence = DOTween.Sequence();

            sequence
                .Append(virtualCamera.transform.DORotate(new Vector3(0, 0, _targetRotation),
                        _duration, RotateMode.LocalAxisAdd)
                    .SetEase(Ease.OutQuad))
                .OnComplete(() => { _isRolling = false; })
                .Pause()
                .SetAutoKill(false)
                .SetLink(gameObject);

            sequence.Play();
        }

        private void LateUpdate()
        {
            if (target == null || virtualCamera == null) return;

            if (_isRolling) return;

            // followのX軸に合わせて、カメラのZ軸の回転をカメラに適用
            // virtualCamera.transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x,
            //     transform.rotation.eulerAngles.y, target.rotation.eulerAngles.x);

            Quaternion desiredRotation = Quaternion.Euler(transform.rotation.eulerAngles.x,
                transform.rotation.eulerAngles.y, target.rotation.eulerAngles.x);
            virtualCamera.transform.rotation = Quaternion.Slerp(virtualCamera.transform.rotation, desiredRotation,
                Time.deltaTime * _rotationSpeed);
        }
    }
}
