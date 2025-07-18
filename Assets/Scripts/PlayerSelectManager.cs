// PlayerSelectManager.cs
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections.Generic;

public class PlayerSelectManager : MonoBehaviour
{
    [System.Serializable]
    public class PlayerUI
    {
        public GameObject panel;           // Conteneur UI du joueur
        public TextMeshProUGUI message;    // Message de statut du joueur
        public Transform previewSlot;      // Slot de prévisualisation du personnage
    }

    public List<GameObject> availableCharacters;
    public PlayerUI[] players = new PlayerUI[3];

    private bool[] isActive = new bool[3];
    private bool[] hasValidated = new bool[3];
    private int[] selectedIndexes = new int[3];
    private GameObject[] currentPreviews = new GameObject[3];

    private void Awake()
    {
        if (FindObjectOfType<PlayerSelectionData>() == null)
        {
            new GameObject("PlayerSelectionData").AddComponent<PlayerSelectionData>();
        }
    }

    void Start()
    {
        isActive[0] = true;
        for (int i = 1; i < 3; i++)
            players[i].panel.SetActive(false);

        UpdateUI();
        ChangeSelection(0, 0); // Premier perso pour le joueur 1
    }

    void Update()
    {
        if (!isActive[1] && Input.GetKeyDown(KeyCode.Z))
        {
            isActive[1] = true;
            players[1].panel.SetActive(true);
            UpdateUI();
            ChangeSelection(1, 0);
        }

        if (!isActive[2] && Input.GetKeyDown(KeyCode.X))
        {
            isActive[2] = true;
            players[2].panel.SetActive(true);
            UpdateUI();
            ChangeSelection(2, 0);
        }

        for (int i = 0; i < 3; i++)
        {
            if (!isActive[i] || hasValidated[i]) continue;

            if (GetLeftInput(i)) ChangeSelection(i, -1);
            if (GetRightInput(i)) ChangeSelection(i, 1);
            if (GetValidateInput(i)) Validate(i);
        }
    }

    void ChangeSelection(int playerIndex, int direction)
    {
        if (availableCharacters.Count == 0) return;

        if (currentPreviews[playerIndex])
            Destroy(currentPreviews[playerIndex]);

        selectedIndexes[playerIndex] = (selectedIndexes[playerIndex] + direction + availableCharacters.Count) % availableCharacters.Count;

        GameObject prefab = availableCharacters[selectedIndexes[playerIndex]];
        currentPreviews[playerIndex] = Instantiate(prefab, players[playerIndex].previewSlot);
        currentPreviews[playerIndex].transform.localPosition = Vector3.zero;
    }

    void Validate(int playerIndex)
    {
        hasValidated[playerIndex] = true;
        GameObject selected = availableCharacters[selectedIndexes[playerIndex]];
        availableCharacters.Remove(selected);

        if (currentPreviews[playerIndex])
            Destroy(currentPreviews[playerIndex]);

        // Stocke le prefab pour la scène suivante
        PlayerSelectionData.Instance.selectedCharacters.Add(selected);

        players[playerIndex].message.text = "Ready !";

        CheckLaunchGame();
    }

    void CheckLaunchGame()
    {
        for (int i = 0; i < 3; i++)
        {
            if (isActive[i] && !hasValidated[i]) return;
        }

        SceneManager.LoadScene("Game");
    }

    void UpdateUI()
    {
        for (int i = 0; i < 3; i++)
        {
            if (isActive[i])
                players[i].message.text = "Select your Character";
            else
                players[i].message.text = i == 1 ? "Press Z for join" : "Press X for join";
        }
    }

    bool GetLeftInput(int i)
    {
        return (i == 0 && Input.GetKeyDown(KeyCode.A)) ||
               (i == 1 && Input.GetKeyDown(KeyCode.J)) ||
               (i == 2 && Input.GetKeyDown(KeyCode.Keypad4));
    }

    bool GetRightInput(int i)
    {
        return (i == 0 && Input.GetKeyDown(KeyCode.D)) ||
               (i == 1 && Input.GetKeyDown(KeyCode.L)) ||
               (i == 2 && Input.GetKeyDown(KeyCode.Keypad6));
    }

    bool GetValidateInput(int i)
    {
        return (i == 0 && Input.GetKeyDown(KeyCode.Space)) ||
               (i == 1 && Input.GetKeyDown(KeyCode.Return)) ||
               (i == 2 && Input.GetKeyDown(KeyCode.KeypadPlus));
    }
}
