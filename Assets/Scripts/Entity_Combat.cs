using Mono.Cecil;
using UnityEngine;

// This script is for target detection when attack by both enemy and player
public class Entity_Combat : MonoBehaviour
{
    public float damage = 10;

    [Header("Target Detection")]
    [SerializeField] private Transform targetCheck;
    [SerializeField] private float targetCheckRadius = 1;
    [SerializeField] private LayerMask whatIsTarget;

    public void PerformAttack()
    {
        foreach (var target in GetDetectionColliders())
        {
            Entity_Health targetHealth = target.GetComponent<Entity_Health>();

            // targetHealth?.TakeDamage(10)
            if (targetHealth != null)
                targetHealth.TakeDamage(damage, transform);
        }
    }    

    private Collider2D[] GetDetectionColliders()
    {
        return Physics2D.OverlapCircleAll(targetCheck.position, targetCheckRadius, whatIsTarget);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(targetCheck.position, targetCheckRadius);
    }
}
