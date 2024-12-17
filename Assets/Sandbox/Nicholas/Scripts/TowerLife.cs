using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class TowerLife: MonoBehaviour
{
    [SerializeField] private int points = 100; // The starting point value
    private Text pointsText; // UI Text to display points

    private void Start()
    {
        pointsText = GameObject.Find("TowerLifePoints").GetComponent<Text>();
        // Update the UI with the initial points
        UpdatePointsText();
    }

    // This function is called when another collider enters this object's collider
    private void OnCollisionEnter(Collision collision)
    {
        // Check if the other object has the "Bullet" tag
        if (collision.gameObject.CompareTag("Bullet"))
        {
            Debug.Log("NICE");
            // Decrease the points by 1
            points -= 1;

            // Ensure points doesn't go below 0
            if (points < 0)
                points = 0;

            // Update the UI text with the new points value
            UpdatePointsText();
        }
    }

    // Updates the UI Text with the current points value
    private void UpdatePointsText()
    {
        if (pointsText != null)
        {
            pointsText.text = "Tower Health: " + points.ToString();
        }
    }
}

