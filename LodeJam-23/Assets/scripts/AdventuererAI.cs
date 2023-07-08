using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class AdventurerAI : MonoBehaviour
{
    private GameObject gameManager;
    private Rigidbody2D rb;

    public float distanceFromPlayer = 5f;
    public float minChangeDirectionInterval = 2f; // minimum time interval to change direction
    public float maxChangeDirectionInterval = 5f; // maximum time interval to change direction
    public float deadbandDistance = 1f; // distance from target at which the enemy stops moving
    public float rotationSpeed = 10f; // speed at which the direction vector rotates over time
    private float timeSinceLastDirectionChange = 0f; // time since last direction change
    private Vector2 direction; // current direction vector
    private float changeDirectionInterval;
    private int rotateDirection; // 1 for clockwise, -1 for counterclockwise

    void Start()
    {
        gameManager = GameObject.Find("GameManager");
        rb = GetComponent<Rigidbody2D>();
        direction = Random.insideUnitCircle.normalized; // initialize direction to zero vector
        changeDirectionInterval = Random.Range(minChangeDirectionInterval, maxChangeDirectionInterval); // initialize changeDirectionInterval to a random value between minChangeDirectionInterval and maxChangeDirectionInterval
        rotateDirection = Random.Range(0, 2) * 2 - 1; // initialize rotateDirection to either 1 or -1
    }

    void Update()
    {
        GameObject player = gameManager.GetComponent<GameManager>().getPlayer().gameObject;
        float speed = GetComponent<EntityStats>().GetSpeed();

        timeSinceLastDirectionChange += Time.deltaTime;

        if (timeSinceLastDirectionChange >= changeDirectionInterval)
        {
            direction = Random.insideUnitCircle.normalized;
            rotateDirection = Random.Range(0, 2) * 2 - 1; // initialize rotateDirection to either 1 or -1
            timeSinceLastDirectionChange = 0f;
            changeDirectionInterval = Random.Range(minChangeDirectionInterval, maxChangeDirectionInterval); // set a new random value for changeDirectionInterval
        }
        // slowly rotate direction vector
        direction = Quaternion.Euler(0, 0, rotateDirection * rotationSpeed * Time.deltaTime) * direction;

        if (player != null)
        {
            Vector2 targetLocation = (Vector2)player.transform.position + direction * distanceFromPlayer;

            Vector2 directionToTarget = (targetLocation - (Vector2)transform.position).normalized;
            if (Vector2.Distance(transform.position, targetLocation) > deadbandDistance)
            {
                        rb.velocity = directionToTarget * speed;
            }
            else
            {
                rb.velocity = Vector2.zero;
            }
        }
    }
}
