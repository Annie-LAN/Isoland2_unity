using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManager : MonoBehaviour, ISaveable
{
    private Dictionary<ItemName, bool> itemAvailableDict = new Dictionary<ItemName, bool>();
    private Dictionary<string, bool> interactiveStateDict = new Dictionary<string, bool>();

    private void OnEnable()
    {
        EventHandler.BeforeSceneUnloadEvent += OnBeforeSceneUnloadEvent;
        EventHandler.AfterSceneLoadedEvent += OnAfterSceneLoadedEvent;
        EventHandler.UpdateUIEvent += OnUpdateUIEvent;
        EventHandler.StartNewGameEvent += OnStartNewGameEvent;
        EventHandler.ItemUsedEvent += OnItemUsedEvent;
    }

    private void OnDisable()
    {
        EventHandler.BeforeSceneUnloadEvent -= OnBeforeSceneUnloadEvent;
        EventHandler.AfterSceneLoadedEvent -= OnAfterSceneLoadedEvent;
        EventHandler.UpdateUIEvent -= OnUpdateUIEvent;
        EventHandler.StartNewGameEvent -= OnStartNewGameEvent;
        EventHandler.ItemUsedEvent -= OnItemUsedEvent;
    }

    // 修复14数据保存和加载bug：拿到钥匙开启信箱停留在H4场景，回主菜单继续，邮箱显示关闭，而且钥匙也没了。
    private void OnItemUsedEvent(ItemName name)
    {
        foreach (var interactive in FindObjectsOfType<Interactive>())
        {
            if (interactiveStateDict.ContainsKey(interactive.name))
                interactiveStateDict[interactive.name] = interactive.isDone;
            else
                interactiveStateDict.Add(interactive.name, interactive.isDone);
        }
    }

    private void Start()
    {
        ISaveable saveable = this;
        saveable.SaveableRegister();
    }
    private void OnStartNewGameEvent(int obj)
    {
        itemAvailableDict.Clear();
        interactiveStateDict.Clear();
    }

    private void OnBeforeSceneUnloadEvent()
    {
       foreach(var item in FindObjectsOfType<Item>())
       {
            if (!itemAvailableDict.ContainsKey(item.itemName))
                itemAvailableDict.Add(item.itemName, true);
       }

       foreach(var item in FindObjectsOfType<Interactive>())
       {
            if (interactiveStateDict.ContainsKey(item.name))            
                interactiveStateDict[item.name] = item.isDone;           
            else
                interactiveStateDict.Add(item.name, item.isDone);
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

        foreach (var item in FindObjectsOfType<Interactive>())
        {
            if (interactiveStateDict.ContainsKey(item.name))
                item.isDone = interactiveStateDict[item.name];
            else
                interactiveStateDict.Add(item.name, item.isDone);
        }
    }

    private void OnUpdateUIEvent(ItemDetails itemDetails, int args)
    {
       if(itemDetails != null)
        {
            itemAvailableDict[itemDetails.itemName] = false;
        }
    }

    public GameSaveData GenerateSaveData()
    {
        GameSaveData saveData = new GameSaveData();
        saveData.itemAvailableDict = this.itemAvailableDict;
        saveData.interactiveStateDict = this.interactiveStateDict;
        return saveData;
    }

    public void RestoreGameData(GameSaveData saveData)
    {
        this.itemAvailableDict = saveData.itemAvailableDict;
        this.interactiveStateDict = saveData.interactiveStateDict;
    }
}
