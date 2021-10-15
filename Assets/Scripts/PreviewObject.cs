using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreviewObject : MonoBehaviour
{
    private List<Collider> _colliderList = new List<Collider>();

    [SerializeField] private int layerGround;
    private const int IGNORE_RAYCAST_LAYER = 2;

    [SerializeField] private Material green;
    [SerializeField] private Material red;
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        ChangeColor();
    }

    private void ChangeColor()
    {
        if (_colliderList.Count > 0) SetColor(red);
        else SetColor(green);
    }

    private void SetColor(Material mat)
    {
        foreach (Transform tfChild in this.transform)
        {
            var newMaterials = new Material[tfChild.GetComponent<Renderer>().materials.Length];
            for (int i = 0; i < newMaterials.Length; i++)
            {
                newMaterials[i] = mat;
            }

            tfChild.GetComponent<Renderer>().materials = newMaterials;
        }
    }

    private void OnTriggerEnter(Collider col)
    {
        if(col.gameObject.layer != layerGround && col.gameObject.layer != IGNORE_RAYCAST_LAYER) _colliderList.Add(col);
    }
    
    private void OnTriggerExit(Collider col)
    {
        if(col.gameObject.layer != layerGround && col.gameObject.layer != IGNORE_RAYCAST_LAYER) _colliderList.Remove(col);
    }
    
    public bool isBuildable()
    {
        return _colliderList.Count == 0;
    }
}
