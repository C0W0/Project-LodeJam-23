using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField]
    private List<Transform> adventurerSpawnLocations;
    [SerializeField]
    private Transform bossSpawnLocation;

    [SerializeField]
    private GameObject adventurerPrefab, bossPrefab;

    private EntityStats _bossEntity;
    private Dictionary<EntityStats, int> _adventurers;
    private EntityStats _playerEntity;


    void Awake()
    {
        Instance = this;
        _adventurers = new Dictionary<EntityStats, int>();
    }

    void Start()
    {
        StartLevel(false, 5);
    }

    void Update()
    {
        if (_adventurers.Count == 0)
        {
            EndGame(PlayerController.Instance.IsPlayingBoss);
        }
    }

    public void StartLevel(bool isPlayingBoss, int advSpawnCount)
    {
        _bossEntity = Instantiate(bossPrefab, bossSpawnLocation.position, Quaternion.identity).GetComponent<EntityStats>();
        for (int i = 0; i < advSpawnCount-1; i++)
        {
            _adventurers.Add(SpawnAdventurer(), i);
        }

        if (!isPlayingBoss)
        {
            _playerEntity = SpawnAdventurer();
            _adventurers.Add(_playerEntity, advSpawnCount-1);
        }
        else
        {
            _playerEntity = _bossEntity;
        }
        
        PlayerController.Instance.SetPlayer(_playerEntity);
        PlayerController.Instance.playerHealthbar.OnPlayerCharacterSwitch();
    }

    private EntityStats SpawnAdventurer()
    {
        Transform spawnLocation = adventurerSpawnLocations[Random.Range(0, 2)];
        var adventurer = Instantiate(adventurerPrefab, spawnLocation.position, Quaternion.identity);
        var component = adventurer.GetComponent<EntityStats>();
        return component;
    }

    public void EndGame(bool isVictory)
    {
        print($"Game ends: {isVictory}");
    }

    public void OnEntityDeath(EntityStats entity)
    {
        if (!entity.IsBoss())
        {
            _adventurers.Remove(entity);
        }
        else
        {
            EndGame(!PlayerController.Instance.IsPlayingBoss);
        }
        Destroy(entity.gameObject);
    }

    // The player's body is swapped between the player and the enemy
    public void SwapPlayer(EntityStats newPlayer)
    {
        // TODO: this is wrong
        SetPlayerEntity(newPlayer);
    }

    private void SetPlayerEntity(EntityStats newPlayer)
    {
        _playerEntity = newPlayer;
        PlayerController.Instance.SetPlayer(newPlayer);
    }

    public EntityStats GetPlayerEntity()
    {
        return _playerEntity;
    }
    
}
