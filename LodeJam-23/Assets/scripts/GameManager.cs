using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField]
    private EntityStats player;

    private HashSet<EntityStats> _enemies;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.Log("Warning: multiple " + this + " in scene!");
        }

        // add every single EntityStats in the scene to the enemies set except for the player
        _enemies = new HashSet<EntityStats>();
        foreach (EntityStats entity in FindObjectsByType<EntityStats>(FindObjectsSortMode.None))
        {
            if (entity != player)
            {
                _enemies.Add(entity);
            }
        }
    }

    // The player's body is swapped between the player and the enemy
    public void SwapPlayer(EntityStats newPlayer){
        _enemies.Add(player);
        player = newPlayer;
        _enemies.Remove(newPlayer);
    }

    public EntityStats GetPlayer()
    {
        return player;
    }

    public HashSet<EntityStats> GetEnemies()
    {
        return _enemies;
    }
    
}
