using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIController : MonoBehaviour
{
    public static UIController instance;
    public GameObject disqualifyPanel;
    public GameObject freezePanel;
    public GameObject dizzyPanel;
    public TMP_Text freezeText;
    public TMP_Text dizzyText;
    public float freezeTimer;
    public float dizzyTimer;
    public float minTimeToFreeze = 10f;
    public float minTimeToDizzy = 10f;
    public bool startFreeze;
    public bool showedDisInfo;
    public bool startDizzy;
    int transformIndex = 0;
    public List<Transform> disqualifiedTransformA = new List<Transform>();
    public List<Transform> disqualifiedTransformB = new List<Transform>();
    Action acFreeze;
    Action acDizzy;


    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(this);
    }

    private void Update()
    {
        if (startFreeze)
        {
            if(freezeTimer >= 0)
            {
                freezeTimer -= Time.deltaTime;
                freezeText.text = $"FROZEN FOR {freezeTimer.ToString("00:00")}";
            }
            else
            {
                startFreeze = false;
                freezePanel.SetActive(false);
                acFreeze.Invoke();
            }
        }
        else if (startDizzy)
        {
            if (dizzyTimer >= 0)
            {
                dizzyTimer -= Time.deltaTime;
                dizzyText.text = $"DIZZY FOR {dizzyTimer.ToString("00:00")}";
            }
            else
            {
                startDizzy = false;
                dizzyPanel.SetActive(false);
                acDizzy.Invoke();
            }
        }

    }

    public void ShowDisqualifey()
    {
        disqualifyPanel.SetActive(true);
        showedDisInfo = true;
    }

    public void FreezePlayer(Action action)
    {
        startFreeze = true;
        freezeTimer = minTimeToFreeze;
        freezePanel.SetActive(true);
        freezeText.text = $"FROZEN FOR {freezeTimer.ToString("00:00")}";
        acFreeze = action;
    }

    public void MakePlayerDizzy(Action action)
    {
        startDizzy = true;
        dizzyTimer = minTimeToDizzy;
        dizzyPanel.SetActive(true);
        dizzyText.text = $"DIZZY FOR {freezeTimer.ToString("00:00")}";
        acDizzy = action;
    }

    public Transform DisqualifyTransform()
    {
        if (PhotonNetwork.LocalPlayer.CustomProperties["Team"].Equals("A"))
        {
            transformIndex = UnityEngine.Random.Range(0, disqualifiedTransformA.Count);
            return disqualifiedTransformA[transformIndex];
        }
        else
        {
            transformIndex = UnityEngine.Random.Range(0, disqualifiedTransformB.Count);
            return disqualifiedTransformB[transformIndex];
        }
      
    }

    public void RemoveTransform(string team)
    {
        if (team.Equals("A"))
        {
            disqualifiedTransformA.RemoveAt(transformIndex);
        }
        else
        {
            disqualifiedTransformB.RemoveAt(transformIndex);
        }
        
    }
}
