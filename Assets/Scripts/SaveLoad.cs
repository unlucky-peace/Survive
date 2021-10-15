using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    public Vector3 playerPos;
    public Vector3 playerRot;
    public List<string> invenItemName = new List<string>();
    public List<int> invenArrayNum = new List<int>();
    public List<int> invenItemNum = new List<int>();
}
public class SaveLoad : MonoBehaviour
{
    private SaveData _saveData = new SaveData();
    
    private string SAVE_DATA_DIRECTORY;
    private string SAVE_FILENAME = "/SaveFile.txt";

    private PlayerController _player;
    private Inventory _inventory;
    void Start()
    {
        SAVE_DATA_DIRECTORY = Application.dataPath + "/Saves/";
        if (!Directory.Exists(SAVE_DATA_DIRECTORY)) Directory.CreateDirectory(SAVE_DATA_DIRECTORY);
        
    }

    public void SaveData()
    {
        _player = FindObjectOfType<PlayerController>();
        _inventory = FindObjectOfType<Inventory>();
        _saveData.playerPos = _player.transform.position;
        _saveData.playerRot = _player.transform.eulerAngles;

        Slot[] slots = _inventory.GetSlots();
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item != null)
            {
                _saveData.invenArrayNum.Add(i);
                _saveData.invenItemName.Add(slots[i].item.itemName);
                _saveData.invenItemNum.Add(slots[i].itemCount);
            }
        }

        string json = JsonUtility.ToJson(_saveData);
        
        File.WriteAllText(SAVE_DATA_DIRECTORY + SAVE_FILENAME, json);
        
        Debug.Log("저장");
        Debug.Log(json);
    }

    public void LoadData()
    {
        if (File.Exists(SAVE_DATA_DIRECTORY + SAVE_FILENAME))
        {
            string loadJson = File.ReadAllText(SAVE_DATA_DIRECTORY + SAVE_FILENAME);
            _saveData = JsonUtility.FromJson<SaveData>(loadJson);

            _player = FindObjectOfType<PlayerController>();
            _player.transform.position = _saveData.playerPos;
            _player.transform.eulerAngles = _saveData.playerRot;

            _inventory = FindObjectOfType<Inventory>();
            for (int i = 0; i < _saveData.invenItemName.Count; i++)
            {
                _inventory.LoadToInven(_saveData.invenArrayNum[i], _saveData.invenItemName[i], _saveData.invenItemNum[i]);
            }
        
            Debug.Log("로드 완료");
        }
    }
}
