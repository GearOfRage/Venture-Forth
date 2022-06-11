using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ProgressTypeE
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
    [SerializeField] Fader fader;

    GameObject showedPanel;
    int toOpen = 0;
    ProgressTypeE panelProgressType;

    Dictionary<ProgressTypeE, GameObject> panels;

    private void Start()
    {
        panels = new()
        {
            { ProgressTypeE.Gold, goldProgressPanel },
            { ProgressTypeE.Equipment, equipProgressPanel },
            { ProgressTypeE.Experience, expProgressPanel },
        };
    }

    public void Next()
    {
        showedPanel = Instantiate(panels[panelProgressType], Vector3.zero, Quaternion.identity);
    }

    public void ShowProgressPanel(ProgressTypeE progressType, int uppedLevels = 1)
    {
        toOpen = uppedLevels;
        panelProgressType = progressType;
        fader.OpenFader();
        Next();
    }

    public void CloseProgressPanel()
    {
        toOpen--;
        Destroy(showedPanel);
        if (toOpen == 0)
        {
            fader.CloseFader();
        } else
        {
            Next();
        }
    }

}
