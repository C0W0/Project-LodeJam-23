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
    [SerializeField] 
    private GameObject gameOverScreen;

    public int adventurerSpawnCount = 5;

    private EntityStats _bossEntity;
    private List<EntityStats> _adventurers;
    private GameObject _adventurerTemplate, _bossTemplate, _adventurerHealth;

    private int _currAdventureIndex; // -1 meaning playing the boss
    public int currAdventureIndex
    {
        get { return _currAdventureIndex; }
        set { _currAdventureIndex = value; }
    }
    private EntityStats _playerEntity;

    private bool _isGameOngoing;

    public ApplyBonusCallback ApplyBonus;

    void Awake()
    {
        Instance = this;
        _adventurers = new List<EntityStats>();
        _isGameOngoing = false;
        ApplyBonus = _ => {};

        _bossTemplate = Instantiate(bossPrefab);
        _bossTemplate.SetActive(false);
        _adventurerTemplate = Instantiate(adventurerPrefab);
        _adventurerTemplate.SetActive(false);
    }

    void Start()
    {
        StartNewLevel();
    }

    void Update()
    {
        if (_adventurers.Count == 0)
        {
            EndGame(PlayerController.Instance.IsPlayingBoss);
        }
    }

    public void StartNewLevel()
    {
        bool isPlayingBoss = Random.Range(-1f, 1f) >= 0;
        StartLevel(isPlayingBoss, adventurerSpawnCount);
    }

    private void StartLevel(bool isPlayingBoss, int advSpawnCount)
    {
        {
            gameOverScreen.SetActive(false);
            _bossTemplate.SetActive(true);
            _adventurerTemplate.SetActive(true);
            var baseBossEntity = _bossTemplate.GetComponent<EntityStats>();
            var baseAdvEntity = _adventurerTemplate.GetComponent<EntityStats>();
            ApplyBonus.Invoke(baseBossEntity);
            ApplyBonus.Invoke(baseAdvEntity);
        }
        
        _bossEntity = Instantiate(_bossTemplate, bossSpawnLocation.position, Quaternion.identity).GetComponent<EntityStats>();

        for (int i = 0; i < advSpawnCount-1; i++)
        {
            var advEntity = SpawnAdventurer();
            _adventurers.Add(advEntity);
        }
        var lastAdvEntity = SpawnAdventurer();
        _adventurers.Add(lastAdvEntity);
        
        _bossTemplate.SetActive(false);
        _adventurerTemplate.SetActive(false);
        
        if (!isPlayingBoss)
        {
            _playerEntity = lastAdvEntity;
            _currAdventureIndex = advSpawnCount - 1;
        }
        else
        {
            _playerEntity = _bossEntity;
            _currAdventureIndex = -1;
        }
        
        PlayerController.Instance.SetPlayer(_playerEntity);
        _isGameOngoing = true;
    }

    private EntityStats SpawnAdventurer()
    {
        Transform spawnLocation = adventurerSpawnLocations[Random.Range(0, 2)];
        var adventurer = Instantiate(_adventurerTemplate, spawnLocation.position, Quaternion.identity);
        var component = adventurer.GetComponent<EntityStats>();
        return component;
    }

    public void EndGame(bool isVictory)
    {
        if (!_isGameOngoing)
        {
            return;
        }

        foreach (var entity in _adventurers)
        {
            Destroy(entity.gameObject);
        }
        _adventurers = new List<EntityStats>();
        Destroy(_bossEntity.gameObject);

        _isGameOngoing = false;
        
        if (isVictory)
        {
            CardContainerController.Instance.CreateThreeCards();
        }
        else
        {
            ApplyBonus = _ => {};
            // TODO: display defeat UI
            gameOverScreen.SetActive(true);
            StartLevel(PlayerController.Instance.IsPlayingBoss, adventurerSpawnCount);
        }
        
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

    public bool TryCycleAdvEntity(bool next)
    {
        if (_currAdventureIndex == -1)
        {
            Debug.LogWarning("Cycling happened when playing as a boss");
            return false;
        }

        if (_adventurers.Count <= 1)
        {
            return false;
        }
        
        if (next)
        {
            _currAdventureIndex = _currAdventureIndex == _adventurers.Count-1 ? 0 : _currAdventureIndex+1;
        }
        else
        {
            _currAdventureIndex = _currAdventureIndex == 0 ? _adventurers.Count-1 : _currAdventureIndex-1;
        }

        _currAdventureIndex = Math.Min(_currAdventureIndex, _adventurers.Count-1);
        SetPlayerEntity(_adventurers[_currAdventureIndex]);
        return true;
    }

    public EntityStats GetPlayerEntity()
    {
        return _playerEntity;
    }

    public EntityStats GetBossEntity()
    {
        return _bossEntity;
    }
    
}
