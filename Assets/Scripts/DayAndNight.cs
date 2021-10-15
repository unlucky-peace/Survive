using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayAndNight : MonoBehaviour
{
    [SerializeField] private float secondPerRealTime;

    [SerializeField] private float deltaFogDensity; //증감량 
    [SerializeField] private float nightFogDensity; //저녁 시간대 fogdensity
    private float dayFogDensity; //낮 시간대 fogdensity
    private float curFogDensity;

    void Start()
    {
        dayFogDensity = RenderSettings.fogDensity;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.right * (0.1f * secondPerRealTime) * Time.deltaTime);
        if (transform.rotation.eulerAngles.x >= 170) GameManager.isNight = true;
        else if (transform.root.eulerAngles.x <= 340) GameManager.isNight = false;

        if (GameManager.isNight)
        {
            if (curFogDensity <= nightFogDensity)
            {
                curFogDensity += 0.1f * deltaFogDensity * Time.deltaTime;
                RenderSettings.fogDensity = curFogDensity;
            }
        }
        else
        {
            if (curFogDensity >= nightFogDensity)
            {
                curFogDensity -= 0.1f * deltaFogDensity * Time.deltaTime;
                RenderSettings.fogDensity = curFogDensity;
            }
        }
    }
}
