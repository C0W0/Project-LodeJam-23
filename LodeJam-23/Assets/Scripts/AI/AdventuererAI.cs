using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

public class AdventurerAI : BaseAI
{
	public float distanceFromPlayer = 5f;
	public float minChangeDirectionInterval = 2f; // minimum time interval to change direction
	public float maxChangeDirectionInterval = 5f; // maximum time interval to change direction
	public float deadbandDistance = 1f; // distance from target at which the enemy stops moving
	public float rotationSpeed = 10f; // speed at which the direction vector rotates over time
	public float minAttackInterval = 2f; // minimum time interval to attack
	public float maxAttackInterval = 5f; // maximum time interval to attack
	private float _timeSinceLastDirectionChange = 0f; // time since last direction change
	private float _timeSinceLastAttack = 0f; // time since last attack
	private Vector2 _direction; // current direction vector
	private float _changeDirectionInterval;
	private int _rotateDirection; // 1 for clockwise, -1 for counterclockwise

	protected override void InitAI()
	{
		_direction = Random.insideUnitCircle.normalized; // initialize direction to zero vector
		_changeDirectionInterval = Random.Range(minChangeDirectionInterval, maxChangeDirectionInterval); // initialize changeDirectionInterval to a random value between minChangeDirectionInterval and maxChangeDirectionInterval
		_rotateDirection = Random.Range(0, 2) * 2 - 1; // initialize rotateDirection to either 1 or -1
	}

	protected override void UpdateMovement()
	{
		_timeSinceLastDirectionChange += Time.deltaTime;

		// Randomly change the "direction" to stay away from the player
		// On a random interval
		if (_timeSinceLastDirectionChange >= _changeDirectionInterval)
		{
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
		if (Vector2.Distance(transform.position, targetLocation) > deadbandDistance)
			_rb.velocity = directionToTarget * _entity.GetSpeed();
		else
			_rb.velocity = Vector2.zero;
	}

    protected override void Start()
    {
        _target = GameManager.Instance.GetBossEntity().gameObject;
        InitAI();
    }


    protected override void UpdateAttack()
	{
		_timeSinceLastAttack += Time.deltaTime;

		if (_target == null)
			return; // no player to attack

		Vector2 targetLocation = (Vector2)_target.transform.position + _direction * distanceFromPlayer;
		if (Vector2.Distance(transform.position, targetLocation) > deadbandDistance)
			return; // too far

		if (_timeSinceLastAttack < Random.Range(minAttackInterval, maxAttackInterval))
			return; // too soon
		_entity.Attack(_target.transform.position);
		_timeSinceLastAttack = 0f;
	}
}

