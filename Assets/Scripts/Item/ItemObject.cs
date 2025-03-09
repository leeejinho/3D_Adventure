using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemObject : MonoBehaviour, IInteractable
{
    public ItemData data;

    public string GetInteractPrompt()
    {
        string str = $"{data.displayName}\n{data.description}";

        return str;
    }

    public void OnInteract()
    {
        GameManager.Instance.player.itemData = data;
        GameManager.Instance.player.addItem?.Invoke();
        Destroy(gameObject);
    }
}
