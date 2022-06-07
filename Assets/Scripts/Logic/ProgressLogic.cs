using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ProgressType
{
    Gold = 1,
    Equipment = 2,
    Experience = 3
}

public class ProgressLogic : MonoBehaviour
{
    GameObject goldProgressPanel;
    GameObject equipProgressPanel;
    GameObject expProgressPanel;

    GameObject showedPanel;

    public void ShowProgressPanel(ProgressType progressType)
    {
        switch (progressType)
        {
            case ProgressType.Gold:
                showedPanel = Instantiate(goldProgressPanel, Vector3.zero, Quaternion.identity);
                break;
            case ProgressType.Equipment:
                showedPanel = Instantiate(equipProgressPanel, Vector3.zero, Quaternion.identity);
                break;
            case ProgressType.Experience:
                showedPanel = Instantiate(expProgressPanel, Vector3.zero, Quaternion.identity);
                break;
            default:
                break;
        }
    }

}
