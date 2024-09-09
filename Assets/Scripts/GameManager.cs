using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        SoundManager.inst.BSound(AudioType.BGM);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
            SoundManager.inst.ESound(AudioType.Ball01);

        if (Input.GetKeyDown(KeyCode.Alpha2))
            SoundManager.inst.ESound(AudioType.Ball02);

        if (Input.GetKeyDown(KeyCode.Alpha3))
            SoundManager.inst.ESound(AudioType.Ball03);

        if (Input.GetKeyDown(KeyCode.Alpha4))
            SoundManager.inst.ESound(AudioType.Click);

        if (Input.GetKeyDown(KeyCode.Alpha5))
            SoundManager.inst.ESound(AudioType.Win);

        if (Input.GetKeyDown(KeyCode.Alpha6))
            SoundManager.inst.ESound(AudioType.Defeat);

        if (Input.GetKeyDown(KeyCode.Alpha0))
            SoundManager.inst.BSound(AudioType.None);
    }
}
