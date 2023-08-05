using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{

    public GameObject circleprefab;
    public int minCircleCount = 5;
    public int maxCircleCount = 10;

    public LineRenderer lineRenderer;

    private Vector3 lineStart;
    private Vector3 lineEnd;

    private List<GameObject> circles = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        SpawnRandomCircles();
    }

    private void SpawnRandomCircles()
    {
        int circleCount = Random.Range(minCircleCount, maxCircleCount + 1);

        for (int i = 0; i < circleCount; i++)
        {
            Vector3 randomPosition = new Vector3(Random.Range(-5f, 5f), Random.Range(-3f, 3f), 0f);
            GameObject circle = Instantiate(circleprefab, randomPosition, Quaternion.identity);
            circles.Add(circle);
        }
    }

    public void OnLineDrawn(Vector3 start, Vector3 end)
    {
        List<GameObject> circlesToRemove = new List<GameObject>();

        foreach (GameObject circle in circles)
        {
            if(IsCircleIntersectedByLine(circle, start, end))
            {
                circlesToRemove.Add(circle);
            }
        }

        foreach (GameObject circleToRemove in circlesToRemove)
        {
            circles.Remove(circleToRemove);
            Destroy(circleToRemove);
        }
    }

    private bool IsCircleIntersectedByLine(GameObject circle, Vector3 lineStart, Vector3 lineEnd)
    {
        Vector3 circleCenter = circle.transform.position;
        float circleRadius = circle.transform.localScale.x / 2f;

        float distance = Mathf.Abs((lineEnd.y - lineStart.y) * circleCenter.x - (lineEnd.x - lineStart.x) * circleCenter.y + lineEnd.x * lineStart.y - lineEnd.y * lineStart.x)
            / Mathf.Sqrt(Mathf.Pow(lineEnd.y - lineStart.y, 2) + Mathf.Pow(lineEnd.x - lineStart.x, 2));

        return distance <= circleRadius;
    }

    
    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            lineStart = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            lineStart.z = 0f;

            lineRenderer.positionCount = 1;
            lineRenderer.SetPosition(0, lineStart);
        }

        else if (Input.GetMouseButton(0))
        {
            lineEnd = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            lineStart.z = 0f;

            lineRenderer.positionCount = 2;
            lineRenderer.SetPosition(1, lineEnd);
        }

        else if (Input.GetMouseButtonUp(0))
        {
            OnLineDrawn(lineStart, lineEnd);
            lineRenderer.positionCount = 0;
        }
    }

    public void OnRestartButtonClicked()
    {
        RestartGame();
    }

    public void RestartGame()
    {
        lineRenderer.positionCount = 0;
        foreach (GameObject circle in circles)
        {
            Destroy(circle);
        }
        circles.Clear();

        SpawnRandomCircles();
    }

}
