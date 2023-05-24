using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LevelManager : MonoBehaviour
{
    //в массиве от 0-го индекса идет усложнение
    [SerializeField] private TargetMovementPath[] targetsLines;

    //через энамы выбирать таргеты
    [SerializeField] private TargetBase[] targetsPrefabs;
    [SerializeField] private Timer TimerObject;
    [SerializeField] private TextMeshProUGUI scoresText;
    [SerializeField] private int maxLvlTargets = 5;
    [HideInInspector] public int currLvlTargets = 0;

    //массив шагов для усложнения(очки от которых идет усложнение)
    [SerializeField] private int[] stepsToHarder;
    //[HideInInspector] 
    public int CurrScores;
    public List<HardLevelSettings> HardLevels;
    private int currHardLevel = -1;
    private bool isStarted = false;

    public void StartGame()
    {
        MakeHarder();
    }

    private void Update()
    {
        if(!isStarted) return;

        if(currHardLevel < stepsToHarder.Length && CurrScores >= stepsToHarder[currHardLevel])
        {
            MakeHarder();
        }
        if(currLvlTargets < maxLvlTargets)
        {
            StartCoroutine(SpawnTargets(maxLvlTargets - currLvlTargets));
        }

        UpdateScoresText();
    }

    private void MakeHarder()
    {
        currHardLevel++;
        StartCoroutine(SpawnTargets(maxLvlTargets - currLvlTargets));
    }

    //1: 1 линия, 1 мишени
    //2: 1 линия, +2 мишени
    //3: 2 линия, +2 мишени
    private IEnumerator SpawnTargets(int count)
    {
        for(var i = 0; i < count; i++)
        {
            currLvlTargets++;
            var line = targetsLines[Random.Range(0, HardLevels[currHardLevel].Lines)];
            var targetPrefab = targetsPrefabs[Random.Range(0, HardLevels[currHardLevel].Targets)];
            var target = line.SpawnAndGetTarget(targetPrefab);
            target.Manager = this;
            yield return new WaitForSeconds(Random.Range(1f, 3f));   
        }
    }

    private void UpdateScoresText()
    {
        scoresText.text = string.Format($"{CurrScores}");
    }
}

[System.Serializable]
public class HardLevelSettings
{
    public int Lines;
    public int Targets;
}