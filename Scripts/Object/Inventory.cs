using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    GameObject inventory;
    public GameObject[] buttonsGO;
    Button[] buttons;
    List<Item> items;
    string[] names;
    public static bool open;
    public static Inventory GC;
    GameObject floater;
    bool placing;
    int index;
    public bool usingInventory;

    void Start ()
    {
        GC = GetComponent<Inventory>();
        //buttonsGO = GameObject.FindGameObjectsWithTag("InventoryButton");
        inventory = GameObject.Find("Inventory");
        buttons = new Button[buttonsGO.Length];
        for (int i = 0; i < buttonsGO.Length; i++)
        {
            buttons[i] = buttonsGO[i].GetComponent<Button>();
        }
        
        items = new List<Item>();
        Hide();
    }
	
	void Update ()
    {
        if(usingInventory)
        Swipe();
        TryDrop();
	}

    public void TryCollect(GameObject objectCollected)
    {
        
        if (items.Count < buttonsGO.Length)
        {
            Item item = new Item();
            item.name = objectCollected.name;
            item.image = objectCollected.GetComponent<SpriteRenderer>().sprite;
            items.Add(item);
            UpdateInventory();
        }
    }

    public void Dropping(int index)
    {
        if (Input.touchCount < 2)
        {
            this.index = index; 
            placing = true;
        }
    }

    void TryDrop()
    {
        GameObject obj;
        try
        {
            if (placing)
            {
                if (floater == null)
                {
                    floater = (GameObject)Instantiate(Resources.Load("Images/" + items[index].name));
                }
                if (floater != null)
                {
                    floater.transform.position = TouchInputs.GC.TapPosition(0);
                }
                if (TouchInputs.GC.TapEnded(0))
                {
                    placing = false;
                    Destroy(floater);
                    obj = (GameObject)Instantiate(Resources.Load("Images/" + items[index].name), TouchInputs.GC.TapPosition(0), Quaternion.identity);
                    obj.name = items[index].name;
                    Remove(index);
                }
            }
        }
        catch
        {

        }
    }

    public void Remove(int index)
    {
        items.RemoveAt(index);
        UpdateInventory();
    }

    public void UpdateInventory()
    {
        for (int i = 0; i < buttonsGO.Length; i++)
        {
            buttonsGO[i].GetComponent<Image>().sprite = null;
            buttons[i].interactable = false;
            buttonsGO[i].transform.Find("Text").GetComponent<Text>().text = "";
            if (i < items.Count)
            {
                buttonsGO[i].GetComponent<Image>().sprite = items[i].image;
                buttons[i].interactable = true;
                buttonsGO[i].transform.Find("Text").GetComponent<Text>().text = items[i].name;
            }
        }
    }

    void Swipe()
    {
        if (TouchInputs.swipedAtBottom)
        {
            Show();
        }
        if (open && TouchInputs.GC.SwipeVector(0).y < -20 && !placing)
        {
            Hide(); 
        }
    }

    public void Show()
    {
        inventory.SetActive(true);
        open = true;
    }

    public void Hide()
    {
        inventory.SetActive(false);
        open = false;
    }

}

[Serializable]

class Item
{
    public string name;
    public Sprite image;
}

