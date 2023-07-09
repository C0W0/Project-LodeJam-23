using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseItem : MonoBehaviour
{
    private bool _isHidden;

    [SerializeField]
    protected int effectDuration;
    [SerializeField]
    protected int respawnCooldown;

    private void Awake()
    {
        _isHidden = false;
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void Dissapear()
    {
        gameObject.GetComponent<Renderer>().enabled = false;
        _isHidden = true;
    }

    void Appear()
    {
        gameObject.GetComponent<Renderer>().enabled = true;
        _isHidden = false;
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (_isHidden)
        {
            return;
        }
        
        var playerScript = collider.gameObject.GetComponent<EntityStats>();
        if (playerScript != GameManager.Instance.GetPlayerEntity())
        {
            return;
        }
        
        OnPlayerPickup();
        Timer.Instance.AddProcess(effectDuration, OnEffectExpire);
        Dissapear();
        Timer.Instance.AddProcess(respawnCooldown, Appear);
    }

    protected virtual void OnPlayerPickup()
    {
        GameManager.Instance.GetPlayerEntity().ChangeSpeed(3);
    }

    protected virtual void OnEffectExpire()
    {
        GameManager.Instance.GetPlayerEntity().ChangeSpeed(-3);
    }

}
