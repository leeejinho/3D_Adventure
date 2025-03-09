using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Interaction : MonoBehaviour
{
    public float checkRate = 0.05f;
    private float lastCheckTime;
    public float maxCheckDistance;
    public LayerMask layerMask;

    public GameObject curInteractGameObject;
    private IInteractable curInteractable;

    public TextMeshProUGUI promptText;
    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;
        GameManager.Instance.player.controller.actionInteract += InteractInput;
    }

    void Update()
    {
        if(Time.time - lastCheckTime > checkRate)
        {
            lastCheckTime = Time.time;

            Ray ray = mainCamera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
            RaycastHit hit;

            if(Physics.Raycast(ray, out hit, maxCheckDistance, layerMask))
            {
                if(hit.collider.gameObject != curInteractGameObject)
                {
                    // Raycast에 부딪힌 오브젝트 참조
                    curInteractGameObject = hit.collider.gameObject;
                    curInteractable = hit.collider.GetComponent<IInteractable>();
                    
                    // 아이템 이름, 설명 표시
                    SetPromptText();
                }
            }
            else
            {
                curInteractGameObject = null;
                curInteractable = null;
                promptText.gameObject.SetActive(false);
            }
        }
    }

    void SetPromptText()
    {
        promptText.gameObject.SetActive(true);
        promptText.text = curInteractable.GetInteractPrompt();
    }

    void InteractInput()
    {
        if (curInteractable != null)
        {
            // 아이템 상호작용 호출
            curInteractable.OnInteract();

            // 현재 아이템 지워주기
            curInteractGameObject = null;
            curInteractable = null;
            promptText.gameObject.SetActive(false);
        }
    }
}
