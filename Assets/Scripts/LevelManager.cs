using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    //в массиве от 0-го индекса идет усложнение
    [SerializeField] private TargetMovementPath[] targetsLines;

    //через энамы выбирать таргеты
    [SerializeField] private TargetBase[] targetsPrefabs;

    //[SerializeField] private Timer TimerObject;
    //[SerializeField] private Scores ScoresObject;

    //массив шагов для усложнения(очки от которых идет усложнение)
    [SerializeField] private int[] stepsToHarder;
    [HideInInspector] public int CurrScores;
    private int currHardLevel = 0;

    private void Update()
    {
        if(CurrScores >= stepsToHarder[currHardLevel])
        {
            MakeHarder();
        }
    }

    private void MakeHarder()
    {
        SpawnTargets(5);
        currHardLevel++;
    }

    private void SpawnTargets(int count)
    {
        var firstPoint = targetsLines[currHardLevel].PathElements[Random.Range(0, 2)];

        for(var i = 0; i < count; i++)
        {
            var target = Instantiate(targetsPrefabs[currHardLevel], firstPoint.position, firstPoint.rotation);
            target.MovementPath = targetsLines[Random.Range(0, 2)];
            target.Manager = this;
        } 
    }
}