using System.Threading;
using Cysharp.Threading.Tasks;
using MessagePipe;
using Rayser.CustomEditor;
using UniRx;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using VContainer;

namespace _RAYSER.Scripts.UI.Title
{
    public class TutorialDialog : MonoBehaviour
    {
        [Header("UI")]
        [SerializeField] private Button yesButton;
        [SerializeField] private Button noButton;
        [SerializeField] private CanvasGroup canvasGroup;

        [Header("Scene")]
        [SerializeField] private SceneObject tutorialScene;
        [SerializeField] private SceneObject gameScene;

        private IPublisher<TutorialDialogCloseSignal> _tutorialDialogCloseSignalPublisher;
        private ISubscriber<TutorialDialogOpenSignal> _tutorialDialogOpenSignalSubscriber;
        private ISubscriber<TutorialDialogCloseSignal> _tutorialDialogCloseSignalSubscriber;
        private readonly CompositeDisposable _disposables = new CompositeDisposable();

        private UIEffect _uiEffect = new UIEffect();
        private CancellationTokenSource cts = new CancellationTokenSource();

        [Inject]
        public void Construct(IPublisher<TutorialDialogCloseSignal> tutorialDialogCloseSignalPublisher,
            ISubscriber<TutorialDialogOpenSignal> tutorialDialogOpenSignalSubscriber,
            ISubscriber<TutorialDialogCloseSignal> tutorialDialogCloseSignalSubscriber)
        {
            _tutorialDialogCloseSignalPublisher = tutorialDialogCloseSignalPublisher;
            _tutorialDialogOpenSignalSubscriber = tutorialDialogOpenSignalSubscriber;
            _tutorialDialogCloseSignalSubscriber = tutorialDialogCloseSignalSubscriber;

            _tutorialDialogOpenSignalSubscriber.Subscribe(_ => ShowDialog()).AddTo(_disposables);
            _tutorialDialogCloseSignalSubscriber.Subscribe(_ => HideDialog()).AddTo(_disposables);
        }
        private void Start()
        {
            InitializeUI();
            gameObject.SetActive(false);
            yesButton.onClick.AddListener(YesButtonOnClick);
            noButton.onClick.AddListener(NoButtonOnClick);
        }

        private void InitializeUI()
        {
            _uiEffect.SetAlphaZero(canvasGroup);
        }

        private async UniTask ShowDialog()
        {
            Debug.Log("ShowDialog");
            gameObject.SetActive(true);
            await _uiEffect.FadeIn(canvasGroup, cts.Token);

            // ダイアログ表示時にボタンにフォーカスを当てる
            yesButton.Select();
            Debug.Log("ShowDialog end");
        }

        private async UniTask HideDialog()
        {
            Debug.Log("HideDialog");
            gameObject.SetActive(false);
            await _uiEffect.FadeOut(canvasGroup, cts.Token);
            Debug.Log("HideDialog end");
        }

        private async void YesButtonOnClick()
        {
            _tutorialDialogCloseSignalPublisher.Publish(new TutorialDialogCloseSignal());
            await HideDialog();

            SceneManager.LoadScene(tutorialScene);
        }

        private async void NoButtonOnClick()
        {
            _tutorialDialogCloseSignalPublisher.Publish(new TutorialDialogCloseSignal());
            await HideDialog();

            SceneManager.LoadScene(gameScene);
        }

        private void OnDestroy()
        {
            // オブジェクト破棄時にサブスクリプションも破棄する
            _disposables.Dispose();
        }
    }
}
