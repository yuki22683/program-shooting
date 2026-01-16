using UnityEngine;

public class PersistentTransform : MonoBehaviour
{
    [SerializeField] private bool savePosition = true; // 位置の保存を有効/無効
    [SerializeField] private bool saveRotation = true; // 回転の保存を有効/無効
    [SerializeField] private bool saveScale = true;   // スケールの保存を有効/無効

    private string positionKey;
    private string rotationKey;
    private string scaleKey;

    private Vector3 lastPosition;
    private Quaternion lastRotation;
    private Vector3 lastScale;

    void Awake()
    {
        // オブジェクト名をキーに使う（ユニークであることを想定）
        string id = gameObject.name;
        positionKey = $"SavedPosition_{id}";
        rotationKey = $"SavedRotation_{id}";
        scaleKey = $"SavedScale_{id}";
#if true
        // 復元処理：対応するフラグが有効な場合のみ復元
        if (savePosition && PlayerPrefs.HasKey(positionKey))
        {
            Vector3 savedPos = JsonUtility.FromJson<Vector3>(PlayerPrefs.GetString(positionKey));
            transform.position = savedPos;
        }

        if (saveRotation && PlayerPrefs.HasKey(rotationKey))
        {
            Quaternion savedRot = JsonUtility.FromJson<Quaternion>(PlayerPrefs.GetString(rotationKey));
            transform.rotation = savedRot;
        }

        if (saveScale && PlayerPrefs.HasKey(scaleKey))
        {
            Vector3 savedScale = JsonUtility.FromJson<Vector3>(PlayerPrefs.GetString(scaleKey));
            transform.localScale = savedScale;
        }

        if (savePosition || saveRotation || saveScale)
        {
            Debug.Log($"[{gameObject.name}] 復元しました: 位置={savePosition}, 回転={saveRotation}, スケール={saveScale}");
        }

        // 現在の状態を記録
        lastPosition = transform.position;
        lastRotation = transform.rotation;
        lastScale = transform.localScale;
#endif
    }

    void Start()
    {
    }

    void Update()
    {
        bool needsSave = false;

        // 変更チェック：対応するフラグが有効な場合のみチェック
        if (savePosition && Vector3.Distance(transform.position, lastPosition) > 0.01f)
        {
            needsSave = true;
            lastPosition = transform.position;
        }

        if (saveRotation && Quaternion.Angle(transform.rotation, lastRotation) > 0.5f)
        {
            needsSave = true;
            lastRotation = transform.rotation;
        }

        if (saveScale && Vector3.Distance(transform.localScale, lastScale) > 0.01f)
        {
            needsSave = true;
            lastScale = transform.localScale;
        }

        // 変更があった場合に保存
        if (needsSave)
        {
            SaveTransform();
        }
    }

    private void SaveTransform()
    {
        // 保存処理：対応するフラグが有効な場合のみ保存
        if (savePosition)
        {
            PlayerPrefs.SetString(positionKey, JsonUtility.ToJson(transform.position));
        }

        if (saveRotation)
        {
            PlayerPrefs.SetString(rotationKey, JsonUtility.ToJson(transform.rotation));
        }

        if (saveScale)
        {
            PlayerPrefs.SetString(scaleKey, JsonUtility.ToJson(transform.localScale));
        }

        PlayerPrefs.Save();
        Debug.Log($"[{gameObject.name}] 保存しました: 位置={savePosition}, 回転={saveRotation}, スケール={saveScale}");
    }
}