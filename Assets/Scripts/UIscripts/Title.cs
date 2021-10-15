using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Title : MonoBehaviour
{
    public static Title instance;
    
    public string scenename = "GameStage";

    private SaveLoad _saveLoad;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(this.gameObject);
    }

    public void ClickStart()
    {
        SceneManager.LoadScene(scenename);
    }

    public void ClickLoad()
    {
        Debug.Log("로드");
        StartCoroutine(LoadCoroutine());
    }

    private IEnumerator LoadCoroutine()
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(scenename);
        while (!operation.isDone)
        {
            //로딩화면 만들기 가능
            //operation.progress
            yield return null;
        }
        _saveLoad = FindObjectOfType<SaveLoad>();
        _saveLoad.LoadData();
        gameObject.SetActive(false);
    }

    public void ClickExit()
    {
        Debug.Log("게임 종료");
        Application.Quit();
    }
}
