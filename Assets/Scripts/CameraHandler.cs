using TMPro;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.UI;

public class CameraHandler : MonoBehaviour
{
    [SerializeField] private TMP_InputField _xRotation;
    [SerializeField] private TMP_InputField _yRotation;
    [SerializeField] private TMP_InputField _distance;
    [SerializeField] private Button _applyBtn;

    [Space]
    [SerializeField] private TMP_Text _info;

    [Space]
    [SerializeField] private CinemachineCamera _camera;
    [SerializeField] private CinemachinePositionComposer _positionComposer;
    private void OnEnable()
    {
        _applyBtn.onClick.AddListener(OnApplyClick);
    }
    private void OnDisable()
    {
        _applyBtn.onClick.RemoveListener(OnApplyClick);

    }
    private void Start()
    {
        SetInfo();
    }
    private void SetInfo()
    {
        _info.text = $"x:{_camera.transform.eulerAngles.x}, y:{_camera.transform.eulerAngles.y}, distance:{_positionComposer.CameraDistance}";
    }
    private void OnApplyClick()
    {
        Vector3 newRoation = _camera.transform.eulerAngles;
        if (_xRotation.text != string.Empty)
        {
            newRoation.x = float.Parse(_xRotation.text);
        }
        if (_yRotation.text != string.Empty)
        {
            newRoation.y = float.Parse(_yRotation.text);
        }
        _camera.transform.eulerAngles = newRoation;
        if (_distance.text != string.Empty)
        {
            _positionComposer.CameraDistance = float.Parse(_distance.text);
        }
        SetInfo();
    }

}
