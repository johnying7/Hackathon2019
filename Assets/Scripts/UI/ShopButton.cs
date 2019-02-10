using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopButton : MonoBehaviour
{
    public Button buttonComponent;
    public Text nameLabel;
    public Image iconImage;
    public Text priceText;
    public GameObject prefab;
    
    private Item item;
    private ScrollList scrollList;
    
    // Use this for initialization
    void Start () 
    {
        buttonComponent.onClick.AddListener (HandleClick);
    }
    
    public void Setup(Item currentItem, ScrollList currentScrollList)
    {
        item = currentItem;
        nameLabel.text = item.itemName;
        iconImage.sprite = item.icon;
        priceText.text = "$" + item.price.ToString ();
        prefab = item.prefab;
        scrollList = currentScrollList;
    }
    
    public void HandleClick()
    {
        scrollList.SelectedObject (item);
    }
}
