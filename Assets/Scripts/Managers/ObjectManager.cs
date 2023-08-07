using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManager : MonoBehaviour
{
    private Dictionary<ItemName, bool> itemAvailableDict = new Dictionary<ItemName, bool>();

    private void OnEnable()
    {
        EventHandler.BeforeSceneUnloadEvent += OnBeforeSceneUnloadEvent;
        EventHandler.AfterSceneLoadedEvent += OnAfterSceneLoadedEvent;
        EventHandler.UpdateUIEvent += OnUpdateUIEvent;
    }

    private void OnDisable()
    {
        EventHandler.BeforeSceneUnloadEvent -= OnBeforeSceneUnloadEvent;
        EventHandler.AfterSceneLoadedEvent -= OnAfterSceneLoadedEvent;
        EventHandler.UpdateUIEvent -= OnUpdateUIEvent;
    }

    private void OnBeforeSceneUnloadEvent()
    {
       foreach(var item in FindObjectsOfType<Item>())
        {
            if (!itemAvailableDict.ContainsKey(item.itemName))
                itemAvailableDict.Add(item.itemName, true);
        }
    }

    private void OnAfterSceneLoadedEvent()
    {
        //如果已经在字典中则更新显示状态，不在则添加
        foreach(var item in FindObjectsOfType<Item>()) {
            if (!itemAvailableDict.ContainsKey(item.itemName))
                itemAvailableDict.Add(item.itemName, true);
            else
                item.gameObject.SetActive(itemAvailableDict[item.itemName]);
        }
    }

    private void OnUpdateUIEvent(ItemDetails itemDetails, int args)
    {
       if(itemDetails != null)
        {
            itemAvailableDict[itemDetails.itemName] = false;
        }
    }

}
