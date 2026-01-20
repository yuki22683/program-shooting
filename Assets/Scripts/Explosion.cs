using UnityEngine;

public class Explosion : MonoBehaviour
{
    [SerializeField] private GameObject m_explosion;
    [SerializeField] private GameObject m_visual;
	[SerializeField] private GameObject m_mesh;

	[SerializeField] private float m_lightingTime = 1f;

	[SerializeField] private Texture2D m_Texture;
	public bool IsLighting = false;
	public bool IsExploding = false;

	public int m_bombId = 0;

	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Explode()
	{
		IsLighting = true;
		IsExploding = true;

		m_explosion.SetActive(true);
		m_visual.SetActive(false);
		PlayExplosionSound();
		Invoke("InactiveBomb", 2f);
	}

	public void LightBomb()
	{
		IsLighting = true;

		m_mesh.gameObject.GetComponent<Renderer>().material.mainTexture = m_Texture;

		Invoke("Explode", m_lightingTime);
	}


	private void PlayExplosionSound()
	{
		AudioSource audio = gameObject.GetComponent<AudioSource>();
		audio.Stop();
		audio.Play();
	}

	private void InactiveBomb()
	{
		IsLighting = false;
		IsExploding = false;
		gameObject.SetActive(false);
	}
}
