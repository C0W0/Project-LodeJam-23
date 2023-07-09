using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseAI : MonoBehaviour
{
    protected Rigidbody2D _rb;
    protected EntityStats _entity;
    protected GameObject _target;
    public GameObject target
    {
        get { return _target; }
        set { _target = value; }
    }

    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _entity = GetComponent<EntityStats>();
    }

    protected virtual void Start() { }

    private void Update()
    {
        UpdateMovement();
        UpdateAttack();
    }

    protected virtual void InitAI() { }
    protected virtual void UpdateMovement() { }
    protected virtual void UpdateAttack() { }
}
