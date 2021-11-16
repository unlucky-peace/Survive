using UnityEngine;

[System.Serializable]
public class Craft
{
    public string craftName; // 이름
    public GameObject goPrefab; // 실제 설치될 프리팹
    public GameObject goPreviewPrefab; //프리뷰 프리팹
}
public class CraftManual : MonoBehaviour
{
    private bool _isPreviewActivated = false;

    private RaycastHit hit;
    [SerializeField] private LayerMask _layerMask;
    [SerializeField] private float range;
    
    [SerializeField] private GameObject goBaseUI;
    [SerializeField] private Craft[] craftFire; //모닥불용 탭

    private GameObject goPreview; //미리 보기 프리팹을 담을 변수
    private GameObject goPrefab;
    [SerializeField] private Transform tfPlayer;

    public void SlotClick(int _slotNumber)
    {
        goPreview = Instantiate(craftFire[_slotNumber].goPreviewPrefab, tfPlayer.position + tfPlayer.forward,
            Quaternion.identity);
        goPrefab = craftFire[_slotNumber].goPrefab;
        _isPreviewActivated = true;
        CloseWindow();
    }
    
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab) && !_isPreviewActivated)
        {
            CraftWindow();
            Debug.Log("탭 호출");
        }
        
        if (_isPreviewActivated) PreviewPositionUpdate();
        
        if(Input.GetButtonDown("Fire1"))
        {
            Build();
        }
        
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cancel();
        }
    }

    private void Build()
    {
        if (_isPreviewActivated && goPreview.GetComponent<PreviewObject>().isBuildable())
        {
            Instantiate(goPrefab, hit.point, Quaternion.identity);
            Destroy(goPreview);
            Reset();
        }
    }

    private void Cancel()
    {
        if(_isPreviewActivated) Destroy(goPreview);
        Reset();
        CloseWindow();
    }

    private void CraftWindow()
    {
        Debug.Log("CraftWindow()");
        if (!GameManager.isOpenCraftManual) OpenWindow();
        else CloseWindow();
    }

    private void CloseWindow()
    {
        Debug.Log("CloseWindow()");
        GameManager.isOpenCraftManual = false;
        goBaseUI.SetActive(false);
    }

    private void OpenWindow()
    {
        Debug.Log("OpenWindow()");
        GameManager.isOpenCraftManual = true;
        goBaseUI.SetActive(true);
    }
    
    private void PreviewPositionUpdate()
    {
        if (Physics.Raycast(tfPlayer.position, tfPlayer.forward, out hit, range, _layerMask))
        {
            if (hit.transform != null)
            {
                Vector3 _location = hit.point;
                goPreview.transform.position = _location;
            }
        }
    }

    private void Reset()
    {
        GameManager.isOpenCraftManual = false;
        _isPreviewActivated = false;
        goPreview = null;
        goPrefab = null;
    }
}
