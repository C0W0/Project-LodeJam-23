using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemScript : MonoBehaviour
{
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
    }

    void Appear()
    {
        gameObject.GetComponent<Renderer>().enabled = true;
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        var playerScript = collider.gameObject.GetComponent<EntityStats>();
        if (playerScript != GameManager.Instance.GetPlayerEntity())
        {
            return;
        }

        StartCoroutine(PlayerController.Instance.SpeedBoost(3, 2f));
        print(":o");
        Dissapear();
        Invoke(nameof(Appear), 5f);        
    }

}
