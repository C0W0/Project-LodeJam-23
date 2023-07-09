using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationDriver : MonoBehaviour
{
    private Animator animator;
    public float speedMultiplier = 1.0f;
    public float speedThreshold = 0.1f;
    public int filterSize = 30; // size of the moving average filter

    private Vector2[] velocityFilter; // array to store previous speed values for the filter
    private int filterIndex = 0; // index of the oldest value in the filter array

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        velocityFilter = new Vector2[filterSize];
        for (int i = 0; i < filterSize; i++)
        {
            velocityFilter[i] = Vector2.zero;
        }
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 velocity = (Vector2)(transform.position - previousPosition) / Time.deltaTime * speedMultiplier;
        velocityFilter[filterIndex] = velocity;
        filterIndex = (filterIndex + 1) % filterSize;

        Vector2 filteredVelocity = Vector2.zero;
        for (int i = 0; i < filterSize; i++)
        {
            filteredVelocity += velocityFilter[i];
        }
        filteredVelocity /= filterSize;

        float speedMagnitude = filteredVelocity.magnitude;
        bool goingUp = filteredVelocity.y > speedThreshold;
        bool goingDown = filteredVelocity.y < -speedThreshold;

        animator.SetFloat("Speed", speedMagnitude);
        animator.SetBool("GoingUp", goingUp);
        animator.SetBool("GoingDown", goingDown);

        if (filteredVelocity.x > speedThreshold)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        else if (filteredVelocity.x < -speedThreshold)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }

        previousPosition = transform.position;
    }

    private Vector3 previousPosition;
}
