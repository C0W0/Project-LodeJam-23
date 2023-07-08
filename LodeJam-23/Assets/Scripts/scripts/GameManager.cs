using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField]
    private EntityStats playerEntity;
    [SerializeField]
    private PlayerController playerController;

    private HashSet<EntityStats> _enemies;

    void Awake()
    {
        Instance = this;
        playerController.SetPlayer(playerEntity);

        // add every single EntityStats in the scene to the enemies set except for the player
        _enemies = new HashSet<EntityStats>();
        foreach (EntityStats entity in FindObjectsByType<EntityStats>(FindObjectsSortMode.None))
        {
            if (entity != playerEntity)
            {
                _enemies.Add(entity);
            }
        }
    }

    // The player's body is swapped between the player and the enemy
    public void SwapPlayer(EntityStats newPlayer)
    {
        _enemies.Add(playerEntity);
        SetPlayerEntity(newPlayer);
        _enemies.Remove(newPlayer);
    }

    private void SetPlayerEntity(EntityStats newPlayer)
    {
        playerEntity = newPlayer;
        playerController.SetPlayer(newPlayer);
    }

    public EntityStats GetPlayerEntity()
    {
        return playerEntity;
    }

    public HashSet<EntityStats> GetEnemies()
    {
        return _enemies;
    }
    
}
