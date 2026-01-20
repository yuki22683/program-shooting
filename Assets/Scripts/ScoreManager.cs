using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class ScoreManager : MonoBehaviour
{
	private List<int> rolls = new List<int>();

	[SerializeField] private TextMeshProUGUI[] m_FramePointTexts = new TextMeshProUGUI[10];
	[SerializeField] private TextMeshProUGUI m_TotalScoreText;
	[SerializeField] private TextMeshProUGUI[] m_RollPointTexts = new TextMeshProUGUI[21];
	[SerializeField] private GameObject[] m_MissImages = new GameObject[21];
	[SerializeField] private GameObject[] m_StrikeImages = new GameObject[12];
	[SerializeField] private GameObject[] m_SpareImages = new GameObject[11];

	public void Roll(int pins)
	{
		if (pins < 0 || pins > 10)
			throw new System.ArgumentException("ピンの数は0～10でなければなりません。");

		rolls.Add(pins);
		UpdateScoreAndUI();
	}

	private void UpdateScoreAndUI()
	{
		int score = 0;
		int rollIndex = 0;

		for (int frame = 0; frame < 10; frame++)
		{
			if (rollIndex >= rolls.Count) break;

			if (IsStrike(rollIndex))
			{
				if (rollIndex + 2 >= rolls.Count) break;

				int bonus1 = rolls[rollIndex + 1];
				int bonus2 = rolls[rollIndex + 2];
				int frameScore = 10 + bonus1 + bonus2;
				score += frameScore;

				if (frame < m_StrikeImages.Length)
					m_StrikeImages[frame].SetActive(true);

				if (frame < m_FramePointTexts.Length)
					m_FramePointTexts[frame].text = frameScore.ToString();

				if (rollIndex < m_RollPointTexts.Length)
					m_RollPointTexts[rollIndex].text = "";

				rollIndex++;
			}
			else if (IsSpare(rollIndex))
			{
				if (rollIndex + 2 > rolls.Count) break;

				if (frame < m_SpareImages.Length)
					m_SpareImages[frame].SetActive(true);

				DisplayRoll(rollIndex, false);
				DisplaySpare(rollIndex + 1);

				int nextRollIndex = rollIndex + 2;
				if (nextRollIndex < rolls.Count)
				{
					int frameScore = 10 + rolls[nextRollIndex];
					score += frameScore;
					if (frame < m_FramePointTexts.Length)
						m_FramePointTexts[frame].text = frameScore.ToString();
				}

				rollIndex += 2;
			}
			else
			{
				if (rollIndex + 1 >= rolls.Count) break;

				int frameScore = rolls[rollIndex] + rolls[rollIndex + 1];
				if (frameScore > 10)
					throw new System.InvalidOperationException($"フレーム{frame + 1}の合計が10を超えています。");

				score += frameScore;

				if (frame < m_FramePointTexts.Length)
					m_FramePointTexts[frame].text = frameScore.ToString();

				DisplayRoll(rollIndex, false);
				DisplayRoll(rollIndex + 1, false);
				rollIndex += 2;
			}
		}

		for (int i = rollIndex; i < rolls.Count && i < 21; i++)
		{
			if (!IsStrike(i))
			{
				DisplayRoll(i, false);
			}
			else if (i < m_StrikeImages.Length)
			{
				m_StrikeImages[i].SetActive(true);
				m_RollPointTexts[i].text = "";
			}
		}

		m_TotalScoreText.text = score.ToString();
	}

	private bool IsStrike(int rollIndex)
	{
		return rollIndex < rolls.Count && rolls[rollIndex] == 10;
	}

	private bool IsSpare(int rollIndex)
	{
		return rollIndex + 1 < rolls.Count &&
			   rolls[rollIndex] + rolls[rollIndex + 1] == 10 &&
			   rolls[rollIndex] != 10;
	}

	private void DisplayRoll(int index, bool isSpare = false)
	{
		if (index >= rolls.Count || index >= m_RollPointTexts.Length) return;

		if (rolls[index] > 0)
		{
			m_RollPointTexts[index].text = rolls[index].ToString();
		}
		else
		{
			m_RollPointTexts[index].text = "";
			if (index < m_MissImages.Length)
				m_MissImages[index].SetActive(true);
		}
	}

	private void DisplaySpare(int index)
	{
		if (index >= m_RollPointTexts.Length) return;
		m_RollPointTexts[index].text = "";
	}
}
