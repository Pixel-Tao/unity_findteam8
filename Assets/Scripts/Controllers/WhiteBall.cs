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
        // 마우스 좌클릭 시
        if(Input.GetMouseButtonDown(0))
        {
            // 화면의 위치를 게임 세상의 좌표로 변환하고
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            
            // Raycast를 통해 마우스 위치에 무엇이 있는지 확인
            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);
            // 마우스 위치에 아무것도 없다면 리턴
            if(!hit || hit.rigidbody == null) return;
            // 흰공이 아닌 다른 공에 충돌했다면 리턴
            if (hit.rigidbody.gameObject.name != "WhiteBall")
                return;
            // 충돌 이펙트 생성
            GameManager.Instance.ShowParticle(ParticleEffectType.Hit, mousePos);
            // 마우스 위치와 흰공의 위치를 빼서 방향을 구하고
            Vector3 dir = (transform.position - mousePos).normalized;
            // 힘을 가해준다.
            GetComponent<Rigidbody2D>().AddForce(dir * 600, ForceMode2D.Impulse);
        }
    }
    /// <summary>
    /// Rigidbody2D가 다른 Collider2D와 충돌했을 때 발생하는 이벤트
    /// </summary>
    /// <param name="collision">나 자신이 아닌 다른 Collider2D 의 Collision2D</param>
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // 충돌한 오브젝트가 Card 컴포넌트를 가져오고
        Card card = collision.gameObject.GetComponent<Card>();
        // Card 컴포넌트를 가지고 있고, 카드가 열려있지 않은 상태라면
        if (card != null && card.isOpen == false)
        {
            // 카드를 여는 함수를 실행
            card.OpenCard();
        }
    }
}
