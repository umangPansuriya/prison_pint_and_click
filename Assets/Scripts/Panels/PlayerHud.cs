using UnityEngine;
using UnityEngine.UI;

public class PlayerHud : MonoBehaviour
{
    [SerializeField] private PlayerController _playerController;
    [SerializeField] private Run _run;
    [SerializeField] private Button _hide;
    [SerializeField] private Button _unhide;

    private void OnEnable()
    {
        //_hide.onClick.AddListener(OnHideClick);
        _unhide.onClick.AddListener(OnUnhideClick);

        _playerController.Hide_Action += OnPlayerHide;
        _playerController.Unhide_Action += OnPlayerUnhide;
    }
    private void OnDisable()
    {
        //_hide.onClick.RemoveListener(OnHideClick);
        _unhide.onClick.RemoveListener(OnUnhideClick);

        _playerController.Hide_Action -= OnPlayerHide;
        _playerController.Unhide_Action -= OnPlayerUnhide;
    }
    private void OnHideClick()
    {

    }
    private void OnUnhideClick()
    {
        _playerController.UnHide();
    }
    private void OnPlayerHide()
    {
        _run.gameObject.SetActive(false);
        _unhide.gameObject.SetActive(true);
    }
    private void OnPlayerUnhide()
    {
        _unhide.gameObject.SetActive(false);
        _run.gameObject.SetActive(true);
    }
}
