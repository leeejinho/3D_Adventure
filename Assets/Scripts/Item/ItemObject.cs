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
        // ��ȣ�ۿ� Ű �Է� �� �÷��̾�� ������ ����
        GameManager.Instance.player.itemData = data;
        GameManager.Instance.player.addItem?.Invoke();
        Destroy(gameObject);
    }

    public void ThrowItem()
    {
        throw new System.NotImplementedException();
    }
}
