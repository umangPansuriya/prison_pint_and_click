using UnityEngine;

public class Ragdool : MonoBehaviour
{
    [SerializeField] private Rigidbody[] rigidbodies;

    public void ActiveRagdool()
    {
        SetRagdoolTo(false);
        gameObject.SetActive(true);
    }
    public void DeactiveRagdoll()
    {
        SetRagdoolTo(true);
        gameObject.SetActive(false);
    }
    private void SetRagdoolTo(bool value)
    {
        foreach (var rigidbody in rigidbodies)
        {
            rigidbody.isKinematic = value;
        }
    }
    public bool AddForceToCloset(Vector3 point, float force = 100)
    {
        Rigidbody closestRigidbody = null;
        float closestDistance = float.MaxValue;
        foreach (var rigidbody in rigidbodies)
        {
            float distance = Vector3.Distance(point, rigidbody.worldCenterOfMass);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestRigidbody = rigidbody;
            }
            //rigidbody.AddForce((transform.position - point) * force, ForceMode.Impulse);
        }
        if (closestRigidbody != null)
        {
            closestRigidbody.AddForce((transform.position - point) * force, ForceMode.Impulse);
            return true;
        }
        return false;
    }
}
