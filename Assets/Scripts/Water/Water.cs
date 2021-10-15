using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Water : MonoBehaviour
{
    
    [SerializeField] private float waterDrag;
    private float origDrag;

    [SerializeField] private Color waterColor;
    [SerializeField] private float waterFogDensity;
    private Color origColor;
    private float origFogDensity;
    
    [SerializeField] private Color waterNightColor;
    [SerializeField] private float waterNightFogDensity;
    [SerializeField] private Color origNightColor;
    [SerializeField] private float origNightFogDensity;

    [SerializeField] private string soundWaterOut;
    [SerializeField] private string soundWaterIn;
    [SerializeField] private string soundBreathe;

    [SerializeField] private float breatheTime;
    private float curBreatheTime = 0;

    [SerializeField] private float totalOxygen;
    private float curOxy;
    private float curTemp;

    [SerializeField] private GameObject goBaseUI;
    [SerializeField] private Text totalOxyText;
    [SerializeField] private Text curOxyText;
    [SerializeField] private Image gaugeImg;

    private StatusController _statusController;

    void Start()
    {
        origColor = RenderSettings.fogColor;
        origFogDensity = RenderSettings.fogDensity;

        origDrag = 0;
        _statusController = FindObjectOfType<StatusController>();
        curOxy = totalOxygen;
        totalOxyText.text = totalOxygen.ToString();
    }

    private void Update()
    {
        if (GameManager.isWater)
        {
            curBreatheTime += Time.deltaTime;
            if (curBreatheTime > breatheTime)
            {
                SoundManager.instance.PlaySE(soundBreathe);
                curBreatheTime = 0;
            }

            DecreaseOxygen();
        }
    }

    private void DecreaseOxygen()
    {
        curOxy -= Time.deltaTime;
        curOxyText.text = Mathf.RoundToInt(curOxy).ToString();
        gaugeImg.fillAmount = curOxy / totalOxygen;
        if (curOxy <= 0)
        {
            curTemp += Time.deltaTime;
            if (curTemp >= 1)
            {
                _statusController.DecHP(1);
                curTemp = 0;
            }
        }
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.transform.CompareTag("Player"))
        {
            GetWater(col);
        }
    }

    private void GetWater(Collider _player)
    {
        GameManager.isWater = true;
        _player.transform.GetComponent<Rigidbody>().drag = waterDrag;
        SoundManager.instance.PlaySE(soundWaterIn);
        goBaseUI.SetActive(true);

        if (!GameManager.isNight)
        {
            RenderSettings.fogColor = waterColor;
            RenderSettings.fogDensity = waterFogDensity;
        }
        else
        {
            RenderSettings.fogColor = waterNightColor;
            RenderSettings.fogDensity = waterNightFogDensity;
        }
    }

    private void OnTriggerExit(Collider col)
    {
        if (col.transform.CompareTag("Player"))
        {
            GetOutWater(col);
        }
    }

    private void GetOutWater(Collider _player)
    {
        if (GameManager.isWater)
        {
            curOxy = totalOxygen;
            GameManager.isWater = false;
            goBaseUI.SetActive(false);
            _player.transform.GetComponent<Rigidbody>().drag = origDrag;
            SoundManager.instance.PlaySE(soundWaterOut);

            if (!GameManager.isNight)
            {
                RenderSettings.fogColor = origColor;
                RenderSettings.fogDensity = origFogDensity;
            }
            else
            {
                RenderSettings.fogColor = origNightColor;
                RenderSettings.fogDensity = origFogDensity;
            }
        }
    }
}
