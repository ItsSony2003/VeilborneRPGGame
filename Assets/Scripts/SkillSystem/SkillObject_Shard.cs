using System;
using UnityEngine;

public class SkillObject_Shard : SkillObject_Base
{
    [SerializeField] private GameObject vfxPrefab;

    public void SetUpShardExplosion(float ignitionTime)
    {
        Invoke(nameof(Explode), ignitionTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Enemy>() == null)
            return;

        Explode();
    }

    private void Explode()
    {
        DamageEnemiesInRadius(transform, checkRadius);
        Instantiate(vfxPrefab, transform.position, Quaternion.identity);

        Destroy(gameObject);
    }
}
