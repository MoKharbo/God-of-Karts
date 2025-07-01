using UnityEngine;

public class enemyspanwer : MonoBehaviour
{
    [Header("References")]
    public TimeTrialManager timeTrialManager;
    public GameObject enemyPrefab;
    public Transform spawnPoint;

    [Header("Settings")]
    public float triggerThreshold = 20f;

    private bool hasSpawned = false;

    void Update()
    {
        if (hasSpawned || timeTrialManager == null || enemyPrefab == null)
            return;

        float timeLeft = Mathf.Max(0, timeTrialManager.timeLimit - timeTrialManager.timer);

        if (timeLeft <= triggerThreshold)
        {
            SpawnEnemy();
            hasSpawned = true;
        }
    }

    private void SpawnEnemy()
    {
        Vector3 position = spawnPoint != null ? spawnPoint.position : Vector3.zero;
        Instantiate(enemyPrefab, position, Quaternion.identity);
        Debug.Log("Enemy spawned due to low time!");
    }
}
