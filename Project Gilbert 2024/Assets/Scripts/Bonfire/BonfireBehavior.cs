using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonfireBehavior : MonoBehaviour
{
    public PlayerData playerData;
    public static Action OnRest;
    public static Action<ItemData> OnAddItem;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                playerData.trashAmount += playerData.currentLevel * 10;
                PlayerBehavior.OnTrashChange?.Invoke();
                OnRest?.Invoke();
            }
        }
    }
}
