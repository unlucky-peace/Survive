using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class WeaponSway : MonoBehaviour
{
    private Vector3 origPos; // 기존 위치
    private Vector3 currPos; // 현재 위치

    [SerializeField] private Vector3 limitPos; //sway한계
    [SerializeField] private Vector3 fsLimitPos; //정조준 sway 한계
    [SerializeField] private Vector3 smoothSway; //부드러운 움직임 정도

    [SerializeField] private GunController _gunController;

    void Start()
    {
        origPos = this.transform.localPosition;
    }


    void Update()
    {
        if (!Inventory.inventoryActivated)
        {
           TrySway();
        }
    }

    private void TrySway()
    {
        if (Input.GetAxisRaw("Mouse X") != 0 || Input.GetAxisRaw("Mouse Y") != 0) Swaying();
        else BackToOriginPos();
    }

    private void BackToOriginPos()
    {
        currPos = Vector3.Lerp(currPos, origPos, smoothSway.x);
        transform.localPosition = currPos;
    }

    private void Swaying()
    {
        float _moveX = Input.GetAxisRaw("Mouse X");
        float _moveY = Input.GetAxisRaw("Mouse Y");

        if (_gunController.GetFineSightMode())
        {
            currPos.Set(Mathf.Clamp(Mathf.Lerp(currPos.x,-_moveX, smoothSway.x),-fsLimitPos.x, fsLimitPos.x),
                Mathf.Clamp(Mathf.Lerp(currPos.y,-_moveY, smoothSway.x), -fsLimitPos.y, fsLimitPos.y), origPos.z);
            transform.localPosition = currPos;
        }
        else
        {
            currPos.Set(Mathf.Clamp(Mathf.Lerp(currPos.x,-_moveX, smoothSway.y),-limitPos.x, limitPos.x),
                Mathf.Clamp(Mathf.Lerp(currPos.y,-_moveY, smoothSway.y), -limitPos.y, limitPos.y), origPos.z);
            transform.localPosition = currPos;
        }

    }
}
