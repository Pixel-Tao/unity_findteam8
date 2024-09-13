using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    /// <summary>
    /// 공이 움직이는 속도
    /// </summary>
    [SerializeField] 
    float speed = 10f;

    //Vector3 mousePos;
    //Vector3 transPos;
    Vector3 targetPos;

    void Update() 
    {
        //마우스 클릭시 마우스 좌표값으로 이동
        if (Input.GetMouseButtonDown(0))
        {
            targetPos = GetMousePosition();
        }
        MoveToMosePosition();
    }
    /// <summary>
    /// 전역변수에 값을 할당해주는 함수
    /// </summary>
    Vector3 GetMousePosition() 
    {
        // 전역변수는 최소하는 것이 좋습니다.

        //mousePos = Input.mousePosition;
        //마우스 좌표값을 게임 세상의 좌표로 변환
        Vector3 transPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        return new Vector3(transPos.x, transPos.y, 0);
    }

    void MoveToMosePosition() //마우스 좌표값으로 이동
    {
        // 대상 위치와 현재 위치가 0.5f보다 작다면 움직이지 않는다.(즉 거의 다 왔다면)
        if (Vector3.Distance(transform.position, targetPos) < 0.5f)
            return;

        // 대상 위치로 이동
        transform.position = Vector3.MoveTowards(transform.position, targetPos, Time.deltaTime * speed);
    }
}
