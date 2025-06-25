using System.Collections.Generic;
using UnityEngine;

public class Demo : MonoBehaviour
{
    [SerializeField] List<Rigidbody> _bodies;
    [SerializeField] Collider _collider;
    [SerializeField] Animator _animator;
    private void Start()
    {
        _collider.enabled = true;
        _animator.enabled = true;
        foreach (var b in _bodies)
        {
            b.isKinematic = true;
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "Ammo")
        {

            _collider.enabled = false;
            _animator.enabled = false;




        }
    }
    private void ActiveRagDoolApplyForce(Vector3 point)
    {
        float closestDist = float.MaxValue;
        Rigidbody closest = null;
        foreach (var rb in _bodies)
        {
            float dist = Vector3.Distance(rb.worldCenterOfMass, point);
            if (dist < closestDist)
            {
                closestDist = dist;
                closest = rb;
            }
            rb.isKinematic = false;
            if (closest != null)
            {
                rb.AddForce((transform.forward - point) * 10, ForceMode.Impulse);
            }
        }
    }
}


