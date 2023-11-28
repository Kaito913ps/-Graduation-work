using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandleController : MonoBehaviour
{
    [SerializeField, Tooltip("��]������v���b�g�t�H�[��")]
    public Transform platformToRotate;
    [SerializeField, Tooltip("��]���x")]
    private float rotationSpeed = 100f;
    [SerializeField, Tooltip("��]��")]
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
            // �}�E�X��X���̈ړ��ʂɊ�Â��ĉ�]�ʂ��v�Z
            float rotationAmount = Input.GetAxis("Mouse X") * rotationSpeed * Time.deltaTime;

            if (platformRotator != null)
            {
                // �}�E�X�̓����Ɋ�Â��ăn���h������]
                platformRotator.Rotate(rotationAxis, rotationAmount);

                // ��]�p�x�̒����i�K�v�ɉ����āj
                //AdjustRotationAngle();
            }
        }
    }


    void AdjustRotationAngle()
    {
        Vector3 currentRotation = platformToRotate.eulerAngles;

        // X���̊p�x����
        float xAngle = NormalizeAngle(currentRotation.x);
        if (Mathf.Abs(xAngle - 180) < 1f) // X����180�x�ɋ߂��ꍇ
        {
            platformToRotate.eulerAngles = new Vector3(180, currentRotation.y, currentRotation.z);
        }

        // Z���̊p�x����
        float zAngle = NormalizeAngle(currentRotation.z);
        if (Mathf.Abs(zAngle + 90) < 1f) // Z����-90�x�ɋ߂��ꍇ
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
