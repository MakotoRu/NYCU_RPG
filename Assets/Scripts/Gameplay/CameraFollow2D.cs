using UnityEngine;

public class CameraFollow2D : MonoBehaviour
{
    [Header("目標 (通常是玩家)")]
    public Transform target;

    [Header("鏡頭平滑速度")]
    public float smoothSpeed = 5f;

    [Header("鏡頭與角色的偏移量")]
    public Vector3 offset;

    [Header("是否限制鏡頭邊界")]
    public bool useBounds = false;
    public Vector2 minBounds;
    public Vector2 maxBounds;

    void LateUpdate()
    {
        if (target == null) return;

        // 預期鏡頭位置
        Vector3 desiredPosition = target.position + offset;

        // 平滑插值 (SmoothDamp 或 Lerp 都可以)
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);

        // 保持 Z 軸不變 (避免鏡頭跑到角色前面)
        smoothedPosition.z = transform.position.z;

        // 如果開啟邊界限制，限制鏡頭位置
        if (useBounds)
        {
            smoothedPosition.x = Mathf.Clamp(smoothedPosition.x, minBounds.x, maxBounds.x);
            smoothedPosition.y = Mathf.Clamp(smoothedPosition.y, minBounds.y, maxBounds.y);
        }

        // 更新鏡頭位置
        transform.position = smoothedPosition;
    }
}
