using _RAYSER.Scripts.UI.Title;
using Event.Signal;
using UniRx;
using UnityEngine;
using UnityEngine.InputSystem;

namespace InputSystem
{
    public class TitleController : MonoBehaviour
    {
        private PlayerInputActions _playerInputActions;

        [SerializeField] private PlayerInputNavigate _playerInputNavigate;

        private void Awake()
        {
            _playerInputActions = new PlayerInputActions();
            _playerInputActions.UI.Enable();
        }

        private void OnEnable()
        {
            _playerInputActions.UI.Navigate.performed += OnNavigate;
            _playerInputActions.UI.Navigate.canceled += OnNavigateStop;
            _playerInputActions.UI.Cancel.canceled += OnCancel;
        }

        private void OnDisable()
        {
            _playerInputActions.UI.Navigate.performed -= OnNavigate;
            _playerInputActions.UI.Navigate.canceled -= OnNavigateStop;
            _playerInputActions.UI.Cancel.canceled -= OnCancel;
        }

        private void OnNavigate(InputAction.CallbackContext obj)
        {
            var moveValue = obj.ReadValue<Vector2>();
            _playerInputNavigate.SetDirection(moveValue);
        }

        private void OnNavigateStop(InputAction.CallbackContext obj)
        {
            _playerInputNavigate.SetDirection(Vector2.zero);
        }

        private void OnCancel(InputAction.CallbackContext obj)
        {
            MessageBroker.Default.Publish(new GamePadCancelButtonPush());
        }
    }
}
