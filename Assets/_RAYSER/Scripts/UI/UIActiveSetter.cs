using UnityEngine;

namespace _RAYSER.Scripts.UI
{
    /// <summary>
    /// UIのSetActiveを行うクラス
    /// </summary>
    public class UIActiveSetter
    {
        public void SetActive(GameObject gameObject, bool isActive)
        {
            gameObject.SetActive(isActive);
        }
    }
}
