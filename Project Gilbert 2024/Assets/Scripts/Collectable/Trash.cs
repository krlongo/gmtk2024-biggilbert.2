using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trash : MonoBehaviour
{
    public int value;
    public float notGrabbableTimer = .1f;
    public bool isGrabbable = false;

    // Start is called before the first frame update
    void Start()
    {
        SetValue();
    }

    // Update is called once per frame
    void Update()
    {
        if (CheckGrabbableTimer())
        {
            isGrabbable = true;
        }
    }

    public bool CheckGrabbableTimer()
    {
        if (notGrabbableTimer > 0)
        {
            notGrabbableTimer -= Time.deltaTime;
            return false;
        }
        else
        {
            return true;
        }
    }

    public void SetValue()
    {
        value = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerBehavior>().playerData.currentLevel;
    }
}
