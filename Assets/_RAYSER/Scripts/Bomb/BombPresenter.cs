using System;
using TMPro;
using VContainer.Unity;

namespace _RAYSER.Scripts.Bomb
{
    public class BombPresenter : IStartable, IDisposable
    {
        private readonly IBombVisitor bombVisitor;
        private readonly TextMeshProUGUI bombUseCountText;

        public BombPresenter(IBombVisitor bombVisitor, TextMeshProUGUI bombUseCountText)
        {
            this.bombVisitor = bombVisitor;
            this.bombUseCountText = bombUseCountText;
        }

        public void Start()
        {
            bombVisitor.OnUseCountChanged += UpdateBombUseCountView;
            UpdateBombUseCountView(bombVisitor.UseCount); // 初期表示を更新
        }

        public void Dispose()
        {
            bombVisitor.OnUseCountChanged -= UpdateBombUseCountView;
        }



        private void UpdateBombUseCountView(int useCount)
        {
            if (bombUseCountText != null)
            {
                bombUseCountText.text = $"Bombs: {useCount}";
            }
        }
    }
}
