using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ResourceType { Tree, Rock }

public class Resource : MonoBehaviour
{
    public ItemData itemToGive;
    public ResourceType type;
    public int quantityPerHit = 1;
    public int capacy;

    public void Gather(Vector3 hitPoint, Vector3 hitNormal)
    {
        for (int i = 0; i < quantityPerHit; i++)
        {
            capacy--;
            Instantiate(itemToGive.dropPrefab, hitPoint + (Vector3.up * 0.7f), Quaternion.LookRotation(hitNormal, Vector3.up));
        }

        // ���� �ڿ��� ������ ������Ʈ ����
        if (capacy <= 0)
        {
            Destroy(gameObject);
        }
    }
}
