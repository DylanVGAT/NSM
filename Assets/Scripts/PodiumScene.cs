using UnityEngine;
using UnityEngine.SceneManagement;

public class PodiumScene : MonoBehaviour
{
    [SerializeField] private Transform podiumPosition;
    [SerializeField] private Vector3 podiumScale = new Vector3(2f, 2f, 2f);

    private void Start()
    {
        GameObject winner = GameManager.Instance.GetLastAlivePlayer();
        if (winner != null)
        {
            winner.transform.position = podiumPosition.position;
            winner.transform.localScale = podiumScale;

            // Réactive les visuels
            var death = winner.GetComponent<PlayerDeath>();
            if (death != null)
                death.ShowVisuals();

            // Supprime les composants de gameplay
            Component[] comps = winner.GetComponents<Component>();
            foreach (var c in comps)
            {
                if (!(c is Transform) && !(c is SpriteRenderer) && !(c is PlayerDeath))
                    Destroy(c);
            }
        }

    }
}
