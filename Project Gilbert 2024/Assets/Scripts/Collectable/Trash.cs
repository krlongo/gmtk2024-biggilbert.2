using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trash : MonoBehaviour
{
    public int value;

    // Start is called before the first frame update
    void Start()
    {
        RandomizeValue();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RandomizeValue()
    {
        value = Random.Range(1, 5);
    }
}
