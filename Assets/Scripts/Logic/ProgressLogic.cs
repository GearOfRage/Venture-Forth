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
    [SerializeField] GameObject goldProgressPanel;
    [SerializeField] GameObject equipProgressPanel;
    [SerializeField] GameObject expProgressPanel;
    [SerializeField] GameLogic gl;

    GameObject showedPanel;
    int toOpen = 0;
    ProgressType panelProgressType;

    Dictionary<ProgressType, GameObject> panels;

    private void Start()
    {
        panels = new()
        {
            { ProgressType.Gold, goldProgressPanel },
            { ProgressType.Equipment, equipProgressPanel },
            { ProgressType.Experience, expProgressPanel },
        };
    }

    public void Next()
    {
        showedPanel = Instantiate(panels[panelProgressType], Vector3.zero, Quaternion.identity);
        showedPanel.GetComponent<Canvas>().sortingOrder = gl.screenFader.sortingOrder + 1;
    }

    public void ShowProgressPanel(ProgressType progressType, int uppedLevels = 1)
    {
        toOpen = uppedLevels;
        panelProgressType = progressType;
        gl.OpenFader();
        Next();
    }

    public void CloseProgressPanel()
    {
        toOpen--;
        Destroy(showedPanel);
        if (toOpen == 0)
        {
            gl.CloseFader();
        } else
        {
            Next();
        }
    }

}
