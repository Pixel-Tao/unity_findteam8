using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum ParticleEffectType
{
    None,
    Hit,
    Nice,
    Miss
}

public class ParticleEffect : MonoBehaviour
{
    public ParticleEffectType type;

    float time = 0.0f;
    public float destroyTime = 1.0f;
    // Start is called before the first frame update
    void Start()
    {
        time = 0;
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        if (time > destroyTime)
        {
            time = 0;
            GameManager.Instance.HideParticle(gameObject);
        }
    }
}
