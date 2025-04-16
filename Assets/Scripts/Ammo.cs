using UnityEngine;

public class Ammo : MonoBehaviour
{
    [SerializeField] private float _speed;
    private void Update()
    {
        transform.position += transform.forward * Time.deltaTime * _speed;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 6)
        {
            other.GetComponent<PlayerHealth>().Dmage(0);
        }
        Destroy(gameObject);
    }
}
