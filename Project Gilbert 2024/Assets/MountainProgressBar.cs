using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MountainProgressBar : MonoBehaviour
{

    public Slider ProgressSlider;

    public GameObject objectToTrack;
    //public GameObject avalancheObject;
    public GameObject checkpointObject;

    public float maxMountainHeight;
    public float startingMountainHeight = 0.0f;
    public float totalMountainDistance;
    public float playerDistanceToHeight = 0.0f;
    public float avalancheDistanceToHeight = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        startingMountainHeight = objectToTrack.transform.position.y;
       // CalculateMountainHeight();
    }

    public void CalculateMountainHeight()
    {
        maxMountainHeight = checkpointObject.transform.position.y;
        totalMountainDistance = Mathf.Abs(maxMountainHeight - startingMountainHeight);
        //totalMountainDistance = maxMountainHeight - startingMountainHeight;
        playerDistanceToHeight = (maxMountainHeight - objectToTrack.transform.position.y) / maxMountainHeight;
        //avalancheDistanceToHeight = (maxMountainHeight - avalancheObject.transform.position.y) / maxMountainHeight;
        //Debug.Log((playerDistanceToHeight - totalMountainDistance) / totalMountainDistance);
        //playerProgressSprite.gameObject.transform.SetPositionAndRotation(new Vector3(playerProgressSprite.gameObject.transform.position.x, playerDistanceToHeight, 1), Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        CalculateMountainHeight();

        ProgressSlider.maxValue = maxMountainHeight;
        ProgressSlider.value = playerDistanceToHeight;
    }
}
