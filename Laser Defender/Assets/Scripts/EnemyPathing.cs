using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPathing : MonoBehaviour {

    WaveConfig waveConfig;
    List<Transform> waypoints;

    int waypointIndex = 0;
    string objectName;

    // Use this for initialization
    void Start () {

        objectName = gameObject.name.Replace("(Clone)", "");
        waypoints = waveConfig.GetWayPoints();
        transform.position = waypoints[waypointIndex].transform.position;
	}
	
	// Update is called once per frame
	void Update ()
    {
        Move();
    }

    public void SetWaveConfig(WaveConfig waveConfig)
    {
        this.waveConfig = waveConfig;
    }

    private void Move()
    {
        if (waypointIndex <= waypoints.Count - 1)
        {
            var targetPosition = waypoints[waypointIndex].transform.position;
            var movementThisFrame = waveConfig.GetMoveSpeed() * Time.deltaTime;
            transform.position = Vector2.MoveTowards
                (transform.position, targetPosition, movementThisFrame);

            if (transform.position == targetPosition)
            {
                waypointIndex++;
            }
        }
        else
        {
            if (objectName != "Boss")
            {
                Destroy(gameObject);
            }
        }
    }
}
