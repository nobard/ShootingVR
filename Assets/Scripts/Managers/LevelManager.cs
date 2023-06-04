using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;

public class LevelManager : MonoBehaviour
{
    //в массиве от 0-го индекса идет усложнение
    [SerializeField] private TargetMovementPath[] targetsLines;
    [SerializeField] private TargetBase[] targetsPrefabs;
    [SerializeField] private TargetBase[] simpleTargetsPrefabs;
    [SerializeField] private TextMeshProUGUI scoresText;
    [SerializeField] private int maxLvlTargets = 5;
    [SerializeField] private AudioSource circusMusic;
    [SerializeField] private AudioSource gameOverSound;
    [HideInInspector] public int CurrScores = 0;
    [HideInInspector] public List<TargetBase> targetsList = new List<TargetBase>();
    public List<HardLevelSettings> HardLevels;
    private int currHardLevel = -1;
    private bool isStarted = false;
    private bool isTargetSpawning = false;
    public Timer TimerObject;

    public void PlayCircusMusic()
    {
        circusMusic.Play();
    }

    public void PlayGameOverSound()
    {
        circusMusic.Stop();
        gameOverSound.Play();
    }
    
    public void StartGame()
    {
        isStarted = true;
        TimerObject.StartTimer();
        MakeHarder();
    }

    private void Update()
    {
        if(!isStarted) return;

        if(TimerObject.timeLeft <= 0)
        {
            GameOver();
            return;
        }

        //currHardLevel < HardLevels.Count && поидее не нужно
        if(currHardLevel < HardLevels.Count && CurrScores >= HardLevels[currHardLevel].ScoresToHarder) MakeHarder();

        if(targetsList.Count < maxLvlTargets) StartCoroutine(SpawnTargets());

        UpdateScoresText();
    }

    private void GameOver()
    {
        PlayGameOverSound();
        isStarted = false;
        TimerObject.ResetTimer();
        currHardLevel = -1;
        CurrScores = 0;

        foreach(var target in targetsList)
        {
            target.DestroyTarget();
        }
    }

    private void MakeHarder()
    {
        if(currHardLevel < HardLevels.Count - 1) currHardLevel++;

        StartCoroutine(SpawnTargets());
    }

    //1: 1 линия, 1 мишени
    //2: 1 линия, +2 мишени
    //3: 2 линии, +2 мишени
    //4: 3 линии, +2 мишени
    private IEnumerator SpawnTargets()
    {
        if(!isTargetSpawning)
        {
            isTargetSpawning = true;
            var line = targetsLines[Random.Range(0, HardLevels[currHardLevel].Lines)];
            var prefabNum = Random.Range(0, HardLevels[currHardLevel].Targets);

            TargetBase targetPrefab;
            if(line.isSimpleTargets) targetPrefab = simpleTargetsPrefabs[prefabNum];
            else targetPrefab = targetsPrefabs[prefabNum];

            var target = line.SpawnAndGetTarget(targetPrefab);
            target.Manager = this;  
            targetsList.Add(target);

            yield return new WaitForSeconds(Random.Range(1f, 3f));
            isTargetSpawning = false;
        }       
    }

    private void UpdateScoresText()
    {
        scoresText.text = string.Format("{0:0000}", CurrScores);
    }
}

[System.Serializable]
public class HardLevelSettings
{
    //для последней сложности ScoresToHarder нужно ставить недостижимым
    public int ScoresToHarder;
    public int Lines;
    public int Targets;
}