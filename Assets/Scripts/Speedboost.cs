using UnityEngine;

public class Speedboost : MonoBehaviour
{
    [SerializeField] private Carcontroller carcontroller;
    [SerializeField] private Scoredrift scoredrift;
    private KeyCode KeyCode = KeyCode.R;
    private bool Buttoncode => Input.GetKeyDown(KeyCode);
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (scoredrift.score >= 1000 && Buttoncode) 
        {
            carcontroller.maxSpeed += 20f;
        }
    }
}
