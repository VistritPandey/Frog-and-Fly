using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

public class SimulationManager : MonoBehaviour
{

    [Header("Number of times the frog has moved. ")]
    [ReadOnly]
    public float frogMovesValue;

    [Header("Is there a fly active on the scene?")]
    [ReadOnly]
    public bool flyActive = true;

    [Header("The fly prefab")]
    public GameObject flyPrefab;

    [Header("The current fly in the scene")]
    [ReadOnly]
    public GameObject fly;

    
    private float _time = 0;
    private float _numOfFliesCaught = 0;
    private float _frogEfficiencyValue;

    #region UserInterfaceObjects

        [Header("User Interface")]
        [ReadOnly]
        public GameObject simInterface;

        private Button _restartButton;
        private Text _timer;
        private Text _frogMoves;
        private Text _flyDistance;
        private Text _fliesCaught;
        private Text _frogEfficiency;

    #endregion


    private void Start()
    {
        Vector3 position = new Vector3(Random.Range(-9, 9), Random.Range(-5, 5), 0);
        fly = (GameObject) Instantiate(flyPrefab, position, Quaternion.identity);

        #region GetUserInterfaceObjects

        GameObject infoPanel = simInterface.transform.GetChild(0).gameObject;

        _restartButton = simInterface.transform.GetChild(1).gameObject.GetComponent<Button>();

        _restartButton.onClick.AddListener(RestartSim);

        _timer = infoPanel.transform.GetChild(0).gameObject.GetComponent<Text>();
        _frogMoves = infoPanel.transform.GetChild(1).gameObject.GetComponent<Text>();
        _flyDistance = infoPanel.transform.GetChild(2).gameObject.GetComponent<Text>();
        _fliesCaught = infoPanel.transform.GetChild(3).gameObject.GetComponent<Text>();
        _frogEfficiency = infoPanel.transform.GetChild(4).gameObject.GetComponent<Text>();

        #endregion

        _timer.text = "Time: 0 s";
        _frogMoves.text = "Moves: 0";
        _flyDistance.text = "Fly Distance: 0";
        _fliesCaught.text = "Flies Caught: 0";
        _frogEfficiency.text = "Efficiency: 0 flies\\move";

    }

    void Update()
    {
        if (flyActive == false)
        {
            _numOfFliesCaught += 1;
            _fliesCaught.text = "Flies Caught: " + _numOfFliesCaught;

            Vector3 position = new Vector3(Random.Range(-9, 9), Random.Range(-5, 5), 0);
            fly = (GameObject) Instantiate(flyPrefab, position, Quaternion.identity);
            flyActive = true;
        }
        else
        {
            _frogEfficiencyValue = _numOfFliesCaught /_time;
            _frogEfficiency.text = "Efficiency: " + _frogEfficiencyValue + " flies\\sec";
        }
        UpdateTimerUI();
        _frogMoves.text = "Moves: " + frogMovesValue;

    }
    void UpdateTimerUI(){
        _time += Time.deltaTime;
        _timer.text = "Time: " + _time + "s";
    }

    public void UpdateDistance(float distance)
    {
        _flyDistance.text = "Fly Distance: " + distance;
    }

    private void RestartSim()
    {
        SceneManager.LoadScene(1);
    }
}
