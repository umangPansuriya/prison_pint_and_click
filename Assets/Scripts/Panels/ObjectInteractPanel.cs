using UnityEngine;
using UnityEngine.UI;

public class ObjectInteractPanel : MonoBehaviour
{
    [SerializeField] private Button _hideBtn;
    [SerializeField] private Button _pushBtn;
    [SerializeField] private Button _backgroundBtn;

    private PushAndHideObject _pushAndHideObject;
    private void OnEnable()
    {
        _hideBtn.onClick.AddListener(OnHideClick);
        _pushBtn.onClick.AddListener(OnPushClick);
        _backgroundBtn.onClick.AddListener(OnBackgrounClick);
    }
    private void OnDisable()
    {
        _hideBtn.onClick.RemoveListener(OnHideClick);
        _pushBtn.onClick.RemoveListener(OnPushClick);
        _backgroundBtn.onClick.RemoveListener(OnBackgrounClick);
    }
    public void Show(PushAndHideObject obj)
    {
        _pushAndHideObject = obj;
        _backgroundBtn.gameObject.SetActive(true);
        _pushBtn.gameObject.SetActive(_pushAndHideObject.CanPush);
    }
    public void Hide()
    {
        _pushAndHideObject = null;
        _backgroundBtn.gameObject.SetActive(false);
    }
    private void OnPushClick()
    {
        _pushAndHideObject.Push();
        Hide();
    }
    private void OnHideClick()
    {
        _pushAndHideObject.Hide();
        Hide();
    }
    private void OnBackgrounClick()
    {
        _pushAndHideObject.Deselect();
        Hide();
    }
}
