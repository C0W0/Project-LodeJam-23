using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseAI : MonoBehaviour
{
    protected Rigidbody2D _rb;
    protected EntityStats _entity;
    protected GameObject _target;

    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _entity = GetComponent<EntityStats>();
    }

    void Start()
	{
		_target = GameManager.Instance.GetBossEntity().gameObject;
        InitAI();
	}

    private void Update()
    {
        UpdateMovement();
        UpdateAttack();
    }

    protected virtual void InitAI() { }
    protected virtual void UpdateMovement() { }
    protected virtual void UpdateAttack() { }
}
