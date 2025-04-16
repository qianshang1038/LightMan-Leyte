using UnityEngine;

[RequireComponent(typeof(PlotLoader))]
public class PlotBroadExample : MonoBehaviour
{
    private PlotLoader plotLoader;

    private void Start()
    {
        plotLoader = gameObject.GetComponent<PlotLoader>();

        // 加载对应Plot的Path
        string plotJsonName = "Monster2DDialogTest";
        plotLoader.LoadPlot(plotJsonName);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            plotLoader.PlayCurrentPlot();
        }

        // Example for jumping to a specific node
        if (Input.GetKeyDown(KeyCode.J)) // J key for jump
        {
            int jumpToId = 100005; // Replace with your logic to determine the ID
            plotLoader.JumpToNode(jumpToId);
        }
    }
}
