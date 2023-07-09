using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.EventSystems;
using Random = UnityEngine.Random;

public enum CardType
{
    Diamond = 0,
    DoubleGold = 1,
    Gold = 2,
    Silver = 3,
}

public enum BonusType
{
    Damage, Speed, AmmoSpeed, Hp, Defence, Reload
}

public class CardContainerController : MonoBehaviour
{
    public static CardContainerController Instance;

    #region utility
    
    public static readonly Dictionary<int, KeyCode> KeycodeMap = new Dictionary<int, KeyCode>
    {
        {
            1, KeyCode.Alpha1
        },
        {
            2, KeyCode.Alpha2
        },
        {
            3, KeyCode.Alpha3
        },
    };

    public static Sprite GetSprite(CardType type)
    {
        return Instance.cardSprites[(int)type];
    }

    public static string ConstructDesc(BonusType type, float magnitude, bool isBoss)
    {
        string pronoun = isBoss ? "Boss" : "Adventurer";
        string magnitudeFormatted = $"{magnitude:0.##}";
        switch (type)
        {
            case BonusType.Damage:
                return $"{pronoun} deals extra {magnitudeFormatted} damage";
            case BonusType.Speed:
                return $"{pronoun} receives a speed boost";
            case BonusType.Defence:
                return $"{pronoun} receives {magnitudeFormatted} extra defence";
            case BonusType.AmmoSpeed:
                return $"{pronoun} shoots faster rounds";
            case BonusType.Hp:
                return $"{pronoun} receives {magnitudeFormatted} extra HP";
            case BonusType.Reload:
                return $"{pronoun} reloads faster";
        }
        return "";
    }

    public static float GetMagnitude(CardType card, BonusType bonus)
    {
        int randOffset = Random.Range(-1, 2);
        int val = 4 - (int)card + card == CardType.Diamond ? randOffset + 1 : randOffset;
        int randResult = Math.Max(1, val);
        switch (bonus)
        {
            case BonusType.Damage:
                return randResult;
            case BonusType.Speed:
                return (4 - (int)card) / 2f;
            case BonusType.Defence:
                return (4 - (int)card) / 2f;;
            case BonusType.AmmoSpeed: 
                return (4 - (int)card) / 2f;;
            case BonusType.Hp:
                return randResult;
            case BonusType.Reload:
                return (4 - (int)card) / 2f;;
        }
        return 0f;
    }
    
    #endregion

    [SerializeField]
    private GameObject cardPrefab;
    [SerializeField]
    private List<Sprite> cardSprites;

    private GameObject[] _cards = new GameObject[3];

    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void CreateThreeCards()
    {
        
        var bonuses = Enum.GetValues(typeof(BonusType))     
            .OfType<BonusType>().OrderBy(_ => Guid.NewGuid()).ToList();
        _cards[0] = Instantiate(cardPrefab, this.transform);
        _cards[0].GetComponent<CardController>().Init(1, GetRandomCard(), bonuses[Random.Range(0, 6)], bonuses[Random.Range(0, 6)]);
        _cards[0].transform.SetPositionAndRotation(_cards[0].transform.position - new Vector3(8 * _cards[0].GetComponent<RectTransform>().sizeDelta.x, 0), _cards[0].transform.rotation);
        
        _cards[1] = Instantiate(cardPrefab, this.transform);
        _cards[1].GetComponent<CardController>().Init(2, GetRandomCard(), bonuses[Random.Range(0, 6)], bonuses[Random.Range(0, 6)]);
        
        _cards[2] = Instantiate(cardPrefab, this.transform);
        _cards[2].transform.SetPositionAndRotation(_cards[2].transform.position + new Vector3(8 * _cards[2].GetComponent<RectTransform>().sizeDelta.x, 0), _cards[2].transform.rotation);
        _cards[2].GetComponent<CardController>().Init(3, GetRandomCard(), bonuses[Random.Range(0, 6)], bonuses[Random.Range(0, 6)]);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            CreateThreeCards();
        }
    }

    /**
     * 5% diamond, 15% double gold, 30% gold, 50% silver
     */
    private CardType GetRandomCard()
    {
        int selection = Random.Range(0, 100);
        if (selection <= 5)
            return CardType.Diamond;
        if (selection <= 20)
            return CardType.DoubleGold;
        if (selection <= 50)
            return CardType.Gold;
        return CardType.Silver;
    }

    public void DestroyAllCards()
    {
        foreach (var cardObj in _cards)
        {
            Destroy(cardObj);
        }

        _cards = new GameObject[3];
    }
}
