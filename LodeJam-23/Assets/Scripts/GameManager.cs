using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public static bool IsGameOngoing { get { return Instance._isGameOngoing; } }

    [SerializeField]
    private List<Transform> adventurerSpawnLocations;
    [SerializeField]
    private Transform bossSpawnLocation;
    [SerializeField]
    private GameObject adventurerPrefab, bossPrefab;

    private EntityStats _bossEntity;
    private List<EntityStats> _adventurers;

    private int _currAdventureIndex; // -1 meaning playing the boss
    private EntityStats _playerEntity;

    private bool _isGameOngoing;

    void Awake()
    {
        Instance = this;
        _adventurers = new List<EntityStats>();
        _isGameOngoing = false;
    }

    void Start()
    {
        StartLevel(true, 5);
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
            _adventurers.Add(SpawnAdventurer());
        }

        if (!isPlayingBoss)
        {
            _playerEntity = SpawnAdventurer();
            _adventurers.Add(_playerEntity);
            _currAdventureIndex = advSpawnCount - 1;
        }
        else
        {
            _playerEntity = _bossEntity;
            _currAdventureIndex = -1;
        }
        
        PlayerController.Instance.SetPlayer(_playerEntity);
        PlayerController.Instance.playerHealthbar.OnPlayerCharacterSwitch();
        _isGameOngoing = true;
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
        if (!_isGameOngoing)
        {
            return;
        }
        
        _isGameOngoing = false;
        CardContainerController.Instance.CreateThreeCards();
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

    private void SetPlayerEntity(EntityStats newPlayer)
    {
        _playerEntity = newPlayer;
        PlayerController.Instance.SetPlayer(newPlayer);
    }

    public void CycleAdvEntity(bool next)
    {
        if (_currAdventureIndex == -1)
        {
            Debug.LogWarning("Cycling happened when playing as a boss");
            return;
        }
        
        if (next)
        {
            _currAdventureIndex = _currAdventureIndex == _adventurers.Count-1 ? 0 : _currAdventureIndex+1;
        }
        else
        {
            _currAdventureIndex = _currAdventureIndex == 0 ? _adventurers.Count-1 : _currAdventureIndex-1;
        }
        SetPlayerEntity(_adventurers[_currAdventureIndex]);
    }

    public EntityStats GetPlayerEntity()
    {
        return _playerEntity;
    }
    
}
