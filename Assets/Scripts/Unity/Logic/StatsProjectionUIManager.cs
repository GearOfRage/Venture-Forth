using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatsProjectionUIManager : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] GameObject statsProjectionContainer;
    [SerializeField] Text statsProjectionText;

    private StatsProjection statsProjection;

    void Start()
    {
        // Set up the StatsProjection component reference
        statsProjection = FindObjectOfType<StatsProjection>();
        if (statsProjection != null)
        {
            SetupStatsProjectionReferences();
        }

        // Hide the projection container initially
        if (statsProjectionContainer != null)
            statsProjectionContainer.SetActive(false);
    }

    private void SetupStatsProjectionReferences()
    {
        if (statsProjection == null) return;

        // Set the UI references
        statsProjection.projectionUI = statsProjectionContainer;
        statsProjection.statsProjectionText = statsProjectionText;
    }
}
