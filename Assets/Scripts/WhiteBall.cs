using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class WhiteBall : MonoBehaviour
{
    void Start()
    {
    }

    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            
            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);
            if(!hit) return;
            if (hit.rigidbody.gameObject.name != "WhiteBall")
                return;
            GameManager.Instance.ShowParticle(ParticleEffectType.Hit, mousePos);
            Vector3 dir = (transform.position - mousePos).normalized;

            GetComponent<Rigidbody2D>().AddForce(dir * 600, ForceMode2D.Impulse);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Card card = collision.gameObject.GetComponent<Card>();
        if(card != null && card.isOpen == false)
        {
            card.OpenCard();
        }
    }
}
