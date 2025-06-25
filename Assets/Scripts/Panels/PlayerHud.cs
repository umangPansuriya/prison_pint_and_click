using UnityEngine;
using UnityEngine.UI;

public class PlayerHud : MonoBehaviour
{
    [SerializeField] private PlayerController _playerController;
    [SerializeField] private Run _run;
    [SerializeField] private Button _hideBtn;
    [SerializeField] private Button _unhideBtn;

    private void OnEnable()
    {
        _hideBtn.onClick.AddListener(OnHideClick);
        _unhideBtn.onClick.AddListener(OnUnhideClick);

        _playerController.Hide_Action += OnPlayerHide;
        _playerController.Unhide_Action += OnPlayerUnhide;
    }
    private void OnDisable()
    {
        _hideBtn.onClick.RemoveListener(OnHideClick);
        _unhideBtn.onClick.RemoveListener(OnUnhideClick);

        _playerController.Hide_Action -= OnPlayerHide;
        _playerController.Unhide_Action -= OnPlayerUnhide;
    }
    private void OnHideClick()
    {
        _playerController.Hide();
    }
    private void OnUnhideClick()
    {
        _playerController.UnHide();
    }
    private void OnPlayerHide()
    {
        _run.gameObject.SetActive(false);
        _unhideBtn.gameObject.SetActive(true);
    }
    private void OnPlayerUnhide()
    {
        _unhideBtn.gameObject.SetActive(false);
        _run.gameObject.SetActive(true);
        _hideBtn.gameObject.SetActive(false);
    }
    public void EnableHideBtn()
    {
        _hideBtn.gameObject.SetActive(true);
        _run.gameObject.SetActive(false);
    }
    public void DisableHideBtn()
    {
        _hideBtn.gameObject.SetActive(false);
        _run.gameObject.SetActive(true);
    }
}
