using System.Collections.Generic;
using UnityEngine;

public class ChainReactionManager : MonoBehaviour
{
	[SerializeField] private float m_chainReactionDistance = 0.5f;

	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
		CheckForChainReaction();
	}

	private void CheckForChainReaction()
	{
		List<GameObject> bombs = GetBombs();

		foreach (GameObject bomb1 in bombs)
		{
			if (bomb1 == null)
			{
				continue;
			}

			Explosion explosion = bomb1.GetComponent<Explosion>();
			if (explosion.IsExploding)
			{
				foreach(GameObject bomb2 in bombs)
				{
					Explosion explosionState = bomb2.GetComponent<Explosion>();

					if ((bomb2 == null) || (bomb2 == bomb1) || (explosionState.IsLighting == true) || (explosionState.IsExploding == true))
					{
						continue;
					}

					if (Distance(bomb1.transform, bomb2.transform) < m_chainReactionDistance)
					{
						bomb2.GetComponent<Explosion>().LightBomb();
					}
				}
			}
		}
	}

	private float Distance(Transform bomb1, Transform bomb2)
	{
		return Vector3.Distance(bomb1.position, bomb2.position) * transform.parent.localScale.x;
	}

	private List<GameObject> GetBombs()
	{
		List<GameObject> bombs = new List<GameObject>();

		for (int i = 0; i < transform.childCount; i++)
		{
			bombs.Add(transform.GetChild(i).gameObject);
		}

		return bombs;
	}
}
