using System.Collections;
using UnityEngine;

public class Player_VFX : Entity_VFX
{
    [Header("Dash Image Echo VFX")]
    [Range(0.01f, 0.2f)]
    [SerializeField] private float imageEchoInterval = 0.05f;
    [SerializeField] private GameObject targetEchoPrefab;
    private Coroutine imageEchoCo;

    public void DoDashImageEchoEffect(float duration)
    {
        if (imageEchoCo != null)
            StopCoroutine(imageEchoCo);

        imageEchoCo = StartCoroutine(DashImageEchoEffectCo(duration));
    }    

    private IEnumerator DashImageEchoEffectCo(float duration)
    {
        float time = 0;
        while (time < duration)
        {
            CreateImageEcho();

            yield return new WaitForSeconds(imageEchoInterval);
            time += imageEchoInterval;
        }    
    }

    private void CreateImageEcho()
    {
        GameObject imageEcho = Instantiate(targetEchoPrefab, transform.position, transform.rotation);
        imageEcho.GetComponentInChildren<SpriteRenderer>().sprite = sr.sprite;
    }
}
