using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour
{
    [HideInInspector] public ItemData item;

    public Image icon;
    public TextMeshProUGUI quantityText;
    Button button;
    Outline outline;

    public Inventory inventory;

    public int idx;
    public bool equipped;
    public int quantity;

    private void Awake()
    {
        button = GetComponent<Button>();
        outline = GetComponent<Outline>();
    }

    private void OnEnable()
    {
        // 아이템이 장착중이라면 아웃라인 활성화
        outline.enabled = equipped;
        button.onClick.AddListener(OnButtonClick);
    }

    public void Set()
    {
        icon.gameObject.SetActive(true);
        icon.sprite = item.icon;
        quantityText.text = quantity > 1 ? quantity.ToString() : string.Empty;
        outline.enabled = equipped;
    }

    public void Clear()
    {
        item = null;
        icon.sprite = null;
        icon.gameObject.SetActive(false);
        quantityText.text = string.Empty;
    }

    public void OnButtonClick()
    {
        inventory.SelectItem(idx);
    }
}
