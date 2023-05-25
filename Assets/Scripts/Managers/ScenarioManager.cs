using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScenarioManager : MonoBehaviour
{
    [SerializeField] private GameObject pistolLight;
    [SerializeField] private LevelManager levelManager;
    [SerializeField] private GameObject TargetLight;
    [SerializeField] private TextMeshProUGUI StartUI;
    [SerializeField] private GameObject MainLight;
    [SerializeField] private GameObject GameUI;
    [SerializeField] private AudioSource switchSound;
    [SerializeField] private AudioSource mainSwitchSound;
    [SerializeField] private AudioSource circusMusic;


    private void Start()
    {
        StartCoroutine(PlayStartScenario());
    }

    private IEnumerator PlayStartScenario()
    {
        //ВРЕМЕННО через старт метод запускать и временно задержка 5 секунд
        yield return new WaitForSeconds(1f);


        yield return new WaitForSeconds(3f);
        switchSound.Play();
        pistolLight.SetActive(true);
        yield return new WaitForSeconds(4f);
        switchSound.Play();
        //загарается фонарь на мишень
        //UI предлагается стрельнуть в мишень;
    }

    public void PlayMainScenario()
    {
        StartCoroutine(PlayMainScenarioCoroutine());
    }

    private IEnumerator PlayMainScenarioCoroutine()
    {
        //закрыть UI
        yield return new WaitForSeconds(2f);
        mainSwitchSound.Play();
        //включается основной свет
        yield return new WaitForSeconds(4f);
        circusMusic.Play();
        //немного подождать
        //включается UI игры
        levelManager.StartGame();
    }
}
