using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class KeypadLcckController : PuzzlePanel
{
    public string trueCode;
    public Text inputText;
    public Image[] greenSignalsImgsArr;
    int signalCounter = 2;

    public float shakeDuration = 0.5f;
    public float shakeMagnitude = 0.1f;

    private Vector3 originalPosition;
    [SerializeField] private Transform _alertPoint;
    void OnEnable()
    {
        originalPosition = inputText.transform.localPosition;
    }
    public void CodeOnClick(int num)
    {
        string Concatenum = string.Concat(inputText.text, num.ToString());
        inputText.text = Concatenum;
    }
    public void EnterBtnOnClick()
    {
        if (inputText.text.Equals(trueCode))
        {
            Solve();
        }
        else
        {
            inputText.text = "<color='red'>WRONG CODE!</color>";
            StartShake();
        }
    }
    public void ClearKeypadDisplay()
    {
        CLearInputText();
    }
    public void CLearInputText()
    {
        inputText.text = "";
    }
    public void WrongAnswerDisableGreenSignal()
    {
        greenSignalsImgsArr[signalCounter].color = Color.gray;
        if (signalCounter == 0)
        {
            Debug.Log("Play Alarm!");
            inputText.text = "<color='red'>ALARM!</color>";
            EnemyEventBroadcaster.RisePlayerDetected(_alertPoint.position);
            Close();
            return;
        }
        signalCounter--;
        CLearInputText();
    }
    public void StartShake()
    {
        StartCoroutine(Shake());
    }
    IEnumerator Shake()
    {
        float elapsed = 0.0f;

        while (elapsed < shakeDuration)
        {
            float x = Random.Range(-2f, 2f) * shakeMagnitude;
            float y = Random.Range(-2f, 2f) * shakeMagnitude;

            inputText.transform.localPosition = originalPosition + new Vector3(x, 0, 0);

            elapsed += Time.deltaTime;
            yield return null;
        }

        inputText.transform.localPosition = originalPosition;
        WrongAnswerDisableGreenSignal();
    }

}
