using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehavior : MonoBehaviour
{
    private Rigidbody2D target;
    public Vector3 offset = new Vector3(0, 5f, 0);

    public float camMovementOffsetUp = 1;
    public float camMovementOffsetDown = 3;

    private Vector3 targetPosition;

    private Vector3 vel = Vector3.zero;
    public float dampingUp = 1;
    public float dampingDown = 0.25f;

    public float maxCamHeight = 48;
    public bool stopCamera = false;

    private void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (stopCamera) return; 

        targetPosition = transform.position;

        if (target.position.y > transform.position.y + camMovementOffsetUp)
        {
            targetPosition.Set(0, target.position.y + offset.y + camMovementOffsetUp, transform.position.z);
        }
        else if (target.position.y < transform.position.y - camMovementOffsetDown)
        {
            targetPosition.Set(0, target.position.y + offset.y - camMovementOffsetDown, transform.position.z);
        }

        if (target.velocity.y < 0 && target.position.y < transform.position.y - camMovementOffsetDown)
        {
            targetPosition.Set(0, target.position.y, transform.position.z);

            targetPosition.y = Mathf.Clamp(targetPosition.y, -999, maxCamHeight);

            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref vel, dampingDown);
        }
        else
        {
            targetPosition.y = Mathf.Clamp(targetPosition.y, -999, maxCamHeight);

            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref vel, dampingUp);
        }
    }

    public void OnDeath() 
    {
        stopCamera = true;
    }

    private void OnEnable()
    {
        HealthComponent.OnDie += OnDeath;
    }

    private void OnDisable()
    {
        HealthComponent.OnDie -= OnDeath;
    }

}
