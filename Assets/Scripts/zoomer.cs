using Cinemachine;
using UnityEditor;
using UnityEngine;
public class zoomer : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera cvc;
    public Grid grid;
    private float rotation = 0f;
    private float prevRotation = 0f;

    private void Update()
    {
        rotation = grid.transform.rotation.eulerAngles.z;

        if (rotation > 10f && rotation > prevRotation && grid.isStarted)
            cvc.Priority = 9;
        else if (rotation < 45f && prevRotation > rotation && grid.isStarted)
            cvc.Priority = 20;
        else if (rotation < 350f && rotation < prevRotation && grid.isStarted)
            cvc.Priority = 9;
        else if(rotation > 315f && rotation > prevRotation && grid.isStarted)
            cvc.Priority = 20;

        prevRotation = rotation;
    }
}
