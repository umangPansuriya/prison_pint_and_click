using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private float _health;
    public void Dmage(float damage)
    {
        _health -= damage;
        if (_health <= 0)
        {
            GameEvent.RiseGameOver();
        }
    }
}
