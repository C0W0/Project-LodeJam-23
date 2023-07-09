using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

public class AdventurerAI : MonoBehaviour
{
	private Rigidbody2D _rb;

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

	private EntityStats _entity;
	private GameObject _playerObject;
	
	void Awake()
	{
		_rb = GetComponent<Rigidbody2D>();
		_entity = GetComponent<EntityStats>();
	}

	void Start()
	{
		_direction = Random.insideUnitCircle.normalized; // initialize direction to zero vector
		_changeDirectionInterval = Random.Range(minChangeDirectionInterval, maxChangeDirectionInterval); // initialize changeDirectionInterval to a random value between minChangeDirectionInterval and maxChangeDirectionInterval
		_rotateDirection = Random.Range(0, 2) * 2 - 1; // initialize rotateDirection to either 1 or -1
		_playerObject = GameManager.Instance.GetPlayerEntity().gameObject;
	}

	void Update()
	{
		_timeSinceLastDirectionChange += Time.deltaTime;
		_timeSinceLastAttack += Time.deltaTime;

		if (_timeSinceLastDirectionChange >= _changeDirectionInterval)
		{
			_direction = Random.insideUnitCircle.normalized;
			_rotateDirection = Random.Range(0, 2) * 2 - 1; // initialize rotateDirection to either 1 or -1
			_timeSinceLastDirectionChange = 0f;
			_changeDirectionInterval = Random.Range(minChangeDirectionInterval, maxChangeDirectionInterval); // set a new random value for changeDirectionInterval
		}
		// slowly rotate direction vector
		_direction = Quaternion.Euler(0, 0, _rotateDirection * rotationSpeed * Time.deltaTime) * _direction;

		if (_playerObject != null)
		{
			Vector2 targetLocation = (Vector2)_playerObject.transform.position + _direction * distanceFromPlayer;

			Vector2 directionToTarget = (targetLocation - (Vector2)transform.position).normalized;
			if (Vector2.Distance(transform.position, targetLocation) > deadbandDistance)
			{
				_rb.velocity = directionToTarget * _entity.GetSpeed();
			}
			else
			{
				_rb.velocity = Vector2.zero;
				if (_timeSinceLastAttack >= Random.Range(minAttackInterval, maxAttackInterval))
				{
					_entity.Attack(_playerObject.transform.position);
					_timeSinceLastAttack = 0f;
				}
			}
		}
	}
}
