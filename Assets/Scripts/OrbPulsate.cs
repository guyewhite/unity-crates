using UnityEngine;

public class OrbPulsate : MonoBehaviour
{
    [SerializeField] private float minScale = 0.28f;
    [SerializeField] private float maxScale = 0.32f;
    [SerializeField] private float pulsateSpeed = 0.5f;

    [Header("Emission Settings")]
    [SerializeField] private float minEmissionIntensity = 1.0f;
    [SerializeField] private float maxEmissionIntensity = 2.5f;
    [SerializeField] private Color emissionColor = new Color(1.5f, 1.2f, 0.3f);

    private Material orbMaterial;
    private Material coreMaterial;
    private float time = 0f;

    void Start()
    {
        // Get materials from child objects
        Transform outerGlow = transform.Find("OuterGlow");
        Transform innerCore = transform.Find("InnerCore");

        if (outerGlow != null)
        {
            MeshRenderer renderer = outerGlow.GetComponent<MeshRenderer>();
            if (renderer != null)
            {
                orbMaterial = renderer.material;
                orbMaterial.EnableKeyword("_EMISSION");
            }
        }

        if (innerCore != null)
        {
            MeshRenderer renderer = innerCore.GetComponent<MeshRenderer>();
            if (renderer != null)
            {
                coreMaterial = renderer.material;
                coreMaterial.EnableKeyword("_EMISSION");
            }
        }
    }

    void Update()
    {
        time += Time.deltaTime * pulsateSpeed;

        // Smooth sine wave pulsation
        float sineWave = (Mathf.Sin(time * Mathf.PI) + 1f) / 2f;

        // Apply scale pulsation
        float currentScale = Mathf.Lerp(minScale, maxScale, sineWave);
        transform.localScale = Vector3.one * currentScale;

        // Apply emission pulsation to both materials
        if (orbMaterial != null)
        {
            float currentIntensity = Mathf.Lerp(minEmissionIntensity, maxEmissionIntensity, sineWave);
            Color currentEmission = emissionColor * currentIntensity;
            orbMaterial.SetColor("_EmissionColor", currentEmission);
        }

        if (coreMaterial != null)
        {
            // Core glows brighter and inverse to create depth
            float coreIntensity = Mathf.Lerp(maxEmissionIntensity * 1.5f, minEmissionIntensity * 1.5f, sineWave);
            Color coreEmission = Color.white * coreIntensity;
            coreMaterial.SetColor("_EmissionColor", coreEmission);
        }

        // Add subtle rotation for extra beauty
        transform.Rotate(Vector3.up, 15f * Time.deltaTime);
    }

    void OnDestroy()
    {
        // Clean up the material instances
        if (orbMaterial != null)
        {
            Destroy(orbMaterial);
        }
        if (coreMaterial != null)
        {
            Destroy(coreMaterial);
        }
    }
}