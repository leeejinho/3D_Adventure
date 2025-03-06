using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public PlayerController controller;

    private void Awake()
    {
        GameManager.Instance.player = this;
        controller = GetComponent<PlayerController>();
    }
}
