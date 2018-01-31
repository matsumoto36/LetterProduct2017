using UnityEngine;

public class ScoreScript : MonoBehaviour
{
    public int count=1000;

    public ScoreOther scoreOther;

    // Use this for initialization
    void Start ()
    {
		scoreOther.ShowScore(count);
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}
}
