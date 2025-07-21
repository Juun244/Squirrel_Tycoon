using UnityEngine;

public class Inventory : MonoBehaviour
{
    //인벤토리를 하나만 유지하고, 전역에서 Inventory.instance로 접근
    #region Singleton
    public static Inventory instance;
    private void Awake()
    {
        if(instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }
    #endregion

    public delegate void OnSlotCountChange(int val);
    public OnSlotCountChange onSlotCountChange;

    //슬롯 수가 변경되면 자동으로 delegate 실행
    private int slotCnt;
    public int SlotCnt
    {
        get { return slotCnt; }
        set
        {
            slotCnt = value;
            onSlotCountChange.Invoke(slotCnt);
        }
    }

    void Start()
    {
        //초기 인벤토리의 슬롯을 4로 초기화
        SlotCnt = 4;
    }


    void Update()
    {
        
    }
}
