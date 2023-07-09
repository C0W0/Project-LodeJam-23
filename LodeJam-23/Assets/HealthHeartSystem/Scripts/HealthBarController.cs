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

    private void Start()
    {
        // Should I use lists? Maybe :)
        int maxHealth = (int)GameManager.Instance.GetPlayerEntity().GetMaxHealth();
        heartContainers = new GameObject[maxHealth];
        heartFills = new Image[maxHealth];

        InstantiateHeartContainers();
        UpdateHeartsHUD();
    }

    public override void OnPlayerHealthChange()
    {
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
            if (i < GameManager.Instance.GetPlayerEntity().GetMaxHealth())
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
            if (i < GameManager.Instance.GetPlayerEntity().GetCurrentHealth())
            {
                heartFills[i].fillAmount = 1;
            }
            else
            {
                heartFills[i].fillAmount = 0;
            }
        }

        float currHealth = GameManager.Instance.GetPlayerEntity().GetCurrentHealth();
        if (currHealth % 1 != 0)
        {
            int lastPos = Mathf.FloorToInt(currHealth);
            heartFills[lastPos].fillAmount = currHealth % 1;
        }
    }

    void InstantiateHeartContainers()
    {
        for (int i = 0; i < GameManager.Instance.GetPlayerEntity().GetMaxHealth(); i++)
        {
            GameObject temp = Instantiate(heartContainerPrefab);
            temp.transform.SetParent(heartsParent, false);
            heartContainers[i] = temp;
            heartFills[i] = temp.transform.Find("HeartFill").GetComponent<Image>();
        }
    }
}
