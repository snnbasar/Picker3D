using UnityEngine;
 
public class CameraFollow : MonoBehaviour
{
    
    [SerializeField] private Transform Target; // camera will follow this object

    private Transform camTransform;
    
    [SerializeField] private Vector3 Offset; // offset between camera and target

    [SerializeField] private float SmoothTime = 0.3f;


    private Vector3 velocity = Vector3.zero;

    private void Start()
    {
        camTransform = this.transform;

        camTransform.LookAt(Target);
    }



    private void LateUpdate()
    {
        // update z position and if y >= 1 update y position to avoid little y movements.
        Vector3 targetPosition = Offset + new Vector3(0, (Target.position.y >= 2f) ? Target.position.y : 0, Target.position.z);
        camTransform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, SmoothTime);

        

        camTransform.LookAt(targetPosition - Offset);

    }
}