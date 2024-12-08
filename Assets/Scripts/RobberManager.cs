using UnityEngine;

public class RobberManager : MonoBehaviour
{
    public GameObject robberPrefab; // Prefab of the robber
    public Transform[] spawnPoints; // Array of spawn points for robbers
    public float killDistance = 2f; // Distance within which the player can kill a robber

    private int score = 0; // Player's score

    private void Start()
    {
        SpawnRobbers();
    }

    private void Update()
    {
        CheckForKill();
    }

    private void SpawnRobbers()
    {
        foreach (Transform spawnPoint in spawnPoints)
        {
            Instantiate(robberPrefab, spawnPoint.position, Quaternion.identity);
        }
    }

    private void CheckForKill()
    {
        GameObject[] robbers = GameObject.FindGameObjectsWithTag("Robber");

        foreach (GameObject robber in robbers)
        {
            float distance = Vector2.Distance(transform.position, robber.transform.position);

            if (distance <= killDistance && Input.GetKeyDown(KeyCode.Space))
            {
                Destroy(robber);
                score++;
                Debug.Log("Robber killed! Score: " + score);
            }
        }
    }
}
