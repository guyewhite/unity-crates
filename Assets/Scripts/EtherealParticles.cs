using UnityEngine;
using System.Collections.Generic;

public class EtherealParticles : MonoBehaviour
{
    [Header("Particle Settings")]
    [SerializeField] private int particleCount = 12;
    [SerializeField] private float orbitRadius = 0.3f;
    [SerializeField] private float orbitRadiusVariation = 0.1f;
    [SerializeField] private float particleSize = 0.03f;
    [SerializeField] private float particleSizeVariation = 0.01f;

    [Header("Movement Settings")]
    [SerializeField] private float orbitSpeed = 0.3f;
    [SerializeField] private float floatAmplitude = 0.05f;
    [SerializeField] private float floatFrequency = 0.5f;
    [SerializeField] private float spiralIntensity = 0.1f;

    [Header("Visual Settings")]
    [SerializeField] private Color particleColor = new Color(1f, 0.5f, 0.1f, 0.8f); // Rubber-duck orange
    [SerializeField] private float emissionIntensity = 2f;
    [SerializeField] private float pulseSpeed = 1f;
    [SerializeField] private float pulseIntensity = 0.3f;

    [Header("Prefab")]
    [SerializeField] private GameObject particlePrefab;

    private List<ParticleOrbit> particles = new List<ParticleOrbit>();
    private float globalTime = 0f;

    private class ParticleOrbit
    {
        public GameObject gameObject;
        public float radius;
        public float angle;
        public float height;
        public float speed;
        public float floatOffset;
        public float size;
        public Material material;
    }

    void Start()
    {
        CreateParticles();
    }

    void CreateParticles()
    {
        // Create a simple particle prefab if none is assigned
        GameObject templatePrefab = null;
        if (particlePrefab == null)
        {
            templatePrefab = CreateParticlePrefab();
        }

        for (int i = 0; i < particleCount; i++)
        {
            GameObject particle = Instantiate(particlePrefab ?? templatePrefab, transform);

            ParticleOrbit orbitData = new ParticleOrbit
            {
                gameObject = particle,
                radius = orbitRadius + Random.Range(-orbitRadiusVariation, orbitRadiusVariation),
                angle = (360f / particleCount) * i + Random.Range(-15f, 15f),
                height = Random.Range(-0.1f, 0.1f),
                speed = orbitSpeed * Random.Range(0.7f, 1.3f),
                floatOffset = Random.Range(0f, Mathf.PI * 2),
                size = particleSize + Random.Range(-particleSizeVariation, particleSizeVariation)
            };

            // Set particle size
            particle.transform.localScale = Vector3.one * orbitData.size;

            // Get and configure material
            MeshRenderer renderer = particle.GetComponent<MeshRenderer>();
            if (renderer != null)
            {
                orbitData.material = renderer.material;
                orbitData.material.EnableKeyword("_EMISSION");
                Color emissive = particleColor * emissionIntensity;
                orbitData.material.SetColor("_EmissionColor", emissive);
            }

            particles.Add(orbitData);
        }

        // Destroy the template prefab after we're done with it
        if (templatePrefab != null)
        {
            DestroyImmediate(templatePrefab);
        }
    }

    GameObject CreateParticlePrefab()
    {
        GameObject prefab = GameObject.CreatePrimitive(PrimitiveType.Sphere);

        // Remove collider
        Collider collider = prefab.GetComponent<Collider>();
        if (collider != null) DestroyImmediate(collider);

        // Configure renderer
        MeshRenderer renderer = prefab.GetComponent<MeshRenderer>();
        if (renderer != null)
        {
            renderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
            renderer.receiveShadows = false;

            // Create a simple emissive material
            Material mat = new Material(Shader.Find("Universal Render Pipeline/Lit"));
            mat.SetFloat("_Surface", 1); // Transparent
            mat.SetFloat("_Blend", 0);
            mat.SetFloat("_SrcBlend", 5);
            mat.SetFloat("_DstBlend", 10);
            mat.SetFloat("_ZWrite", 0);
            mat.SetFloat("_AlphaClip", 0);
            mat.EnableKeyword("_SURFACE_TYPE_TRANSPARENT");
            mat.EnableKeyword("_EMISSION");
            mat.SetColor("_BaseColor", particleColor);
            mat.SetColor("_EmissionColor", particleColor * emissionIntensity);
            mat.renderQueue = 3000;

            renderer.material = mat;
        }

        prefab.name = "EtherealParticle";
        return prefab;
    }

    void Update()
    {
        globalTime += Time.deltaTime;

        foreach (var particle in particles)
        {
            // Update angle for orbit
            particle.angle += particle.speed * Time.deltaTime * 50f;

            // Calculate position with spiral motion
            float spiralOffset = Mathf.Sin(globalTime * spiralIntensity) * 0.05f;
            float currentRadius = particle.radius + spiralOffset;

            // Calculate float motion (vertical bobbing)
            float floatY = Mathf.Sin(globalTime * floatFrequency + particle.floatOffset) * floatAmplitude;

            // Apply final position
            float x = Mathf.Cos(particle.angle * Mathf.Deg2Rad) * currentRadius;
            float z = Mathf.Sin(particle.angle * Mathf.Deg2Rad) * currentRadius;
            float y = particle.height + floatY;

            particle.gameObject.transform.localPosition = new Vector3(x, y, z);

            // Pulse the emission
            if (particle.material != null)
            {
                float pulse = 1f + Mathf.Sin(globalTime * pulseSpeed + particle.floatOffset) * pulseIntensity;
                Color emissive = particleColor * emissionIntensity * pulse;
                particle.material.SetColor("_EmissionColor", emissive);

                // Slight transparency pulse
                Color baseColor = particleColor;
                baseColor.a = 0.6f + Mathf.Sin(globalTime * pulseSpeed * 0.7f + particle.floatOffset) * 0.2f;
                particle.material.SetColor("_BaseColor", baseColor);
            }

            // Gentle size pulse
            float sizePulse = 1f + Mathf.Sin(globalTime * pulseSpeed * 0.5f + particle.floatOffset) * 0.1f;
            particle.gameObject.transform.localScale = Vector3.one * particle.size * sizePulse;
        }
    }

    void OnDestroy()
    {
        // Clean up materials
        foreach (var particle in particles)
        {
            if (particle.material != null)
            {
                Destroy(particle.material);
            }
        }
    }
}