using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Item
{
    public string itemName;
    public Sprite icon;
    public float price = 1;
    public GameObject prefab;
}

public class ScrollList : MonoBehaviour {

    public List<Item> itemList;
    public Transform contentPanel;
    public Text cash_display;
    public Text selected_text;
    public ObjectPool object_pool;
    public ObjectPlacer objPlacer;
    
    //public float cash = 1000f;
    public GameObject cur_obj = null;

    public GameManager gM;

    // Use this for initialization
    void Start () 
    {
        objPlacer.gM = gM;
        objPlacer.scrollList = this;
        RefreshDisplay ();
    }

    public void RefreshDisplay()
    {
        cash_display.text = "$" + gM.currentAmount.ToString ();
        RemoveButtons ();
        AddButtons ();
    }

    private void RemoveButtons()
    {
        while (contentPanel.childCount > 0) 
        {
            GameObject toRemove = transform.GetChild(0).gameObject;
            object_pool.ReturnObject(toRemove);
        }
    }

    private void AddButtons()
    {
        for (int i = 0; i < itemList.Count; i++) 
        {
            Item item = itemList[i];
            GameObject newButton = object_pool.GetObject();
            newButton.transform.SetParent(contentPanel, false);

            ShopButton shop_button = newButton.GetComponent<ShopButton>();
            shop_button.Setup(item, this);
        }
    }

    void AddItem(Item itemToAdd, ScrollList shopList)
    {
        shopList.itemList.Add (itemToAdd);
    }

    private void RemoveItem(Item itemToRemove, ScrollList shopList)
    {
        for (int i = shopList.itemList.Count - 1; i >= 0; i--) 
        {
            if (shopList.itemList[i] == itemToRemove)
            {
                shopList.itemList.RemoveAt(i);
            }
        }
    }

    public void SelectedObject(Item item) {
        cur_obj = item.prefab;
        selected_text.text = item.itemName;
        objPlacer.roomCost = item.price;
    }

    public void PurchaseAndPlace() {
        objPlacer.SetPlacementObject(cur_obj);
    }
}
