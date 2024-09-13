using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CursorRenderer : MonoBehaviour
{
    /// <summary>
    /// 커서 이미지
    /// </summary>
    Image CursorImage;
    
    private void Awake()
    {
        Cursor.visible = false;
    }
    private void Start()
    {
        // 이미지 컴포넌트를 가져온다.
        CursorImage = GetComponent<Image>();
    }

    private void Update()
    {
        // 마우스 위치가 null이면 아무런 행위를 하지 않는다.
        if (Input.mousePosition == null) return;

        // 이미지를 마우스 커서 위치로 이동시킨다.
        this.transform.position = Input.mousePosition;
    }
}
