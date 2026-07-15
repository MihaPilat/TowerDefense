using UnityEngine;
using Zenject;

public class Billboard : MonoBehaviour
{
    private Camera _mainCamera;

    [Inject]
    private void Consruct(Camera camera)
    {
        _mainCamera = camera;
    }

    private void LateUpdate()
    {
        transform.LookAt(
            transform.position + _mainCamera.transform.rotation * Vector3.forward,
            _mainCamera.transform.rotation * Vector3.up);
    }
}
