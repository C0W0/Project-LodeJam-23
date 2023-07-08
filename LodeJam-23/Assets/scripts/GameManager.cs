using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public EntityStats player;

    public List<EntityStats> enemies;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.Log("Warning: multiple " + this + " in scene!");
        }

        // add every single EntityStats in the scene to the enemies list except for the player
        enemies = new List<EntityStats>();
        foreach (EntityStats entity in FindObjectsOfType<EntityStats>())
        {
            if (entity != player)
            {
                enemies.Add(entity);
            }
        }
    }

    // The player's body is swapped between the player and the enemy
    public void swapPlayer(EntityStats newPlayer){
        enemies.Add(player);
        player = newPlayer;
        enemies.Remove(newPlayer);
    }
}
