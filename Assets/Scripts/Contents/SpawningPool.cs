using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

/// <summary>
/// GameObject Pooling 클래스
/// </summary>
class Pool
{
    /// <summary>
    /// 대상 프리팹
    /// </summary>
    GameObject prefab;
    /// <summary>
    /// 풀링되는 
    /// </summary>
    IObjectPool<GameObject> pool;
    /// <summary>
    /// 부모 Transform
    /// </summary>
    Transform root;
    /// <summary>
    /// 생성자
    /// </summary>
    /// <param name="prefab"></param>
    /// <param name="root"></param>
    public Pool(GameObject prefab, Transform root)
    {
        this.prefab = prefab;
        this.root = root;

        pool = new ObjectPool<GameObject>(OnCreate, OnGet, OnRelease, OnDestroy);
    }
    /// <summary>
    /// 외부에서 GameObject를 사용하지 않을때 사용    
    /// </summary>
    /// <param name="go"></param>
    public void Push(GameObject go)
    {
        pool.Release(go);
    }
    /// <summary>
    /// Pooling 된 GameObject를 반환한다.
    /// </summary>
    /// <returns></returns>
    public GameObject Pop()
    {
        return pool.Get();
    }
    /// <summary>
    /// ObjectPool이 인스턴스 화 할때 Pooling 대상이 없다면 프리팹으로 Instantiate 하고 반환한다.
    /// </summary>
    /// <returns></returns>
    GameObject OnCreate()
    {
        GameObject go = GameObject.Instantiate(prefab);
        go.transform.SetParent(root);
        go.name = prefab.name;
        return go;
    }
    /// <summary>
    /// Pooling 대상이 있다면 가져와서 SetActive(true)로 활성화한다.
    /// </summary>
    /// <param name="go"></param>
    void OnGet(GameObject go)
    {
        go.SetActive(true);
    }

    /// <summary>
    /// Pooling 대상을 SetActive(false)로 비활성화한다.
    /// </summary>
    /// <param name="go"></param>
    void OnRelease(GameObject go)
    {
        go.SetActive(false);
    }
    /// <summary>
    /// Pooling 대상을 Destroy 한다. (실제 Destory가 발생한다.)
    /// </summary>
    /// <param name="go"></param>
    void OnDestroy(GameObject go)
    {
        GameObject.Destroy(go);
    }
}

/// <summary>
/// SpawningPool 클래스
/// SpawningPool 을 사용하는 이유
/// 1. Instantiate, Destroy 를 사용하지 않고 Pooling 을 사용하여 성능을 향상시킨다.
/// 2. Pooling 을 사용하여 메모리를 효율적으로 사용한다.
/// 3. Pooling 을 사용하여 게임 오브젝트를 재사용한다.
/// </summary>
public class SpawningPool : MonoBehaviour
{
    /// <summary>
    /// Pooling 할 대상 GameObject
    /// </summary>
    Dictionary<string, Pool> pools = new Dictionary<string, Pool>();

    [Header("풀링할 프리팹")]
    public GameObject hitPrefab;
    public GameObject nicePrefab;
    public GameObject missPrefab;

    /// <summary>
    /// Pool 에서 GameObject 를 생성하고 위치를 설정한다.
    /// </summary>
    /// <param name="pos">생성 할 위치</param>
    public void Spawn(string key, Vector3 pos)
    {
        if (pools.TryGetValue(key, out Pool pool) == false)
        {
            // 처음 Pool을 생성한다.
            // SpawningPool GameObject 하위에 GameObject를 생성할 수 있게 된다.
            if (key == ParticleEffectType.Hit.ToString())
            {
                pool = new Pool(hitPrefab, transform);
            }
            else if (key == ParticleEffectType.Nice.ToString())
            {
                pool = new Pool(nicePrefab, transform);
            }
            else if (key == ParticleEffectType.Miss.ToString())
            {
                pool = new Pool(missPrefab, transform);
            }
            pools.Add(key, pool);
        }

        GameObject hit = pool.Pop();
        hit.transform.position = pos;
    }

    /// <summary>
    /// Pool 에 GameObject 를 반환한다.
    /// Destory를 사용하지 않는다.
    /// </summary>
    /// <param name="go"></param>
    public void Despawn(GameObject go)
    {
        ParticleEffect pe = go.GetComponent<ParticleEffect>();
        if (pe == null)
        {
            Debug.LogError("Not Found ParticleEffect");
            return;
        }

        pools[pe.type.ToString()].Push(go);
    }

    void Start()
    {
    }

    void Update()
    {

    }
}
