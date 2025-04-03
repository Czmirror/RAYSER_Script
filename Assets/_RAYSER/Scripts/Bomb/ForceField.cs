using UnityEngine;
using Cysharp.Threading.Tasks;
using System;
using MessagePipe;

namespace _RAYSER.Scripts.Bomb
{
    /// <summary>
    /// フォースフィールド : ボムの一種 Visitor
    /// </summary>
    [System.Serializable]
    public class ForceField : IBombVisitor
    {
        private BombPrefab forceFieldPrefab;
        private int useCount = 5;
        private int maxUseCount = 5;
        private float duration = 5.0f;
        private DateTime lastUseTime = DateTime.MinValue;
        private BombPrefab currentInstance;

        /// <summary>
        /// ボム使用判定
        /// </summary>
        private bool isUse = false;

        public int UseCount => useCount;
        public event Action<int> OnUseCountChanged;

        private IPublisher<BombActiveSignal> bombActiveSignalPublisher;

        public void Visit(BombAction action)
        {
            Use(action.Position);
        }

        public void SetPrefab(BombPrefab prefab)
        {
            forceFieldPrefab = prefab;
        }

        public void SetPublisher(IPublisher<BombActiveSignal> publisher)
        {
            bombActiveSignalPublisher = publisher;
        }

        public bool CanUse()
        {
            return useCount > 0 && !isUse;
        }

        public async void Use(Vector3 position)
        {
            if (!CanUse()) return;

            isUse = true;
            useCount--;
            NotifyUseCountChanged();
            bombActiveSignalPublisher.Publish(new BombActiveSignal(BombActiveType.Active)); // 使用開始時にPublish

            if (currentInstance != null) UnityEngine.Object.Destroy(currentInstance.gameObject);
            currentInstance = UnityEngine.Object.Instantiate(forceFieldPrefab, position, Quaternion.identity);

            await UniTask.Delay(TimeSpan.FromSeconds(duration));

            if (currentInstance != null) UnityEngine.Object.Destroy(currentInstance.gameObject);

            isUse = false;
            bombActiveSignalPublisher.Publish(new BombActiveSignal(BombActiveType.Inactive));
        }

        public void Reset()
        {
            useCount = maxUseCount;
            NotifyUseCountChanged();
            if (currentInstance != null) UnityEngine.Object.Destroy(currentInstance.gameObject);
            lastUseTime = DateTime.MinValue;
            isUse = false;
        }

        private void NotifyUseCountChanged()
        {
            OnUseCountChanged?.Invoke(useCount);
        }
    }
}
