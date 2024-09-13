using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 파티클 이펙트 타입
/// </summary>
public enum ParticleEffectType
{
    None,
    Hit,
    Nice,
    Miss
}

public class ParticleEffect : MonoBehaviour
{
    /// <summary>
    /// 객체별 파티클 이펙트 타입
    /// </summary>
    public ParticleEffectType type;
    /// <summary>
    /// 파티클 유지 시간
    /// </summary>
    float time = 0.0f;
    /// <summary>
    /// 파티클을 삭제하는 시간
    /// </summary>
    public float destroyTime = 1.0f;
    
    void Start()
    {
        // 파티클을 생성한 시간을 초기화
        time = 0;
    }

    // Update is called once per frame
    void Update()
    {
        // 유지 시간을 업데이트
        time += Time.deltaTime;
        // 유지 시간이 파티클을 삭제하는 시간보다 크다면
        if (time > destroyTime)
        {
            // 유지시간을 0으로 초기화
            // 0으로 초기화 하는 이유는 객체를 다시 생성하게 아니라서
            // Awake, Start 함수가 호출되지 않기 때문이다.
            // SpawningPool 에서 관리되는 객체는 항상 직접 초기화 해줘야 한다.
            time = 0; 
            // 파티클을 숨긴다.
            GameManager.Instance.HideParticle(gameObject);
        }
    }
}
