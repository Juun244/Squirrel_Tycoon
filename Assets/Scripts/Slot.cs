using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    public Item item;
    public Image itemImage;

    public void UpdateSlotUI()
    {
        itemImage.sprite = item.itemImage;
        itemImage.gameObject.SetActive(true);
    }
    public void RemoveSlot()
    {
        item = null;
        itemImage.gameObject.SetActive(false);
    }
}
