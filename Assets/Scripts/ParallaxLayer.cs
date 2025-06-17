using UnityEngine;
using UnityEngine.Tilemaps;

[System.Serializable]
public class ParallaxLayer
{
    [SerializeField] private Transform background;
    [SerializeField] private float parallaxMultiplier;
    [SerializeField] private float imageWidthOffset = 10;
    [SerializeField] private float imageHeightOffset = 10;

    private float imageFullWidth;
    private float imageFullHeight;
    private float imageHalfWidth;
    private float imageHalfHeight;

    public void CalculateImageSize()
    {
        var bounds = background.GetComponent<SpriteRenderer>().bounds;
        imageFullWidth = bounds.size.x;
        imageHalfWidth = imageFullWidth / 2;
        imageFullHeight = bounds.size.y;
        imageHalfHeight = imageFullHeight / 2;
    }

    public void Move(float distanceToMoveX, float distanceToMoveY)
    {
        // or = background.position + new Vector3(distanceToMove * parallaxMultiplier, 0);
        //background.position += new Vector3(distanceToMove.x * parallaxMultiplier, distanceToMove.y * parallaxMultiplier);
        background.position += Vector3.right * (distanceToMoveX * parallaxMultiplier);
        background.position += Vector3.up * (distanceToMoveY * parallaxMultiplier);
    }

    public void LoopBackground(float cameraLeftEdge, float cameraRightEdge, float cameraTopEdge, float cameraBottomEdge)
    {
        float imageRightEdge = (background.position.x + imageHalfWidth) - imageWidthOffset;
        float imageLeftEdge = (background.position.x - imageHalfWidth) + imageWidthOffset;

        float imageTopEdge = (background.position.y + imageHalfHeight) - imageHeightOffset;
        float imageBottomEdge = (background.position.y - imageHalfHeight) + imageHeightOffset;

        if (imageRightEdge < cameraLeftEdge)
            background.position += Vector3.right * imageFullWidth;
        else if (imageLeftEdge > cameraRightEdge)
            background.position += Vector3.right * -imageFullWidth;

        if (imageTopEdge < cameraBottomEdge)
            background.position += Vector3.up * imageFullHeight;
        else if (imageBottomEdge > cameraTopEdge)
            background.position += Vector3.up * -imageFullHeight;
    }
}
