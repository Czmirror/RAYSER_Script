using UnityEngine;

namespace Target
{
    public class EnemyTarget : MonoBehaviour, IEnemyTarget
    {

        public GameObject Target { get; set; }

        public void TargetInitialize(GameObject target)
        {
            Target = target;
        }

        public GameObject CurrentTarget()
        {
            return Target;
        }
    }
}
