using TMPro;
using UnityEngine;
public class FpsCounter : MonoBehaviour
{
    public TMP_Text fpsText;

    float deltaTime;
    float fps;
    private void Awake()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 120;
    }
    //void Update()
    //{
    //    deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
    //    fps = 1.0f / deltaTime;
    //    fpsText.text = "FPS: " + Mathf.Ceil(fps).ToString();
    //}
}
