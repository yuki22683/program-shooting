using UnityEngine;
using static BeadCollisionDetector;
using static BeadValue;

public class BeadColorManager : MonoBehaviour
{
	[SerializeField] private Material srcMaterial;

	private BeadValue beadValue;

	void Awake()
	{
		beadValue = transform.parent.GetComponent<BeadValue>();
	}

	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
    {
	}

	public enum BeadColorType
	{
		None,
		Touch,
		Disable,
		Active,
	}

    // Update is called once per frame
    void Update()
    {
        
    }

	public void SetMeshColor(BeadColorType colorType)
	{
		Material material = GetComponent<Renderer>().material;
		int rodId = transform.parent.parent.GetComponent<RodValue>().digitPosition;
		int beadId = transform.parent.GetComponent<BeadValue>().id;	

		switch (colorType)
		{
#if false
			case BeadColorType.None:
				material.color = DataManager.gameSettings.colorSettings.beadsColors[rodId].colors[beadId]; break;
			case BeadColorType.Touch:
				material.color = RotateHue(DataManager.gameSettings.colorSettings.beadsColors[0].colors[0], 0.5f); break;
			case BeadColorType.Disable:
				material.color = DataManager.gameSettings.colorSettings.debounceColor; break;
			case BeadColorType.Active:
				material.color = RotateHue(DataManager.gameSettings.colorSettings.beadsColors[0].colors[0], 0.75f); break;
			default: break;
#else
			case BeadColorType.None:
				//material.color = DataManager.gameSettings.colorSettings.beadsColors[rodId].colors[beadId]; break;
			case BeadColorType.Touch:
				//material.color = DataManager.gameSettings.colorSettings.touchColor; break;
			case BeadColorType.Disable:
				//material.color = DataManager.gameSettings.colorSettings.debounceColor; break;
			case BeadColorType.Active:
				//material.color = DataManager.gameSettings.colorSettings.activeColor; break;
			default: break;
#endif
		}
	}

	Color RotateHue(Color color, float degrees)
    {
        // RGBをHSVに変換
        float h, s, v;
        Color.RGBToHSV(color, out h, out s, out v);

        // 回転角度を色相の範囲（0～1）に変換（360度 = 1）
        float hueShift = degrees;
        h = (h + hueShift) % 1f; // 色相を回転し、0～1に収める

        // HSVをRGBに戻す
        return Color.HSVToRGB(h, s, v, true); // trueでアルファ値を保持
    }
}
