using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TimelineUI : MonoBehaviour
{
    // only one timeline UI exist in the game
    public static TimelineUI Instance { get; private set; }

    // later on the character tags should be spawned together with each unit
    // refer to the chracter tag prefab
    [SerializeField] GameObject EnemyTagPrefab;
    [SerializeField] GameObject PlayerTagPrefab;


    // the slider bar
    [SerializeField] Transform SliderBarMain;

    // list that holds all the character tags
    private List<GameObject> charaTagList;

    // the enemyUnitList and unit list
    private List<UnitBasic> enemyUnitList;
    private List<UnitBasic> playerUnitList;


    // the start point of the slider
    [SerializeField] float sliderStartPoint;
    // the end point of the slider
    [SerializeField] float sliderEndPoint;
    // the Y value of the slider
    float sliderYValue = 11;


    // bool to check if all the character Tag are created or not
    private bool HasUnitTagsCreated = false;

    private void Awake()
    {
        // check if there's multiple instance for this class, if so destory them and only leave one exist
        if (Instance != null)
        {
            Destroy(this.gameObject);
            // exit the function so no more new instance created
            return;
        }

        // if not instance yet, sign the instance
        Instance = this;

        // instantiate the list
        charaTagList = new List<GameObject>(); 
    }


    private void Start()
    {
        // check how many enemies in game and spawn the enemyslider accordingly
        enemyUnitList = UnitManager.Instance.GetEnemyUnitList();
        // check how many player in game and spawn the playerslider accordingly
        playerUnitList = UnitManager.Instance.GetPlayerUnitList();

    }

    private void Update()
    {
        // if the unit tags not created, creat them
        if (HasUnitTagsCreated == false)
        {
            // spawn the enemy unit tags prefab accordingly
            foreach (UnitBasic enemyUnit in enemyUnitList)
            {
                // instantiate the charact tag as a child to the SliderBar
                GameObject charaTag = Instantiate(EnemyTagPrefab, SliderBarMain);
                // bind the tag to the unit
                enemyUnit.gameObject.GetComponent<IndieEnemyAI>().SetCharacterTagUI(charaTag);
                // set the location of the character tag to 0
                charaTag.transform.localPosition = new Vector3(250, sliderYValue, 0);
                // set the name
                charaTag.GetComponentInChildren<TextMeshProUGUI>().text = enemyUnit.GetUnitName();
                // add it to the tag list
                charaTagList.Add(charaTag);
            }

            // spawn the player unit tags prefab accordingly
            foreach (UnitBasic playerUnit in playerUnitList)
            {
                // instantiate the charact tag as a child to the SliderBar
                GameObject charaTag = Instantiate(PlayerTagPrefab, SliderBarMain);
                // bind the tag to the unit
                playerUnit.gameObject.GetComponent<UnitTurnTimer>().SetCharacterTagUI(charaTag);
                // set the location of the character tag to 0
                charaTag.transform.localPosition = new Vector3(250, -sliderYValue, 0);
                // set the name
                charaTag.GetComponentInChildren<TextMeshProUGUI>().text = playerUnit.GetUnitName();
                // add it to the tag list
                charaTagList.Add(charaTag);
            }


            HasUnitTagsCreated = true;

        }
    }


    // function used to expose the max value for the sldier
    public float GetMaxSliderValue()
    {
        return sliderEndPoint;
    }

    // function used to expose the min value for the sldier
    public float GetMinSliderValue()
    {
        return sliderStartPoint;
    }

    // function used to expose the Y value for the sldier
    public float GetSliderYValue()
    {
        return sliderYValue;
    }


}
