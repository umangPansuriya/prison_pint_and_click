using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverPanel : MonoBehaviour
{
    private void OnEnable()
    {
        GameEvent.GameOver += OnGameOver;
    }
    private void OnDisable()
    {
        GameEvent.GameOver -= OnGameOver;
    }
    private void OnGameOver()
    {
        StartCoroutine(OpenPanel());
    }
    private IEnumerator OpenPanel()
    {
        yield return new WaitForSeconds(2);
        transform.GetChild(0).gameObject.SetActive(true);
    }
    public void Restart()
    {
        SceneManager.LoadScene(0);
    }
}
