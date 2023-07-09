using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public delegate void ApplyBonusCallback(EntityStats entity);

public class CardController : MonoBehaviour
{
    private int _index = -1;
    [SerializeField]
    private TextMeshProUGUI bossBonusDesc, advBonusDesc;

    private ApplyBonusCallback _applyBonusCallback;

    public void Init(int index, CardType cardType, BonusType bossBonus, BonusType advBonus)
    {
        GetComponent<Image>().sprite = CardContainerController.GetSprite(cardType);
        
        float bossBonusMag = CardContainerController.GetMagnitude(cardType, bossBonus);
        float advBonusMag = CardContainerController.GetMagnitude(cardType, advBonus);

        bossBonusDesc.text = CardContainerController.ConstructDesc(bossBonus, bossBonusMag, true);
        advBonusDesc.text = CardContainerController.ConstructDesc(advBonus, advBonusMag, false);

        _applyBonusCallback = entity =>
        {
            float magnitude = entity.IsBoss() ? bossBonusMag : advBonusMag;
            switch (bossBonus)
            {
                case BonusType.Damage:
                    entity.changeDamage(magnitude);
                    break;
                case BonusType.Speed:
                    entity.ChangeSpeed((int)magnitude);
                    break;
                case BonusType.Defence:
                    entity.changeDefence(magnitude);
                    break;
                case BonusType.AmmoSpeed:
                    print("not implemented");
                    break;
                case BonusType.Hp:
                    entity.changeMaxHp(magnitude);
                    break;
                case BonusType.Reload:
                    print("not implemented");
                    break;
            }
        };
        _index = index;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (_index != -1)
        {
            if (Input.GetKeyDown(CardContainerController.KeycodeMap[_index]))
            {
                GameManager.Instance.ApplyBonus = _applyBonusCallback;
                CardContainerController.Instance.DestroyAllCards();
            }
        }
    }
}
