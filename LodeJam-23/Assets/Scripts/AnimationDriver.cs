using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationDriver : MonoBehaviour
{
    private Animator animator;
    public float speedMultiplier = 1.0f;
    public float speedThreshold = 0.01f;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        float speed = (transform.position - previousPosition).magnitude / Time.deltaTime * speedMultiplier;
        bool goingUp = transform.position.y > previousPosition.y;
        bool goingDown = transform.position.y < previousPosition.y;

        animator.SetFloat("Speed", speed);
        animator.SetBool("GoingUp", goingUp);
        animator.SetBool("GoingDown", goingDown);

        if(goingUp )
        {
            Debug.Log("Going up");
        }

        if (goingDown)
        {
            Debug.Log("Going up");
        }

        previousPosition = transform.position;
    }

    private Vector3 previousPosition;
}
