using UI.ExamplePanel;
using UnityEngine;

namespace Test
{
    public class Test : MonoBehaviour
    {
        private void Start()
        {
            //GameStartPanel.Instance.ShowMe();
            Debug.Log(Player.Instance.transform.position);
        }
    }
}