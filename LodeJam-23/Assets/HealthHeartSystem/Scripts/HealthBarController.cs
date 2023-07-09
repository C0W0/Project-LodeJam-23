/*
 *  Author: ariel oliveira [o.arielg@gmail.com]
 */

using UnityEngine;
using UnityEngine.UI;

public class HealthBarController : BaseHealthBar
{
    private GameObject[] heartContainers;
    private Image[] heartFills;

    public Transform heartsParent;
    public GameObject heartContainerPrefab;

    private EntityStats _entity;

    public override void OnEntityHealthChange()
    {
        UpdateHeartsHUD();
    }

    public override void OnAttachedEntitySwitch(EntityStats entity)
    {
        _entity = entity;
        // Should I use lists? Maybe :)
        int maxHealth = (int)_entity.GetMaxHealth();

        if (heartContainers != null)
        {
            foreach (var obj in heartContainers)
            {
                Destroy(obj);
            }
        }
        
        heartContainers = new GameObject[maxHealth];
        heartFills = new Image[maxHealth];

        InstantiateHeartContainers();
        UpdateHeartsHUD();
    }

    private void UpdateHeartsHUD()
    {
        SetHeartContainers();
        SetFilledHearts();
    }

    void SetHeartContainers()
    {
        for (int i = 0; i < heartContainers.Length; i++)
        {
            if (i < _entity.GetMaxHealth())
            {
                heartContainers[i].SetActive(true);
            }
            else
            {
                heartContainers[i].SetActive(false);
            }
        }
    }

    void SetFilledHearts()
    {
        for (int i = 0; i < heartFills.Length; i++)
        {
            if (i < _entity.GetCurrentHealth())
            {
                heartFills[i].fillAmount = 1;
            }
            else
            {
                heartFills[i].fillAmount = 0;
            }
        }

        float currHealth = _entity.GetCurrentHealth();
        if (currHealth % 1 != 0)
        {
            int lastPos = Mathf.FloorToInt(currHealth);
            heartFills[lastPos].fillAmount = currHealth % 1;
        }
    }

    void InstantiateHeartContainers()
    {
        for (int i = 0; i < _entity.GetMaxHealth(); i++)
        {
            GameObject temp = Instantiate(heartContainerPrefab);
            temp.transform.SetParent(heartsParent, false);
            heartContainers[i] = temp;
            heartFills[i] = temp.transform.Find("HeartFill").GetComponent<Image>();
        }
    }
}
