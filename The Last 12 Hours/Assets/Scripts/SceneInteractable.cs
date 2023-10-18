using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneInteractable : Interactable
{
    [SerializeField]
    private float shakeIntensity;


    [SerializeField]
    private bool interactToNextScene;

    public override void Interact()
    {
        if (interactToNextScene)
        {
            GameManager.NextScene();
        }
    }

    public void ShakeCamera()
    {
        PlayerCamera.Instance.StartShakeCamera(shakeIntensity);
    }

    public void StopShakeCamera()
    {
        PlayerCamera.Instance.StopShakeCamera();
    }
}
