using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HealthComponent : MonoBehaviour
{
    public bool isDeadTemp;
    [Header("Here is death event")]
    public UnityEvent onPlayerDeath;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isDeadTemp){
            onPlayerDeath.Invoke();
            isDeadTemp = false;
        }
    }
}
