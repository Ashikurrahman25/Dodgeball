using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PhotonManager : MonoBehaviourPunCallbacks
{

    public GameObject roomJoinPopup;
    public GameObject playerListPopup;
    public GameObject loadingPanel;
    public TMP_Text playerCountA;
    public TMP_Text playerCountB;
    public TMP_Text roomNameText;
    public TMP_Text countDownText;
    public TMP_InputField roomName;

    public GameObject playerListPrefabA;
    public GameObject playerListPrefabB;
    public RectTransform playerListParentA;
    public RectTransform playerListParentB;
    public GameObject startButton;
    public GameObject countdown;
    public List<GameObject> playerList;

    public int maxPlayer = 6;
    public int teamAPlayer = 0;
    public int teamBPlayer = 0;
    public double startTime;
    double timerIncrementValue;
    public double timer = 60;
    public bool startTimer;
    TimeSpan time;
    Dictionary<string, string> playerProperties = new Dictionary<string, string>();

    void Start()
    {
        loadingPanel.SetActive(true);
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.ConnectUsingSettings();
    }
    public override void OnConnectedToMaster()
    {
        if (PhotonNetwork.CloudRegion.Trim().Contains("eu"))
            loadingPanel.SetActive(false);
        else
        {
            PhotonNetwork.Disconnect();
            StartCoroutine(DelayConnect());
        }


        Debug.Log(PhotonNetwork.CloudRegion);
        Debug.Log("Connected to master");
    }


    IEnumerator DelayConnect()
    {
        yield return new WaitForSeconds(1f);
        PhotonNetwork.ConnectToRegion("eu");
        Debug.Log("Connecting to eu");
    }
    public void JoinLobby()
    {
        if (PhotonNetwork.IsConnected)
        {
            loadingPanel.SetActive(true);
            if (string.IsNullOrEmpty(PhotonNetwork.NickName))
                PhotonNetwork.NickName = "GUEST" + UnityEngine.Random.Range(999, 18000).ToString();

            PhotonNetwork.JoinLobby();
        }

    }

    public override void OnJoinedLobby()
    {
        loadingPanel.SetActive(false);
        roomJoinPopup.SetActive(true);
    }

    public override void OnJoinedRoom()
    {
        roomNameText.text = $"Room: {PhotonNetwork.CurrentRoom.Name}";

        //if (PhotonNetwork.CurrentRoom.PlayerCount > 1)
        //{



        //}
        //else
        //{
        //    GameObject g = null;
        //    int temp = UnityEngine.Random.Range(0, 2);

        //    if (temp == 1)
        //    {
        //        AddPlayerProperty("Team", "A");
        //        //g = Instantiate(playerListPrefabA, playerListParentA);

        //    }
        //    else
        //    {
        //        AddPlayerProperty("Team", "B");
        //        //g = Instantiate(playerListPrefabB, playerListParentB);
        //    }

        //    //playerList.Add(g);
        //    //g.GetComponentInChildren<TMP_Text>().text = PhotonNetwork.NickName;

        //}

        CheckPlayersTeam();
        if (teamAPlayer > teamBPlayer)
            AddPlayerProperty("Team", "B");
        else if (teamAPlayer == teamBPlayer)
            AddPlayerProperty("Team", "A");

        SetPlayerProperties(PhotonNetwork.LocalPlayer);

        Waiter.Wait(1f, () =>
        {
          
            roomJoinPopup.SetActive(false);
            playerListPopup.SetActive(true);
            loadingPanel.SetActive(false);


            SetCustomProp();
            UpdatePlayerList();
            UpdatePlayerCount();

        });


        //Waiter.Wait(3f, () =>
        //{
        //    roomJoinPopup.SetActive(false);
        //    playerListPopup.SetActive(true);
        //    loadingPanel.SetActive(false);


        //    SetCustomProp();
        //    UpdatePlayerCount();
        //    UpdatePlayerList();
        //});
    }

    IEnumerator WaitAndSet()
    {
        yield return new WaitForSeconds(2f);

        roomJoinPopup.SetActive(false);
        playerListPopup.SetActive(true);
        loadingPanel.SetActive(false);


        UpdatePlayerList();
        SetCustomProp();
        UpdatePlayerCount();
    }

    public void SetCustomProp()
    {
        //int temp = UnityEngine.Random.Range(0, 2);

        //if (temp == 1)
        //    playerProperties.Add("Team", "A");
        //else
        //    playerProperties.Add("Team", "B");

        Debug.Log(GetPlayerProperty("Team", PhotonNetwork.LocalPlayer));

        //Debug.Log(playerProperties["Team"]);
        ////PhotonNetwork.LocalPlayer.SetCustomProperties(playerProperties);
        //Debug.Log(PhotonNetwork.LocalPlayer.ToStringFull());
    }

    // Update is called once per frame
    void Update()
    {
        if (!startTimer) return;

        timerIncrementValue = PhotonNetwork.Time - startTime;
        time = TimeSpan.FromSeconds(timer - timerIncrementValue);
        countDownText.text = $"{time.Minutes}: {time.Seconds}";

        if (timerIncrementValue >= timer)
        {
            startTimer = false;

            if (PhotonNetwork.IsMasterClient)
                StartGame();

            Debug.Log(timerIncrementValue);
        }
    }

    public void UpdatePlayerCount()
    {
        //CheckPlayersTeam();
        //playerCountA.text = $"Team A  {playerListParentA.childCount}/{5}";
        //playerCountB.text = $"Team B  {playerListParentB.childCount}/{5}";
    }

    public void UpdatePlayerList()
    {
        for (int i = 0; i < playerList.Count; i++)
        {
            Destroy(playerList[i]);
        }
        playerList.Clear();
        for (int i = 0; i < PhotonNetwork.CurrentRoom.PlayerCount; i++)
        {
            GameObject g = null;

            if (PhotonNetwork.PlayerList[i].CustomProperties["Team"].Equals("A"))
                g = Instantiate(playerListPrefabA, playerListParentA);
            else if(PhotonNetwork.PlayerList[i].CustomProperties["Team"].Equals("B"))
                g = Instantiate(playerListPrefabB, playerListParentB);

            playerList.Add(g);
            g.GetComponentInChildren<TMP_Text>().text = PhotonNetwork.PlayerList[i].NickName;
        }
        //playerCount.text = PhotonNetwork.PlayerList[0].NickName;
    }


    public void LoadScene()
    {
        SceneManager.LoadScene("Game");
    }


    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        if (newMasterClient.ActorNumber == PhotonNetwork.LocalPlayer.ActorNumber)
        {
            startTime = double.Parse(PhotonNetwork.CurrentRoom.CustomProperties["StartTime"].ToString());
            startTimer = true;
            startButton.SetActive(true);
            countdown.SetActive(true);
        }
    }

    IEnumerator UpdateList()
    {
        yield return new WaitForSeconds(1f);
        UpdatePlayerCount();
        UpdatePlayerList();
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {

        Waiter.Wait(1f, () =>
        {
            UpdatePlayerList();
            UpdatePlayerCount();
        });

        if (PhotonNetwork.PlayerList.Length >= 2)
        {
            startButton.SetActive(true);
            countdown.SetActive(true);
            if (PhotonNetwork.LocalPlayer.IsMasterClient)
            {
                ExitGames.Client.Photon.Hashtable customeValue = new ExitGames.Client.Photon.Hashtable();
                startTime = PhotonNetwork.Time;
                startTimer = true;
                customeValue.Add("StartTime", startTime);
                PhotonNetwork.CurrentRoom.SetCustomProperties(customeValue);
            }
            else
            {
                startTime = double.Parse(PhotonNetwork.CurrentRoom.CustomProperties["StartTime"].ToString());
                startTimer = true;
            }
        }

    }

    public void StartGame()
    {
        if (PhotonNetwork.IsMasterClient)
            PhotonNetwork.CurrentRoom.IsOpen = false;
        startButton.SetActive(false);
        PhotonNetwork.LoadLevel(PlayerPrefs.GetString("Level","Game"));
    }
    
    public void CreateRoom()
    {
        loadingPanel.SetActive(true);
        string roomNameTxt = "";

        if (!string.IsNullOrEmpty(roomName.text))
            roomNameTxt = roomName.text;
        else
            roomNameTxt = "XYZ " + UnityEngine.Random.Range(10000, 100000);



        RoomOptions newRoom = new RoomOptions()
        {
            MaxPlayers = (byte)maxPlayer,
            CleanupCacheOnLeave = false
        };
        PhotonNetwork.CreateRoom(roomNameTxt, newRoom);

    }
    public void JoinRoom()
    {
        loadingPanel.SetActive(true);
        if (!string.IsNullOrEmpty(roomName.text))
        {
            RoomOptions room = new RoomOptions();
            room.MaxPlayers = (byte)maxPlayer;
            room.CleanupCacheOnLeave = false;
            PhotonNetwork.JoinOrCreateRoom(roomName.text, room,TypedLobby.Default);
        }
        else
        {
            PhotonNetwork.JoinRandomRoom();
        }

    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        RoomOptions room = new RoomOptions();
        room.MaxPlayers = (byte)maxPlayer;
        room.CleanupCacheOnLeave = false;
        PhotonNetwork.CreateRoom("XYZ " + UnityEngine.Random.Range(10000, 100000));
    }

    IEnumerator ShowCountdown()
    {
        yield return new WaitForSeconds(1f);
        startTime = double.Parse(PhotonNetwork.CurrentRoom.CustomProperties["StartTime"].ToString());
        startTimer = true;
        countdown.SetActive(true);
    }

    public void CheckPlayersTeam()
    {
                
        foreach (Player p in PhotonNetwork.PlayerList)
        {
            Debug.Log(p.CustomProperties.Count);
            object isPlayerReady;
            if (p.CustomProperties.TryGetValue("Team", out isPlayerReady))
            {

                if (isPlayerReady.ToString().Equals("A"))
                    teamAPlayer++;
                else
                    teamBPlayer++;
            }
            else
            {
                Debug.Log("No property found");
            }
        }
    }
    public void AddPlayerProperty(string key, string value)
    {
        if (playerProperties.ContainsKey(key))
            playerProperties[key] = value;
        else
            playerProperties.Add(key, value);
    }

    public string GetPlayerProperty(string key, Player player)
    {
        ExitGames.Client.Photon.Hashtable properties = player.CustomProperties;
        Debug.Log("Player Properties: " + properties.Count);

        if (properties.ContainsKey(key))
            return properties[key].ToString();
        else
            Debug.LogWarning("Custom property doesn't found!");
        return "";
    }

    public void SetPlayerProperties(Player player)
    {
        player.SetCustomProperties(AddPlayerCustomProperties());
    }

    ExitGames.Client.Photon.Hashtable AddPlayerCustomProperties()
    {
        ExitGames.Client.Photon.Hashtable playerProperties = new ExitGames.Client.Photon.Hashtable();

        foreach (var property in this.playerProperties)
        {
            playerProperties.Add(property.Key, property.Value);
        }

        return playerProperties;
    }
}
