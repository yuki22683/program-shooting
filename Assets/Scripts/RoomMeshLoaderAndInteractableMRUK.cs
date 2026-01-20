using UnityEngine;
using Oculus.Interaction;
using Meta.XR.MRUtilityKit;
using Oculus.Interaction.Surfaces;
using NUnit.Framework.Internal;
using UnityEngine.Android;
using System.Threading.Tasks;
public class RoomMeshLoaderAndInteractableMRUKManual : MonoBehaviour
{
	private MRUK mruk;

	async void Start()
	{
		// MRUK コンポーネントを取得または追加
		mruk = GetComponent<MRUK>();
		if (mruk == null)
		{
			mruk = gameObject.AddComponent<MRUK>();
			Debug.Log("MRUK コンポーネントが追加されました。");
		}

		// シーン許可の確認とリクエスト
		if (!Permission.HasUserAuthorizedPermission(OVRPermissionsRequester.ScenePermission))
		{
			var callbacks = new PermissionCallbacks();
			callbacks.PermissionGranted += OnPermissionGranted;
			callbacks.PermissionDenied += OnPermissionDenied;
			Permission.RequestUserPermission(OVRPermissionsRequester.ScenePermission, callbacks);
		}
		else
		{
			// 許可がある場合、再スキャンを開始
			await RequestRescan();
		}

		// MRUK.Instance が正しく初期化されているか確認
		if (MRUK.Instance == null)
		{
			Debug.LogError("MRUK.Instance が初期化されていません。シーンに MRUK コンポーネントが存在することを確認してください。");
			return;
		}

		// デバイスからシーンを手動でロード
		Debug.Log("デバイスからシーンをロード中...");
		var result = await MRUK.Instance.LoadSceneFromDevice(); // ← 修正済
		OnSceneLoaded(result == MRUK.LoadDeviceResult.Success); // ← 明示的に呼ぶ
	}

	private void OnPermissionGranted(string permissionId)
	{
		Debug.Log("Scene permission granted!");
		// 許可が得られたら再スキャンを開始
		RequestRescan().GetAwaiter().OnCompleted(() => Debug.Log("Rescan process completed"));
	}

	private void OnPermissionDenied(string permissionId)
	{
		Debug.LogWarning("Scene permission denied. Cannot proceed with rescan.");
		// 許可が必要であることをユーザーに通知するUIを表示
	}

	private async Task RequestRescan()
	{
		MRUK mruk = MRUK.Instance;

		// Meta Questのネイティブダイアログで新しい空間スキャンをリクエスト
		bool setupRequested = await OVRScene.RequestSpaceSetup();
		if (setupRequested)
		{
			Debug.Log("Space setup requested successfully. Waiting for user to complete the scan.");
		}
		else
		{
			Debug.LogWarning("Failed to request space setup. User may have canceled or an error occurred.");
			return;
		}

		// 既存のシーンをクリア
		mruk.ClearScene();
		Debug.Log("Current scene data cleared after rescan request.");

		// スキャン完了後に新しいシーンをロード
		MRUK.LoadDeviceResult result = await mruk.LoadSceneFromDevice(requestSceneCaptureIfNoDataFound: false);

		switch (result)
		{
			case MRUK.LoadDeviceResult.Success:
				Debug.Log("New spatial mapping data loaded successfully!");
				// 新しいルーム情報を確認
				MRUKRoom currentRoom = mruk.GetCurrentRoom();
				if (currentRoom != null)
				{
					Debug.Log($"Current room has {currentRoom.Anchors.Count} anchors.");
				}
				break;
			case MRUK.LoadDeviceResult.NoScenePermission:
				Debug.LogWarning("No scene permission. Cannot load new spatial data.");
				break;
			case MRUK.LoadDeviceResult.NoRoomsFound:
				Debug.LogWarning("No new rooms found. User may not have completed the scan.");
				break;
			default:
				Debug.LogError($"Failed to load new spatial data: {result}");
				break;
		}
	}

	private void OnSceneLoaded(bool success)
	{
		if (success)
		{
			Debug.Log("シーンのロードに成功しました。");

			var room = MRUK.Instance.GetCurrentRoom();
			if (room != null)
			{
				OnRoomLoaded(room);
			}
			else
			{
				Debug.LogError("部屋のロードに成功しましたが、部屋データが見つかりません。");
			}
		}
		else
		{
			Debug.LogError("シーンのロードに失敗しました。デバイスが空間マッピングをサポートしているか確認してください。");
		}
	}

	private void OnRoomLoaded(MRUKRoom room)
	{
		Debug.Log($"部屋のメッシュがロードされました。部屋に含まれるアンカー数: {room.Anchors.Count}");

		foreach (var anchor in room.Anchors)
		{
			if (anchor.transform.childCount == 0)
			{
				Debug.LogWarning($"アンカー {anchor.name} に子オブジェクトが見つかりません。");
				continue;
			}
			var meshFilter = anchor.transform.GetChild(0).GetComponent<MeshFilter>();
			if (meshFilter != null)
			{
				Mesh roomMesh = meshFilter.mesh;
				Debug.Log($"メッシュ取得: {roomMesh.name} (頂点数: {roomMesh.vertexCount})");
				MakeMeshInteractable(anchor.transform.GetChild(0).gameObject);
			}
			else
			{
				Debug.LogWarning($"アンカー {anchor.name} に MeshFilter が見つかりません。");
			}
		}
	}

	private void MakeMeshInteractable(GameObject target)
	{
		ColliderSurface colliderSurface = target.AddComponent<ColliderSurface>();

		if (target.TryGetComponent<MeshCollider>(out var meshCollider))
		{
			colliderSurface.InjectCollider(meshCollider);
		}
		else if (target.TryGetComponent<BoxCollider>(out var boxCollider))
		{
			colliderSurface.InjectCollider(boxCollider);
		}

		RayInteractable rayInteractable = target.AddComponent<RayInteractable>();
		rayInteractable.InjectSurface(colliderSurface);

		Debug.Log($"{target.name} を RayInteractable に設定しました。");

		target.tag = "Room";
	}
}
