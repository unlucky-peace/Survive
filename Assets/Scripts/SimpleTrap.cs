using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleTrap : MonoBehaviour
{
    private Rigidbody[] rigid;
    [SerializeField] private GameObject goMeat;

    [SerializeField] private int damage;
    private bool isActivated = false;

    private const string TRAP_SOUND = "Trap_activate";
    
    // Start is called before the first frame update
    void Start()
    {
        rigid = GetComponentsInChildren<Rigidbody>();
    }

    private void OnTriggerEnter(Collider col)
    {
        if (!col.transform.CompareTag("Untagged"))
        {
            isActivated = true;
            SoundManager.instance.PlaySE(TRAP_SOUND);
            Destroy(goMeat);
            for (int i = 0; i < rigid.Length; i++)
            {
                rigid[i].useGravity = true;
                rigid[i].isKinematic = false;
            }

            if (col.transform.CompareTag("Player"))
            {
               col.transform.GetComponent<PlayerController>()._statusController.DecHP(damage);
            }
        }
    }
}
