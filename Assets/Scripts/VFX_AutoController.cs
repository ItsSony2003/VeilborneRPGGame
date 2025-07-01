using UnityEngine;

public class VFX_AutoController : MonoBehaviour
{
    [SerializeField] private bool autoDestroy = true;
    [SerializeField] private float destroyDelay = 1;
    [Space]
    [SerializeField] private bool randomOffset = true;
    [SerializeField] private bool randomRotation = true;
    [Header("Random Rotation")]
    [SerializeField] private float minRotation = 0;
    [SerializeField] private float maxRotation = 360;

    [Header("Random Position")]
    [SerializeField] private float xMinOffset = -0.2f;
    [SerializeField] private float xMaxOffset = 0.2f;
    [Space]
    [SerializeField] private float yMinOffset = -0.2f;
    [SerializeField] private float yMaxOffset = 0.2f;

    private void Start()
    {
        ApplyRandomOffset();
        ApplyRandomRotation();

        if (autoDestroy)
            Destroy(gameObject, destroyDelay);
    }

    private void ApplyRandomOffset()
    {
        if (randomOffset == true)
            return;

        float xoffset = Random.Range(xMinOffset, xMaxOffset);
        float yoffset = Random.Range(yMinOffset, yMaxOffset);

        transform.position = transform.position + new Vector3(xoffset, yoffset);
    }

    private void ApplyRandomRotation()
    {
        if (randomRotation == true)
            return;

        float zRotation = Random.Range(minRotation, maxRotation);
        transform.Rotate(0, 0, zRotation);
    }
}
