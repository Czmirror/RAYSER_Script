using System;
using DG.Tweening;
using UnityEngine;

namespace EnemyMove
{
    public class Stage2EnemyShipMoveAndStop : MonoBehaviour
    {
        /// <summary>
        /// 初期地点移動スピード
        /// </summary>
        private float moveInitialTime = 2f;

        /// <summary>
        /// 移動場所X軸
        /// </summary>
        private float movePositionX = -820;

        private void Start()
        {
            // ステージ２の自機の方へ向ける
            transform.localRotation = Quaternion.Euler(0, 90, 0);

            transform.DOLocalMoveX(movePositionX, moveInitialTime)
                .Pause()
                .SetAutoKill(false)
                .SetLink(gameObject)
                .Restart();
        }
    }
}
