using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    private Camera mainCamera;
    private float lastCameraPositionX;
    private float lastCameraPositionY;
    private float cameraHalfWidth;
    private float cameraHalfHeight;

    [SerializeField] private ParallaxLayer[] backgroundLayers;

    private void Awake()
    {
        mainCamera = Camera.main;
        cameraHalfWidth = mainCamera.orthographicSize * mainCamera.aspect;
        cameraHalfHeight = mainCamera.orthographicSize;
        CalculateImageLengthAndHeight();
    }

    private void FixedUpdate()
    {
        float currentcameraPositionX = mainCamera.transform.position.x;
        float distanceToMoveX = currentcameraPositionX - lastCameraPositionX;
        lastCameraPositionX = currentcameraPositionX;

        float currentcameraPositionY = mainCamera.transform.position.y;
        float distanceToMoveY = currentcameraPositionY - lastCameraPositionY;
        lastCameraPositionY = currentcameraPositionY;

        float cameraLeftEdge = currentcameraPositionX - cameraHalfWidth;
        float cameraRightEdge = currentcameraPositionX + cameraHalfWidth;
        float cameraTopEdge = currentcameraPositionY + cameraHalfHeight;
        float cameraBottomEdge = currentcameraPositionY - cameraHalfHeight;

        foreach (ParallaxLayer layer in backgroundLayers)
        {
            layer.Move(distanceToMoveX, distanceToMoveY);
            layer.LoopBackground(cameraLeftEdge, cameraRightEdge, cameraTopEdge, cameraBottomEdge);
        }
    }

    private void CalculateImageLengthAndHeight()
    {
        foreach (ParallaxLayer layer in backgroundLayers)
        {
            //layer.CalculateImageWidth();
            //layer.CalculateImageHeight();
            layer.CalculateImageSize();
        }
    }
}
