using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{

    [SerializeField] float speed = 10f;
    Vector3 mousePos, transPos, targetPos;
    void Update() //마우스 클릭시 마우스 좌표값으로 이동
    {
        if (Input.GetMouseButtonDown(0))
        {
            MousePosition();
        }
        MoveToMosePosition();
    }

    void MousePosition() //마우스 좌표값 
    {
        mousePos = Input.mousePosition;
        transPos = Camera.main.ScreenToWorldPoint(mousePos);
        targetPos = new Vector3(transPos.x, transPos.y, 0);
    }

    void MoveToMosePosition() //마우스 좌표값으로 이동
    {
        if (Vector3.Distance(transform.position, targetPos) < 0.5f)
            return;
        transform.position = Vector3.MoveTowards(transform.position, targetPos, Time.deltaTime * speed);
    }
}
