﻿using UnityEngine;
using UnityEngine.Networking;

public class Inventory : NetworkBehaviour
{

    public int space = 20;
    public event SyncList<Item>.SyncListChanged onItemChanged;

    public Player player;
    public SyncListItem items = new SyncListItem();

    UserData data;

    public void Load(UserData data)
    {
        this.data = data;
        for (int i = 0; i < data.inventory.Count; i++)
        {
            items.Add(ItemBase.GetItem(data.inventory[i]));
        }
    }

    public override void OnStartLocalPlayer()
    {
        items.Callback += ItemChanged;
    }

    private void ItemChanged(SyncList<Item>.Operation op, int itemIndex)
    {
        onItemChanged(op, itemIndex);
    }

    public bool AddItem(Item item)
    {
        if (items.Count < space)
        {
            items.Add(item);
            data.inventory.Add(ItemBase.GetItemId(item));
            return true;
        }
        else return false;
    }

    public void UseItem(Item item)
    {
        CmdUseItem(items.IndexOf(item));
    }

    [Command]
    void CmdUseItem(int index)
    {
        if (items[index] != null)
        {
            items[index].Use(player);
        }
    }

    public void DropItem(Item item)
    {
        CmdDropItem(items.IndexOf(item));
    }

    [Command]
    void CmdDropItem(int index)
    {
        if (items[index] != null)
        {
            Drop(items[index]);
            RemoveItem(items[index]);
        }
    }

    public void RemoveItem(Item item)
    {
        items.Remove(item);
        data.inventory.Remove(ItemBase.GetItemId(item));
    }

    private void Drop(Item item)
    {
        ItemPickup pickupItem = Instantiate(item.pickupPrefab, player.character.transform.position, Quaternion.Euler(0, Random.Range(0, 360f), 0));
        pickupItem.item = item;
        NetworkServer.Spawn(pickupItem.gameObject);
    }
}