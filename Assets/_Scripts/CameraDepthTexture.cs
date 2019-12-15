using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraDepthTexture : MonoBehaviour
{
    private void Awake()
    {
        gameObject.GetComponent<Camera>().depthTextureMode = DepthTextureMode.Depth;
    }
}
