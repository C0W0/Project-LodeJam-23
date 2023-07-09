using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField]
    private EntityStats playerEntity;
    [SerializeField]
    private List<Transform> adventurerSpawnLocations;

    [SerializeField]
    private GameObject adventurerPrefab;

    private EntityStats _bossEntity;
    private HashSet<EntityStats> _adventurers;


    void Awake()
    {
        Instance = this;
        _adventurers = new HashSet<EntityStats>();
    }

    void Start()
    {
        PlayerController.Instance.SetPlayer(playerEntity);
        StartLevel();
    }

    public void StartLevel()
    {
        for (int _ = 0; _ < 5; _++)
        {
            Transform spawnLocation = adventurerSpawnLocations[Random.Range(0, 2)];
            var adventurer = Instantiate(adventurerPrefab, spawnLocation.position, Quaternion.identity);
            var component = adventurer.GetComponent<EntityStats>();
            _adventurers.Add(component);
        }
    }

    // The player's body is swapped between the player and the enemy
    public void SwapPlayer(EntityStats newPlayer)
    {
        // TODO: this is wrong
        SetPlayerEntity(newPlayer);
    }

    private void SetPlayerEntity(EntityStats newPlayer)
    {
        playerEntity = newPlayer;
        PlayerController.Instance.SetPlayer(newPlayer);
    }

    public EntityStats GetPlayerEntity()
    {
        return playerEntity;
    }
    
}
