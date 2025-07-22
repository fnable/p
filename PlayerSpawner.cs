using UnityEngine;
using Photon.Pun;
using System.Collections;

public class PlayerSpawner : MonoBehaviourPunCallbacks
{
    public GameObject playerPrefab;
    public Transform[] spawnPoints;

    void Start()
    {
        StartCoroutine(WaitForPhotonAndSpawn());
    }

    IEnumerator WaitForPhotonAndSpawn()
    {
        while (!PhotonNetwork.InRoom)
        {
            Debug.Log("Waiting for Photon to join room...");
            yield return null;
        }

        Debug.Log("Photon joined room, spawning player...");
        SpawnPlayer();
    }

    void SpawnPlayer()
    {
        if (!PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue("character", out object characterNameObj))
        {
            Debug.LogWarning("No character selected. Defaulting to John.");
            characterNameObj = "John";
        }

        string characterName = characterNameObj.ToString();

        Transform spawn = GetRandomSpawnPoint();
        GameObject player = PhotonNetwork.Instantiate("Characters/" + characterName, spawn.position, spawn.rotation);
    }

    Transform GetRandomSpawnPoint()
    {
        if (spawnPoints != null && spawnPoints.Length > 0)
        {
            return spawnPoints[Random.Range(0, spawnPoints.Length)];
        }

        Debug.LogWarning("No spawn points found. Using fallback.");
        return new GameObject("FallbackSpawn").transform;
    }
}
