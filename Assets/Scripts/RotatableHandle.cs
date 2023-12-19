using UnityEngine;

public class RotatableHandle : MonoBehaviour
{
    public Transform rotateObject; // ��]������I�u�W�F�N�g

    [Header("Rotation Settings")]
    public Vector3 targetRotation; // �ڕW��]�p�x
    public float rotationSpeed = 1f; // ��]���x

    private Vector3 initialMousePosition;
    private bool isRotating;
    private Quaternion initialRotation;

    public AudioClip rotateSound; // �n���h����]���̌��ʉ�

    private void Update()
    {
        HandleRotation();
    }

    private void HandleRotation()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit) && hit.transform == transform)
            {
                isRotating = true;
                initialMousePosition = Input.mousePosition;
                initialRotation = rotateObject.rotation;
                Debug.Log("Hit Rotatable Handle: " + hit.transform.name);
            }
        }

        if (isRotating && Input.GetMouseButton(0))
        {
            Vector3 delta = Input.mousePosition - initialMousePosition;
            float rotationAmount = delta.x * rotationSpeed;

            // �����ł̉�]�ʂ̌v�Z�ƓK�p�𒲐�
            Vector3 currentRotation = rotateObject.eulerAngles;
            Vector3 newRotation = currentRotation + new Vector3(0, rotationAmount, 0);
            rotateObject.eulerAngles = newRotation;
            Debug.Log("Rotation Amount: " + rotationAmount);
            //Quaternion targetQuaternion = Quaternion.Euler(targetRotation);
            //rotateObject.rotation = Quaternion.RotateTowards(
            //    initialRotation, targetQuaternion, rotationAmount);

            //if (!AudioManager.instance.IsPlaying(rotateSound))
            //{
            //    AudioManager.instance.PlaySFX(rotateSound);
            //}
        }

        //if (Input.GetMouseButtonUp(0))
        //{
        //    isRotating = false;
        //    AudioManager.instance.StopSFX();
        //}
    }
}
