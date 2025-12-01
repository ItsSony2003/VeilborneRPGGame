using System.Collections;
using UnityEngine;

public class Entity_VFX : MonoBehaviour
{
    protected SpriteRenderer sr;
    private Entity entity;

    [Header("Dash Image Echo VFX")]
    [Range(0.01f, 0.2f)]
    [SerializeField] private float imageEchoInterval = 0.05f;
    [SerializeField] private GameObject targetEchoPrefab;
    private Coroutine imageEchoCo;

    [Header("On Taking Damage VFX")]
    [SerializeField] private Material onDamageMaterial;
    [SerializeField] private float onDamageVFXDuration = .1f;
    private Material originalMaterial;
    private Coroutine onDamageVfxCoroutine;

    [Header("On Doing Damage VFX")]
    [SerializeField] private Color hitVfxColor = Color.white;
    [SerializeField] private GameObject hitVfx;
    [SerializeField] private GameObject critHitVfx;

    [SerializeField] private Color slowVfx = Color.cyan;
    [SerializeField] private Color fireVfx = Color.red;
    [SerializeField] private Color lightningVfx = Color.yellow;
    private Color originalHitVfxColor;

    private void Awake()
    {
        entity = GetComponent<Entity>();
        sr = GetComponentInChildren<SpriteRenderer>();
        originalMaterial = sr.material;
        originalHitVfxColor = hitVfxColor;
    }

    public void DoDashImageEchoEffect(float duration)
    {
        StopImageEchoEffect();
        imageEchoCo = StartCoroutine(DashImageEchoEffectCo(duration));
    }

    public void StopImageEchoEffect()
    {
        if (imageEchoCo != null)
            StopCoroutine(imageEchoCo);
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
        Vector3 position = entity.anim.transform.position;
        float scale = entity.anim.transform.localScale.x;

        GameObject imageEcho = Instantiate(targetEchoPrefab, transform.position, transform.rotation);
        imageEcho.transform.localScale = new Vector3(scale, scale, scale);
        imageEcho.GetComponentInChildren<SpriteRenderer>().sprite = sr.sprite;
    }

    public void PlayOnStatusVfx(float duration, ElementType element)
    {
        if (element == ElementType.Ice)
            StartCoroutine(PlayStatusVfxCo(duration, slowVfx));

        if (element == ElementType.Fire)
            StartCoroutine(PlayStatusVfxCo(duration, fireVfx));

        if (element == ElementType.Lightning)
            StartCoroutine(PlayStatusVfxCo(duration, lightningVfx));
    }

    private IEnumerator PlayStatusVfxCo(float duration, Color effectColor)
    {
        float tickInterval = 0.25f;
        float timerHasPassed = 0;

        Color lightColor = effectColor * 1.5f;
        Color darkColor = effectColor * 0.9f;

        bool toggle = false;

        while (timerHasPassed < duration)
        {
            sr.color = toggle ? lightColor : darkColor;
            toggle = !toggle;

            yield return new WaitForSeconds(tickInterval);
            timerHasPassed += tickInterval;
        }

        sr.color = Color.white;
    }

    public void CreateOnHitVFX(Transform target, bool isCrit, ElementType element)
    {
        GameObject hitPrefab = isCrit ? critHitVfx : hitVfx;
        GameObject vfx = Instantiate(hitPrefab, target.position, Quaternion.identity);
        //vfx.GetComponentInChildren<SpriteRenderer>().color = GetElementalColor(element);

        if (entity.facingDirection == -1 && isCrit)
            vfx.transform.Rotate(0, 180, 0);
    }

    public void StopAllVfx()
    {
        StopAllCoroutines();
        sr.color = Color.white;
        sr.material = originalMaterial;
    }

    public Color GetElementalColor(ElementType element)
    {
        switch (element)
        {
            case ElementType.Ice:
                return slowVfx;
            case ElementType.Fire:
                return fireVfx;
            case ElementType.Lightning:
                return lightningVfx;
            default:
                return Color.white;
        }
    }

    public void PlayOnDamageVfx()
    {
        if (onDamageVfxCoroutine != null)
            StopCoroutine(onDamageVfxCoroutine);

        onDamageVfxCoroutine = StartCoroutine(OnDamageVfxCo());
    }

    private IEnumerator OnDamageVfxCo()
    {
        sr.material = onDamageMaterial;

        yield return new WaitForSeconds(onDamageVFXDuration);
        sr.material = originalMaterial;
    }
}
