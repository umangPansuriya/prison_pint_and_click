using UnityEngine;

public class LaserAnimController : MonoBehaviour
{
    public KeyCardScanner key_cardScanner;

    public void CheckCardValidation()
    {
        key_cardScanner.CheckCardIdetity();
    }
}
