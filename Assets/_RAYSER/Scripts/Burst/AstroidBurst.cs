using System;
using System.Collections;
using Shield;
using UnityEngine;

namespace Burst
{
    public class AstroidBurst : MonoBehaviour
    {
        private EnemyShield _enemyShield;

        /// <summary>
        /// 自爆までの時間
        /// </summary>
        [SerializeField] private float burstTime = 10.0f;
        private void Start()
        {
            _enemyShield = gameObject.GetComponent<EnemyShield>();
            StartCoroutine(DelayMethod(burstTime));
        }

        private IEnumerator DelayMethod(float waitTime)
        {
            yield return new WaitForSeconds(waitTime);
            _enemyShield.ShieldReduction(9999);
        }
    }
}
