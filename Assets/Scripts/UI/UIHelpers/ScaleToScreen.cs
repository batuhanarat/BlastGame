using UnityEngine;

public class ScaleToScreen : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;
    private Camera _mainCamera;

    void Start()
    {
        _mainCamera = Camera.main;
        _spriteRenderer = GetComponent<SpriteRenderer>();
        ScaleSpriteToCoverScreen();
    }

    void ScaleSpriteToCoverScreen()
    {
        Vector2 spriteSize = _spriteRenderer.sprite.bounds.size;
        float cameraHeight = 2f * _mainCamera.orthographicSize;
        float cameraWidth = cameraHeight * Screen.width / Screen.height;

        float scaleX = cameraWidth / spriteSize.x;
        float scaleY = cameraHeight / spriteSize.y;

        float scale = Mathf.Max(scaleX, scaleY);

        transform.localScale = new Vector3(scale, scale, 1);
    }

}