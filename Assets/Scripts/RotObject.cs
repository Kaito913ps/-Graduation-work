using DG.Tweening.Core.Easing;
using DG.Tweening.Plugins.Options;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

// オブジェクトの回転を管理するクラス
public class RotObject : MonoBehaviour
{
    public Vector3 rotationAxis = Vector3.up;
    [SerializeField]
    private float rotationSpeed = 45.0f;

    private void Update()
    {
        transform.Rotate(rotationAxis,rotationSpeed * Time.deltaTime);
    }
}

