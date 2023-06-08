using System.Xml;
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
    [SerializeField] private TextMeshProUGUI highScoreText;
    [SerializeField] private int maxLvlTargets = 5;
    [SerializeField] private AudioSource circusMusic;
    [SerializeField] private AudioSource gameOverSound;
    [SerializeField] private List<ProgressGun> progressGuns;
    [HideInInspector] public int CurrScore = 0;
    [HideInInspector] public int HighScore;
    [HideInInspector] public List<TargetBase> targetsList = new List<TargetBase>();
    [SerializeField] private List<GameObject> confettiObjects;
    public List<HardLevelSettings> HardLevels;
    public Timer TimerObject;
    public StaticTarget StartTarget;
    private int currHardLevel = -1;
    private int highScore;
    private bool isStarted = false;
    private bool isTargetSpawning = false;

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
        if(currHardLevel < HardLevels.Count && CurrScore >= HardLevels[currHardLevel].ScoresToHarder) MakeHarder();

        if(targetsList.Count < maxLvlTargets) StartCoroutine(SpawnTargets());

        UpdateScoresText();
    }

    private void GameOver()
    {
        PlayGameOverSound();
        
        if(CurrScore > HighScore) 
        {
            HighScore = CurrScore;
            UpdateHighScoresText();
            LaunchConfetti();
        }

        TimerObject.ResetTimer();
        currHardLevel = -1;
        CurrScore = 0;
        isStarted = false;

        foreach(var target in targetsList)
        {
            target.DestroyTarget();
        }

        targetsList.Clear();
        StartTarget.TargetAnimator.Play("SpawnTarget");

        foreach(var gun in progressGuns)
        {
            if(HighScore >= gun.HighScoreToOpen) gun.Gun.SetActive(true);
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
        scoresText.text = string.Format("{0:0000}", CurrScore);
    }

    private void UpdateHighScoresText()
    {
        highScoreText.text = string.Format("{0:0000}", HighScore);
    }

    private void LaunchConfetti()
    {
        foreach(var confetti in confettiObjects)
        {
            confetti.GetComponent<ParticleSystem>().Play();
            confetti.GetComponent<AudioSource>().Play();
        }
    }
}

[System.Serializable]
public class HardLevelSettings
{
    //для последней сложности ScoresToHarder должен быть недостижимым
    public int ScoresToHarder;
    public int Lines;
    public int Targets;
}

[System.Serializable]
public class ProgressGun
{
    public int HighScoreToOpen;
    public GameObject Gun;
}