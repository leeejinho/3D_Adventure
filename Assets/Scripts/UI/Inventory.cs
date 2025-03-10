using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    [Header("Select Item")]
    public Transform dropPos;
    public TextMeshProUGUI selectedItemName;
    public TextMeshProUGUI selectedItemDesc;
    public TextMeshProUGUI selectedStatName;
    public TextMeshProUGUI selectedStatValue;
    public Button useButton;
    public TextMeshProUGUI useButtonText;
    public Button dropButton;

    ItemSlot[] slots;
    PlayerController controller;
    PlayerCondition condition;

    ItemData selectedItem;
    int selectedItemIdx = 0;

    int curEquipIdx;

    private void Start()
    {
        controller = GameManager.Instance.player.controller;
        condition = GameManager.Instance.player.condition;

        controller.actionInventory += ToggleInventory;
        GameManager.Instance.player.addItem += AddItem;
        
        dropButton.onClick.AddListener(OnDropButton);

        gameObject.SetActive(false);
        slots = GetComponentsInChildren<ItemSlot>();

        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].idx = i;
            slots[i].inventory = this;
        }

        ClearSelectedItemWindow();
        UpdateUI();
    }

    void ClearSelectedItemWindow()
    {
        selectedItemName.text = string.Empty;
        selectedItemDesc.text = string.Empty;
        selectedStatName.text = string.Empty;
        selectedStatValue.text = string.Empty;

        useButton.gameObject.SetActive(false);
        dropButton.gameObject.SetActive(false);
    }

    void ToggleInventory()
    {
        if (gameObject.activeInHierarchy)
            gameObject.SetActive(false);
        else
            gameObject.SetActive(true);
    }

    void AddItem()
    {
        ItemData data = GameManager.Instance.player.itemData;

        // ��ø������ ������ Ȯ��
        if (data.canStack)
        {
            ItemSlot slot = GetItemStack(data);
            if (slot != null)
            {
                slot.quantity++;
                UpdateUI();
                GameManager.Instance.player.itemData = null;
                return;
            }
        }

        // �󽽷� ��������
        ItemSlot emptySlot = GetEmptySlot();
        if (emptySlot != null)
        {
            emptySlot.item = data;
            emptySlot.quantity = 1;
            UpdateUI();
            return;
        }

        // ������ĭ�� �� ���ִٸ�
        ThrowItem(data);
        GameManager.Instance.player.itemData = null;
    }

    void UpdateUI()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item != null)
                slots[i].Set();
            else
                slots[i].Clear();
        }
    }

    ItemSlot GetItemStack(ItemData data)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            // ������ �����۵����Ϳ� �߰��� �����۵����Ͱ� ����
            // ���� ������ ������ �ִ밳������ ���ٸ� ������ ���� ��ȯ
            if (slots[i].item == data && slots[i].quantity < data.maxStackAmount)
                return slots[i];
        }

        return null;
    }

    ItemSlot GetEmptySlot()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            // ������ ������ �����Ͱ� ��������� ���� ��ȯ
            if (slots[i].item == null)
                return slots[i];
        }

        return null;
    }

    void ThrowItem(ItemData data)
    {
        Instantiate(data.dropPrefab, dropPos.position, Quaternion.Euler(Vector3.one * Random.value * 360));
    }

    public void SelectItem(int idx)
    {
        if (slots[idx].item == null) 
            return;

        selectedItem = slots[idx].item;
        selectedItemIdx = idx;

        selectedItemName.text = selectedItem.displayName;
        selectedItemDesc.text = selectedItem.description;

        selectedStatName.text = string.Empty;
        selectedStatValue.text = string.Empty;

        for (int i = 0; i < selectedItem.consumables.Length; i++)
        {
            selectedStatName.text += selectedItem.consumables[i].type.ToString() + "\n";
            selectedStatValue.text += selectedItem.consumables[i].value.ToString() + "\n";
        }

        UpdateButton(slots[idx].equipped);
    }

    void UpdateButton(bool equipped)
    {
        // ��� ��ư�� ��� ���� ����
        useButton.onClick.RemoveAllListeners();
        useButton.gameObject.SetActive(true);

        // ������ Ÿ�Կ� ���� Ŭ�� �̺�Ʈ ����
        switch (selectedItem.type)
        {
            case ItemType.Equipable:
                useButtonText.text = equipped ? "��������" : "�����ϱ�";
                useButton.onClick.AddListener(equipped ? OnUnEquipButton : OnEquipButton);
                break;
            case ItemType.Consumable:
                useButtonText.text = "����ϱ�";
                useButton.onClick.AddListener(OnUseConsumableButton);
                break;
            case ItemType.Resource:
                useButton.gameObject.SetActive(false);
                break;
        }

        dropButton.gameObject.SetActive(true);
    }

    void OnDropButton()
    {
        ThrowItem(selectedItem);
        RemoveSelectedItem();
    }

    void RemoveSelectedItem()
    {
         // �������� �������̶�� ����
        if (slots[selectedItemIdx].equipped)
        {
            slots[selectedItemIdx].equipped = false;
            GameManager.Instance.player.equip.UnEquip();
            // ���� �ƿ����� ����
            slots[selectedItemIdx].Set();
        }

        // ������ ���� ����
        slots[selectedItemIdx].quantity--;

        // ������ ������ 0���� ������ ������ �����۵����� ����
        if (slots[selectedItemIdx].quantity <= 0)
        {
            selectedItem = null;
            slots[selectedItemIdx].item = null;
            selectedItemIdx = -1;
            ClearSelectedItemWindow();
        }

        UpdateUI();
    }

    void OnUseConsumableButton()
    {
        // �Һ� ������ ȿ���� ���� �ɷ� ����
        for (int i = 0; i < selectedItem.consumables.Length; i++)
        {
            switch (selectedItem.consumables[i].type)
            {
                case ConsumableType.Health:
                    condition.Heal(selectedItem.consumables[i].value);
                    break;
                case ConsumableType.Attack:
                    break;
                case ConsumableType.Speed:
                    break;
            }
        }
        RemoveSelectedItem();
    }

    void OnEquipButton()
    {
        // ���� �������� �������̶��
        if (slots[curEquipIdx].equipped)
        {
            slots[curEquipIdx].equipped = false;
            GameManager.Instance.player.equip.UnEquip();
        }

        curEquipIdx = selectedItemIdx;
        slots[selectedItemIdx].equipped = true;
        GameManager.Instance.player.equip.EquipNew(selectedItem);
        UpdateUI();
        useButtonText.text = "��������";
    }

    void OnUnEquipButton()
    {
        slots[selectedItemIdx].equipped = false;
        GameManager.Instance.player.equip.UnEquip();
        UpdateUI();
        useButtonText.text = "�����ϱ�";
    }
}
