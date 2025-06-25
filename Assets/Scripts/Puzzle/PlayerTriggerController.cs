using UnityEngine;

public class PlayerTriggerController : MonoBehaviour
{
    public GameObject[] panelObjArr;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("player trigger");
            for (int i = 0; i < panelObjArr.Length; i++)
            {
                panelObjArr[i].SetActive(true);
            }
        }
    }
}
