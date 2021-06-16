using System;
using System.Collections;
using System.Collections.Generic;
using SensorToolkit;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class Enemy : MonoBehaviour {
    [Header("Settings")]
    [SerializeField] private List<Transform> waypoints = new List<Transform>();
    [SerializeField] private float toleranceDistance = 0.7f;
    private NavMeshAgent agent;
    private bool ascending = true;
    private int nextWaypoint;
    private Sensor sight;

    private void Awake() {
        agent = GetComponent<NavMeshAgent>();
        sight = GetComponentInChildren<Sensor>();
        sight.OnDetected.AddListener((arg0, sensor) => SceneManager.LoadSceneAsync(0, LoadSceneMode.Single));
        StartCoroutine(nameof(Patrol));
    }

    private IEnumerator Patrol() {
        agent.SetDestination(waypoints[nextWaypoint].position);

        yield return new WaitUntil((() =>
            (transform.position - waypoints[nextWaypoint].position).magnitude < toleranceDistance));
        
        nextWaypoint = ascending ? nextWaypoint + 1 : nextWaypoint - 1;
        
        if(nextWaypoint >= waypoints.Count - 1 || nextWaypoint <= 0) {
            ascending = !ascending;
        }
        
        StartCoroutine(nameof(Patrol));
    }
}
