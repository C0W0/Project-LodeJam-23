using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardContainerController : MonoBehaviour
{
    [SerializeField]
    private GameObject cardPrefab;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void CreateThreeCards()
    {
        GameObject newCard1 = Instantiate(cardPrefab, this.transform);
        newCard1.GetComponent<CardController>().index = 1;
        newCard1.transform.SetPositionAndRotation(newCard1.transform.position - new Vector3(8 * newCard1.GetComponent<RectTransform>().sizeDelta.x, 0), newCard1.transform.rotation);
        GameObject newCard3 = Instantiate(cardPrefab, this.transform);
        newCard3.GetComponent<CardController>().index = 2;
        GameObject newCard2 = Instantiate(cardPrefab, this.transform);
        newCard2.transform.SetPositionAndRotation(newCard2.transform.position + new Vector3(8 * newCard2.GetComponent<RectTransform>().sizeDelta.x, 0), newCard2.transform.rotation);
        newCard2.GetComponent<CardController>().index = 3;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            CreateThreeCards();
        }
    }
}
