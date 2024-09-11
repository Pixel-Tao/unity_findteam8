using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CursorRenderer : MonoBehaviour
{
    Image CursorImage;
    
    private void Awake()
    {
        Cursor.visible = false;
    }
    void Start()
    {
        CursorImage = GetComponent<Image>();
    }

    void Update()
    {
        this.transform.position = Input.mousePosition;
    }
}
