using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemObject : MonoBehaviour, IInteractable, IThrowable
{
    public ItemData data;

    private void FixedUpdate()
    {
        
    }

    public string GetInteractPrompt()
    {
        string str = $"{data.displayName}\n{data.description}";

        return str;
    }

    public void OnInteract()
    {
        // 상호작용 키 입력 시 플레이어에게 데이터 전달
        GameManager.Instance.player.itemData = data;
        GameManager.Instance.player.addItem?.Invoke();
        Destroy(gameObject);
    }

    public void ThrowItem()
    {
        throw new System.NotImplementedException();
    }
}
