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
                    entity.ChangeDamage(magnitude);
                    break;
                case BonusType.Speed:
                    entity.ChangeSpeed(magnitude);
                    break;
                case BonusType.Defence:
                    entity.ChangeDefence(magnitude);
                    break;
                case BonusType.AmmoSpeed:
                    entity.ChangeBulletSpeed(magnitude);
                    break;
                case BonusType.Hp:
                    entity.ChangeMaxHp(magnitude);
                    break;
                case BonusType.Spawn:
                    GameManager.Instance.adventurerSpawnCount ++;
                    break;
            }
        };
        _index = index;
    }

    public void OnCardClick()
    {
        GameManager.Instance.ApplyBonus = _applyBonusCallback;
        CardContainerController.Instance.DestroyAllCards();
    }
}
