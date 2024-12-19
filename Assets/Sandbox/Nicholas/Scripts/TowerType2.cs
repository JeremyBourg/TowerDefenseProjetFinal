using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerType2 : MonoBehaviour
{
    [Header("Shield Settings")]
    public float growSpeed = 2f; // Speed at which the shield grows
    public float shrinkSpeed = 2f; // Speed at which the shield shrinks
    public float activeDuration = 8f; // How long the shield stays active
    public Vector3 originalScale = new Vector3(1f, 1f, 1f); // Original size of the shield
    public Vector3 disabledScale = new Vector3(0.1f, 0.1f, 0.1f); // Minimal size when disabled

    private bool isGrowing = false;
    private bool isShrinking = false;

    private void Start()
    {
        // Initialize shield to its disabled state
        transform.localScale = disabledScale;

        // Automatically activate the shield at the start
        ActivateShield();
    }

    private void Update()
    {
        // Handle shield growing
        if (isGrowing)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, originalScale, Time.deltaTime * growSpeed);

            if (Vector3.Distance(transform.localScale, originalScale) < 0.01f)
            {
                transform.localScale = originalScale;
                isGrowing = false;
                Invoke(nameof(StartShrinking), activeDuration);
            }
        }

        // Handle shield shrinking
        if (isShrinking)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, disabledScale, Time.deltaTime * shrinkSpeed);

            if (Vector3.Distance(transform.localScale, disabledScale) < 0.01f)
            {
                transform.localScale = disabledScale;
                isShrinking = false;
                Invoke(nameof(ActivateShield), activeDuration); // Re-activate the shield after shrinking
            }
        }
    }

    public void ActivateShield()
    {
        if (!isGrowing && !isShrinking)
        {
            isGrowing = true;
        }
    }

    private void StartShrinking()
    {
        isShrinking = true;
    }
}