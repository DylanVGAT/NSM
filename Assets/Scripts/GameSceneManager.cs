using UnityEngine;

public class GameSceneManager : MonoBehaviour
{
    public Transform[] spawnPoints; // Player_1, Player_2, Player_3

    void Start()
    {
        var selected = PlayerSelectionData.Instance.selectedCharacters;

        for (int i = 0; i < spawnPoints.Length; i++)
        {
            if (i < selected.Count)
            {
                // Instancie le perso
                GameObject instance = Instantiate(selected[i], spawnPoints[i]);
                instance.transform.localPosition = Vector3.zero;

                // Récupère le groundCheck et l'assigne
                Transform groundCheck = instance.transform.Find("GroundCheck");
                MovementManager mm = spawnPoints[i].GetComponent<MovementManager>();

                if (groundCheck != null && mm != null)
                    mm.AssignGroundCheck(groundCheck);
            }
            else
            {
                // AUCUN perso sélectionné pour ce slot → on désactive le GameObject
                spawnPoints[i].gameObject.SetActive(false);
            }
        }
    }
}
