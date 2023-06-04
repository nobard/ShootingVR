using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScenarioManager : MonoBehaviour
{
    [SerializeField] private GameObject weaponLight;
    [SerializeField] private LevelManager levelManager;
    [SerializeField] private GameObject targetLight;
    [SerializeField] private TextMeshProUGUI startUI;
    [SerializeField] private GameObject mainLight;
    [SerializeField] private GameObject gameUI;
    [SerializeField] private AudioSource switchSound;
    [SerializeField] private AudioSource mainSwitchSound;
    public StaticTarget target;

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
        weaponLight.SetActive(true);
        target.TargetAnimator.Play("SpawnTarget");
        yield return new WaitForSeconds(4f);
        
        switchSound.Play();
        targetLight.SetActive(true);
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
        gameUI.SetActive(true);
        targetLight.SetActive(false);
        weaponLight.SetActive(false);
        mainLight.SetActive(true);
        yield return new WaitForSeconds(4f);

        levelManager.PlayCircusMusic();
        yield return new WaitForSeconds(2f);

        levelManager.StartGame();
    }
}
