using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Yarn.Unity;

public class HardCut : MonoBehaviour
{
    [SerializeField] private GameObject floor;
    [SerializeField] private GameObject cut;

    [YarnCommand("StartCut1")]
    public void StartCut1()
    {
        cut.SetActive(true);
    }
    
    [YarnCommand("StartCut2")]
    public void StartCut2()
    {
        floor.SetActive(false);
    }

    [YarnCommand("EndCut")]
    public void EndCut()
    {
        cut.SetActive(false);
    }

    [YarnCommand("NextScene")]
    public void NextScene()
    {
        SceneManager.LoadScene("CanonZone1");
    }
    
    [YarnCommand("NextScene2")]
    public void NextScene2()
    {
        SceneManager.LoadScene("HeronFight");
    }
}
