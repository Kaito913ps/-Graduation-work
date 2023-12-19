using UnityEngine;

public class RotatableHandle : MonoBehaviour
{
    public Transform rotateObject; // 回転させるオブジェクト

    [Header("Rotation Settings")]
    public Vector3 targetRotation; // 目標回転角度
    public float rotationSpeed = 1f; // 回転速度

    private Vector3 initialMousePosition;
    private bool isRotating;
    private Quaternion initialRotation;

    public AudioClip rotateSound; // ハンドル回転時の効果音

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

            // ここでの回転量の計算と適用を調整
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
