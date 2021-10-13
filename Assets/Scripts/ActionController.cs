using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionController : MonoBehaviour
{
    [SerializeField] private float range; //습득 가능 범위

    private bool pickupActivated; //습득 가능할 시 true
    
    private RaycastHit hit; //충돌체 정보
    
    [SerializeField] private LayerMask layerMask; //아이템 레이어에만 반응하도록
    [SerializeField] private Text actionText;
    [SerializeField] private Inventory inventory;
    
    
    void Update()
    {
        CheckItem();
        TryAction();
    }

    private void TryAction()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            CheckItem();
            CanPickUp();
        }
    }

    private void CanPickUp()
    {
        if (pickupActivated)
        {
            if (hit.transform != null)
            {
                Debug.Log(hit.transform.GetComponent<ItemPickUp>().item.itemName + "획득");
                inventory.AcquireItem(hit.transform.GetComponent<ItemPickUp>().item);
                Destroy(hit.transform.gameObject);
                ItemInfoDisappear();
            }
        }
    }

    private void CheckItem()
    {
        if (Physics.Raycast(transform.position, transform.forward, out hit, range, layerMask))
        {
            if (hit.transform.CompareTag("Item"))
            {
                ItemInfoAppear();
            }
        }
        else ItemInfoDisappear();
    }

    private void ItemInfoDisappear()
    {
        pickupActivated = false;
        actionText.gameObject.SetActive(false);
        actionText.text = "";
    }

    private void ItemInfoAppear()
    {
        pickupActivated = true;
        actionText.gameObject.SetActive(true);
        actionText.text = hit.transform.GetComponent<ItemPickUp>().item.itemName + " 획득 " + "<color=#FFE400>[E]</color>";
    }
}
