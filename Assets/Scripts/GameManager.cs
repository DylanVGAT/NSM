using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private GameObject lastAlivePlayer;
    private int totalPlayerCount = -1;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void OnPlayerDeath(GameObject player)
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        // Init total player count une seule fois
        if (totalPlayerCount == -1)
            totalPlayerCount = players.Length;

        int aliveCount = 0;
        GameObject alivePlayer = null;

        foreach (GameObject p in players)
        {
            var pd = p.GetComponent<PlayerDeath>();
            if (p.activeInHierarchy && pd != null && !pd.IsDead)
            {
                aliveCount++;
                alivePlayer = p;
            }
        }

        bool isSolo = totalPlayerCount == 1;
        bool shouldGoToPodium = isSolo || aliveCount == 1;

        if (shouldGoToPodium)
        {
            GameObject toClone = isSolo ? player : alivePlayer;

            lastAlivePlayer = Instantiate(toClone);
            lastAlivePlayer.name = "PodiumWinner";
            DontDestroyOnLoad(lastAlivePlayer);

            foreach (var comp in lastAlivePlayer.GetComponents<MonoBehaviour>())
            {
                if (!(comp is PlayerDeath))
                    Destroy(comp);
            }

            var rb = lastAlivePlayer.GetComponent<Rigidbody2D>();
            if (rb != null) rb.simulated = false;

            StartCoroutine(LoadPodiumAfterDelay());
        }
    }

    private IEnumerator LoadPodiumAfterDelay()
    {
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene("Podium");
    }

    public GameObject GetLastAlivePlayer()
    {
        return lastAlivePlayer;
    }

    public void ResetGame()
    {
        // Détruire le clone podium si il existe
        if (lastAlivePlayer != null)
        {
            Destroy(lastAlivePlayer);
            lastAlivePlayer = null;
        }

        // Détruire tous les joueurs persistants (avec tag Player)
        var players = GameObject.FindGameObjectsWithTag("Player");
        foreach (var p in players)
        {
            Destroy(p);
        }

        // Autres nettoyages nécessaires

        if (PlayerSelectionData.Instance != null)
        {
            PlayerSelectionData.Instance.ResetSelection();
        }

        Debug.Log("[GameManager] Game reset done");
    }

}
