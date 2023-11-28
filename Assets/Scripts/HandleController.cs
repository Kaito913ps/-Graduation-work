using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandleController : MonoBehaviour
{
    [SerializeField, Tooltip("回転させるプラットフォーム")]
    public Transform platformToRotate;
    [SerializeField, Tooltip("回転速度")]
    private float rotationSpeed = 100f;
    [SerializeField, Tooltip("回転軸")]
    private Vector3 rotationAxis;

    private bool isInteractable;
    private bool isHoldingHandle;

    private PlatformRotator platformRotator;

    void Start()
    {
        isInteractable = true;
        isHoldingHandle = false;
        platformRotator = platformToRotate.GetComponent<PlatformRotator>();
    }

    void Update()
    {
        if (isInteractable)
        {
            HandleMouseInput();
            RotateHandleIfHolding();
        }
    }

    void HandleMouseInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform == transform || (hit.transform.parent == transform))
                {
                    isHoldingHandle = true;
                }
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            isHoldingHandle = false;
        }
    }

    void RotateHandleIfHolding()
    {
        if (isHoldingHandle)
        {
            // マウスのX軸の移動量に基づいて回転量を計算
            float rotationAmount = Input.GetAxis("Mouse X") * rotationSpeed * Time.deltaTime;

            if (platformRotator != null)
            {
                // マウスの動きに基づいてハンドルを回転
                platformRotator.Rotate(rotationAxis, rotationAmount);

                // 回転角度の調整（必要に応じて）
                //AdjustRotationAngle();
            }
        }
    }


    void AdjustRotationAngle()
    {
        Vector3 currentRotation = platformToRotate.eulerAngles;

        // X軸の角度調整
        float xAngle = NormalizeAngle(currentRotation.x);
        if (Mathf.Abs(xAngle - 180) < 1f) // X軸が180度に近い場合
        {
            platformToRotate.eulerAngles = new Vector3(180, currentRotation.y, currentRotation.z);
        }

        // Z軸の角度調整
        float zAngle = NormalizeAngle(currentRotation.z);
        if (Mathf.Abs(zAngle + 90) < 1f) // Z軸が-90度に近い場合
        {
            platformToRotate.eulerAngles = new Vector3(currentRotation.x, currentRotation.y, -90);
        }
    }



    private float NormalizeAngle(float angle)
    {
        while (angle > 180) angle -= 360;
        while (angle < -180) angle += 360;
        return angle;
    }
    public void SetInteractable(bool state)
    {
        isInteractable = state;
        isHoldingHandle = false;
    }
}
