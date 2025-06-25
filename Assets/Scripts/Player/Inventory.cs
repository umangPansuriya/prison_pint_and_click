using System.Collections.Generic;

public class Inventory
{
    private List<Items> _items;
    public Inventory()
    {
        _items = new List<Items>();
    }
    public void AddItem(Items itemName)
    {
        _items.Add(itemName);
    }
    public void RemoveItem(Items itemName)
    {
        if (_items.Contains(itemName))
        {
            _items.Remove(itemName);
        }
    }
    public bool HasItem(Items itemName)
    {
        foreach (Items item in _items)
        {
            if (item == itemName) return true;
        }
        return false;
    }
}
public enum Items
{
    StoreRoomKey
}