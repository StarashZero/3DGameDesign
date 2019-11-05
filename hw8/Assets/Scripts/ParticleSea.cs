using UnityEngine;
using System.Collections;

public class ParticleSea : MonoBehaviour
{

    ParticleSystem particleSystem;
    ParticleSystem.Particle[] particlesArray;

    public float spacing = 1;
    public int seaResolution = 100;
    public float noiseScale = 0.1f;
    public float heightScale = 4f;
    float perlinNoiseAnimX = 0.01f;
    float perlinNoiseAnimY = 0.01f;
    public Gradient colorGradient;

    void Start()
    {
        particleSystem = gameObject.GetComponent<ParticleSystem>();
        particlesArray = new ParticleSystem.Particle[seaResolution * seaResolution];
        particleSystem.maxParticles = seaResolution * seaResolution;
        particleSystem.Emit(seaResolution * seaResolution);
        particleSystem.GetParticles(particlesArray);
    }

    private void Update()
    {
        for (int i = 0; i < seaResolution; i++)
        {
            for (int j = 0; j < seaResolution; j++)
            {
                float zPos = Mathf.PerlinNoise(i * noiseScale + perlinNoiseAnimX, j * noiseScale + perlinNoiseAnimY);
                particlesArray[i * seaResolution + j].startColor = colorGradient.Evaluate(zPos);
                particlesArray[i * seaResolution + j].position = new Vector3(i * spacing, zPos * heightScale, j * spacing);
            }
        }

        perlinNoiseAnimX += 0.01f;
        perlinNoiseAnimY += 0.01f;

        particleSystem.SetParticles(particlesArray, particlesArray.Length);
    }

}