using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    public TMP_InputField createInput;
    public TMP_InputField joinInput;
    public TMP_Dropdown floorDropdown;
    public TMP_Dropdown characterDropdown;
    [SerializeField] private Button quitButton;

    public byte maxPlayers;

    void Start()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        quitButton.onClick.AddListener(() => { SceneManager.LoadScene(0); });
    }

    public void OnCharacterSelectChanged()
    {
        string selectedCharacter = characterDropdown.options[characterDropdown.value].text;
        ExitGames.Client.Photon.Hashtable playerProps = new ExitGames.Client.Photon.Hashtable
    {
        { "character", selectedCharacter }
    };
        PhotonNetwork.LocalPlayer.SetCustomProperties(playerProps);
    }

    public void CreateBtn()
    {
        OnCharacterSelectChanged();
        string roomName = createInput.text.Trim();
        if (string.IsNullOrEmpty(roomName))
        {
            Debug.LogWarning("Room name cannot be empty.");
            return;
        }

        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = maxPlayers;

        string selectedFloor = floorDropdown.options[floorDropdown.value].text;

        roomOptions.CustomRoomProperties = new ExitGames.Client.Photon.Hashtable
    {
        { "floor", selectedFloor }
    };

        roomOptions.CustomRoomPropertiesForLobby = new string[] { "floor" };

        PhotonNetwork.CreateRoom(roomName, roomOptions);
    }

    public void JoinBtn()
    {
        OnCharacterSelectChanged();
        PhotonNetwork.JoinRoom(joinInput.text);
    }

    public override void OnJoinedRoom()
    {
        object floorName;
        if (PhotonNetwork.CurrentRoom.CustomProperties.TryGetValue("floor", out floorName))
        {
            string sceneToLoad = floorName as string;
            PhotonNetwork.LoadLevel(sceneToLoad);
        }
        else
        {
            Debug.LogWarning("No floor defined in room properties.");
        }
    }

}