using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

public class BossAI : BaseAI
{
    public float distanceFromPlayer = 20f;
    public float minChangeDirectionInterval = 1f; // minimum time interval to change direction
    public float maxChangeDirectionInterval = 1f; // maximum time interval to change direction
    public float deadbandDistance = 10f; // distance from target at which the enemy stops moving
    public float rotationSpeed = 50f; // speed at which the direction vector rotates over time
    public float minAttackInterval = 0f; // minimum time interval to attack
    public float maxAttackInterval = 1f; // maximum time interval to attack
    private float _timeSinceLastDirectionChange = 0f; // time since last direction change
    private float _timeUntilAttack = 0f;
    private Vector2 _direction; // current direction vector
    private float _changeDirectionInterval;
    private int _rotateDirection; // 1 for clockwise, -1 for counterclockwise
    private bool _enabled = true;

    protected override void InitAI()
    {
        if (GameManager.Instance.CurrAdventureIndex == -1)
        {
            enabled = false;
            return;
        }
        _direction = Random.insideUnitCircle.normalized; // initialize direction to zero vector
        _changeDirectionInterval = Random.Range(minChangeDirectionInterval, maxChangeDirectionInterval); // initialize changeDirectionInterval to a random value between minChangeDirectionInterval and maxChangeDirectionInterval
        _rotateDirection = Random.Range(0, 2) * 2 - 1; // initialize rotateDirection to either 1 or -1
    }

    protected override void Start()
    {
        _target = GameManager.Instance.GetPlayerEntity().gameObject;
        InitAI();
    }

    protected override void UpdateMovement()
    {
        if (enabled == false)
        {
            return;
        }
        _timeSinceLastDirectionChange += Time.deltaTime;

        // Randomly change the "direction" to stay away from the player
        // On a random interval
        if (_timeSinceLastDirectionChange >= _changeDirectionInterval)
        {
            print("ok");
            _direction = Random.insideUnitCircle.normalized;
            _rotateDirection = Random.Range(0, 2) * 2 - 1; // initialize rotateDirection to either 1 or -1
            _timeSinceLastDirectionChange = 0f;
            _changeDirectionInterval = Random.Range(minChangeDirectionInterval, maxChangeDirectionInterval); // set a new random value for changeDirectionInterval
        }
        // Slowly rotate direction vector
        _direction = Quaternion.Euler(0, 0, _rotateDirection * rotationSpeed * Time.deltaTime) * _direction;

        if (_target == null)
            return; // no player to move towards

        Vector2 targetLocation = (Vector2)_target.transform.position + _direction * distanceFromPlayer;
        Vector2 directionToTarget = (targetLocation - (Vector2)transform.position).normalized;
        //if (Vector2.Distance(transform.position, targetLocation) > deadbandDistance)
            _rb.velocity = directionToTarget * _entity.GetSpeed();
        //else
          //  _rb.velocity = Vector2.zero;
    }


    protected override void UpdateAttack()
    {
        _timeUntilAttack -= Time.deltaTime;

        if (_enabled == false)
        {
            return; // no player to attack
        }


        if (_timeUntilAttack > 0)
            return; // too soon

        // Attack in a cross pattern
        Vector2[] attackDirections = new Vector2[4];
        attackDirections[0] = Vector2.up;
        attackDirections[1] = Vector2.down;
        attackDirections[2] = Vector2.left;
        attackDirections[3] = Vector2.right;
        for (int i = 0; i < attackDirections.Length; i++)
        {
            _entity.Attack(attackDirections[i]);
        }

        _timeUntilAttack = Random.Range(minAttackInterval, maxAttackInterval);
    }
}

