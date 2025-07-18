using System.Collections.Generic;
using UnityEngine;

public class PlayerSelectionData : MonoBehaviour
{
    public static PlayerSelectionData Instance;

    public List<GameObject> selectedCharacters = new List<GameObject>();

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void ResetSelection()
    {
        selectedCharacters.Clear();
    }

}
