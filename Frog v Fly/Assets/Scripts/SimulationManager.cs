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

    // time sim has been running
    private float _time = 0;
    // number of flies the frog has caught
    private float _numOfFliesCaught = 0;
    // how efficient the frog is
    private float _frogEfficiencyValue;

    // objects on the user interface
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
        // get a random position on screen and create a fly there
        Vector3 position = new Vector3(Random.Range(-9, 9), Random.Range(-5, 5), 0);
        fly = (GameObject) Instantiate(flyPrefab, position, Quaternion.identity);


        // get all the user interface objects during run time
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

        // initialize all the text UI objects
        _timer.text = "Time: 0 s";
        _frogMoves.text = "Moves: 0";
        _flyDistance.text = "Fly Distance: 0";
        _fliesCaught.text = "Flies Caught: 0";
        _frogEfficiency.text = "Efficiency: 0 flies\\move";

    }

    // Update is called once per frame
    void Update()
    {
        // if there's no fly on the scene, make a new one
        if (flyActive == false)
        {
            // update the number of flies caught
            _numOfFliesCaught += 1;
            _fliesCaught.text = "Flies Caught: " + _numOfFliesCaught;

            // get a random position on screen and create a fly there
            Vector3 position = new Vector3(Random.Range(-9, 9), Random.Range(-5, 5), 0);
            fly = (GameObject) Instantiate(flyPrefab, position, Quaternion.identity);
            flyActive = true;
        }
        else
        {
            // update the frog's efficiency
            _frogEfficiencyValue = _numOfFliesCaught /_time;
            _frogEfficiency.text = "Efficiency: " + _frogEfficiencyValue + " flies\\sec";
        }

        // update the timer and frog's number of moves
        UpdateTimerUI();
        _frogMoves.text = "Moves: " + frogMovesValue;

    }

    // update the timer
    void UpdateTimerUI(){
        //set timer UI
        _time += Time.deltaTime;
        _timer.text = "Time: " + _time + "s";
    }

    // update the distance between the fly and the frog
    public void UpdateDistance(float distance)
    {
        _flyDistance.text = "Fly Distance: " + distance;
    }

    // restart the simulation
    private void RestartSim()
    {
        SceneManager.LoadScene(1);
    }
}
