using System;
using Meta.XR.MRUtilityKit.SceneDecorator;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static GameManager;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject rightTouchHandInteractor;
    [SerializeField] private GameObject rightGrabInteractor;
    [SerializeField] private GameObject leftTouchHandInteractor;
    [SerializeField] private GameObject leftGrabInteractor;
	[SerializeField] private GameObject rightCDHTouchHandInteractor;
	[SerializeField] private GameObject rightCDHGrabInteractor;
	[SerializeField] private GameObject leftCDHTouchHandInteractor;
	[SerializeField] private GameObject leftCDHGrabInteractor;

	[SerializeField] private GameObject abacusObject;
	[SerializeField] private GameObject abacusPanel;
	[SerializeField] private GameObject questionAbacusObject;

	[SerializeField] private GameObject tenkeyPanel;
	[SerializeField] private GameObject timerPanel;
	[SerializeField] private TimerController timerPanelController;
	[SerializeField] private GameObject menuPanel;
	[SerializeField] private GameObject tutorialPanel;
	[SerializeField] private GameObject tutorialBodyPanel;
	[SerializeField] private GameObject challengeHomePanel;
	[SerializeField] private GameObject challengeBasicPanel;
	[SerializeField] private ToggleBool challengeBasicPanelRead;
	[SerializeField] private ToggleBool challengeBasicPanelWrite;
	[SerializeField] private ToggleBool challengeBasicPanelEasy;
	[SerializeField] private ToggleBool challengeBasicPanelMedium;
	[SerializeField] private ToggleBool challengeBasicPanelHard;
	[SerializeField] private ToggleBool[] challengeBasicPanelQuestions;
	[SerializeField] private GameObject challengeAdditionPanel;
	[SerializeField] private ToggleBool challengeAdditionPanelNormal;
	[SerializeField] private ToggleBool challengeAdditionPanelChain;
	[SerializeField] private ToggleBool challengeAdditionPanelFlash;
	[SerializeField] private ToggleBool challengeAdditionPanelEasy;
	[SerializeField] private ToggleBool challengeAdditionPanelMedium;
	[SerializeField] private ToggleBool challengeAdditionPanelHard;
	[SerializeField] private ToggleBool[] challengeAdditionPanelQuestions;
	[SerializeField] private GameObject challengeSubtractionPanel;
	[SerializeField] private ToggleBool challengeSubtractionPanelEasy;
	[SerializeField] private ToggleBool challengeSubtractionPanelMedium;
	[SerializeField] private ToggleBool challengeSubtractionPanelHard;
	[SerializeField] private ToggleBool[] challengeSubtractionPanelQuestions;
	[SerializeField] private GameObject challengeMultiplicationPanel;
	[SerializeField] private ToggleBool challengeMultiplicationPanelEasy;
	[SerializeField] private ToggleBool challengeMultiplicationPanelMedium;
	[SerializeField] private ToggleBool challengeMultiplicationPanelHard;
	[SerializeField] private ToggleBool[] challengeMultiplicationPanelQuestions;
	[SerializeField] private GameObject challengeDivisionPanel;
	[SerializeField] private ToggleBool challengeDivisionPanelEasy;
	[SerializeField] private ToggleBool challengeDivisionPanelMedium;
	[SerializeField] private ToggleBool challengeDivisionPanelHard;
	[SerializeField] private ToggleBool[] challengeDivisionPanelQuestions;
	[SerializeField] private GameObject challengeCustomPanel;
	[SerializeField] private ToggleBool challengeCustomPanelAddition;
	[SerializeField] private ToggleBool challengeCustomPanelSubtraction;
	[SerializeField] private ToggleBool challengeCustomPanelMultiplication;
	[SerializeField] private ToggleBool challengeCustomPanelDivision;
	[SerializeField] private ToggleBool challengeCustomPanelEasy;
	[SerializeField] private ToggleBool challengeCustomPanelMedium;
	[SerializeField] private ToggleBool challengeCustomPanelHard;
	[SerializeField] private ToggleBool[] challengeCustomPanelQuestions;
	[SerializeField] private GameObject recordHomePanel;
	[SerializeField] private GameObject recordBasicPanel;
	[SerializeField] private ToggleBool recordBasicPanelRead;
	[SerializeField] private ToggleBool recordBasicPanelWrite;
	[SerializeField] private ToggleBool recordBasicPanelEasy;
	[SerializeField] private ToggleBool recordBasicPanelMedium;
	[SerializeField] private ToggleBool recordBasicPanelHard;
	[SerializeField] private ToggleBool[] recordBasicPanelQuestions;
	[SerializeField] private GameObject recordAdditionPanel;
	[SerializeField] private ToggleBool recordAdditionPanelNormal;
	[SerializeField] private ToggleBool recordAdditionPanelChain;
	[SerializeField] private ToggleBool recordAdditionPanelFlash;
	[SerializeField] private ToggleBool recordAdditionPanelEasy;
	[SerializeField] private ToggleBool recordAdditionPanelMedium;
	[SerializeField] private ToggleBool recordAdditionPanelHard;
	[SerializeField] private ToggleBool[] recordAdditionPanelQuestions;
	[SerializeField] private GameObject recordSubtractionPanel;
	[SerializeField] private ToggleBool recordSubtractionPanelEasy;
	[SerializeField] private ToggleBool recordSubtractionPanelMedium;
	[SerializeField] private ToggleBool recordSubtractionPanelHard;
	[SerializeField] private ToggleBool[] recordSubtractionPanelQuestions;
	[SerializeField] private GameObject recordMultiplicationPanel;
	[SerializeField] private ToggleBool recordMultiplicationPanelEasy;
	[SerializeField] private ToggleBool recordMultiplicationPanelMedium;
	[SerializeField] private ToggleBool recordMultiplicationPanelHard;
	[SerializeField] private ToggleBool[] recordMultiplicationPanelQuestions;
	[SerializeField] private GameObject recordDivitionPanel;
	[SerializeField] private ToggleBool recordDivisionPanelEasy;
	[SerializeField] private ToggleBool recordDivisionPanelMedium;
	[SerializeField] private ToggleBool recordDivisionPanelHard;
	[SerializeField] private ToggleBool[] recordDivisionPanelQuestions;
	[SerializeField] private GameObject recordCustomPanel;
	[SerializeField] private ToggleBool recordCustomPanelAddition;
	[SerializeField] private ToggleBool recordCustomPanelSubtraction;
	[SerializeField] private ToggleBool recordCustomPanelMultiplication;
	[SerializeField] private ToggleBool recordCustomPanelDivision;
	[SerializeField] private ToggleBool recordCustomPanelEasy;
	[SerializeField] private ToggleBool recordCustomPanelMedium;
	[SerializeField] private ToggleBool recordCustomPanelHard;
	[SerializeField] private ToggleBool[] recordCustomPanelQuestions;
	[SerializeField] private GameObject recordPanel;
	[SerializeField] private GameObject settingHomePanel;
	[SerializeField] private GameObject settingAbacusPanel;
	[SerializeField] private ButtonFlgExclusiveManager settingAbacusRodValueVisible;
	[SerializeField] private GameObject settingControlPanel;
	[SerializeField] private ButtonFlgExclusiveManager settingControlDebounceTime;
	//[SerializeField] private GameObject settingColorPanel;
	[SerializeField] private GameObject settingImmersivePanel;
	[SerializeField] private ButtonFlgExclusiveManager settingImmersiveToggle;
	[SerializeField] private GameObject settingSoundPanel;
	[SerializeField] private ButtonFlgExclusiveManager settingSoundSoundToggle;
	[SerializeField] private ButtonFlgExclusiveManager settingSoundSoundVolume;
	[SerializeField] private ButtonFlgExclusiveManager settingSoundBgmToggle;
	[SerializeField] private ButtonFlgExclusiveManager settingSoundBgmVolume;
	//[SerializeField] private GameObject settingLanguagePanel;
	[SerializeField] private GameObject privacyPolicyPanel;
	[SerializeField] private GameObject questionPanel;
	[SerializeField] private TextMeshProUGUI questionTextMeshPro;
	[SerializeField] private TextMeshProUGUI questionNoTextMeshPro;
	[SerializeField] private RectTransform questionPanelProgressActive;
	[SerializeField] private RectTransform questionPanelProgressInactive;
	[SerializeField] private TextMeshProUGUI questionPanelCountDownTextMeshPro;
	[SerializeField] private GameObject questionPanelCountDownImangeObject;
	[SerializeField] private KeyPadValue keyPadValue;
    [SerializeField] private TotalValue totalValue;
	[SerializeField] private TotalValue questionAbacusTotalValue;
	[SerializeField] private GameObject completeDialog;
	[SerializeField] private TextMeshProUGUI completeDialogTime;
	[SerializeField] private GameObject completeDialogNewRecordObject;
	[SerializeField] private TextMeshProUGUI coimpleteDialogHighScoreTime;
	[SerializeField] private FollowTarget followTarget;
	[SerializeField] private GameObject tenkeyBasicRead;
	[SerializeField] private KeyPadValue keyPadValueBasicRead;
	[SerializeField] private GameObject dialogPanel;
	[SerializeField] private TextMeshProUGUI dialogTextMeshPro;
	[SerializeField] private TextMeshProUGUI record_1st;
	[SerializeField] private TextMeshProUGUI record_2nd;
	[SerializeField] private TextMeshProUGUI record_3rd;
	[SerializeField] private TextMeshProUGUI record_4th;
	[SerializeField] private TextMeshProUGUI record_5th;
	[SerializeField] private TextMeshProUGUI record_date_1st;
	[SerializeField] private TextMeshProUGUI record_date_2nd;
	[SerializeField] private TextMeshProUGUI record_date_3rd;
	[SerializeField] private TextMeshProUGUI record_date_4th;
	[SerializeField] private TextMeshProUGUI record_date_5th;
	[SerializeField] private Transform centerEye;
	[SerializeField] private ToggleState abacusSoundMuteBtnToggleState;
	[SerializeField] private ToggleState keyPadSoundMuteBtnToggleState;
	[SerializeField] private ToggleState abacusBGMMuteBtnToggleState;
	[SerializeField] private ToggleState keyPadBGMMuteBtnToggleState;
	[SerializeField] private ToggleState abacusImmersiveBtnToggleState;
	[SerializeField] private ToggleState keyPadImmersiveBtnToggleState;
	[SerializeField] private ToggleState abacusRodValueVisibleBtnToggleState;
	[SerializeField] private OVRPassthroughLayer passthroughLayer;
	[SerializeField] private Material[] skyboxMaterials;
	[SerializeField] private GameObject[] skyboxPreviewImageObjects;
	[SerializeField] private GameObject[] tutorialRods;
	[SerializeField] private GameObject[] tutorialRod1Beads;
	[SerializeField] private GameObject[] tutorialRod2Beads;
	[SerializeField] private GameObject[] tutorialRod3Beads;
	[SerializeField] private GameObject[] tutorialRod4Beads;
	[SerializeField] private GameObject[] tutorialRod5Beads;
	[SerializeField] private GameObject[] tutorialRod6Beads;
	[SerializeField] private GameObject[] tutorialRod7Beads;
	[SerializeField] private GameObject tutorialBeam;
	[SerializeField] private Button tutorialLeftArrow;
	[SerializeField] private Button tutorialRightArrow;
	[SerializeField] private TextMeshProUGUI tutorialTextTitle;
	[SerializeField] private TextMeshProUGUI tutorialTextBody;
	[SerializeField] private TextMeshProUGUI tutorialTextPage;
	[SerializeField] private GameObject[] tutorialRodArrows;
	[SerializeField] private GameObject[] tutorialDigitArrows;
	[SerializeField] private GameObject[] tutorialDigitLines;
	[SerializeField] private Material tutorialBeadMaterialActive;
	[SerializeField] private Material tutorialBeadMaterialInActive;
	[SerializeField] private Material tutorialRodMaterialActive;
	[SerializeField] private Material tutorialRodMaterialInActive;
	[SerializeField] private Material tutorialBeamMaterialActive;
	[SerializeField] private Material tutorialBeamMaterialInActive;
	[SerializeField] private GameObject tutorialHand;
	public const float PROGRESS_BAR_WIDTH = 1000;
	public const int COUNT_DONW_TIME = 3;
	public const int BEADS_COUNT = 5;
	public const int ROD_COUNT = 7;
	public const int DIGIT_RANGE_MIN_DEF = 2;
	public const int DIGIT_RANGE_MAX_DEF = 3;
	public const int DIGIT_RANGE_DIVISION_NUM1_MIN_DEF = 3;
	public const int DIGIT_RANGE_DIVISION_NUM1_MAX_DEF = 4;
	public const int DIGIT_RANGE_DIVISION_NUM2_MIN_DEF = 1;
	public const int DIGIT_RANGE_DIVISION_NUM2_MAX_DEF = 2;
	public const float ROD_DEFAULT_COLOR_R = 0.706f;
	public const float ROD_DEFAULT_COLOR_G = 0.529f;
	public const float ROD_DEFAULT_COLOR_B = 0.035f;

	public const float FRAME_DEFAULT_COLOR_R = 0.25f;
	public const float FRAME_DEFAULT_COLOR_G = 0.25f;
	public const float FRAME_DEFAULT_COLOR_B = 0.25f;

	public const float BEAD_DEFAULT_COLOR_R = 0.784f;
	public const float BEAD_DEFAULT_COLOR_G = 0.392f;
	public const float BEAD_DEFAULT_COLOR_B = 0.02f;
	public const int QUESTION_COUNT_ID_MAX = 10;

	public const float COUNTROL_DEBOUNCE_TIME_SHORT = 0.2f;
	public const float COUNTROL_DEBOUNCE_TIME_MEDIUM = 0.8f;
	public const float COUNTROL_DEBOUNCE_TIME_LONG = 1.4f;
	public const float SOUND_VOLUME_SOFT = 0.2f;
	public const float SOUND_VOLUME_MEDIUM = 0.6f;
	public const float SOUND_VOLUME_LOUD = 1.0f;
	public const int SKYBOX_ID_MAX = 10;
	public const int TUTORIAL_BASIC_SEQ_LEN = 27;
	public const int TUTORIAL_ADD_SEQ_LEN = 44;
	public const int TUTORIAL_SUB_SEQ_LEN = 37;
	public const int TUTORIAL_MULTI_SEQ_LEN = 54;
	public const int TUTORIAL_DIVISION_SEQ_LEN = 31;
	public static readonly int[] seqNums = new int[] {
		TUTORIAL_BASIC_SEQ_LEN,
		TUTORIAL_ADD_SEQ_LEN,
		TUTORIAL_SUB_SEQ_LEN,
		TUTORIAL_MULTI_SEQ_LEN,
		TUTORIAL_DIVISION_SEQ_LEN
	};
	private AudioSource[] audioSources;
	private string valueBasicWrite = "0";
	private string valueTutorial = "0";
	public enum AudioType
	{
		CountDown = 0,
		NextQuestion = 1,
		Correct = 2,
		Complete = 3,
	}

    private int _keyPadValue = 0;
	private MenuModeInfo menuModeInfo;
	private TutorialModeInfo tutorialModeInfo;
	private ChallengeModeInfo challengeModeInfo;
	private RecordModeInfo recordModeInfo;
	private SettingModeInfo settingModeInfo;
	private DialogType dialogType = DialogType.None;

	public struct MenuModeInfo
	{
		public MenuModeType menuModeType;
		public ChallengeMenuType challengeMenuType;
		public RecordMenuType recordMenuType;
		public SettingMenuType settingMenuType;
	}

	public struct TutorialModeInfo
	{
		public TutorialModeState tutorialModeState;
		public TutorialType tutorialType;
		public int tutorialSeqNo;
		public int answer;
	}

	public struct ChallengeModeInfo
	{
		public ChallengeModeState challengeModeState;
		public ChallengeType challengeType;
		public ChallengeDifficultyType challengeDifficultyType;
		public ChallengeAdditionType challengeAdditionType;
		public ChallengeBasicType challengeBasicType;
		public int challengeNumberOfQuestions;
		public int currentNumberOfQuestion;
		public int num1;
		public int num2;
		public ArithmeticType arithmeticType;
		public int answer;
		public DigitRangeInfo digitRangeInfo;

		public ChallengeCustomInfo challengeCustomInfo;
		public int count;
		public string completeTime;
	}

	public struct RecordModeInfo
	{
		public RecordMenuType recordType;
		public ChallengeType challengeType;
		public ChallengeDifficultyType recordDifficultyType;
		public ChallengeAdditionType recordAdditionType;
		public ChallengeBasicType recordBasicType;
		public int recordNumberOfQuestions;
		public ChallengeCustomInfo recordCustomInfo;
	}

	public struct SettingModeInfo
	{
		public SettingMenuType settingMenuType;
	}

	public struct PrivacyPolicyModeInfo
	{
	}

    public GameMode gameMode = GameMode.MenuMode;
	public int abacusDigit = 7;

	public bool isActive = true;

    public enum GameMode
    {
        MenuMode = 0,
		TutorialMode = 1,
		ChallengeMode = 2,
        RecordMode = 3,
		SettingMode = 4,
        PrivacyPolicyMode =5
	}

    public enum MenuModeType
    {
		HomeMenu = 0,
        TutorialMenu = 1,
        ChallengeMenu = 2,
		RecordMenu = 3,
		SettingMenu = 4,
	}
    public enum  ChallengeMenuType
    {
        Home = 0,
		Basic = 1,
		Addition = 2,
		Subtraction = 3,
		Multiplication = 4,
		Division = 5,
		Custom = 6,
	}

    public enum RecordMenuType
    {
		Home = 0,
		Basic = 1,
		Addition = 2,
		Subtraction = 3,
		Multiplication = 4,
		Division = 5,
		Custom = 6,
	}

	public enum SettingMenuType
	{
        Home = 0,
		Abacus = 1,
		Control = 2,
		Color = 3,
		Immersive = 4,
		Sound = 5,
		Language = 6,
	}

	public enum HomeMenuItem
	{
		Tutorial = 0,
		Challenge = 1,
		Record = 2,
		Setting = 3,
		PrivacyPolicy = 4,
	}

	public enum  TutorialType
    {
		Basic = 0,
		Addition = 1,
		Subtraction = 2,
		Multiplication = 3,
		Division = 4,
		Num = 5
	}

	public enum  ChallengeType
    {
		Basic = 0,
		Addition = 1,
		Subtraction = 2,
		Multiplication = 3,
		Division = 4,
		Custom = 5,
	}

	public enum ChallengeDifficultyType
    {
		Easy = 0,
		Medium = 1,
		Hard = 2,
	}

	public enum ChallengeBasicType
	{
		None = 0,
		Read = 1,
		Write = 2,
	}

	public enum ChallengeAdditionType
	{
		None = 0,
		Normal = 1,
		Chain = 2,
		Flash = 3,
	}

    public enum TutorialModeState
    {
		Playing = 1,
		Waiting = 2,
		Finish = 3,
	}

    public enum ChallengeModeState
    {
		CountDown = 0,
		Playing = 1,
		Waiting = 2,
		Finish = 3,
	}

	public enum ArithmeticType
	{
		None = 0,
		Addition = 1,
		Subtraction = 2,
		Multiplication = 3,
		Division = 4,
		Num = 5,
	}

	[System.Serializable]
	public struct DigitRangeInfo
	{
		public DifficultyTypeDigitRange easy;
		public DifficultyTypeDigitRange medium;
		public DifficultyTypeDigitRange hard;
	}

	[System.Serializable]
	public struct DifficultyTypeDigitRange
	{
		public ArithmeticDigitRange basic;
		public ArithmeticDigitRange add;
		public ArithmeticDigitRange sub;
		public ArithmeticDigitRange multi;
		public ArithmeticDigitRange divide;
		
	}
	[System.Serializable]
	public struct ArithmeticDigitRange
	{
		public DigitRange num1;
		public DigitRange num2;
	}
	[System.Serializable]
	public struct DigitRange
	{
		public int min;
		public int max;
	}

	public struct ChallengeCustomInfo
    {
		public bool add;
		public bool sub;
		public bool multi;
		public bool divide;
	}

	public enum DialogType
	{
		None = 0,
		HomeButton = 1,
		DeleteRecord = 2,
		Num = 3,
	}

	void Awake()
	{
		// SoundManager.Instance.Initialize();
		// SetBgmVolume();
		// SetSoundVolume();
		// ImmersiveSetting();
		// ImmersiveOnOff();
		// abacusRodValueVisibleBtnToggleState.SetOnOff(DataManager.gameSettings.abacusSettings.isRodValueVisible);
		// RenderSettings.skybox = skyboxMaterials[DataManager.gameSettings.immersiveSettings.skyBoxId];
	}

	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
    {
		challengeModeInfo.digitRangeInfo = DataManager.gameSettings.challengeModeSettings.digitRangeInfo;
		audioSources = GetComponents<AudioSource>();

		Invoke("SwitchStartGameMode", 0.5f);
    }

	// Update is called once per frame
	void Update()
    {
#if true
		CheckKeyInput();
#endif
		CheckGameMode();
    }

	private void SwitchStartGameMode()
	{
		if (DataManager.gameSettings.isPrivacyPolicyAccept)
		{
			menuPanel.transform.position = followTarget.transform.position;
			menuPanel.transform.rotation = followTarget.transform.rotation;
			followTarget.target = menuPanel.transform;
			SwitchGameMode(GameMode.MenuMode);
		}
		else
		{
			privacyPolicyPanel.transform.position = followTarget.transform.position;
			privacyPolicyPanel.transform.rotation = followTarget.transform.rotation;
			followTarget.target = privacyPolicyPanel.transform;
			SwitchGameMode(GameMode.PrivacyPolicyMode);
		}
	}

	private void ChallengePose(bool flg)
	{
		if (flg)
		{
			challengeModeInfo.challengeModeState = ChallengeModeState.Waiting;
			timerPanelController.StopTimer();
		}
		else
		{
			challengeModeInfo.challengeModeState = ChallengeModeState.Playing;
			timerPanelController.ReStartTimer();
		}
	}

	private void TutorialPose(bool flg)
	{
		if (flg)
		{
			tutorialModeInfo.tutorialModeState = TutorialModeState.Waiting;
			tutorialBodyPanel.SetActive(false);
		}
		else
		{
			tutorialModeInfo.tutorialModeState = TutorialModeState.Playing;
			SetActive(tutorialBodyPanel);
		}
	}

	private void SetActive(GameObject gameObject)
	{
		gameObject.SetActive(true);
		SoundManager.Instance.ApplyMuteToNewAudioSource(gameObject);
	}

	private void SetBgmVolume()
	{
		if (DataManager.gameSettings.soundSettings.isBgmMute)
		{
			abacusBGMMuteBtnToggleState.SetOnOff(false);
			keyPadBGMMuteBtnToggleState.SetOnOff(false);
			settingSoundBgmToggle.ToggleExclusive(1);
			SoundManager.Instance.SetVolume(0.0f);
		}
		else
		{
			abacusBGMMuteBtnToggleState.SetOnOff(true);
			keyPadBGMMuteBtnToggleState.SetOnOff(true);
			settingSoundBgmToggle.ToggleExclusive(0);
			SoundManager.Instance.SetVolume(DataManager.gameSettings.soundSettings.bgmVolume);
		}
	}

	private void SetSoundVolume()
	{
		if (DataManager.gameSettings.soundSettings.isSoundMute)
		{
			abacusSoundMuteBtnToggleState.SetOnOff(false);
			keyPadSoundMuteBtnToggleState.SetOnOff(false);
			settingSoundSoundToggle.ToggleExclusive(1);
			SoundManager.Instance.SetSoundVolume(0.0f);
		}
		else
		{
			abacusSoundMuteBtnToggleState.SetOnOff(true);
			keyPadSoundMuteBtnToggleState.SetOnOff(true);
			settingSoundSoundToggle.ToggleExclusive(0);
			SoundManager.Instance.SetSoundVolume(DataManager.gameSettings.soundSettings.soundVolume);
		}
	}

	public void ToggleBgmMute()
	{
		if (DataManager.gameSettings.soundSettings.isBgmMute)
		{
			DataManager.gameSettings.soundSettings.isBgmMute = false;
		}
		else
		{
			DataManager.gameSettings.soundSettings.isBgmMute = true;
		}
		DataManager.SaveSettings();

		SetBgmVolume();
	}

	public void ToggleSoundMute()
	{
		if (DataManager.gameSettings.soundSettings.isSoundMute)
		{
			DataManager.gameSettings.soundSettings.isSoundMute = false;
		}
		else
		{
			DataManager.gameSettings.soundSettings.isSoundMute = true;
		}
		abacusSoundMuteBtnToggleState.SetOnOff(!DataManager.gameSettings.soundSettings.isSoundMute);
		keyPadSoundMuteBtnToggleState.SetOnOff(!DataManager.gameSettings.soundSettings.isSoundMute);
		DataManager.SaveSettings();

		Invoke("SetSoundVolume", 0.5f);
	}

	public void ImmersiveSetting()
	{
		PassthroughManager.SetPassthroughLayer(passthroughLayer);
		PassthroughManager.SetCamera(centerEye.GetComponent<Camera>());
	}

	void ImmersiveOnOff()
	{
		if (DataManager.gameSettings.immersiveSettings.isImmersive)
		{
			abacusImmersiveBtnToggleState.SetOnOff(false);
			keyPadImmersiveBtnToggleState.SetOnOff(false);
			settingImmersiveToggle.ToggleExclusive(0);
			PassthroughManager.DisablePassthrough();
		}
		else
		{
			abacusImmersiveBtnToggleState.SetOnOff(true);
			keyPadImmersiveBtnToggleState.SetOnOff(true);
			settingImmersiveToggle.ToggleExclusive(1);
			PassthroughManager.EnablePassthrough();
		}
	}

	public void ToggleRodValueVisible()
	{
		DataManager.gameSettings.abacusSettings.isRodValueVisible = !DataManager.gameSettings.abacusSettings.isRodValueVisible;
		abacusRodValueVisibleBtnToggleState.SetOnOff(DataManager.gameSettings.abacusSettings.isRodValueVisible);
		DataManager.SaveSettings();	
	}

	public void ToggleImmersive()
	{
		if (DataManager.gameSettings.immersiveSettings.isImmersive)
		{
			DataManager.gameSettings.immersiveSettings.isImmersive = false;
		}
		else
		{
			DataManager.gameSettings.immersiveSettings.isImmersive = true;
		}
		abacusImmersiveBtnToggleState.SetOnOff(!DataManager.gameSettings.immersiveSettings.isImmersive);
		keyPadImmersiveBtnToggleState.SetOnOff(!DataManager.gameSettings.immersiveSettings.isImmersive);
		DataManager.SaveSettings();

		ImmersiveOnOff();
	}

	private void CheckKeyInput()
	{
		if ((gameMode == GameMode.ChallengeMode)
		&& (challengeModeInfo.challengeType == ChallengeType.Basic))
		{
			if (challengeModeInfo.challengeBasicType == ChallengeBasicType.Read)
			{
				if (Input.GetKeyDown(KeyCode.Alpha0))		{ keyPadValueBasicRead.SetValue(0); }
				if (Input.GetKeyDown(KeyCode.Alpha1))		{ keyPadValueBasicRead.SetValue(1); }
				if (Input.GetKeyDown(KeyCode.Alpha2))		{ keyPadValueBasicRead.SetValue(2); }
				if (Input.GetKeyDown(KeyCode.Alpha3))		{ keyPadValueBasicRead.SetValue(3); }
				if (Input.GetKeyDown(KeyCode.Alpha4))		{ keyPadValueBasicRead.SetValue(4); }
				if (Input.GetKeyDown(KeyCode.Alpha5))		{ keyPadValueBasicRead.SetValue(5); }
				if (Input.GetKeyDown(KeyCode.Alpha6))		{ keyPadValueBasicRead.SetValue(6); }
				if (Input.GetKeyDown(KeyCode.Alpha7))		{ keyPadValueBasicRead.SetValue(7); }
				if (Input.GetKeyDown(KeyCode.Alpha8))		{ keyPadValueBasicRead.SetValue(8); }
				if (Input.GetKeyDown(KeyCode.Alpha9))		{ keyPadValueBasicRead.SetValue(9); }
				if (Input.GetKeyDown(KeyCode.Backspace))	{ keyPadValueBasicRead.BackSpace(); }
				if (Input.GetKeyDown(KeyCode.Delete))		{ keyPadValueBasicRead.ClearValue(); }
				if (Input.GetKeyDown(KeyCode.Space))		{ SceneManager.LoadScene("Main"); }
			}
			else
			{
				if (Input.GetKeyDown(KeyCode.Alpha0))		{ valueBasicWrite += "0"; totalValue.SetTotalValue(int.Parse(valueBasicWrite)); }
				if (Input.GetKeyDown(KeyCode.Alpha1))		{ valueBasicWrite += "1"; totalValue.SetTotalValue(int.Parse(valueBasicWrite)); }
				if (Input.GetKeyDown(KeyCode.Alpha2))		{ valueBasicWrite += "2"; totalValue.SetTotalValue(int.Parse(valueBasicWrite)); }
				if (Input.GetKeyDown(KeyCode.Alpha3))		{ valueBasicWrite += "3"; totalValue.SetTotalValue(int.Parse(valueBasicWrite)); }
				if (Input.GetKeyDown(KeyCode.Alpha4))		{ valueBasicWrite += "4"; totalValue.SetTotalValue(int.Parse(valueBasicWrite)); }
				if (Input.GetKeyDown(KeyCode.Alpha5))		{ valueBasicWrite += "5"; totalValue.SetTotalValue(int.Parse(valueBasicWrite)); }
				if (Input.GetKeyDown(KeyCode.Alpha6))		{ valueBasicWrite += "6"; totalValue.SetTotalValue(int.Parse(valueBasicWrite)); }
				if (Input.GetKeyDown(KeyCode.Alpha7))		{ valueBasicWrite += "7"; totalValue.SetTotalValue(int.Parse(valueBasicWrite)); }
				if (Input.GetKeyDown(KeyCode.Alpha8))		{ valueBasicWrite += "8"; totalValue.SetTotalValue(int.Parse(valueBasicWrite)); }
				if (Input.GetKeyDown(KeyCode.Alpha9))		{ valueBasicWrite += "9"; totalValue.SetTotalValue(int.Parse(valueBasicWrite)); }
				if (Input.GetKeyDown(KeyCode.Delete))		{ valueBasicWrite = "0"; totalValue.SetTotalValue(int.Parse(valueBasicWrite)); }
				if (Input.GetKeyDown(KeyCode.Space))		{ SceneManager.LoadScene("Main"); }
				if (Input.GetKeyDown(KeyCode.Backspace))
				{
					if (valueBasicWrite.Length > 1)
					{
						valueBasicWrite = valueBasicWrite.Remove(valueBasicWrite.Length - 1);
					}
					else
					{
						valueBasicWrite = "0";
					}
					totalValue.SetTotalValue(int.Parse(valueBasicWrite));
				}
			}
		}
		else if (gameMode == GameMode.TutorialMode)
		{
				if (Input.GetKeyDown(KeyCode.Alpha0))		{ valueTutorial += "0"; totalValue.SetTotalValue(int.Parse(valueTutorial)); }
				if (Input.GetKeyDown(KeyCode.Alpha1))		{ valueTutorial += "1"; totalValue.SetTotalValue(int.Parse(valueTutorial)); }
				if (Input.GetKeyDown(KeyCode.Alpha2))		{ valueTutorial += "2"; totalValue.SetTotalValue(int.Parse(valueTutorial)); }
				if (Input.GetKeyDown(KeyCode.Alpha3))		{ valueTutorial += "3"; totalValue.SetTotalValue(int.Parse(valueTutorial)); }
				if (Input.GetKeyDown(KeyCode.Alpha4))		{ valueTutorial += "4"; totalValue.SetTotalValue(int.Parse(valueTutorial)); }
				if (Input.GetKeyDown(KeyCode.Alpha5))		{ valueTutorial += "5"; totalValue.SetTotalValue(int.Parse(valueTutorial)); }
				if (Input.GetKeyDown(KeyCode.Alpha6))		{ valueTutorial += "6"; totalValue.SetTotalValue(int.Parse(valueTutorial)); }
				if (Input.GetKeyDown(KeyCode.Alpha7))		{ valueTutorial += "7"; totalValue.SetTotalValue(int.Parse(valueTutorial)); }
				if (Input.GetKeyDown(KeyCode.Alpha8))		{ valueTutorial += "8"; totalValue.SetTotalValue(int.Parse(valueTutorial)); }
				if (Input.GetKeyDown(KeyCode.Alpha9))		{ valueTutorial += "9"; totalValue.SetTotalValue(int.Parse(valueTutorial)); }
				if (Input.GetKeyDown(KeyCode.Delete))		{ valueTutorial = "0"; totalValue.SetTotalValue(int.Parse(valueTutorial)); }
				if (Input.GetKeyDown(KeyCode.Space))		{ SceneManager.LoadScene("Main"); }
				if (Input.GetKeyDown(KeyCode.Backspace))
				{
					if (valueTutorial.Length > 1)
					{
						valueTutorial = valueTutorial.Remove(valueTutorial.Length - 1);
					}
					else
					{
						valueTutorial = "0";
					}
					totalValue.SetTotalValue(int.Parse(valueTutorial));
				}
				if (Input.GetKeyDown(KeyCode.RightArrow)) { TutorialSetNextSeq(); }
				if (Input.GetKeyDown(KeyCode.LeftArrow)) { TutorialSetPreviousSeq(); }
		}
		else
		{
			if (Input.GetKeyDown(KeyCode.Alpha0))		{ keyPadValue.SetValue(0); }
			if (Input.GetKeyDown(KeyCode.Alpha1))		{ keyPadValue.SetValue(1); }
			if (Input.GetKeyDown(KeyCode.Alpha2))		{ keyPadValue.SetValue(2); }
			if (Input.GetKeyDown(KeyCode.Alpha3))		{ keyPadValue.SetValue(3); }
			if (Input.GetKeyDown(KeyCode.Alpha4))		{ keyPadValue.SetValue(4); }
			if (Input.GetKeyDown(KeyCode.Alpha5))		{ keyPadValue.SetValue(5); }
			if (Input.GetKeyDown(KeyCode.Alpha6))		{ keyPadValue.SetValue(6); }
			if (Input.GetKeyDown(KeyCode.Alpha7))		{ keyPadValue.SetValue(7); }
			if (Input.GetKeyDown(KeyCode.Alpha8))		{ keyPadValue.SetValue(8); }
			if (Input.GetKeyDown(KeyCode.Alpha9))		{ keyPadValue.SetValue(9); }
			if (Input.GetKeyDown(KeyCode.Backspace))	{ keyPadValue.BackSpace(); }
			if (Input.GetKeyDown(KeyCode.Delete))		{ keyPadValue.ClearValue(); }
			if (Input.GetKeyDown(KeyCode.Space))		{ SceneManager.LoadScene("Main"); }
		}
	}

	public void SetAbacusInactive()
    {
        BeadColorManager[] beads = GetComponentsInChildren<BeadColorManager>();

        foreach(BeadColorManager bead in beads)
        {
            bead.SetMeshColor(BeadColorManager.BeadColorType.Disable);
        }
    }

	public void SetAbacusActive()
	{
		BeadColorManager[] beads = GetComponentsInChildren<BeadColorManager>();

		foreach (BeadColorManager bead in beads)
		{
            int value = bead.transform.parent.GetComponent<BeadValue>().value;
            if (value > 0)
            {
				bead.SetMeshColor(BeadColorManager.BeadColorType.Active);
			}
            else
            {
				bead.SetMeshColor(BeadColorManager.BeadColorType.None);
			}
		}
	}

	public void OnClickHomeTutorial()
	{
		SwitchMenu(MenuModeType.TutorialMenu);
	}

	public void OnClickHomeChallenge()
	{
		SwitchMenu(MenuModeType.ChallengeMenu);
	}

	public void OnClickHomeRecord()
	{
		SwitchMenu(MenuModeType.RecordMenu);
	}

	public void OnClickHomeSetting()
	{
		SwitchMenu(MenuModeType.SettingMenu);
	}

	public void OnClickHomePrivacyPolicy()
	{
		SwitchGameMode(GameMode.PrivacyPolicyMode);
	}

	public void OnClickTutorialBasic()
	{
		tutorialModeInfo.tutorialType = TutorialType.Basic;
		TutorialSetTextTitle();
		SwitchGameMode(GameMode.TutorialMode);
	}

	public void OnClickTutorialAddition()
	{
		tutorialModeInfo.tutorialType = TutorialType.Addition;
		TutorialSetTextTitle();
		SwitchGameMode(GameMode.TutorialMode);
	}

	public void OnClickTutorialSubstruction()
	{
		tutorialModeInfo.tutorialType = TutorialType.Subtraction;
		TutorialSetTextTitle();
		SwitchGameMode(GameMode.TutorialMode);
	}

	public void OnClickTutorialMultiplication()
	{
		tutorialModeInfo.tutorialType = TutorialType.Multiplication;
		TutorialSetTextTitle();
		SwitchGameMode(GameMode.TutorialMode);
	}

	public void OnClickTutorialDivision()
	{
		tutorialModeInfo.tutorialType = TutorialType.Division;
		TutorialSetTextTitle();
		SwitchGameMode(GameMode.TutorialMode);
	}

	public void OnClickTutorialLeftArrow()
	{
		TutorialSetPreviousSeq();
	}

	public void OnClickTurotialRightArrow()
	{
		TutorialSetBeadMaterialAll(false);
		TutorialSetNextSeq();
	}

	public void OnClickChallengeHomeBasic()
	{
		challengeModeInfo.challengeType = ChallengeType.Basic;
		SwitchChallengeMenu(ChallengeMenuType.Basic);
	}

	public void OnClickChallengeHomeAddition()
	{
		challengeModeInfo.challengeType = ChallengeType.Addition;
		SwitchChallengeMenu(ChallengeMenuType.Addition);
	}

	public void OnClickChallengeHomeSubstruction()
	{
		challengeModeInfo.challengeType = ChallengeType.Subtraction;
		SwitchChallengeMenu(ChallengeMenuType.Subtraction);
	}

	public void OnClickChallengeHomeMultiplication()
	{
		challengeModeInfo.challengeType = ChallengeType.Multiplication;
		SwitchChallengeMenu(ChallengeMenuType.Multiplication);
	}

	public void OnClickChallengeHomeDivision()
	{
		challengeModeInfo.challengeType = ChallengeType.Division;
		SwitchChallengeMenu(ChallengeMenuType.Division);
	}

	public void OnClickChallengeHomeCustom()
	{
		challengeModeInfo.challengeType =  ChallengeType.Custom;
		SwitchChallengeMenu(ChallengeMenuType.Custom);
	}

	public void OnClickBackHome()
	{
		SwitchMenu(MenuModeType.HomeMenu);
	}

	public void OnClickChallengeBasicStart()
	{
		CheckChallengeBasicPanelInput();

		SwitchGameMode(GameMode.ChallengeMode);
	}

	public void OnClickChallengeAdditionStart()
	{
		CheckChallengeAdditionPanelInput();

		SwitchGameMode(GameMode.ChallengeMode);
	}

	public void OnClickChallengeSubtractionStart()
	{
		CheckChallengeSubtractionPanelInput();

		SwitchGameMode(GameMode.ChallengeMode);
	}

	public void OnClickChallengeMultiplicationStart()
	{
		CheckChallengeMultiplicationPanelInput();

		SwitchGameMode(GameMode.ChallengeMode);
	}

	public void OnClickChallengeDivisionStart()
	{
		CheckChallengeDivisionPanelInput();

		SwitchGameMode(GameMode.ChallengeMode);
	}

	public void OnClickChallengeCustomStart()
	{
		CheckChallengeCustomPanelInput();

		SwitchGameMode(GameMode.ChallengeMode);
	}

	public void OnClickChallengeArithmeticBack()
	{
		SwitchChallengeMenu(ChallengeMenuType.Home);
	}

	public void OnClickRecordHomeBasic()
	{
		recordModeInfo.recordType = RecordMenuType.Basic;
		recordModeInfo.challengeType = ChallengeType.Basic;
		SwitchRecordMenu(RecordMenuType.Basic);
		Invoke("CheckRecordBasicPanelInput", 0.5f);
	}

	public void OnClickRecordHomeAddition()
	{
		recordModeInfo.recordType = RecordMenuType.Addition;
		recordModeInfo.challengeType = ChallengeType.Addition;
		SwitchRecordMenu(RecordMenuType.Addition);
		Invoke("CheckRecordAdditionPanelInput", 0.5f);
	}

	public void OnClickRecordHomeSubstruction()
	{
		recordModeInfo.recordType = RecordMenuType.Subtraction;
		recordModeInfo.challengeType = ChallengeType.Subtraction;
		SwitchRecordMenu(RecordMenuType.Subtraction);
		Invoke("CheckRecordSubtractionPanelInput", 0.5f);
	}

	public void OnClickRecordHomeMultiplication()
	{
		recordModeInfo.recordType = RecordMenuType.Multiplication;
		recordModeInfo.challengeType = ChallengeType.Multiplication;
		SwitchRecordMenu(RecordMenuType.Multiplication);
		Invoke("CheckRecordMultiplicationPanelInput", 0.5f);
	}

	public void OnClickRecordHomeDivision()
	{
		recordModeInfo.recordType = RecordMenuType.Division;
		recordModeInfo.challengeType = ChallengeType.Division;
		SwitchRecordMenu(RecordMenuType.Division);
		Invoke("CheckChallengeDivisionPanelInput", 0.5f);
	}

	public void OnClickRecordHomeCustom()
	{
		recordModeInfo.recordType =  RecordMenuType.Custom;
		recordModeInfo.challengeType = ChallengeType.Custom;
		SwitchRecordMenu(RecordMenuType.Custom);
	}

	public void OnClickRecordArithmeticBack()
	{
		SwitchRecordMenu(RecordMenuType.Home);
	}

	public void OnUpdateRecordBasicParam()
	{
		CheckRecordBasicPanelInput();
	}

	public void OnUpdateRecordAdditionParam()
	{
		CheckRecordAdditionPanelInput();
	}

	public void OnUpdateRecordSubstractionParam()
	{
		CheckRecordSubtractionPanelInput();
	}

	public void OnUpdateRecordMultiplicationParam()
	{
		CheckRecordMultiplicationPanelInput();
	}

	public void OnUpdateRecordDivisionParam()
	{
		CheckRecordDivisionPanelInput();
	}

	public void OnUpdateRecordCustomParam()
	{
		CheckRecordCustomPanelInput();
	}

	public void OnClickDeleteRecord()
	{
		SetDialog(DialogType.DeleteRecord);
	}

	public void OnClickHomeButton()
	{
		SetDialog(DialogType.HomeButton);

		switch (gameMode)
		{
			case GameMode.MenuMode:
				break;
			case GameMode.TutorialMode:
				TutorialPose(true);
				break;
			case GameMode.ChallengeMode:
				ChallengePose(true);
				break;
			case GameMode.RecordMode:
				break;
			case GameMode.SettingMode:
				break;
			case GameMode.PrivacyPolicyMode:
				break;
		}
	}

	public void OnClickPrivacyPolicyButton()
	{
		SetActive(privacyPolicyPanel);
		Vector3 pos = followTarget.transform.localPosition;
		pos.x  +=  1f;
		privacyPolicyPanel.transform.localPosition = pos;
		Quaternion rot = Quaternion.LookRotation(privacyPolicyPanel.transform.position - centerEye.position);
		privacyPolicyPanel.transform.rotation = rot;

		switch (gameMode)
		{
			case GameMode.MenuMode:
				break;
			case GameMode.TutorialMode:
				TutorialPose(true);
				break;
			case GameMode.ChallengeMode:
				ChallengePose(true);
				break;
			case GameMode.RecordMode:
				break;
			case GameMode.SettingMode:
				break;
			case GameMode.PrivacyPolicyMode:
				break;
		}
	}

	public void OnClickPrivacyPolicyAcceptButton()
	{
		privacyPolicyPanel.SetActive(false);
		DataManager.gameSettings.isPrivacyPolicyAccept = true;
		DataManager.SaveSettings();

		switch (gameMode)
		{
			case GameMode.MenuMode:
				break;
			case GameMode.TutorialMode:
				TutorialPose(false);
				break;
			case GameMode.ChallengeMode:
				ChallengePose(false);
				break;
			case GameMode.RecordMode:
				break;
			case GameMode.SettingMode:
				break;
			case GameMode.PrivacyPolicyMode:
				SwitchGameMode(GameMode.MenuMode);
				break;
		}
	}

	public void OnClickPrivacyPolicyDeclineButton()
	{
		privacyPolicyPanel.SetActive(false);
		DataManager.gameSettings.isPrivacyPolicyAccept = false;
		DataManager.SaveSettings();
		SwitchGameMode(GameMode.MenuMode);
	}

	public void OnClickDialogYesButton()
	{
		switch (dialogType)
		{
			case DialogType.HomeButton:
				SwitchGameMode(GameMode.MenuMode);
				break;
			case DialogType.DeleteRecord:
				DeleteRecord();
				break;
		}
		dialogPanel.SetActive(false);
	}

	public void OnClickDialogNoButton()
	{
		dialogPanel.SetActive(false);
	
		switch (gameMode)
		{
			case GameMode.MenuMode:
				break;
			case GameMode.TutorialMode:
				TutorialPose(false);
				break;
			case GameMode.ChallengeMode:
				ChallengePose(false);
				break;
			case GameMode.RecordMode:
				break;
			case GameMode.SettingMode:
				break;
			case GameMode.PrivacyPolicyMode:
				break;
		}
	}

	public void OnClickChallengeCompleteExitButton()
	{
		SwitchGameMode(GameMode.MenuMode);
	}

	public void OnClickSettingHomeAbacus()
	{
		settingModeInfo.settingMenuType = SettingMenuType.Abacus;

		if (DataManager.gameSettings.abacusSettings.isRodValueVisible)
		{
			settingAbacusRodValueVisible.ToggleExclusive(0);
		}
		else
		{
			settingAbacusRodValueVisible.ToggleExclusive(1);
		}

		SwitchSettingMenu(SettingMenuType.Abacus);
	}

	public void OnClickSettingHomeControl()
	{
		settingModeInfo.settingMenuType = SettingMenuType.Control;

		if (DataManager.gameSettings.controlSettings.controlDebounceTime
		< ((COUNTROL_DEBOUNCE_TIME_SHORT + COUNTROL_DEBOUNCE_TIME_MEDIUM) / 2))
		{
			settingControlDebounceTime.ToggleExclusive(0);
		}
		else if (DataManager.gameSettings.controlSettings.controlDebounceTime
			< ((COUNTROL_DEBOUNCE_TIME_MEDIUM + COUNTROL_DEBOUNCE_TIME_LONG) / 2))
		{
			settingControlDebounceTime.ToggleExclusive(1);
		}
		else
		{
			settingControlDebounceTime.ToggleExclusive(2);
		}

		SwitchSettingMenu(SettingMenuType.Control);
	}

	public void OnClickSettingHomeColor()
	{
		settingModeInfo.settingMenuType = SettingMenuType.Color;
		SwitchSettingMenu(SettingMenuType.Color);
	}

	public void OnClickSettingHomeImmersive()
	{
		settingModeInfo.settingMenuType = SettingMenuType.Immersive;

		if (DataManager.gameSettings.immersiveSettings.isImmersive)
		{
			settingImmersiveToggle.ToggleExclusive(0);
		}
		else
		{
			settingImmersiveToggle.ToggleExclusive(1);
		}

		AllSkyboxPreviewImageDisable();
		SetActive(skyboxPreviewImageObjects[DataManager.gameSettings.immersiveSettings.skyBoxId]);

		SwitchSettingMenu(SettingMenuType.Immersive);
	}

	public void OnClickSettingHomeSound()
	{
		settingModeInfo.settingMenuType = SettingMenuType.Sound;

		if (!DataManager.gameSettings.soundSettings.isBgmMute)
		{
			settingSoundBgmToggle.ToggleExclusive(0);
		}
		else
		{
			settingSoundBgmToggle.ToggleExclusive(1);
		}

		if (!DataManager.gameSettings.soundSettings.isSoundMute)
		{
			settingSoundSoundToggle.ToggleExclusive(0);
		}
		else
		{
			settingSoundSoundToggle.ToggleExclusive(1);
		}

		if (DataManager.gameSettings.soundSettings.bgmVolume
		< ((SOUND_VOLUME_SOFT + SOUND_VOLUME_MEDIUM) / 2))
		{
			settingSoundBgmVolume.ToggleExclusive(0);
		}
		else if (DataManager.gameSettings.soundSettings.bgmVolume
		< ((SOUND_VOLUME_MEDIUM + SOUND_VOLUME_LOUD) / 2))
		{
			settingSoundBgmVolume.ToggleExclusive(1);
		}
		else
		{
			settingSoundBgmVolume.ToggleExclusive(2);
		}

		if (DataManager.gameSettings.soundSettings.soundVolume
		< ((SOUND_VOLUME_SOFT + SOUND_VOLUME_MEDIUM) / 2))
		{
			settingSoundSoundVolume.ToggleExclusive(0);
		}
		else if (DataManager.gameSettings.soundSettings.bgmVolume
		< ((SOUND_VOLUME_MEDIUM + SOUND_VOLUME_LOUD) / 2))
		{
			settingSoundSoundVolume.ToggleExclusive(1);
		}
		else
		{
			settingSoundSoundVolume.ToggleExclusive(2);
		}

		SwitchSettingMenu(SettingMenuType.Sound);
	}

	public void OnClickSettingHomeLanguage()
	{
		settingModeInfo.settingMenuType = SettingMenuType.Language;
		SwitchSettingMenu(SettingMenuType.Language);
	}
	public void OnClickSettingBackHome()
	{
		SwitchSettingMenu(SettingMenuType.Home);
	}

	public void OnClickSettingAbacusRodValueVisibleOn()
	{
		DataManager.gameSettings.abacusSettings.isRodValueVisible = true;
		abacusRodValueVisibleBtnToggleState.SetOnOff(DataManager.gameSettings.abacusSettings.isRodValueVisible);
		DataManager.SaveSettings();	
	}

	public void OnClickSettingAbacusRodValueVisibleOff()
	{
		DataManager.gameSettings.abacusSettings.isRodValueVisible = false;
		abacusRodValueVisibleBtnToggleState.SetOnOff(DataManager.gameSettings.abacusSettings.isRodValueVisible);
		DataManager.SaveSettings();	
	}

	public void OnClickSettingControlDebounceTimeShort()
	{
		DataManager.gameSettings.controlSettings.controlDebounceTime = COUNTROL_DEBOUNCE_TIME_SHORT;
		DataManager.SaveSettings();
	}

	public void OnClickSettingControlDebounceTimeMedium()
	{
		DataManager.gameSettings.controlSettings.controlDebounceTime = COUNTROL_DEBOUNCE_TIME_MEDIUM;
		DataManager.SaveSettings();
	}

	public void OnClickSettingControlDebounceTimeLong()
	{
		DataManager.gameSettings.controlSettings.controlDebounceTime = COUNTROL_DEBOUNCE_TIME_LONG;
		DataManager.SaveSettings();
	}

	public void OnClickSettingImmersiveOn()
	{
		DataManager.gameSettings.immersiveSettings.isImmersive = true;
		DataManager.SaveSettings();
		ImmersiveOnOff();
	}

	public void OnClickSettingImmersiveOff()
	{
		DataManager.gameSettings.immersiveSettings.isImmersive = false;
		DataManager.SaveSettings();
		ImmersiveOnOff();
	}

	public void OnClickSettingImmersiveBackgroundRightArrow()
	{
		skyboxPreviewImageObjects[DataManager.gameSettings.immersiveSettings.skyBoxId].SetActive(false);
		
		if (DataManager.gameSettings.immersiveSettings.skyBoxId < (skyboxMaterials.Length - 1))
		{
			DataManager.gameSettings.immersiveSettings.skyBoxId++;
		}
		else
		{
			DataManager.gameSettings.immersiveSettings.skyBoxId = 0;
		}
		DataManager.SaveSettings();

		SetActive(skyboxPreviewImageObjects[DataManager.gameSettings.immersiveSettings.skyBoxId]);
		RenderSettings.skybox = skyboxMaterials[DataManager.gameSettings.immersiveSettings.skyBoxId];
	}
	
	public void OnClickSettingImmersiveBackgroundLeftArrow()
	{
		skyboxPreviewImageObjects[DataManager.gameSettings.immersiveSettings.skyBoxId].SetActive(false);
		
		if (DataManager.gameSettings.immersiveSettings.skyBoxId > 0)
		{
			DataManager.gameSettings.immersiveSettings.skyBoxId--;
		}
		else
		{
			DataManager.gameSettings.immersiveSettings.skyBoxId = (skyboxMaterials.Length - 1);
		}
		DataManager.SaveSettings();

		SetActive(skyboxPreviewImageObjects[DataManager.gameSettings.immersiveSettings.skyBoxId]);
		RenderSettings.skybox = skyboxMaterials[DataManager.gameSettings.immersiveSettings.skyBoxId];
	}

	public void OnClickSettingSoundSoundOn()
	{
		DataManager.gameSettings.soundSettings.isSoundMute = false;
		DataManager.SaveSettings();
		SetSoundVolume();
	}

	public void OnClickSettingSoundSoundOff()
	{
		DataManager.gameSettings.soundSettings.isSoundMute = true;
		DataManager.SaveSettings();
		SetSoundVolume();
	}

	public void OnClickSettingSoundSoundVolumeSoft()
	{
		DataManager.gameSettings.soundSettings.soundVolume = SOUND_VOLUME_SOFT;
		DataManager.SaveSettings();
		SetSoundVolume();
	}

	public void OnClickSettingSoundSoundVolumeMedium()
	{
		DataManager.gameSettings.soundSettings.soundVolume = SOUND_VOLUME_MEDIUM;
		DataManager.SaveSettings();
		SetSoundVolume();
	}

	public void OnClickSettingSoundSoundVolumeLoud()
	{
		DataManager.gameSettings.soundSettings.soundVolume = SOUND_VOLUME_LOUD;
		DataManager.SaveSettings();
		SetSoundVolume();
	}

	public void OnClickSettingSoundBgmOn()
	{
		DataManager.gameSettings.soundSettings.isBgmMute = false;
		DataManager.SaveSettings();
		SetBgmVolume();
	}

	public void OnClickSettingSoundBgmOnff()
	{
		DataManager.gameSettings.soundSettings.isBgmMute = true;
		DataManager.SaveSettings();
		SetBgmVolume();
	}

	public void OnClickSettingSoundBgmVolumeSoft()
	{
		DataManager.gameSettings.soundSettings.bgmVolume = SOUND_VOLUME_SOFT;
		DataManager.SaveSettings();
		SetBgmVolume();
	}

	public void OnClickSettingSoundBgmVolumeMedium()
	{
		DataManager.gameSettings.soundSettings.bgmVolume = SOUND_VOLUME_MEDIUM;
		DataManager.SaveSettings();
		SetBgmVolume();
	}

	public void OnClickSettingSoundBgmVolumeLoud()
	{
		DataManager.gameSettings.soundSettings.bgmVolume = SOUND_VOLUME_LOUD;
		DataManager.SaveSettings();
		SetBgmVolume();
	}

	private void AllSkyboxPreviewImageDisable()
	{
		foreach(GameObject skyboxPreviewImageObject in skyboxPreviewImageObjects)
		{
			skyboxPreviewImageObject.SetActive(false);
		}
	}

	private void SetDialog(DialogType type)
	{
		Vector3 pos;
		dialogType = type;
		switch (dialogType)
		{
			case DialogType.HomeButton:
				SetActive(dialogPanel);
				pos = followTarget.transform.localPosition;
				pos.z -= 0.1f;
				dialogPanel.transform.localPosition = pos;
				dialogPanel.transform.rotation = followTarget.transform.rotation;
				SetKeyLocalizedText(dialogTextMeshPro, "challenge_dialog_back_body");
				break;
			case DialogType.DeleteRecord:
				SetActive(dialogPanel);
				pos = recordPanel.transform.localPosition;
				pos.z -= 0.1f;
				dialogPanel.transform.localPosition = pos;
				dialogPanel.transform.rotation = recordPanel.transform.rotation;
				SetKeyLocalizedText(dialogTextMeshPro, "record_dialog_delete_body");
				break;
			default: break;
		}
	}

	private void TutorialSetNextSeq()
	{
		if (tutorialModeInfo.tutorialSeqNo >= (seqNums[(int)tutorialModeInfo.tutorialType] - 1))
		{
			SwitchGameMode(GameMode.MenuMode);
			SwitchMenu(MenuModeType.TutorialMenu);
		}
		else
		{
			tutorialModeInfo.tutorialSeqNo++;
			TutorialSetTextPage();
			valueTutorial = "0";
			totalValue.SetTotalValue(0);
			TutorialSetBeadMuskAll(true);
			//TutorialSetBeadMaterialAll(false);
			TutorialSetArrowAll(false);
			TutorialSetDigitLineAll(false);
			TutorialSetDigitArrowAll(false);
			tutorialModeInfo.tutorialModeState = TutorialModeState.Waiting;
			TutorialSetRodMaterialAll(false);
			tutorialHand.SetActive(false);
			TutorialSetBeamMaterial(false);
			TutorialSeqExe();
		}
	}

	private void TutorialSetPreviousSeq()
	{
		if (tutorialModeInfo.tutorialSeqNo > 0)
		{
			tutorialModeInfo.tutorialSeqNo--;
			TutorialSetTextPage();
			valueTutorial = "0";
			totalValue.SetTotalValue(0);
			TutorialSetBeadMuskAll(true);
			TutorialSetBeadMaterialAll(false);
			TutorialSetArrowAll(false);
			TutorialSetDigitLineAll(false);
			TutorialSetDigitArrowAll(false);
			tutorialModeInfo.tutorialModeState = TutorialModeState.Waiting;
			TutorialSetRodMaterialAll(false);
			tutorialHand.SetActive(false);
			TutorialSetBeamMaterial(false);
			TutorialSeqExe();
		}
		else
		{
			SwitchGameMode(GameMode.MenuMode);
			SwitchMenu(MenuModeType.TutorialMenu);
		}
	}

	private void TutorialSeqExe()
	{
		switch(tutorialModeInfo.tutorialType)
		{
			case TutorialType.Basic:
				TutorialBasicSeqExe();
				break;
			case TutorialType.Addition:
				TutorialAdditionSeqExe();
				break;
			case TutorialType.Subtraction:
				TutorialSubtractionSeqExe();
				break;
			case TutorialType.Multiplication:
				TutorialMultiplicationSeqExe();
				break;
			case TutorialType.Division:
				TutorialDivisionSeqExe();
				break;
			default: break;
		}
	}

	private void TutorialBasicSeqExe()
	{
		switch (tutorialModeInfo.tutorialSeqNo)
		{
			case 0:
				SetKeyLocalizedText(tutorialTextBody, "tutorial_basic_body_1");
				Invoke("TutorialSetRightArrowActive", 0.3f);
				totalValue.SetTotalValue(0);
				break;
			case 1:
				SetKeyLocalizedText(tutorialTextBody, "tutorial_basic_body_2");
				Invoke("TutorialSetRightArrowActive", 0.3f);
				totalValue.SetTotalValue(0);
				TutorialSetRodMaterialAll(true);
				TutorialSetArrowAll(true);
				break;
			case 2:
				SetKeyLocalizedText(tutorialTextBody, "tutorial_basic_body_3");
				Invoke("TutorialSetRightArrowActive", 0.3f);
				totalValue.SetTotalValue(0);
				TutorialSetBeadMaterialAll(true);
				for (int i = 0; i < ROD_COUNT; i++)
				{
					TutorialSetBeadMaterial(false, i, 4);
				}
				break;
			case 3:
				SetKeyLocalizedText(tutorialTextBody, "tutorial_basic_body_4");
				Invoke("TutorialSetRightArrowActive", 0.3f);
				totalValue.SetTotalValue(0);
				for (int i = 0; i < ROD_COUNT; i++)
				{
					TutorialSetBeadMaterial(true, i, 4);
				}
				break;
			case 4:
				SetKeyLocalizedText(tutorialTextBody, "tutorial_basic_body_5");
				Invoke("TutorialSetRightArrowActive", 0.3f);
				totalValue.SetTotalValue(0);
				TutorialSetBeamMaterial(true);
				break;
			case 5:
				SetKeyLocalizedText(tutorialTextBody, "tutorial_basic_body_6");
				Invoke("TutorialSetRightArrowActive", 0.3f);
				totalValue.SetTotalValue(2);
				TutorialSetBeadMaterial(true, 0, 0);
				TutorialSetBeadMaterial(true, 0, 1);
				break;
			case 6:
				SetKeyLocalizedText(tutorialTextBody, "tutorial_basic_body_7");
				Invoke("TutorialSetRightArrowActive", 0.3f);
				totalValue.SetTotalValue(8);
				TutorialSetBeadMaterial(true, 0, 0);
				TutorialSetBeadMaterial(true, 0, 1);
				TutorialSetBeadMaterial(true, 0, 2);
				TutorialSetBeadMaterial(true, 0, 4);
				break;
			case 7:
				SetKeyLocalizedText(tutorialTextBody, "tutorial_basic_body_8");
				Invoke("TutorialSetRightArrowActive", 0.3f);
				totalValue.SetTotalValue(16);
				TutorialSetBeadMaterial(true, 0, 0);
				TutorialSetBeadMaterial(true, 0, 4);
				TutorialSetBeadMaterial(true, 1, 0);
				break;
			case 8:
				SetKeyLocalizedText(tutorialTextBody, "tutorial_basic_body_9");
				Invoke("TutorialSetRightArrowActive", 0.3f);
				totalValue.SetTotalValue(164);
				TutorialSetBeadMaterial(true, 0, 0);
				TutorialSetBeadMaterial(true, 0, 1);
				TutorialSetBeadMaterial(true, 0, 2);
				TutorialSetBeadMaterial(true, 0, 3);
				TutorialSetBeadMaterial(true, 1, 0);
				TutorialSetBeadMaterial(true, 1, 4);
				TutorialSetBeadMaterial(true, 2, 0);
				break;
			case 9:
				SetKeyLocalizedText(tutorialTextBody, "tutorial_basic_body_10");
				Invoke("TutorialSetRightArrowActive", 0.3f);
				totalValue.SetTotalValue(3072);
				TutorialSetBeadMaterial(true, 0, 0);
				TutorialSetBeadMaterial(true, 0, 1);
				TutorialSetBeadMaterial(true, 1, 0);
				TutorialSetBeadMaterial(true, 1, 1);
				TutorialSetBeadMaterial(true, 1, 4);
				TutorialSetBeadMaterial(true, 3, 0);
				TutorialSetBeadMaterial(true, 3, 1);
				TutorialSetBeadMaterial(true, 3, 2);
				break;
			case 10:
				SetKeyLocalizedText(tutorialTextBody, "tutorial_basic_body_11");
				Invoke("TutorialSetRightArrowActive", 0.3f);
				totalValue.SetTotalValue(0);
				break;
			case 11:
				SetKeyLocalizedText(tutorialTextBody, "tutorial_basic_body_12");
				Invoke("TutorialSetRightArrowInactive", 0.3f);
				totalValue.SetTotalValue(0);
				tutorialModeInfo.answer = 10;
				tutorialModeInfo.tutorialModeState = TutorialModeState.Playing;
				TutorialSetBeadMusk(false, 1, 0);
				TutorialSetBeadMaterial(true, 1, 0);
				TutorialSetHand(1, 0);
				break;
			case 12:
				SetKeyLocalizedText(tutorialTextBody, "tutorial_basic_body_13");
				Invoke("TutorialSetRightArrowInactive", 0.3f);
				tutorialModeInfo.answer = 13;
				tutorialModeInfo.tutorialModeState = TutorialModeState.Playing;
				totalValue.SetTotalValue(10);
				TutorialSetBeadMusk(false, 0, 2);
				TutorialSetBeadMaterial(true, 0, 2);
				TutorialSetHand(0, 2);
				break;
			case 13:
				SetKeyLocalizedText(tutorialTextBody, "tutorial_basic_body_14");
				Invoke("TutorialSetRightArrowActive", 0.3f);
				totalValue.SetTotalValue(13);
				break;
			case 14:
				SetKeyLocalizedText(tutorialTextBody, "tutorial_basic_body_15");
				Invoke("TutorialSetRightArrowActive", 0.3f);
				totalValue.SetTotalValue(0);
				break;
			case 15:
				SetKeyLocalizedText(tutorialTextBody, "tutorial_basic_body_16");
				Invoke("TutorialSetRightArrowInactive", 0.3f);
				totalValue.SetTotalValue(0);
				tutorialModeInfo.answer = 50;
				tutorialModeInfo.tutorialModeState = TutorialModeState.Playing;
				TutorialSetBeadMusk(false, 1, 4);
				TutorialSetBeadMaterial(true, 1, 4);
				TutorialSetHand(1, 4);
				break;
			case 16:
				SetKeyLocalizedText(tutorialTextBody, "tutorial_basic_body_17");
				Invoke("TutorialSetRightArrowInactive", 0.3f);
				tutorialModeInfo.answer = 70;
				tutorialModeInfo.tutorialModeState = TutorialModeState.Playing;
				totalValue.SetTotalValue(50);
				TutorialSetBeadMusk(false, 1, 1);
				TutorialSetBeadMaterial(true, 1, 1);
				TutorialSetHand(1, 1);
				break;
			case 17:
				SetKeyLocalizedText(tutorialTextBody, "tutorial_basic_body_18");
				Invoke("TutorialSetRightArrowInactive", 0.3f);
				tutorialModeInfo.answer = 74;
				tutorialModeInfo.tutorialModeState = TutorialModeState.Playing;
				totalValue.SetTotalValue(70);
				TutorialSetBeadMusk(false, 0, 3);
				TutorialSetBeadMaterial(true, 0, 3);
				TutorialSetHand(0, 3);
				break;
			case 18:
				SetKeyLocalizedText(tutorialTextBody, "tutorial_basic_body_19");
				Invoke("TutorialSetRightArrowActive", 0.3f);
				totalValue.SetTotalValue(74);
				break;
			case 19:
				SetKeyLocalizedText(tutorialTextBody, "tutorial_basic_body_20");
				Invoke("TutorialSetRightArrowActive", 0.3f);
				totalValue.SetTotalValue(0);
				break;
			case 20:
				SetKeyLocalizedText(tutorialTextBody, "tutorial_basic_body_21");
				Invoke("TutorialSetRightArrowInactive", 0.3f);
				totalValue.SetTotalValue(0);
				tutorialModeInfo.answer = 500;
				tutorialModeInfo.tutorialModeState = TutorialModeState.Playing;
				TutorialSetBeadMusk(false, 2, 4);
				TutorialSetBeadMaterial(true, 2, 4);
				TutorialSetHand(2, 4);
				break;
			case 21:
				SetKeyLocalizedText(tutorialTextBody, "tutorial_basic_body_21");
				Invoke("TutorialSetRightArrowInactive", 0.3f);
				tutorialModeInfo.answer = 600;
				tutorialModeInfo.tutorialModeState = TutorialModeState.Playing;
				totalValue.SetTotalValue(500);
				TutorialSetBeadMusk(false, 2, 0);
				TutorialSetBeadMaterial(true, 2, 0);
				TutorialSetHand(2, 0);
				break;
			case 22:
				SetKeyLocalizedText(tutorialTextBody, "tutorial_basic_body_22");
				Invoke("TutorialSetRightArrowInactive", 0.3f);
				tutorialModeInfo.answer = 650;
				tutorialModeInfo.tutorialModeState = TutorialModeState.Playing;
				totalValue.SetTotalValue(600);
				TutorialSetBeadMusk(false, 1, 4);
				TutorialSetBeadMaterial(true, 1, 4);
				TutorialSetHand(1, 4);
				break;
			case 23:
				SetKeyLocalizedText(tutorialTextBody, "tutorial_basic_body_22");
				Invoke("TutorialSetRightArrowInactive", 0.3f);
				tutorialModeInfo.answer = 680;
				tutorialModeInfo.tutorialModeState = TutorialModeState.Playing;
				totalValue.SetTotalValue(650);
				TutorialSetBeadMusk(false, 1, 2);
				TutorialSetBeadMaterial(true, 1, 2);
				TutorialSetHand(1, 2);
				break;
			case 24:
				SetKeyLocalizedText(tutorialTextBody, "tutorial_basic_body_23");
				Invoke("TutorialSetRightArrowInactive", 0.3f);
				tutorialModeInfo.answer = 683;
				tutorialModeInfo.tutorialModeState = TutorialModeState.Playing;
				totalValue.SetTotalValue(680);
				TutorialSetBeadMusk(false, 0, 2);
				TutorialSetBeadMaterial(true, 0, 2);
				TutorialSetHand(0, 2);
				break;
			case 25:
				SetKeyLocalizedText(tutorialTextBody, "tutorial_basic_body_24");
				Invoke("TutorialSetRightArrowActive", 0.3f);
				totalValue.SetTotalValue(683);
				break;
			case 26:
				SetKeyLocalizedText(tutorialTextBody, "tutorial_basic_body_25");
				Invoke("TutorialSetRightArrowActive", 0.3f);
				totalValue.SetTotalValue(683);
				break;
			default: break;
		}
	}

	private void TutorialAdditionSeqExe()
	{
		switch (tutorialModeInfo.tutorialSeqNo)
		{
			case 0:
				SetKeyLocalizedText(tutorialTextBody, "tutorial_add_body_1");
				Invoke("TutorialSetRightArrowActive", 0.3f);
				totalValue.SetTotalValue(0);
				break;
			case 1:
				SetKeyLocalizedText(tutorialTextBody, "tutorial_add_body_2");
				Invoke("TutorialSetRightArrowActive", 0.3f);
				totalValue.SetTotalValue(0);
				break;
			case 2:
				SetKeyLocalizedText(tutorialTextBody, "tutorial_add_body_3");
				Invoke("TutorialSetRightArrowInactive", 0.3f);
				totalValue.SetTotalValue(0);
				tutorialModeInfo.answer = 1;
				tutorialModeInfo.tutorialModeState = TutorialModeState.Playing;
				TutorialSetBeadMusk(false, 0, 0);
				TutorialSetBeadMaterial(true, 0, 0);
				TutorialSetHand(0, 0);
				break;
			case 3:
				SetKeyLocalizedText(tutorialTextBody, "tutorial_add_body_4");
				Invoke("TutorialSetRightArrowInactive", 0.3f);
				tutorialModeInfo.answer = 4;
				tutorialModeInfo.tutorialModeState = TutorialModeState.Playing;
				totalValue.SetTotalValue(1);
				TutorialSetBeadMusk(false, 0, 3);
				TutorialSetBeadMaterial(true, 0, 3);
				TutorialSetHand(0, 3);
				break;
			case 4:
				SetKeyLocalizedText(tutorialTextBody, "tutorial_add_body_5");
				Invoke("TutorialSetRightArrowActive", 0.3f);
				totalValue.SetTotalValue(4);
				break;
			case 5:
				SetKeyLocalizedText(tutorialTextBody, "tutorial_add_body_6");
				Invoke("TutorialSetRightArrowActive", 0.3f);
				totalValue.SetTotalValue(4);
				break;
			case 6:
				SetKeyLocalizedText(tutorialTextBody, "tutorial_add_body_7");
				Invoke("TutorialSetRightArrowActive", 0.3f);
				totalValue.SetTotalValue(0);
				break;
			case 7:
				SetKeyLocalizedText(tutorialTextBody, "tutorial_add_body_8");
				Invoke("TutorialSetRightArrowInactive", 0.3f);
				totalValue.SetTotalValue(0);
				tutorialModeInfo.answer = 2;
				tutorialModeInfo.tutorialModeState = TutorialModeState.Playing;
				TutorialSetBeadMusk(false, 0, 1);
				TutorialSetBeadMaterial(true, 0, 1);
				TutorialSetHand(0, 1);
				break;
			case 8:
				SetKeyLocalizedText(tutorialTextBody, "tutorial_add_body_9");
				Invoke("TutorialSetRightArrowActive", 0.3f);
				totalValue.SetTotalValue(2);
				break;
			case 9:
				SetKeyLocalizedText(tutorialTextBody, "tutorial_add_body_10");
				Invoke("TutorialSetRightArrowActive", 0.3f);
				totalValue.SetTotalValue(2);
				break;
			case 10:
				SetKeyLocalizedText(tutorialTextBody, "tutorial_add_body_11");
				Invoke("TutorialSetRightArrowActive", 0.3f);
				totalValue.SetTotalValue(2);
				break;
			case 11:
				SetKeyLocalizedText(tutorialTextBody, "tutorial_add_body_12");
				Invoke("TutorialSetRightArrowActive", 0.3f);
				totalValue.SetTotalValue(2);
				break;
			case 12:
				SetKeyLocalizedText(tutorialTextBody, "tutorial_add_body_13");
				Invoke("TutorialSetRightArrowInactive", 0.3f);
				tutorialModeInfo.answer = 7;
				tutorialModeInfo.tutorialModeState = TutorialModeState.Playing;
				totalValue.SetTotalValue(2);
				TutorialSetBeadMusk(false, 0, 4);
				TutorialSetBeadMaterial(true, 0, 4);
				TutorialSetHand(0, 4);
				break;
			case 13:
				SetKeyLocalizedText(tutorialTextBody, "tutorial_add_body_14");
				Invoke("TutorialSetRightArrowInactive", 0.3f);
				tutorialModeInfo.answer = 6;
				tutorialModeInfo.tutorialModeState = TutorialModeState.Playing;
				totalValue.SetTotalValue(7);
				TutorialSetBeadMusk(false, 0, 1);
				TutorialSetBeadMaterial(true, 0, 1);
				TutorialSetHand(0, 1);
				break;
			case 14:
				SetKeyLocalizedText(tutorialTextBody, "tutorial_add_body_15");
				Invoke("TutorialSetRightArrowActive", 0.3f);
				totalValue.SetTotalValue(6);
				break;
			case 15:
				SetKeyLocalizedText(tutorialTextBody, "tutorial_add_body_16");
				Invoke("TutorialSetRightArrowActive", 0.3f);
				totalValue.SetTotalValue(6);
				break;
			case 16:
				SetKeyLocalizedText(tutorialTextBody, "tutorial_add_body_17");
				Invoke("TutorialSetRightArrowActive", 0.3f);
				totalValue.SetTotalValue(0);
				break;
			case 17:
				SetKeyLocalizedText(tutorialTextBody, "tutorial_add_body_18");
				Invoke("TutorialSetRightArrowInactive", 0.3f);
				totalValue.SetTotalValue(0);
				tutorialModeInfo.answer = 5;
				tutorialModeInfo.tutorialModeState = TutorialModeState.Playing;
				TutorialSetBeadMusk(false, 0, 4);
				TutorialSetBeadMaterial(true, 0, 4);
				TutorialSetHand(0, 4);
				break;
			case 18:
				SetKeyLocalizedText(tutorialTextBody, "tutorial_add_body_18");
				Invoke("TutorialSetRightArrowInactive", 0.3f);
				tutorialModeInfo.answer = 9;
				tutorialModeInfo.tutorialModeState = TutorialModeState.Playing;
				totalValue.SetTotalValue(5);
				TutorialSetBeadMusk(false, 0, 3);
				TutorialSetBeadMaterial(true, 0, 3);
				TutorialSetHand(0, 3);
				break;
			case 19:
				SetKeyLocalizedText(tutorialTextBody, "tutorial_add_body_19");
				Invoke("TutorialSetRightArrowActive", 0.3f);
				totalValue.SetTotalValue(9);
				break;
			case 20:
				SetKeyLocalizedText(tutorialTextBody, "tutorial_add_body_20");
				Invoke("TutorialSetRightArrowActive", 0.3f);
				totalValue.SetTotalValue(9);
				break;
			case 21:
				SetKeyLocalizedText(tutorialTextBody, "tutorial_add_body_21");
				Invoke("TutorialSetRightArrowActive", 0.3f);
				totalValue.SetTotalValue(9);
				break;
			case 22:
				SetKeyLocalizedText(tutorialTextBody, "tutorial_add_body_22");
				Invoke("TutorialSetRightArrowInactive", 0.3f);
				tutorialModeInfo.answer = 5;
				tutorialModeInfo.tutorialModeState = TutorialModeState.Playing;
				totalValue.SetTotalValue(9);
				TutorialSetBeadMusk(false, 0, 0);
				TutorialSetBeadMaterial(true, 0, 0);
				TutorialSetHand(0, 0);
				break;
			case 23:
				SetKeyLocalizedText(tutorialTextBody, "tutorial_add_body_23");
				Invoke("TutorialSetRightArrowInactive", 0.3f);
				tutorialModeInfo.answer = 15;
				tutorialModeInfo.tutorialModeState = TutorialModeState.Playing;
				totalValue.SetTotalValue(5);
				TutorialSetBeadMusk(false, 1, 0);
				TutorialSetBeadMaterial(true, 1, 0);
				TutorialSetHand(1, 0);
				break;
			case 24:
				SetKeyLocalizedText(tutorialTextBody, "tutorial_add_body_24");
				Invoke("TutorialSetRightArrowActive", 0.3f);
				totalValue.SetTotalValue(15);
				break;
			case 25:
				SetKeyLocalizedText(tutorialTextBody, "tutorial_add_body_25");
				Invoke("TutorialSetRightArrowActive", 0.3f);
				totalValue.SetTotalValue(0);
				break;
			case 26:
				SetKeyLocalizedText(tutorialTextBody, "tutorial_add_body_26");
				Invoke("TutorialSetRightArrowInactive", 0.3f);
				totalValue.SetTotalValue(0);
				tutorialModeInfo.answer = 30;
				tutorialModeInfo.tutorialModeState = TutorialModeState.Playing;
				TutorialSetBeadMusk(false, 1, 2);
				TutorialSetBeadMaterial(true, 1, 2);
				TutorialSetHand(1, 2);
				break;
			case 27:
				SetKeyLocalizedText(tutorialTextBody, "tutorial_add_body_27");
				Invoke("TutorialSetRightArrowInactive", 0.3f);
				tutorialModeInfo.answer = 32;
				tutorialModeInfo.tutorialModeState = TutorialModeState.Playing;
				totalValue.SetTotalValue(30);
				TutorialSetBeadMusk(false, 0, 1);
				TutorialSetBeadMaterial(true, 0, 1);
				TutorialSetHand(0, 1);
				break;
			case 28:
				SetKeyLocalizedText(tutorialTextBody, "tutorial_add_body_28");
				Invoke("TutorialSetRightArrowActive", 0.3f);
				totalValue.SetTotalValue(32);
				break;
			case 29:
				SetKeyLocalizedText(tutorialTextBody, "tutorial_add_body_29");
				Invoke("TutorialSetRightArrowInactive", 0.3f);
				tutorialModeInfo.answer = 42;
				tutorialModeInfo.tutorialModeState = TutorialModeState.Playing;
				totalValue.SetTotalValue(32);
				TutorialSetBeadMusk(false, 1, 3);
				TutorialSetBeadMaterial(true, 1, 3);
				TutorialSetHand(1, 3);
				break;
			case 30:
				SetKeyLocalizedText(tutorialTextBody, "tutorial_add_body_30");
				Invoke("TutorialSetRightArrowInactive", 0.3f);
				tutorialModeInfo.answer = 47;
				tutorialModeInfo.tutorialModeState = TutorialModeState.Playing;
				totalValue.SetTotalValue(42);
				TutorialSetBeadMusk(false, 0, 4);
				TutorialSetBeadMaterial(true, 0, 4);
				TutorialSetHand(0, 4);
				break;
			case 31:
				SetKeyLocalizedText(tutorialTextBody, "tutorial_add_body_31");
				Invoke("TutorialSetRightArrowActive", 0.3f);
				totalValue.SetTotalValue(47);
				break;
			case 32:
				SetKeyLocalizedText(tutorialTextBody, "tutorial_add_body_32");
				Invoke("TutorialSetRightArrowActive", 0.3f);
				totalValue.SetTotalValue(0);
				break;
			case 33:
				SetKeyLocalizedText(tutorialTextBody, "tutorial_add_body_33");
				Invoke("TutorialSetRightArrowInactive", 0.3f);
				totalValue.SetTotalValue(0);
				tutorialModeInfo.answer = 40;
				tutorialModeInfo.tutorialModeState = TutorialModeState.Playing;
				TutorialSetBeadMusk(false, 1, 3);
				TutorialSetBeadMaterial(true, 1, 3);
				TutorialSetHand(1, 3);
				break;
			case 34:
				SetKeyLocalizedText(tutorialTextBody, "tutorial_add_body_33");
				Invoke("TutorialSetRightArrowInactive", 0.3f);
				tutorialModeInfo.answer = 43;
				tutorialModeInfo.tutorialModeState = TutorialModeState.Playing;
				totalValue.SetTotalValue(40);
				TutorialSetBeadMusk(false, 0, 2);
				TutorialSetBeadMaterial(true, 0, 2);
				TutorialSetHand(0, 2);
				break;
			case 35:
				SetKeyLocalizedText(tutorialTextBody, "tutorial_add_body_34");
				Invoke("TutorialSetRightArrowActive", 0.3f);
				totalValue.SetTotalValue(43);
				break;
			case 36:
				SetKeyLocalizedText(tutorialTextBody, "tutorial_add_body_35");
				Invoke("TutorialSetRightArrowActive", 0.3f);
				totalValue.SetTotalValue(43);
				break;
			case 37:
				SetKeyLocalizedText(tutorialTextBody, "tutorial_add_body_36");
				Invoke("TutorialSetRightArrowInactive", 0.3f);
				tutorialModeInfo.answer = 13;
				tutorialModeInfo.tutorialModeState = TutorialModeState.Playing;
				totalValue.SetTotalValue(43);
				TutorialSetBeadMusk(false, 1, 1);
				TutorialSetBeadMaterial(true, 1, 1);
				TutorialSetHand(1, 1);
				break;
			case 38:
				SetKeyLocalizedText(tutorialTextBody, "tutorial_add_body_37");
				Invoke("TutorialSetRightArrowInactive", 0.3f);
				tutorialModeInfo.answer = 113;
				tutorialModeInfo.tutorialModeState = TutorialModeState.Playing;
				totalValue.SetTotalValue(13);
				TutorialSetBeadMusk(false, 2, 0);
				TutorialSetBeadMaterial(true, 2, 0);
				TutorialSetHand(2, 0);
				break;
			case 39:
				SetKeyLocalizedText(tutorialTextBody, "tutorial_add_body_38");
				Invoke("TutorialSetRightArrowActive", 0.3f);
				totalValue.SetTotalValue(113);
				break;
			case 40:
				SetKeyLocalizedText(tutorialTextBody, "tutorial_add_body_39");
				Invoke("TutorialSetRightArrowInactive", 0.3f);
				tutorialModeInfo.answer = 111;
				tutorialModeInfo.tutorialModeState = TutorialModeState.Playing;
				totalValue.SetTotalValue(113);
				TutorialSetBeadMusk(false, 0, 1);
				TutorialSetBeadMaterial(true, 0, 1);
				TutorialSetHand(0, 1);
				break;
			case 41:
				SetKeyLocalizedText(tutorialTextBody, "tutorial_add_body_40");
				Invoke("TutorialSetRightArrowInactive", 0.3f);
				tutorialModeInfo.answer = 121;
				tutorialModeInfo.tutorialModeState = TutorialModeState.Playing;
				totalValue.SetTotalValue(111);
				TutorialSetBeadMusk(false, 1, 1);
				TutorialSetBeadMaterial(true, 1, 1);
				TutorialSetHand(1, 1);
				break;
			case 42:
				SetKeyLocalizedText(tutorialTextBody, "tutorial_add_body_41");
				Invoke("TutorialSetRightArrowActive", 0.3f);
				totalValue.SetTotalValue(121);
				break;
			case 43:
				SetKeyLocalizedText(tutorialTextBody, "tutorial_add_body_42");
				Invoke("TutorialSetRightArrowActive", 0.3f);
				totalValue.SetTotalValue(121);
				break;
			default: break;
		}
	}

	private void TutorialSubtractionSeqExe()
	{
		switch (tutorialModeInfo.tutorialSeqNo)
		{
			case 0:
				SetKeyLocalizedText(tutorialTextBody, "tutorial_sub_body_1");
				Invoke("TutorialSetRightArrowActive", 0.3f);
				totalValue.SetTotalValue(0);
				break;
			case 1:
				SetKeyLocalizedText(tutorialTextBody, "tutorial_sub_body_2");
				Invoke("TutorialSetRightArrowActive", 0.3f);
				totalValue.SetTotalValue(0);
				break;
			case 2:
				SetKeyLocalizedText(tutorialTextBody, "tutorial_sub_body_3");
				Invoke("TutorialSetRightArrowInactive", 0.3f);
				totalValue.SetTotalValue(0);
				tutorialModeInfo.answer = 50;
				tutorialModeInfo.tutorialModeState = TutorialModeState.Playing;
				TutorialSetBeadMusk(false, 1, 4);
				TutorialSetBeadMaterial(true, 1, 4);
				TutorialSetHand(1, 4);
				break;
			case 3:
				SetKeyLocalizedText(tutorialTextBody, "tutorial_sub_body_3");
				Invoke("TutorialSetRightArrowInactive", 0.3f);
				tutorialModeInfo.answer = 80;
				tutorialModeInfo.tutorialModeState = TutorialModeState.Playing;
				totalValue.SetTotalValue(50);
				TutorialSetBeadMusk(false, 1, 2);
				TutorialSetBeadMaterial(true, 1, 2);
				TutorialSetHand(1, 2);
				break;
			case 4:
				SetKeyLocalizedText(tutorialTextBody, "tutorial_sub_body_3");
				Invoke("TutorialSetRightArrowInactive", 0.3f);
				tutorialModeInfo.answer = 83;
				tutorialModeInfo.tutorialModeState = TutorialModeState.Playing;
				totalValue.SetTotalValue(80);
				TutorialSetBeadMusk(false, 0, 2);
				TutorialSetBeadMaterial(true, 0, 2);
				TutorialSetHand(0, 2);
				break;
			case 5:
				SetKeyLocalizedText(tutorialTextBody, "tutorial_sub_body_4");
				Invoke("TutorialSetRightArrowActive", 0.3f);
				totalValue.SetTotalValue(83);
				break;
			case 6:
				SetKeyLocalizedText(tutorialTextBody, "tutorial_sub_body_5");
				Invoke("TutorialSetRightArrowInactive", 0.3f);
				tutorialModeInfo.answer = 53;
				tutorialModeInfo.tutorialModeState = TutorialModeState.Playing;
				totalValue.SetTotalValue(83);
				TutorialSetBeadMusk(false, 1, 0);
				TutorialSetBeadMaterial(true, 1, 0);
				TutorialSetHand(1, 0);
				break;
			case 7:
				SetKeyLocalizedText(tutorialTextBody, "tutorial_sub_body_6");
				Invoke("TutorialSetRightArrowInactive", 0.3f);
				tutorialModeInfo.answer = 51;
				tutorialModeInfo.tutorialModeState = TutorialModeState.Playing;
				totalValue.SetTotalValue(53);
				TutorialSetBeadMusk(false, 0, 1);
				TutorialSetBeadMaterial(true, 0, 1);
				TutorialSetHand(0, 1);
				break;
			case 8:
				SetKeyLocalizedText(tutorialTextBody, "tutorial_sub_body_7");
				Invoke("TutorialSetRightArrowActive", 0.3f);
				totalValue.SetTotalValue(51);
				break;
			case 9:
				SetKeyLocalizedText(tutorialTextBody, "tutorial_sub_body_8");
				Invoke("TutorialSetRightArrowActive", 0.3f);
				totalValue.SetTotalValue(0);
				break;
			case 10:
				SetKeyLocalizedText(tutorialTextBody, "tutorial_sub_body_9");
				Invoke("TutorialSetRightArrowInactive", 0.3f);
				totalValue.SetTotalValue(0);
				tutorialModeInfo.answer = 40;
				tutorialModeInfo.tutorialModeState = TutorialModeState.Playing;
				TutorialSetBeadMusk(false, 1, 3);
				TutorialSetBeadMaterial(true, 1, 3);
				TutorialSetHand(1, 3);
				break;
			case 11:
				SetKeyLocalizedText(tutorialTextBody, "tutorial_sub_body_9");
				Invoke("TutorialSetRightArrowInactive", 0.3f);
				tutorialModeInfo.answer = 41;
				tutorialModeInfo.tutorialModeState = TutorialModeState.Playing;
				totalValue.SetTotalValue(40);
				TutorialSetBeadMusk(false, 0, 0);
				TutorialSetBeadMaterial(true, 0, 0);
				TutorialSetHand(0, 0);
				break;
			case 12:
				SetKeyLocalizedText(tutorialTextBody, "tutorial_sub_body_10");
				Invoke("TutorialSetRightArrowActive", 0.3f);
				totalValue.SetTotalValue(41);
				break;
			case 13:
				SetKeyLocalizedText(tutorialTextBody, "tutorial_sub_body_11");
				Invoke("TutorialSetRightArrowInactive", 0.3f);
				tutorialModeInfo.answer = 31;
				tutorialModeInfo.tutorialModeState = TutorialModeState.Playing;
				totalValue.SetTotalValue(41);
				TutorialSetBeadMusk(false, 1, 3);
				TutorialSetBeadMaterial(true, 1, 3);
				TutorialSetHand(1, 3);
				break;
			case 14:
				SetKeyLocalizedText(tutorialTextBody, "tutorial_sub_body_12");
				Invoke("TutorialSetRightArrowActive", 0.3f);
				totalValue.SetTotalValue(31);
				break;
			case 15:
				SetKeyLocalizedText(tutorialTextBody, "tutorial_sub_body_13");
				Invoke("TutorialSetRightArrowActive", 0.3f);
				totalValue.SetTotalValue(31);
				break;
			case 16:
				SetKeyLocalizedText(tutorialTextBody, "tutorial_sub_body_14");
				Invoke("TutorialSetRightArrowActive", 0.3f);
				totalValue.SetTotalValue(31);
				break;
			case 17:
				SetKeyLocalizedText(tutorialTextBody, "tutorial_sub_body_15");
				Invoke("TutorialSetRightArrowActive", 0.3f);
				totalValue.SetTotalValue(31);
				break;
			case 18:
				SetKeyLocalizedText(tutorialTextBody, "tutorial_sub_body_16");
				Invoke("TutorialSetRightArrowInactive", 0.3f);
				tutorialModeInfo.answer = 34;
				tutorialModeInfo.tutorialModeState = TutorialModeState.Playing;
				totalValue.SetTotalValue(31);
				TutorialSetBeadMusk(false, 0, 3);
				TutorialSetBeadMaterial(true, 0, 3);
				TutorialSetHand(0, 3);
				break;
			case 19:
				SetKeyLocalizedText(tutorialTextBody, "tutorial_sub_body_17");
				Invoke("TutorialSetRightArrowInactive", 0.3f);
				tutorialModeInfo.answer = 24;
				tutorialModeInfo.tutorialModeState = TutorialModeState.Playing;
				totalValue.SetTotalValue(34);
				TutorialSetBeadMusk(false, 1, 2);
				TutorialSetBeadMaterial(true, 1, 2);
				TutorialSetHand(1, 2);
				break;
			case 20:
				SetKeyLocalizedText(tutorialTextBody, "tutorial_sub_body_18");
				Invoke("TutorialSetRightArrowActive", 0.3f);
				totalValue.SetTotalValue(24);
				break;
			case 21:
				SetKeyLocalizedText(tutorialTextBody, "tutorial_sub_body_19");
				Invoke("TutorialSetRightArrowActive", 0.3f);
				totalValue.SetTotalValue(0);
				break;
			case 22:
				SetKeyLocalizedText(tutorialTextBody, "tutorial_sub_body_20");
				Invoke("TutorialSetRightArrowInactive", 0.3f);
				totalValue.SetTotalValue(0);
				tutorialModeInfo.answer = 100;
				tutorialModeInfo.tutorialModeState = TutorialModeState.Playing;
				TutorialSetBeadMusk(false, 2, 0);
				TutorialSetBeadMaterial(true, 2, 0);
				TutorialSetHand(2, 0);
				break;
			case 23:
				SetKeyLocalizedText(tutorialTextBody, "tutorial_sub_body_20");
				Invoke("TutorialSetRightArrowInactive", 0.3f);
				tutorialModeInfo.answer = 130;
				tutorialModeInfo.tutorialModeState = TutorialModeState.Playing;
				totalValue.SetTotalValue(100);
				TutorialSetBeadMusk(false, 1, 2);
				TutorialSetBeadMaterial(true, 1, 2);
				TutorialSetHand(1, 2);
				break;
			case 24:
				SetKeyLocalizedText(tutorialTextBody, "tutorial_sub_body_20");
				Invoke("TutorialSetRightArrowInactive", 0.3f);
				tutorialModeInfo.answer = 134;
				tutorialModeInfo.tutorialModeState = TutorialModeState.Playing;
				totalValue.SetTotalValue(130);
				TutorialSetBeadMusk(false, 0, 3);
				TutorialSetBeadMaterial(true, 0, 3);
				TutorialSetHand(0, 3);
				break;
			case 25:
				SetKeyLocalizedText(tutorialTextBody, "tutorial_sub_body_21");
				Invoke("TutorialSetRightArrowActive", 0.3f);
				totalValue.SetTotalValue(134);
				break;
			case 26:
				SetKeyLocalizedText(tutorialTextBody, "tutorial_sub_body_22");
				Invoke("TutorialSetRightArrowInactive", 0.3f);
				tutorialModeInfo.answer = 114;
				tutorialModeInfo.tutorialModeState = TutorialModeState.Playing;
				totalValue.SetTotalValue(134);
				TutorialSetBeadMusk(false, 1, 1);
				TutorialSetBeadMaterial(true, 1, 1);
				TutorialSetHand(1, 1);
				break;
			case 27:
				SetKeyLocalizedText(tutorialTextBody, "tutorial_sub_body_23");
				Invoke("TutorialSetRightArrowActive", 0.3f);
				totalValue.SetTotalValue(114);
				break;
			case 28:
				SetKeyLocalizedText(tutorialTextBody, "tutorial_sub_body_24");
				Invoke("TutorialSetRightArrowActive", 0.3f);
				totalValue.SetTotalValue(114);
				break;
			case 29:
				SetKeyLocalizedText(tutorialTextBody, "tutorial_sub_body_25");
				Invoke("TutorialSetRightArrowActive", 0.3f);
				totalValue.SetTotalValue(114);
				break;
			case 30:
				SetKeyLocalizedText(tutorialTextBody, "tutorial_sub_body_26");
				Invoke("TutorialSetRightArrowActive", 0.3f);
				totalValue.SetTotalValue(114);
				break;
			case 31:
				SetKeyLocalizedText(tutorialTextBody, "tutorial_sub_body_27");
				Invoke("TutorialSetRightArrowActive", 0.3f);
				totalValue.SetTotalValue(114);
				break;
			case 32:
				SetKeyLocalizedText(tutorialTextBody, "tutorial_sub_body_28");
				Invoke("TutorialSetRightArrowInactive", 0.3f);
				tutorialModeInfo.answer = 119;
				tutorialModeInfo.tutorialModeState = TutorialModeState.Playing;
				totalValue.SetTotalValue(114);
				TutorialSetBeadMusk(false, 0, 4);
				TutorialSetBeadMaterial(true, 0, 4);
				TutorialSetHand(0, 4);
				break;
			case 33:
				SetKeyLocalizedText(tutorialTextBody, "tutorial_sub_body_29");
				Invoke("TutorialSetRightArrowInactive", 0.3f);
				tutorialModeInfo.answer = 116;
				tutorialModeInfo.tutorialModeState = TutorialModeState.Playing;
				totalValue.SetTotalValue(119);
				TutorialSetBeadMusk(false, 0, 1);
				TutorialSetBeadMaterial(true, 0, 1);
				TutorialSetHand(0, 1);
				break;
			case 34:
				SetKeyLocalizedText(tutorialTextBody, "tutorial_sub_body_30");
				Invoke("TutorialSetRightArrowInactive", 0.3f);
				tutorialModeInfo.answer = 106;
				tutorialModeInfo.tutorialModeState = TutorialModeState.Playing;
				totalValue.SetTotalValue(116);
				TutorialSetBeadMusk(false, 1, 0);
				TutorialSetBeadMaterial(true, 1, 0);
				TutorialSetHand(1, 0);
				break;
			case 35:
				SetKeyLocalizedText(tutorialTextBody, "tutorial_sub_body_31");
				Invoke("TutorialSetRightArrowActive", 0.3f);
				totalValue.SetTotalValue(106);
				break;
			case 36:
				SetKeyLocalizedText(tutorialTextBody, "tutorial_sub_body_32");
				Invoke("TutorialSetRightArrowActive", 0.3f);
				totalValue.SetTotalValue(106);
				break;
			default: break;
		}
	}

	private void TutorialMultiplicationSeqExe()
	{
		switch (tutorialModeInfo.tutorialSeqNo)
		{
			case 0:
				SetKeyLocalizedText(tutorialTextBody, "tutorial_multi_body_1");
				Invoke("TutorialSetRightArrowActive", 0.3f);
				totalValue.SetTotalValue(0);
				break;
			case 1:
				SetKeyLocalizedText(tutorialTextBody, "tutorial_multi_body_2");
				Invoke("TutorialSetRightArrowActive", 0.3f);
				totalValue.SetTotalValue(0);
				break;
			case 2:
				SetKeyLocalizedText(tutorialTextBody, "tutorial_multi_body_3");
				Invoke("TutorialSetRightArrowInactive", 0.3f);
				totalValue.SetTotalValue(0);
				tutorialModeInfo.answer = 50000;
				tutorialModeInfo.tutorialModeState = TutorialModeState.Playing;
				TutorialSetBeadMusk(false, 4, 4);
				TutorialSetBeadMaterial(true, 4, 4);
				TutorialSetHand(4, 4);
				TutorialSetDigitLine(true, 1);
				TutorialSetDigitArrow(true, 1);
				break;
			case 3:
				SetKeyLocalizedText(tutorialTextBody, "tutorial_multi_body_3");
				Invoke("TutorialSetRightArrowInactive", 0.3f);
				totalValue.SetTotalValue(50000);
				tutorialModeInfo.answer = 55000;
				tutorialModeInfo.tutorialModeState = TutorialModeState.Playing;
				TutorialSetBeadMusk(false, 3, 4);
				TutorialSetBeadMaterial(true, 3, 4);
				TutorialSetHand(3, 4);
				TutorialSetDigitLine(true, 1);
				TutorialSetDigitArrow(true, 1);
				break;
			case 4:
				SetKeyLocalizedText(tutorialTextBody, "tutorial_multi_body_3");
				Invoke("TutorialSetRightArrowInactive", 0.3f);
				totalValue.SetTotalValue(55000);
				tutorialModeInfo.answer = 57000;
				tutorialModeInfo.tutorialModeState = TutorialModeState.Playing;
				TutorialSetBeadMusk(false, 3, 1);
				TutorialSetBeadMaterial(true, 3, 1);
				TutorialSetHand(3, 1);
				TutorialSetDigitLine(true, 1);
				TutorialSetDigitArrow(true, 1);
				break;
			case 5:
				SetKeyLocalizedText(tutorialTextBody, "tutorial_multi_body_3");
				Invoke("TutorialSetRightArrowInactive", 0.3f);
				totalValue.SetTotalValue(57000);
				tutorialModeInfo.answer = 57400;
				tutorialModeInfo.tutorialModeState = TutorialModeState.Playing;
				TutorialSetBeadMusk(false, 2, 3);
				TutorialSetBeadMaterial(true, 2, 3);
				TutorialSetHand(2, 3);
				TutorialSetDigitLine(true, 1);
				TutorialSetDigitArrow(true, 1);
				break;
			case 6:
				SetKeyLocalizedText(tutorialTextBody, "tutorial_multi_body_4");
				Invoke("TutorialSetRightArrowActive", 0.3f);
				totalValue.SetTotalValue(57400);
				TutorialSetArrow(true, 2);
				break;
			case 7:
				SetKeyLocalizedText(tutorialTextBody, "tutorial_multi_body_5");
				Invoke("TutorialSetRightArrowActive", 0.3f);
				totalValue.SetTotalValue(57400);
				TutorialSetArrow(true, 2);
				break;
			case 8:
				SetKeyLocalizedText(tutorialTextBody, "tutorial_multi_body_6");
				Invoke("TutorialSetRightArrowActive", 0.3f);
				totalValue.SetTotalValue(57400);
				TutorialSetArrow(true, 2);
				break;
			case 9:
				SetKeyLocalizedText(tutorialTextBody, "tutorial_multi_body_7");
				Invoke("TutorialSetRightArrowActive", 0.3f);
				totalValue.SetTotalValue(57400);
				TutorialSetArrow(true, 0);
				TutorialSetArrow(true, 1);
				break;
			case 10:
				SetKeyLocalizedText(tutorialTextBody, "tutorial_multi_body_7");
				Invoke("TutorialSetRightArrowInactive", 0.3f);
				totalValue.SetTotalValue(57400);
				tutorialModeInfo.answer = 57410;
				tutorialModeInfo.tutorialModeState = TutorialModeState.Playing;
				TutorialSetBeadMusk(false, 1, 0);
				TutorialSetBeadMaterial(true, 1, 0);
				TutorialSetHand(1, 0);
				break;
			case 11:
				SetKeyLocalizedText(tutorialTextBody, "tutorial_multi_body_7");
				Invoke("TutorialSetRightArrowInactive", 0.3f);
				totalValue.SetTotalValue(57410);
				tutorialModeInfo.answer = 57412;
				tutorialModeInfo.tutorialModeState = TutorialModeState.Playing;
				TutorialSetBeadMusk(false, 0, 1);
				TutorialSetBeadMaterial(true, 0, 1);
				TutorialSetHand(0, 1);
				break;
			case 12:
				SetKeyLocalizedText(tutorialTextBody, "tutorial_multi_body_8");
				Invoke("TutorialSetRightArrowInactive", 0.3f);
				totalValue.SetTotalValue(57412);
				tutorialModeInfo.answer = 57012;
				tutorialModeInfo.tutorialModeState = TutorialModeState.Playing;
				TutorialSetBeadMusk(false, 2, 0);
				TutorialSetBeadMaterial(true, 2, 0);
				TutorialSetHand(2, 0);
				TutorialSetArrow(true, 2);
				break;
			case 13:
				SetKeyLocalizedText(tutorialTextBody, "tutorial_multi_body_9");
				Invoke("TutorialSetRightArrowActive", 0.3f);
				totalValue.SetTotalValue(57012);
				TutorialSetArrow(true, 3);
				break;
			case 14:
				SetKeyLocalizedText(tutorialTextBody, "tutorial_multi_body_10");
				Invoke("TutorialSetRightArrowActive", 0.3f);
				totalValue.SetTotalValue(57012);
				TutorialSetArrow(true, 1);
				TutorialSetArrow(true, 2);
				break;
			case 15:
				SetKeyLocalizedText(tutorialTextBody, "tutorial_multi_body_10");
				Invoke("TutorialSetRightArrowInactive", 0.3f);
				totalValue.SetTotalValue(57012);
				tutorialModeInfo.answer = 57212;
				tutorialModeInfo.tutorialModeState = TutorialModeState.Playing;
				TutorialSetBeadMusk(false, 2, 1);
				TutorialSetBeadMaterial(true, 2, 1);
				TutorialSetHand(2, 1);
				break;
			case 16:
				SetKeyLocalizedText(tutorialTextBody, "tutorial_multi_body_10");
				Invoke("TutorialSetRightArrowInactive", 0.3f);
				totalValue.SetTotalValue(57212);
				tutorialModeInfo.answer = 57222;
				tutorialModeInfo.tutorialModeState = TutorialModeState.Playing;
				TutorialSetBeadMusk(false, 1, 1);
				TutorialSetBeadMaterial(true, 1, 1);
				TutorialSetHand(1, 1);
				break;
			case 17:
				SetKeyLocalizedText(tutorialTextBody, "tutorial_multi_body_11");
				Invoke("TutorialSetRightArrowInactive", 0.3f);
				totalValue.SetTotalValue(57222);
				tutorialModeInfo.answer = 52222;
				tutorialModeInfo.tutorialModeState = TutorialModeState.Playing;
				TutorialSetBeadMusk(false, 3, 4);
				TutorialSetBeadMaterial(true, 3, 4);
				TutorialSetHand(3, 4);
				TutorialSetArrow(true, 3);
				break;
			case 18:
				SetKeyLocalizedText(tutorialTextBody, "tutorial_multi_body_11");
				Invoke("TutorialSetRightArrowInactive", 0.3f);
				totalValue.SetTotalValue(52222);
				tutorialModeInfo.answer = 50222;
				tutorialModeInfo.tutorialModeState = TutorialModeState.Playing;
				TutorialSetBeadMusk(false, 3, 0);
				TutorialSetBeadMaterial(true, 3, 0);
				TutorialSetHand(3, 0);
				TutorialSetArrow(true, 3);
				break;
			case 19:
				SetKeyLocalizedText(tutorialTextBody, "tutorial_multi_body_12");
				Invoke("TutorialSetRightArrowActive", 0.3f);
				totalValue.SetTotalValue(50222);
				TutorialSetArrow(true, 4);
				break;
			case 20:
				SetKeyLocalizedText(tutorialTextBody, "tutorial_multi_body_13");
				Invoke("TutorialSetRightArrowActive", 0.3f);
				totalValue.SetTotalValue(50222);
				TutorialSetArrow(true, 2);
				TutorialSetArrow(true, 3);
				break;
			case 21:
				SetKeyLocalizedText(tutorialTextBody, "tutorial_multi_body_13");
				Invoke("TutorialSetRightArrowInactive", 0.3f);
				totalValue.SetTotalValue(50222);
				tutorialModeInfo.answer = 51222;
				tutorialModeInfo.tutorialModeState = TutorialModeState.Playing;
				TutorialSetBeadMusk(false, 3, 0);
				TutorialSetBeadMaterial(true, 3, 0);
				TutorialSetHand(3, 0);
				break;
			case 22:
				SetKeyLocalizedText(tutorialTextBody, "tutorial_multi_body_13");
				Invoke("TutorialSetRightArrowInactive", 0.3f);
				totalValue.SetTotalValue(51222);
				tutorialModeInfo.answer = 51722;
				tutorialModeInfo.tutorialModeState = TutorialModeState.Playing;
				TutorialSetBeadMusk(false, 2, 4);
				TutorialSetBeadMaterial(true, 2, 4);
				TutorialSetHand(2, 4);
				break;
			case 23:
				SetKeyLocalizedText(tutorialTextBody, "tutorial_multi_body_14");
				Invoke("TutorialSetRightArrowInactive", 0.3f);
				totalValue.SetTotalValue(51722);
				tutorialModeInfo.answer = 1722;
				tutorialModeInfo.tutorialModeState = TutorialModeState.Playing;
				TutorialSetBeadMusk(false, 4, 4);
				TutorialSetBeadMaterial(true, 4, 4);
				TutorialSetHand(4, 4);
				TutorialSetArrow(true, 4);
				break;
			case 24:
				SetKeyLocalizedText(tutorialTextBody, "tutorial_multi_body_15");
				Invoke("TutorialSetRightArrowActive", 0.3f);
				totalValue.SetTotalValue(1722);
				break;
			case 25:
				SetKeyLocalizedText(tutorialTextBody, "tutorial_multi_body_16");
				Invoke("TutorialSetRightArrowActive", 0.3f);
				totalValue.SetTotalValue(0);
				break;
			case 26:
				SetKeyLocalizedText(tutorialTextBody, "tutorial_multi_body_17");
				Invoke("TutorialSetRightArrowInactive", 0.3f);
				totalValue.SetTotalValue(0);
				tutorialModeInfo.answer = 200000;
				tutorialModeInfo.tutorialModeState = TutorialModeState.Playing;
				TutorialSetBeadMusk(false, 5, 1);
				TutorialSetBeadMaterial(true, 5, 1);
				TutorialSetHand(5, 1);
				TutorialSetDigitLine(true, 2);
				TutorialSetDigitArrow(true, 2);
				break;
			case 27:
				SetKeyLocalizedText(tutorialTextBody, "tutorial_multi_body_17");
				Invoke("TutorialSetRightArrowInactive", 0.3f);
				totalValue.SetTotalValue(200000);
				tutorialModeInfo.answer = 210000;
				tutorialModeInfo.tutorialModeState = TutorialModeState.Playing;
				TutorialSetBeadMusk(false, 4, 0);
				TutorialSetBeadMaterial(true, 4, 0);
				TutorialSetHand(4, 0);
				TutorialSetDigitLine(true, 2);
				TutorialSetDigitArrow(true, 2);
				break;
			case 28:
				SetKeyLocalizedText(tutorialTextBody, "tutorial_multi_body_17");
				Invoke("TutorialSetRightArrowInactive", 0.3f);
				totalValue.SetTotalValue(210000);
				tutorialModeInfo.answer = 214000;
				tutorialModeInfo.tutorialModeState = TutorialModeState.Playing;
				TutorialSetBeadMusk(false, 3, 3);
				TutorialSetBeadMaterial(true, 3, 3);
				TutorialSetHand(3, 3);
				TutorialSetDigitLine(true, 2);
				TutorialSetDigitArrow(true, 2);
				break;
			case 29:
				SetKeyLocalizedText(tutorialTextBody, "tutorial_multi_body_18");
				Invoke("TutorialSetRightArrowActive", 0.3f);
				totalValue.SetTotalValue(214000);
				TutorialSetArrow(true, 3);
				break;
			case 30:
				SetKeyLocalizedText(tutorialTextBody, "tutorial_multi_body_19");
				Invoke("TutorialSetRightArrowActive", 0.3f);
				totalValue.SetTotalValue(214000);
				TutorialSetArrow(true, 3);
				break;
			case 31:
				SetKeyLocalizedText(tutorialTextBody, "tutorial_multi_body_20");
				Invoke("TutorialSetRightArrowActive", 0.3f);
				totalValue.SetTotalValue(214000);
				TutorialSetArrow(true, 3);
				break;
			case 32:
				SetKeyLocalizedText(tutorialTextBody, "tutorial_multi_body_21");
				Invoke("TutorialSetRightArrowActive", 0.3f);
				totalValue.SetTotalValue(214000);
				TutorialSetArrow(true, 1);
				TutorialSetArrow(true, 2);
				break;
			case 33:
				SetKeyLocalizedText(tutorialTextBody, "tutorial_multi_body_21");
				Invoke("TutorialSetRightArrowInactive", 0.3f);
				totalValue.SetTotalValue(214000);
				tutorialModeInfo.answer = 214200;
				tutorialModeInfo.tutorialModeState = TutorialModeState.Playing;
				TutorialSetBeadMusk(false, 2, 1);
				TutorialSetBeadMaterial(true, 2, 1);
				TutorialSetHand(2, 1);
				break;
			case 34:
				SetKeyLocalizedText(tutorialTextBody, "tutorial_multi_body_22");
				Invoke("TutorialSetRightArrowActive", 0.3f);
				totalValue.SetTotalValue(214200);
				TutorialSetArrow(true, 0);
				TutorialSetArrow(true, 1);
				break;
			case 35:
				SetKeyLocalizedText(tutorialTextBody, "tutorial_multi_body_22");
				Invoke("TutorialSetRightArrowInactive", 0.3f);
				totalValue.SetTotalValue(214200);
				tutorialModeInfo.answer = 214220;
				tutorialModeInfo.tutorialModeState = TutorialModeState.Playing;
				TutorialSetBeadMusk(false, 1, 1);
				TutorialSetBeadMaterial(true, 1, 1);
				TutorialSetHand(1, 1);
				break;
			case 36:
				SetKeyLocalizedText(tutorialTextBody, "tutorial_multi_body_22");
				Invoke("TutorialSetRightArrowInactive", 0.3f);
				totalValue.SetTotalValue(214220);
				tutorialModeInfo.answer = 214224;
				tutorialModeInfo.tutorialModeState = TutorialModeState.Playing;
				TutorialSetBeadMusk(false, 0, 3);
				TutorialSetBeadMaterial(true, 0, 3);
				TutorialSetHand(0, 3);
				break;
			case 37:
				SetKeyLocalizedText(tutorialTextBody, "tutorial_multi_body_23");
				Invoke("TutorialSetRightArrowInactive", 0.3f);
				totalValue.SetTotalValue(214224);
				tutorialModeInfo.answer = 210224;
				tutorialModeInfo.tutorialModeState = TutorialModeState.Playing;
				TutorialSetBeadMusk(false, 3, 0);
				TutorialSetBeadMaterial(true, 3, 0);
				TutorialSetHand(3, 0);
				TutorialSetArrow(true, 3);
				break;
			case 38:
				SetKeyLocalizedText(tutorialTextBody, "tutorial_multi_body_24");
				Invoke("TutorialSetRightArrowActive", 0.3f);
				totalValue.SetTotalValue(210224);
				TutorialSetArrow(true, 4);
				break;
			case 39:
				SetKeyLocalizedText(tutorialTextBody, "tutorial_multi_body_25");
				Invoke("TutorialSetRightArrowActive", 0.3f);
				totalValue.SetTotalValue(210224);
				TutorialSetArrow(true, 2);
				break;
			case 40:
				SetKeyLocalizedText(tutorialTextBody, "tutorial_multi_body_25");
				Invoke("TutorialSetRightArrowInactive", 0.3f);
				totalValue.SetTotalValue(210224);
				tutorialModeInfo.answer = 210724;
				tutorialModeInfo.tutorialModeState = TutorialModeState.Playing;
				TutorialSetBeadMusk(false, 2, 4);
				TutorialSetBeadMaterial(true, 2, 4);
				TutorialSetHand(2, 4);
				break;
			case 41:
				SetKeyLocalizedText(tutorialTextBody, "tutorial_multi_body_26");
				Invoke("TutorialSetRightArrowActive", 0.3f);
				totalValue.SetTotalValue(210724);
				TutorialSetArrow(true, 1);
				break;
			case 42:
				SetKeyLocalizedText(tutorialTextBody, "tutorial_multi_body_26");
				Invoke("TutorialSetRightArrowInactive", 0.3f);
				totalValue.SetTotalValue(210724);
				tutorialModeInfo.answer = 210774;
				tutorialModeInfo.tutorialModeState = TutorialModeState.Playing;
				TutorialSetBeadMusk(false, 1, 4);
				TutorialSetBeadMaterial(true, 1, 4);
				TutorialSetHand(1, 4);
				break;
			case 43:
				SetKeyLocalizedText(tutorialTextBody, "tutorial_multi_body_26");
				Invoke("TutorialSetRightArrowInactive", 0.3f);
				totalValue.SetTotalValue(210774);
				tutorialModeInfo.answer = 210784;
				tutorialModeInfo.tutorialModeState = TutorialModeState.Playing;
				TutorialSetBeadMusk(false, 1, 2);
				TutorialSetBeadMaterial(true, 1, 2);
				TutorialSetHand(1, 2);
				break;
			case 44:
				SetKeyLocalizedText(tutorialTextBody, "tutorial_multi_body_27");
				Invoke("TutorialSetRightArrowInactive", 0.3f);
				totalValue.SetTotalValue(210784);
				tutorialModeInfo.answer = 200784;
				tutorialModeInfo.tutorialModeState = TutorialModeState.Playing;
				TutorialSetBeadMusk(false, 4, 0);
				TutorialSetBeadMaterial(true, 4, 0);
				TutorialSetHand(4, 0);
				TutorialSetArrow(true, 4);
				break;
			case 45:
				SetKeyLocalizedText(tutorialTextBody, "tutorial_multi_body_28");
				Invoke("TutorialSetRightArrowActive", 0.3f);
				totalValue.SetTotalValue(200784);
				TutorialSetArrow(true, 5);
				break;
			case 46:
				SetKeyLocalizedText(tutorialTextBody, "tutorial_multi_body_29");
				Invoke("TutorialSetRightArrowActive", 0.3f);
				totalValue.SetTotalValue(200784);
				TutorialSetArrow(true, 3);
				TutorialSetArrow(true, 4);
				break;
			case 47:
				SetKeyLocalizedText(tutorialTextBody, "tutorial_multi_body_29");
				Invoke("TutorialSetRightArrowInactive", 0.3f);
				totalValue.SetTotalValue(200784);
				tutorialModeInfo.answer = 210784;
				tutorialModeInfo.tutorialModeState = TutorialModeState.Playing;
				TutorialSetBeadMusk(false, 4, 0);
				TutorialSetBeadMaterial(true, 4, 0);
				TutorialSetHand(4, 0);
				break;
			case 48:
				SetKeyLocalizedText(tutorialTextBody, "tutorial_multi_body_30");
				Invoke("TutorialSetRightArrowActive", 0.3f);
				totalValue.SetTotalValue(210784);
				TutorialSetArrow(true, 2);
				TutorialSetArrow(true, 3);
				break;
			case 49:
				SetKeyLocalizedText(tutorialTextBody, "tutorial_multi_body_30");
				Invoke("TutorialSetRightArrowInactive", 0.3f);
				totalValue.SetTotalValue(210784);
				tutorialModeInfo.answer = 211784;
				tutorialModeInfo.tutorialModeState = TutorialModeState.Playing;
				TutorialSetBeadMusk(false, 3, 0);
				TutorialSetBeadMaterial(true, 3, 0);
				TutorialSetHand(3, 0);
				break;
			case 50:
				SetKeyLocalizedText(tutorialTextBody, "tutorial_multi_body_30");
				Invoke("TutorialSetRightArrowInactive", 0.3f);
				totalValue.SetTotalValue(211784);
				tutorialModeInfo.answer = 211984;
				tutorialModeInfo.tutorialModeState = TutorialModeState.Playing;
				TutorialSetBeadMusk(false, 2, 3);
				TutorialSetBeadMaterial(true, 2, 3);
				TutorialSetHand(2, 3);
				break;
			case 51:
				SetKeyLocalizedText(tutorialTextBody, "tutorial_multi_body_31");
				Invoke("TutorialSetRightArrowInactive", 0.3f);
				totalValue.SetTotalValue(211984);
				tutorialModeInfo.answer = 11984;
				tutorialModeInfo.tutorialModeState = TutorialModeState.Playing;
				TutorialSetBeadMusk(false, 5, 0);
				TutorialSetBeadMaterial(true, 5, 0);
				TutorialSetHand(5, 0);
				TutorialSetArrow(true, 5);
				break;
			case 52:
				SetKeyLocalizedText(tutorialTextBody, "tutorial_multi_body_32");
				Invoke("TutorialSetRightArrowActive", 0.3f);
				totalValue.SetTotalValue(11984);
				break;
			case 53:
				SetKeyLocalizedText(tutorialTextBody, "tutorial_multi_body_33");
				Invoke("TutorialSetRightArrowActive", 0.3f);
				totalValue.SetTotalValue(11984);
				break;
			default: break;
		}
	}

	private void TutorialDivisionSeqExe()
	{
		switch (tutorialModeInfo.tutorialSeqNo)
		{
			case 0:
				SetKeyLocalizedText(tutorialTextBody, "tutorial_division_body_1");
				Invoke("TutorialSetRightArrowActive", 0.3f);
				totalValue.SetTotalValue(0);
				break;
			case 1:
				SetKeyLocalizedText(tutorialTextBody, "tutorial_division_body_2");
				Invoke("TutorialSetRightArrowActive", 0.3f);
				totalValue.SetTotalValue(0);
				break;
			case 2:
				SetKeyLocalizedText(tutorialTextBody, "tutorial_division_body_3");
				Invoke("TutorialSetRightArrowInactive", 0.3f);
				totalValue.SetTotalValue(0);
				tutorialModeInfo.answer = 500000;
				tutorialModeInfo.tutorialModeState = TutorialModeState.Playing;
				TutorialSetBeadMusk(false, 5, 4);
				TutorialSetBeadMaterial(true, 5, 4);
				TutorialSetHand(5, 4);
				TutorialSetDigitLine(true, 2);
				TutorialSetDigitArrow(true, 2);
				break;
			case 3:
				SetKeyLocalizedText(tutorialTextBody, "tutorial_division_body_3");
				Invoke("TutorialSetRightArrowInactive", 0.3f);
				totalValue.SetTotalValue(500000);
				tutorialModeInfo.answer = 700000;
				tutorialModeInfo.tutorialModeState = TutorialModeState.Playing;
				TutorialSetBeadMusk(false, 5, 1);
				TutorialSetBeadMaterial(true, 5, 1);
				TutorialSetHand(5, 1);
				TutorialSetDigitLine(true, 2);
				TutorialSetDigitArrow(true, 2);
				break;
			case 4:
				SetKeyLocalizedText(tutorialTextBody, "tutorial_division_body_3");
				Invoke("TutorialSetRightArrowInactive", 0.3f);
				totalValue.SetTotalValue(700000);
				tutorialModeInfo.answer = 730000;
				tutorialModeInfo.tutorialModeState = TutorialModeState.Playing;
				TutorialSetBeadMusk(false, 4, 2);
				TutorialSetBeadMaterial(true, 4, 2);
				TutorialSetHand(4, 2);
				TutorialSetDigitLine(true, 2);
				TutorialSetDigitArrow(true, 2);
				break;
			case 5:
				SetKeyLocalizedText(tutorialTextBody, "tutorial_division_body_3");
				Invoke("TutorialSetRightArrowInactive", 0.3f);
				totalValue.SetTotalValue(730000);
				tutorialModeInfo.answer = 735000;
				tutorialModeInfo.tutorialModeState = TutorialModeState.Playing;
				TutorialSetBeadMusk(false, 3, 4);
				TutorialSetBeadMaterial(true, 3, 4);
				TutorialSetHand(3, 4);
				TutorialSetDigitLine(true, 2);
				TutorialSetDigitArrow(true, 2);
				break;
			case 6:
				SetKeyLocalizedText(tutorialTextBody, "tutorial_division_body_3");
				Invoke("TutorialSetRightArrowInactive", 0.3f);
				totalValue.SetTotalValue(735000);
				tutorialModeInfo.answer = 738000;
				tutorialModeInfo.tutorialModeState = TutorialModeState.Playing;
				TutorialSetBeadMusk(false, 3, 2);
				TutorialSetBeadMaterial(true, 3, 2);
				TutorialSetHand(3, 2);
				TutorialSetDigitLine(true, 2);
				TutorialSetDigitArrow(true, 2);
				break;
			case 7:
				SetKeyLocalizedText(tutorialTextBody, "tutorial_division_body_4");
				Invoke("TutorialSetRightArrowActive", 0.3f);
				totalValue.SetTotalValue(738000);
				break;
			case 8:
				SetKeyLocalizedText(tutorialTextBody, "tutorial_division_body_5");
				Invoke("TutorialSetRightArrowActive", 0.3f);
				totalValue.SetTotalValue(738000);
				break;
			case 9:
				SetKeyLocalizedText(tutorialTextBody, "tutorial_division_body_6");
				Invoke("TutorialSetRightArrowActive", 0.3f);
				totalValue.SetTotalValue(738000);
				TutorialSetArrow(true, 2);
				TutorialSetArrow(true, 5);
				break;
			case 10:
				SetKeyLocalizedText(tutorialTextBody, "tutorial_division_body_7");
				Invoke("TutorialSetRightArrowActive", 0.3f);
				totalValue.SetTotalValue(738000);
				TutorialSetArrow(true, 2);
				TutorialSetArrow(true, 5);
				break;
			case 11:
				SetKeyLocalizedText(tutorialTextBody, "tutorial_division_body_8");
				Invoke("TutorialSetRightArrowInactive", 0.3f);
				totalValue.SetTotalValue(738000);
				tutorialModeInfo.answer = 738200;
				tutorialModeInfo.tutorialModeState = TutorialModeState.Playing;
				TutorialSetBeadMusk(false, 2, 1);
				TutorialSetBeadMaterial(true, 2, 1);
				TutorialSetHand(2, 1);
				TutorialSetArrow(true, 2);
				TutorialSetArrow(true, 5);
				break;
			case 12:
				SetKeyLocalizedText(tutorialTextBody, "tutorial_division_body_9");
				Invoke("TutorialSetRightArrowActive", 0.3f);
				totalValue.SetTotalValue(738200);
				TutorialSetArrow(true, 2);
				TutorialSetArrow(true, 5);
				break;
			case 13:
				SetKeyLocalizedText(tutorialTextBody, "tutorial_division_body_10");
				Invoke("TutorialSetRightArrowInactive", 0.3f);
				totalValue.SetTotalValue(738200);
				tutorialModeInfo.answer = 238200;
				tutorialModeInfo.tutorialModeState = TutorialModeState.Playing;
				TutorialSetBeadMusk(false, 5, 4);
				TutorialSetBeadMaterial(true, 5, 4);
				TutorialSetHand(5, 4);
				TutorialSetArrow(true, 2);
				TutorialSetArrow(true, 5);
				break;
			case 14:
				SetKeyLocalizedText(tutorialTextBody, "tutorial_division_body_10");
				Invoke("TutorialSetRightArrowInactive", 0.3f);
				totalValue.SetTotalValue(238200);
				tutorialModeInfo.answer = 138200;
				tutorialModeInfo.tutorialModeState = TutorialModeState.Playing;
				TutorialSetBeadMusk(false, 5, 1);
				TutorialSetBeadMaterial(true, 5, 1);
				TutorialSetHand(5, 1);
				TutorialSetArrow(true, 2);
				TutorialSetArrow(true, 5);
				break;
			case 15:
				SetKeyLocalizedText(tutorialTextBody, "tutorial_division_body_11");
				Invoke("TutorialSetRightArrowActive", 0.3f);
				totalValue.SetTotalValue(138200);
				TutorialSetArrow(true, 2);
				TutorialSetArrow(true, 5);
				break;
			case 16:
				SetKeyLocalizedText(tutorialTextBody, "tutorial_division_body_12");
				Invoke("TutorialSetRightArrowActive", 0.3f);
				totalValue.SetTotalValue(138200);
				TutorialSetArrow(true, 1);
				TutorialSetArrow(true, 4);
				break;
			case 17:
				SetKeyLocalizedText(tutorialTextBody, "tutorial_division_body_13");
				Invoke("TutorialSetRightArrowInactive", 0.3f);
				totalValue.SetTotalValue(138200);
				tutorialModeInfo.answer = 138240;
				tutorialModeInfo.tutorialModeState = TutorialModeState.Playing;
				TutorialSetBeadMusk(false, 1, 3);
				TutorialSetBeadMaterial(true, 1, 3);
				TutorialSetHand(1, 3);
				TutorialSetArrow(true, 1);
				TutorialSetArrow(true, 4);
				break;
			case 18:
				SetKeyLocalizedText(tutorialTextBody, "tutorial_division_body_14");
				Invoke("TutorialSetRightArrowActive", 0.3f);
				totalValue.SetTotalValue(138240);
				TutorialSetArrow(true, 1);
				TutorialSetArrow(true, 4);
				break;
			case 19:
				SetKeyLocalizedText(tutorialTextBody, "tutorial_division_body_15");
				Invoke("TutorialSetRightArrowInactive", 0.3f);
				totalValue.SetTotalValue(138240);
				tutorialModeInfo.answer = 38240;
				tutorialModeInfo.tutorialModeState = TutorialModeState.Playing;
				TutorialSetBeadMusk(false, 5, 0);
				TutorialSetBeadMaterial(true, 5, 0);
				TutorialSetHand(5, 0);
				TutorialSetArrow(true, 1);
				TutorialSetArrow(true, 4);
				break;
			case 20:
				SetKeyLocalizedText(tutorialTextBody, "tutorial_division_body_15");
				Invoke("TutorialSetRightArrowInactive", 0.3f);
				totalValue.SetTotalValue(38240);
				tutorialModeInfo.answer = 18240;
				tutorialModeInfo.tutorialModeState = TutorialModeState.Playing;
				TutorialSetBeadMusk(false, 4, 1);
				TutorialSetBeadMaterial(true, 4, 1);
				TutorialSetHand(4, 1);
				TutorialSetArrow(true, 1);
				TutorialSetArrow(true, 4);
				break;
			case 21:
				SetKeyLocalizedText(tutorialTextBody, "tutorial_division_body_16");
				Invoke("TutorialSetRightArrowActive", 0.3f);
				totalValue.SetTotalValue(18240);
				TutorialSetArrow(true, 1);
				TutorialSetArrow(true, 4);
				break;
			case 22:
				SetKeyLocalizedText(tutorialTextBody, "tutorial_division_body_17");
				Invoke("TutorialSetRightArrowActive", 0.3f);
				totalValue.SetTotalValue(18240);
				TutorialSetArrow(true, 0);
				TutorialSetArrow(true, 3);
				break;
			case 23:
				SetKeyLocalizedText(tutorialTextBody, "tutorial_division_body_18");
				Invoke("TutorialSetRightArrowInactive", 0.3f);
				totalValue.SetTotalValue(18240);
				tutorialModeInfo.answer = 18245;
				tutorialModeInfo.tutorialModeState = TutorialModeState.Playing;
				TutorialSetBeadMusk(false, 0, 4);
				TutorialSetBeadMaterial(true, 0, 4);
				TutorialSetHand(0, 4);
				TutorialSetArrow(true, 0);
				TutorialSetArrow(true, 3);
				break;
			case 24:
				SetKeyLocalizedText(tutorialTextBody, "tutorial_division_body_18");
				Invoke("TutorialSetRightArrowInactive", 0.3f);
				totalValue.SetTotalValue(18245);
				tutorialModeInfo.answer = 18246;
				tutorialModeInfo.tutorialModeState = TutorialModeState.Playing;
				TutorialSetBeadMusk(false, 0, 0);
				TutorialSetBeadMaterial(true, 0, 0);
				TutorialSetHand(0, 0);
				TutorialSetArrow(true, 0);
				TutorialSetArrow(true, 3);
				break;
			case 25:
				SetKeyLocalizedText(tutorialTextBody, "tutorial_division_body_19");
				Invoke("TutorialSetRightArrowActive", 0.3f);
				totalValue.SetTotalValue(18246);
				TutorialSetArrow(true, 0);
				TutorialSetArrow(true, 3);
				break;
			case 26:
				SetKeyLocalizedText(tutorialTextBody, "tutorial_division_body_20");
				Invoke("TutorialSetRightArrowInactive", 0.3f);
				totalValue.SetTotalValue(18246);
				tutorialModeInfo.answer = 8246;
				tutorialModeInfo.tutorialModeState = TutorialModeState.Playing;
				TutorialSetBeadMusk(false, 4, 0);
				TutorialSetBeadMaterial(true, 4, 0);
				TutorialSetHand(4, 0);
				TutorialSetArrow(true, 0);
				TutorialSetArrow(true, 3);
				break;
			case 27:
				SetKeyLocalizedText(tutorialTextBody, "tutorial_division_body_20");
				Invoke("TutorialSetRightArrowInactive", 0.3f);
				totalValue.SetTotalValue(8246);
				tutorialModeInfo.answer = 3246;
				tutorialModeInfo.tutorialModeState = TutorialModeState.Playing;
				TutorialSetBeadMusk(false, 3, 4);
				TutorialSetBeadMaterial(true, 3, 4);
				TutorialSetHand(3, 4);
				TutorialSetArrow(true, 0);
				TutorialSetArrow(true, 3);
				break;
			case 28:
				SetKeyLocalizedText(tutorialTextBody, "tutorial_division_body_20");
				Invoke("TutorialSetRightArrowInactive", 0.3f);
				totalValue.SetTotalValue(3246);
				tutorialModeInfo.answer = 246;
				tutorialModeInfo.tutorialModeState = TutorialModeState.Playing;
				TutorialSetBeadMusk(false, 3, 0);
				TutorialSetBeadMaterial(true, 3, 0);
				TutorialSetHand(3, 0);
				TutorialSetArrow(true, 0);
				TutorialSetArrow(true, 3);
				break;
			case 29:
				SetKeyLocalizedText(tutorialTextBody, "tutorial_division_body_21");
				Invoke("TutorialSetRightArrowActive", 0.3f);
				totalValue.SetTotalValue(246);
				break;
			case 30:
				SetKeyLocalizedText(tutorialTextBody, "tutorial_division_body_22");
				Invoke("TutorialSetRightArrowActive", 0.3f);
				totalValue.SetTotalValue(246);
				break;
			default: break;
		}
	}

	private void TutorialSetRightArrowActive()
	{
		tutorialRightArrow.interactable = true;
	}

	private void TutorialSetRightArrowInactive()
	{
		tutorialRightArrow.interactable = false;
	}

	private void TutorialSetRodMaterialAll(bool flg)
	{
		foreach (GameObject tutorialRod in tutorialRods)
		{
			tutorialRod.GetComponent<MeshRenderer>().material = flg ? tutorialRodMaterialActive : tutorialRodMaterialInActive;
		}
	}

	private void TutorialSetRodMaterial(bool flg ,int column)
	{
		tutorialRods[column].GetComponent<MeshRenderer>().material = flg ? tutorialRodMaterialActive : tutorialRodMaterialInActive;
	}

	private void TutorialSetBeamMaterial(bool flg)
	{
		tutorialBeam.GetComponent<MeshRenderer>().material = flg ? tutorialBeamMaterialActive : tutorialBeamMaterialInActive;
	}

	private void TutorialSetHand(int column, int row)
	{
		SetActive(tutorialHand);
		tutorialHand.transform.position = GetBead(column, row).transform.position;
	}

	private GameObject GetBead(int column, int row)
	{
		GameObject bead = new GameObject();

		switch (column)
		{
			case 0:
				bead = tutorialRod1Beads[row];
				break;
			case 1:
				bead = tutorialRod2Beads[row];
				break;
			case 2:
				bead = tutorialRod3Beads[row];
				break;
			case 3:
				bead = tutorialRod4Beads[row];
				break;
			case 4:
				bead = tutorialRod5Beads[row];
				break;
			case 5:
				bead = tutorialRod6Beads[row];
				break;
			case 6:
				bead = tutorialRod7Beads[row];
				break;
			default: break;
		}
		return bead;
	}

	private void SetKeyLocalizedText(TextMeshProUGUI textMeshProUGUI, string key)
	{
		textMeshProUGUI.GetComponent<LocalizationText>().SetKey(key);
	}

	private void TutorialSetDigitLine(bool flg, int column)
	{
		if (flg)
		{
			SetActive(tutorialDigitLines[column]);
		}
		else
		{
			tutorialDigitLines[column].SetActive(false);
		}
	}

	private void TutorialSetDigitLineAll(bool flg)
	{
		foreach (GameObject tutorialDigitLine in tutorialDigitLines)
		{
			if (flg)
			{
				SetActive(tutorialDigitLine);
			}
			else
			{
				tutorialDigitLine.SetActive(false);
			}
		}
	}

	private void TutorialSetDigitArrow(bool flg, int column)
	{
		if (flg)
		{
			SetActive(tutorialDigitArrows[column]);
		}
		else
		{
			tutorialDigitArrows[column].SetActive(false);
		}
	}

	private void TutorialSetDigitArrowAll(bool flg)
	{
		foreach (GameObject tutorialDigitArrow in tutorialDigitArrows)
		{
			if (flg)
			{
				SetActive(tutorialDigitArrow);
			}
			else
			{
				tutorialDigitArrow.SetActive(false);
			}
		}
	}
	
	private void TutorialSetArrow(bool flg, int column)
	{
		if (flg)
		{
			SetActive(tutorialRodArrows[column]);
		}
		else
		{
			tutorialRodArrows[column].SetActive(false);
		}
	}

	private void TutorialSetArrowAll(bool flg)
	{
		foreach (GameObject tutorialRodArrow in tutorialRodArrows)
		{
			if (flg)
			{
				SetActive(tutorialRodArrow);
			}
			else
			{
				tutorialRodArrow.SetActive(false);
			}
		}
	}

	private void TutorialSetTextTitle()
	{
		switch (tutorialModeInfo.tutorialType)
		{
			case TutorialType.Basic:
				SetKeyLocalizedText(tutorialTextTitle, "tutorial_menu_basic_title");
				break;
			case TutorialType.Addition:
				SetKeyLocalizedText(tutorialTextTitle, "tutorial_menu_add_title");
				break;
			case TutorialType.Subtraction:
				SetKeyLocalizedText(tutorialTextTitle, "tutorial_menu_sub_title");
				break;
			case TutorialType.Multiplication:
				SetKeyLocalizedText(tutorialTextTitle, "tutorial_menu_multi_title");
				break;
			case TutorialType.Division:
				SetKeyLocalizedText(tutorialTextTitle, "tutorial_menu_division_title");
				break;
			default: break;
		}
	}

	private void TutorialSetTextPage()
	{
		tutorialTextPage.text = (tutorialModeInfo.tutorialSeqNo + 1).ToString() + " / " + seqNums[(int)tutorialModeInfo.tutorialType].ToString();
	}

	private void TutorialSetBeadMaterial(bool flg, int column, int row)
	{
		switch (column)
		{
			case 0:
				tutorialRod1Beads[row].GetComponent<MeshRenderer>().material = flg ? tutorialBeadMaterialActive : tutorialBeadMaterialInActive;
				break;
			case 1:
				tutorialRod2Beads[row].GetComponent<MeshRenderer>().material = flg ? tutorialBeadMaterialActive : tutorialBeadMaterialInActive;
				break;
			case 2:
				tutorialRod3Beads[row].GetComponent<MeshRenderer>().material = flg ? tutorialBeadMaterialActive : tutorialBeadMaterialInActive;
				break;
			case 3:
				tutorialRod4Beads[row].GetComponent<MeshRenderer>().material = flg ? tutorialBeadMaterialActive : tutorialBeadMaterialInActive;
				break;
			case 4:
				tutorialRod5Beads[row].GetComponent<MeshRenderer>().material = flg ? tutorialBeadMaterialActive : tutorialBeadMaterialInActive;
				break;
			case 5:
				tutorialRod6Beads[row].GetComponent<MeshRenderer>().material = flg ? tutorialBeadMaterialActive : tutorialBeadMaterialInActive;
				break;
			case 6:
				tutorialRod7Beads[row].GetComponent<MeshRenderer>().material = flg ? tutorialBeadMaterialActive : tutorialBeadMaterialInActive;
				break;
			default: break;
		}
	}

	private void TutorialSetBeadMaterialAll(bool flg)
	{
		foreach (GameObject tutorialRod1Bead in tutorialRod1Beads)
		{
			tutorialRod1Bead.GetComponent<MeshRenderer>().material = flg ? tutorialBeadMaterialActive : tutorialBeadMaterialInActive;	
		}
		foreach (GameObject tutorialRod2Bead in tutorialRod2Beads)
		{
			tutorialRod2Bead.GetComponent<MeshRenderer>().material = flg ? tutorialBeadMaterialActive : tutorialBeadMaterialInActive;	
		}
		foreach (GameObject tutorialRod3Bead in tutorialRod3Beads)
		{
			tutorialRod3Bead.GetComponent<MeshRenderer>().material = flg ? tutorialBeadMaterialActive : tutorialBeadMaterialInActive;	
		}
		foreach (GameObject tutorialRod4Bead in tutorialRod4Beads)
		{
			tutorialRod4Bead.GetComponent<MeshRenderer>().material = flg ? tutorialBeadMaterialActive : tutorialBeadMaterialInActive;	
		}
		foreach (GameObject tutorialRod5Bead in tutorialRod5Beads)
		{
			tutorialRod5Bead.GetComponent<MeshRenderer>().material = flg ? tutorialBeadMaterialActive : tutorialBeadMaterialInActive;	
		}
		foreach (GameObject tutorialRod6Bead in tutorialRod6Beads)
		{
			tutorialRod6Bead.GetComponent<MeshRenderer>().material = flg ? tutorialBeadMaterialActive : tutorialBeadMaterialInActive;	
		}
		foreach (GameObject tutorialRod7Bead in tutorialRod7Beads)
		{
			tutorialRod7Bead.GetComponent<MeshRenderer>().material = flg ? tutorialBeadMaterialActive : tutorialBeadMaterialInActive;	
		}
	}

	private void TutorialSetBeadMusk(bool isMusk, int column, int row)
	{
		switch (column)
		{
			case 0:
				tutorialRod1Beads[row].GetComponent<BeadMusk>().isMusk = isMusk;
				break;
			case 1:
				tutorialRod2Beads[row].GetComponent<BeadMusk>().isMusk = isMusk;
				break;
			case 2:
				tutorialRod3Beads[row].GetComponent<BeadMusk>().isMusk = isMusk;
				break;
			case 3:
				tutorialRod4Beads[row].GetComponent<BeadMusk>().isMusk = isMusk;
				break;
			case 4:
				tutorialRod5Beads[row].GetComponent<BeadMusk>().isMusk = isMusk;
				break;
			case 5:
				tutorialRod6Beads[row].GetComponent<BeadMusk>().isMusk = isMusk;
				break;
			case 6:
				tutorialRod7Beads[row].GetComponent<BeadMusk>().isMusk = isMusk;
				break;
			default: break;
		}
	}

	private void TutorialSetBeadMuskAll(bool isMusk)
	{
		foreach (GameObject tutorialRod1Bead in tutorialRod1Beads)
		{
			tutorialRod1Bead.GetComponent<BeadMusk>().isMusk = isMusk;
		}
		foreach (GameObject tutorialRod2Bead in tutorialRod2Beads)
		{
			tutorialRod2Bead.GetComponent<BeadMusk>().isMusk = isMusk;
		}
		foreach (GameObject tutorialRod3Bead in tutorialRod3Beads)
		{
			tutorialRod3Bead.GetComponent<BeadMusk>().isMusk = isMusk;
		}
		foreach (GameObject tutorialRod4Bead in tutorialRod4Beads)
		{
			tutorialRod4Bead.GetComponent<BeadMusk>().isMusk = isMusk;
		}
		foreach (GameObject tutorialRod5Bead in tutorialRod5Beads)
		{
			tutorialRod5Bead.GetComponent<BeadMusk>().isMusk = isMusk;
		}
		foreach (GameObject tutorialRod6Bead in tutorialRod6Beads)
		{
			tutorialRod6Bead.GetComponent<BeadMusk>().isMusk = isMusk;
		}
		foreach (GameObject tutorialRod7Bead in tutorialRod7Beads)
		{
			tutorialRod7Bead.GetComponent<BeadMusk>().isMusk = isMusk;
		}
	}

	private void SetRecordPanel(bool flg)
	{
		if (flg)
		{
			SetActive(recordPanel);
			Vector3 pos = followTarget.transform.localPosition;
			pos.x  +=  1f;
			recordPanel.transform.localPosition = pos;
			Quaternion rot = Quaternion.LookRotation(recordPanel.transform.position - centerEye.position);
			recordPanel.transform.rotation = rot;
			Invoke("GetRecords", 0.5f);
		}
		else
		{
			recordPanel.SetActive(false);
		}
	}
	private void GetRecords()
	{
		OperationType operationType = OperationType.None;
		operationType = GetOperationType(recordModeInfo.challengeType, recordModeInfo.recordBasicType, recordModeInfo.recordAdditionType);

		GameRecord _record_1st = DataManager.GetRecordByRank(operationType, recordModeInfo.recordDifficultyType, recordModeInfo.recordNumberOfQuestions, 1);
		
		if (_record_1st != null)
		{
			record_1st.text = _record_1st.completionTime;
			record_date_1st.text = _record_1st.completionDateTime;
		}
		else
		{
			SetKeyLocalizedText(record_1st, "record_body_nothing");
			record_date_1st.text = "";
		}

		GameRecord _record_2nd = DataManager.GetRecordByRank(operationType, recordModeInfo.recordDifficultyType, recordModeInfo.recordNumberOfQuestions, 2);
		
		if (_record_2nd != null)
		{
			record_2nd.text = _record_2nd.completionTime;
			record_date_2nd.text = _record_2nd.completionDateTime;
		}
		else
		{
			SetKeyLocalizedText(record_2nd, "record_body_nothing");
			record_date_2nd.text = "";
		}

		GameRecord _record_3rd = DataManager.GetRecordByRank(operationType, recordModeInfo.recordDifficultyType, recordModeInfo.recordNumberOfQuestions, 3);

		if (_record_3rd != null)
		{
			record_3rd.text = _record_3rd.completionTime;
			record_date_3rd.text = _record_3rd.completionDateTime;
		}
		else
		{
			SetKeyLocalizedText(record_3rd, "record_body_nothing");
			record_date_3rd.text = "";
		}

		GameRecord _record_4th = DataManager.GetRecordByRank(operationType, recordModeInfo.recordDifficultyType, recordModeInfo.recordNumberOfQuestions, 4);

		if (_record_4th != null)
		{
			record_4th.text = _record_4th.completionTime;
			record_date_4th.text = _record_4th.completionDateTime;
		}
		else
		{
			SetKeyLocalizedText(record_4th, "record_body_nothing");
			record_date_4th.text = "";
		}

		GameRecord _record_5th = DataManager.GetRecordByRank(operationType, recordModeInfo.recordDifficultyType, recordModeInfo.recordNumberOfQuestions, 5);

		if (_record_5th != null)
		{
			record_5th.text = _record_5th.completionTime;
			record_date_5th.text = _record_5th.completionDateTime;
		}
		else
		{
			SetKeyLocalizedText(record_5th, "record_body_nothing");
			record_date_5th.text = "";
		}
	}

	private void DeleteRecord()
	{
		OperationType operationType = OperationType.None;
		operationType = GetOperationType(recordModeInfo.challengeType, recordModeInfo.recordBasicType, recordModeInfo.recordAdditionType);

		DataManager.DeleteAndSaveRecords(operationType, recordModeInfo.recordDifficultyType, recordModeInfo.recordNumberOfQuestions);

		switch (recordModeInfo.recordType)
		{
			case RecordMenuType.Basic:
				CheckRecordBasicPanelInput();
				break;
			case RecordMenuType.Addition:
				CheckRecordAdditionPanelInput();
				break;
			case RecordMenuType.Subtraction:
				CheckRecordSubtractionPanelInput();
				break;
			case RecordMenuType.Multiplication:
				CheckRecordMultiplicationPanelInput();
				break;
			case RecordMenuType.Division:
				CheckRecordDivisionPanelInput();
				break;
			case RecordMenuType.Custom:
				CheckRecordCustomPanelInput();
				break;
			default: break;
		}
	}

	private void CheckRecordBasicPanelInput()
	{
		if (recordBasicPanelRead.flg)
		{
			recordModeInfo.recordBasicType = ChallengeBasicType.Read;
		}
		else
		{
			recordModeInfo.recordBasicType = ChallengeBasicType.Write;
		}
		if (recordBasicPanelEasy.flg)
		{
			recordModeInfo.recordDifficultyType = ChallengeDifficultyType.Easy;
		}
		else if (recordBasicPanelMedium.flg)
		{
			recordModeInfo.recordDifficultyType = ChallengeDifficultyType.Medium;
		}
		else
		{
			recordModeInfo.recordDifficultyType = ChallengeDifficultyType.Hard;
		}

		for (int i = 0; i < recordBasicPanelQuestions.Length; i++)
		{
			if (recordBasicPanelQuestions[i].flg)
			{
				recordModeInfo.recordNumberOfQuestions = (i + 1) * 10;
			}
		}
		SetRecordPanel(true);
	}

	private void CheckRecordAdditionPanelInput()
	{
		if (recordAdditionPanelNormal.flg)
		{
			recordModeInfo.recordAdditionType = ChallengeAdditionType.Normal;
		}
		else if (recordAdditionPanelChain.flg)
		{
			recordModeInfo.recordAdditionType = ChallengeAdditionType.Chain;
		}
		else
		{
			recordModeInfo.recordAdditionType = ChallengeAdditionType.Flash;
		}
		if (recordAdditionPanelEasy.flg)
		{
			recordModeInfo.recordDifficultyType = ChallengeDifficultyType.Easy;
		}
		else if (recordAdditionPanelMedium.flg)
		{
			recordModeInfo.recordDifficultyType = ChallengeDifficultyType.Medium;
		}
		else
		{
			recordModeInfo.recordDifficultyType = ChallengeDifficultyType.Hard;
		}

		for (int i = 0; i < recordAdditionPanelQuestions.Length; i++)
		{
			if (recordAdditionPanelQuestions[i].flg)
			{
				recordModeInfo.recordNumberOfQuestions = (i + 1) * 10;
			}
		}
		SetRecordPanel(true);
	}

	private void CheckRecordSubtractionPanelInput()
	{
		if (recordSubtractionPanelEasy.flg)
		{
			recordModeInfo.recordDifficultyType = ChallengeDifficultyType.Easy;
		}
		else if (recordSubtractionPanelMedium.flg)
		{
			recordModeInfo.recordDifficultyType = ChallengeDifficultyType.Medium;
		}
		else
		{
			recordModeInfo.recordDifficultyType = ChallengeDifficultyType.Hard;
		}

		for (int i = 0; i < recordSubtractionPanelQuestions.Length; i++)
		{
			if (recordSubtractionPanelQuestions[i].flg)
			{
				recordModeInfo.recordNumberOfQuestions = (i + 1) * 10;
			}
		}
		SetRecordPanel(true);
	}

	private void CheckRecordMultiplicationPanelInput()
	{
		if (recordMultiplicationPanelEasy.flg)
		{
			recordModeInfo.recordDifficultyType = ChallengeDifficultyType.Easy;
		}
		else if (recordMultiplicationPanelMedium.flg)
		{
			recordModeInfo.recordDifficultyType = ChallengeDifficultyType.Medium;
		}
		else
		{
			recordModeInfo.recordDifficultyType = ChallengeDifficultyType.Hard;
		}

		for (int i = 0; i < recordMultiplicationPanelQuestions.Length; i++)
		{
			if (recordMultiplicationPanelQuestions[i].flg)
			{
				recordModeInfo.recordNumberOfQuestions = (i + 1) * 10;
			}
		}
		SetRecordPanel(true);
	}

	private void CheckRecordDivisionPanelInput()
	{
		if (recordDivisionPanelEasy.flg)
		{
			recordModeInfo.recordDifficultyType = ChallengeDifficultyType.Easy;
		}
		else if (recordDivisionPanelMedium.flg)
		{
			recordModeInfo.recordDifficultyType = ChallengeDifficultyType.Medium;
		}
		else
		{
			recordModeInfo.recordDifficultyType = ChallengeDifficultyType.Hard;
		}

		for (int i = 0; i < recordDivisionPanelQuestions.Length; i++)
		{
			if (recordDivisionPanelQuestions[i].flg)
			{
				recordModeInfo.recordNumberOfQuestions = (i + 1) * 10;
			}
		}
		SetRecordPanel(true);
	}

	private void CheckRecordCustomPanelInput()
	{
		recordModeInfo.recordCustomInfo.add = recordCustomPanelAddition.flg;
		recordModeInfo.recordCustomInfo.sub = recordCustomPanelSubtraction.flg;
		recordModeInfo.recordCustomInfo.multi = recordCustomPanelMultiplication.flg;
		recordModeInfo.recordCustomInfo.divide = recordCustomPanelDivision.flg;
		if (recordCustomPanelEasy.flg)
		{
			recordModeInfo.recordDifficultyType = ChallengeDifficultyType.Easy;
		}
		else if (recordCustomPanelMedium.flg)
		{
			recordModeInfo.recordDifficultyType = ChallengeDifficultyType.Medium;
		}
		else
		{
			recordModeInfo.recordDifficultyType = ChallengeDifficultyType.Hard;
		}

		for (int i = 0; i < recordCustomPanelQuestions.Length; i++)
		{
			if (recordCustomPanelQuestions[i].flg)
			{
				recordModeInfo.recordNumberOfQuestions = (i + 1) * 10;
			}
		}
		if ((recordModeInfo.recordCustomInfo.add)
		|| (recordModeInfo.recordCustomInfo.sub)
		|| (recordModeInfo.recordCustomInfo.multi)
		|| (recordModeInfo.recordCustomInfo.divide))
		{
			SetRecordPanel(true);
		}
		else
		{
			SetRecordPanel(false);
		}
	}

	public void SwitchGameMode(GameMode mode)
	{
		switch (gameMode)
		{
			case GameMode.MenuMode:
				ExitMenu();
				break;
			case GameMode.TutorialMode:
				ExitTutorial();
				break;
			case GameMode.ChallengeMode:
				ExitChallenge();
				break;
			case GameMode.RecordMode:
				ExitRecord();
				break;
			case GameMode.SettingMode:
				ExitSetting();
				break;
			case GameMode.PrivacyPolicyMode:
				ExitPrivacyPolicy();
				break;
			default:
				break;
		}

		switch (mode)
		{
			case GameMode.MenuMode:
				SetMenuMode(); break;
			case GameMode.TutorialMode:
				SetTutorialMode(); break;
			case GameMode.ChallengeMode:
				SetChallengeMode(); break;
			case GameMode.RecordMode:
				SetRecordMode(); break;
			case GameMode.SettingMode:
				SetSettingMode(); break;
			case GameMode.PrivacyPolicyMode:
				SetPrivacyPolicyMode(); break;
			default: break;
		}
	}

	public void RetryChallengeMode()
	{
		if (challengeModeInfo.challengeModeState == ChallengeModeState.Finish)
		{
			SwitchGameMode(GameMode.ChallengeMode);
		}
	}

	private void CheckChallengeBasicPanelInput()
	{
		if (challengeBasicPanelRead.flg)
		{
			challengeModeInfo.challengeBasicType = ChallengeBasicType.Read;
		}
		else
		{
			challengeModeInfo.challengeBasicType = ChallengeBasicType.Write;
		}
		if (challengeBasicPanelEasy.flg)
		{
			challengeModeInfo.challengeDifficultyType = ChallengeDifficultyType.Easy;
		}
		else if (challengeBasicPanelMedium.flg)
		{
			challengeModeInfo.challengeDifficultyType = ChallengeDifficultyType.Medium;
		}
		else
		{
			challengeModeInfo.challengeDifficultyType = ChallengeDifficultyType.Hard;
		}

		for (int i = 0; i < challengeBasicPanelQuestions.Length; i++)
		{
			if (challengeBasicPanelQuestions[i].flg)
			{
				challengeModeInfo.challengeNumberOfQuestions = (i + 1) * 10;
			}
		}
	}

	private void CheckChallengeAdditionPanelInput()
	{
		if (challengeAdditionPanelNormal.flg)
		{
			challengeModeInfo.challengeAdditionType = ChallengeAdditionType.Normal;
		}
		else if (challengeAdditionPanelChain.flg)
		{
			challengeModeInfo.challengeAdditionType = ChallengeAdditionType.Chain;
		}
		else
		{
			challengeModeInfo.challengeAdditionType = ChallengeAdditionType.Flash;
		}
		if (challengeAdditionPanelEasy.flg)
		{
			challengeModeInfo.challengeDifficultyType = ChallengeDifficultyType.Easy;
		}
		else if (challengeAdditionPanelMedium.flg)
		{
			challengeModeInfo.challengeDifficultyType = ChallengeDifficultyType.Medium;
		}
		else
		{
			challengeModeInfo.challengeDifficultyType = ChallengeDifficultyType.Hard;
		}

		for (int i = 0; i < challengeAdditionPanelQuestions.Length; i++)
		{
			if (challengeAdditionPanelQuestions[i].flg)
			{
				challengeModeInfo.challengeNumberOfQuestions = (i + 1) * 10;
			}
		}
	}

	private void CheckChallengeSubtractionPanelInput()
	{
		if (challengeSubtractionPanelEasy.flg)
		{
			challengeModeInfo.challengeDifficultyType = ChallengeDifficultyType.Easy;
		}
		else if (challengeSubtractionPanelMedium.flg)
		{
			challengeModeInfo.challengeDifficultyType = ChallengeDifficultyType.Medium;
		}
		else
		{
			challengeModeInfo.challengeDifficultyType = ChallengeDifficultyType.Hard;
		}

		for (int i = 0; i < challengeSubtractionPanelQuestions.Length; i++)
		{
			if (challengeSubtractionPanelQuestions[i].flg)
			{
				challengeModeInfo.challengeNumberOfQuestions = (i + 1) * 10;
			}
		}
	}

	private void CheckChallengeMultiplicationPanelInput()
	{
		if (challengeMultiplicationPanelEasy.flg)
		{
			challengeModeInfo.challengeDifficultyType = ChallengeDifficultyType.Easy;
		}
		else if (challengeMultiplicationPanelMedium.flg)
		{
			challengeModeInfo.challengeDifficultyType = ChallengeDifficultyType.Medium;
		}
		else
		{
			challengeModeInfo.challengeDifficultyType = ChallengeDifficultyType.Hard;
		}

		for (int i = 0; i < challengeMultiplicationPanelQuestions.Length; i++)
		{
			if (challengeMultiplicationPanelQuestions[i].flg)
			{
				challengeModeInfo.challengeNumberOfQuestions = (i + 1) * 10;
			}
		}
	}

	private void CheckChallengeDivisionPanelInput()
	{
		if (challengeDivisionPanelEasy.flg)
		{
			challengeModeInfo.challengeDifficultyType = ChallengeDifficultyType.Easy;
		}
		else if (challengeDivisionPanelMedium.flg)
		{
			challengeModeInfo.challengeDifficultyType = ChallengeDifficultyType.Medium;
		}
		else
		{
			challengeModeInfo.challengeDifficultyType = ChallengeDifficultyType.Hard;
		}

		for (int i = 0; i < challengeDivisionPanelQuestions.Length; i++)
		{
			if (challengeDivisionPanelQuestions[i].flg)
			{
				challengeModeInfo.challengeNumberOfQuestions = (i + 1) * 10;
			}
		}
	}

	private void CheckChallengeCustomPanelInput()
	{
		challengeModeInfo.challengeCustomInfo.add = challengeCustomPanelAddition.flg;
		challengeModeInfo.challengeCustomInfo.sub = challengeCustomPanelSubtraction.flg;
		challengeModeInfo.challengeCustomInfo.multi = challengeCustomPanelMultiplication.flg;
		challengeModeInfo.challengeCustomInfo.divide = challengeCustomPanelDivision.flg;
		if (challengeCustomPanelEasy.flg)
		{
			challengeModeInfo.challengeDifficultyType = ChallengeDifficultyType.Easy;
		}
		else if (challengeCustomPanelMedium.flg)
		{
			challengeModeInfo.challengeDifficultyType = ChallengeDifficultyType.Medium;
		}
		else
		{
			challengeModeInfo.challengeDifficultyType = ChallengeDifficultyType.Hard;
		}

		for (int i = 0; i < challengeCustomPanelQuestions.Length; i++)
		{
			if (challengeCustomPanelQuestions[i].flg)
			{
				challengeModeInfo.challengeNumberOfQuestions = (i + 1) * 10;
			}
		}
	}

	private void ExitMenu()
	{
	}

	private void ExitTutorial()
	{
		TutorialSetBeadMuskAll(false);
		tutorialModeInfo.tutorialModeState = TutorialModeState.Waiting;
		TutorialSetRodMaterialAll(false);
		TutorialSetBeamMaterial(false);
	}

	private void ExitChallenge()
	{
		if ((challengeModeInfo.arithmeticType == ArithmeticType.None)
		&& (challengeModeInfo.challengeBasicType == ChallengeBasicType.Read))
		{
			SetAbacus(true);
			tenkeyBasicRead.SetActive(false);
		}
	}

	private void ExitRecord()
	{
	}

	private void ExitSetting()
	{
	}

	private void ExitPrivacyPolicy()
	{
	}

	private void SetMenuMode()
	{
		gameMode = GameMode.MenuMode;

		SwitchObjectActive(gameMode);

		InitMenu();
	}

	private void SetTutorialMode()
	{
		gameMode = GameMode.TutorialMode;

		SwitchObjectActive(gameMode);

		InitTutorial();
	}

	private void SetChallengeMode()
	{
		gameMode = GameMode.ChallengeMode;

		SwitchObjectActive(gameMode);

		InitChallenge();
	}

	private void SetRecordMode()
	{
		gameMode = GameMode.RecordMode;

		SwitchObjectActive(gameMode);

		InitRecord();
	}

	private void SetSettingMode()
	{
		gameMode = GameMode.SettingMode;

		SwitchObjectActive(gameMode);

		InitSetting();
	}

	private void SetPrivacyPolicyMode()
	{
		gameMode = GameMode.PrivacyPolicyMode;

		SwitchObjectActive(gameMode);

		InitPrivacyPolicy();
	}

	private void InitMenu()
	{
		menuModeInfo.menuModeType = MenuModeType.HomeMenu;

		SetFollowTarget(menuPanel.transform);
	}

	private void InitTutorial()
	{
		tutorialBodyPanel.transform.localPosition = abacusObject.transform.localPosition + abacusObject.transform.up * abacusObject.transform.localScale.y * 1.2f;
		Quaternion rot = Quaternion.LookRotation(tutorialBodyPanel.transform.position - centerEye.position);
		tutorialBodyPanel.transform.localRotation = rot;

		tutorialModeInfo.tutorialSeqNo = 0;
		valueTutorial = "0";
		tutorialTextBody.text = "";
		TutorialSetTextPage();
		TutorialSetBeadMuskAll(true);
		//TutorialSetBeadMaterialAll(false);
		TutorialSetArrowAll(false);
		TutorialSetDigitLineAll(false);
		TutorialSetDigitArrowAll(false);
		Invoke("TutorialSeqExe", 0.5f);
	}

	private void InitChallenge()
	{
		if (challengeModeInfo.challengeType == ChallengeType.Basic)
		{
			if (challengeModeInfo.challengeBasicType == ChallengeBasicType.Read)
			{
				SetAbacus(false);
				SetActive(tenkeyBasicRead);
			}
			tenkeyPanel.SetActive(false);
		}

		challengeModeInfo.challengeModeState = ChallengeModeState.CountDown;
		challengeModeInfo.count = COUNT_DONW_TIME;

		questionTextMeshPro.text = "";
		questionNoTextMeshPro.text = "";
		questionPanelProgressActive.gameObject.SetActive(false);
		questionPanelProgressInactive.gameObject.SetActive(false);

		CountDownEffect();

		SetCountDownText();

		SetFollowTarget(questionPanel.transform);
	}

	private void InitRecord()
	{
	}

	private void InitSetting()
	{
	}

	private void InitPrivacyPolicy()
	{
		SetFollowTarget(privacyPolicyPanel.transform);
	}

	private void SetAbacus(bool flg)
	{
		abacusObject.SetActive(flg);
		abacusPanel.SetActive(flg);

		if (flg)
		{
			Invoke("ClearValues", 0.2f);
		}
	}

	private void ClearValues()
	{
		totalValue.SetTotalValue(0);
		keyPadValue.ClearValue();
		keyPadValueBasicRead.ClearValue();
	}

	private void SetQuesitonAbacus(bool flg)
	{
		questionAbacusObject.SetActive(flg);
	}

	private void SetFollowTarget(Transform transform)
	{
		transform.position = followTarget.transform.position;
		transform.rotation = followTarget.transform.rotation;
		followTarget.target = transform;
	}

	private void SwitchMenu(MenuModeType menuModeType)
	{
		AllObjectDisable();

		switch (menuModeType)
		{
			case MenuModeType.HomeMenu:
				menuModeInfo.menuModeType = MenuModeType.HomeMenu;
				SetActive(menuPanel);
				SetFollowTarget(menuPanel.transform);
				break;
			case MenuModeType.TutorialMenu:
				menuModeInfo.menuModeType = MenuModeType.TutorialMenu;
				SetActive(tutorialPanel);
				SetFollowTarget(tutorialPanel.transform);
				break;
			case MenuModeType.ChallengeMenu:
				menuModeInfo.menuModeType = MenuModeType.ChallengeMenu;
				SetActive(challengeHomePanel);
				SetFollowTarget(challengeHomePanel.transform);
				break;
			case MenuModeType.RecordMenu:
				menuModeInfo.menuModeType = MenuModeType.RecordMenu;
				SetActive(recordHomePanel);
				SetFollowTarget(recordHomePanel.transform);
				break;
			case MenuModeType.SettingMenu:
				menuModeInfo.menuModeType = MenuModeType.SettingMenu;
				SetActive(settingHomePanel);
				SetFollowTarget(settingHomePanel.transform);
				break;
			default: break;
		}
	}

	private void SwitchChallengeMenu(ChallengeMenuType challengeMenuType)
	{
		AllObjectDisable();

		switch (challengeMenuType)
		{
			case ChallengeMenuType.Home:
				menuModeInfo.challengeMenuType = ChallengeMenuType.Home;
				SetActive(challengeHomePanel);
				followTarget.target = challengeHomePanel.transform;
				SetFollowTarget(challengeHomePanel.transform);
				break;
			case ChallengeMenuType.Basic:
				menuModeInfo.challengeMenuType = ChallengeMenuType.Basic;
				SetActive(challengeBasicPanel);
				followTarget.target = challengeBasicPanel.transform;
				SetFollowTarget(challengeBasicPanel.transform);
				break;
			case ChallengeMenuType.Addition:
				menuModeInfo.challengeMenuType = ChallengeMenuType.Addition;
				SetActive(challengeAdditionPanel);
				followTarget.target = challengeAdditionPanel.transform;
				SetFollowTarget(challengeAdditionPanel.transform);
				break;
			case ChallengeMenuType.Subtraction:
				menuModeInfo.challengeMenuType = ChallengeMenuType.Subtraction;
				SetActive(challengeSubtractionPanel);
				followTarget.target = challengeSubtractionPanel.transform;
				SetFollowTarget(challengeSubtractionPanel.transform);
				break;
			case ChallengeMenuType.Multiplication:
				menuModeInfo.challengeMenuType = ChallengeMenuType.Multiplication;
				SetActive(challengeMultiplicationPanel);
				followTarget.target = challengeMultiplicationPanel.transform;
				SetFollowTarget(challengeMultiplicationPanel.transform);
				break;
			case ChallengeMenuType.Division:
				menuModeInfo.challengeMenuType = ChallengeMenuType.Division;
				SetActive(challengeDivisionPanel);
				followTarget.target = challengeDivisionPanel.transform;
				SetFollowTarget(challengeDivisionPanel.transform);
				break;
			case ChallengeMenuType.Custom:
				menuModeInfo.challengeMenuType = ChallengeMenuType.Custom;
				SetActive(challengeCustomPanel);
				SetFollowTarget(challengeCustomPanel.transform);
				break;
			default: break;
		}
	}

	private void SwitchRecordMenu(RecordMenuType recordMenuType)
	{
		AllObjectDisable();

		switch (recordMenuType)
		{
			case RecordMenuType.Home:
				menuModeInfo.recordMenuType = RecordMenuType.Home;
				SetActive(recordHomePanel);
				SetFollowTarget(recordHomePanel.transform);
				break;
			case RecordMenuType.Basic:
				menuModeInfo.recordMenuType = RecordMenuType.Basic;
				SetActive(recordBasicPanel);
				SetFollowTarget(recordBasicPanel.transform);
				break;
			case RecordMenuType.Addition:
				menuModeInfo.recordMenuType = RecordMenuType.Addition;
				SetActive(recordAdditionPanel);
				SetFollowTarget(recordAdditionPanel.transform);
				break;
			case RecordMenuType.Subtraction:
				menuModeInfo.recordMenuType = RecordMenuType.Subtraction;
				SetActive(recordSubtractionPanel);
				SetFollowTarget(recordSubtractionPanel.transform);
				break;
			case RecordMenuType.Multiplication:
				menuModeInfo.recordMenuType = RecordMenuType.Multiplication;
				SetActive(recordMultiplicationPanel);
				SetFollowTarget(recordMultiplicationPanel.transform);
				break;
			case RecordMenuType.Division:
				menuModeInfo.recordMenuType = RecordMenuType.Division;
				SetActive(recordDivitionPanel);
				SetFollowTarget(recordDivitionPanel.transform);
				break;
			case RecordMenuType.Custom:
				menuModeInfo.recordMenuType = RecordMenuType.Custom;
				SetActive(recordCustomPanel);
				SetFollowTarget(recordCustomPanel.transform);
				break;
			default: break;
		}
	}

	private void SwitchSettingMenu(SettingMenuType settingMenuType)
	{
		AllObjectDisable();

		switch (settingMenuType)
		{
			case SettingMenuType.Home:
				menuModeInfo.settingMenuType = SettingMenuType.Home;
				SetActive(settingHomePanel);
				SetFollowTarget(settingHomePanel.transform);
				break;
			case SettingMenuType.Abacus:
				menuModeInfo.settingMenuType = SettingMenuType.Abacus;
				SetActive(settingAbacusPanel);
				SetFollowTarget(settingAbacusPanel.transform);
				break;
			case SettingMenuType.Control:
				menuModeInfo.settingMenuType = SettingMenuType.Control;
				SetActive(settingControlPanel);
				SetFollowTarget(settingControlPanel.transform);
				break;
			case SettingMenuType.Immersive:
				menuModeInfo.settingMenuType = SettingMenuType.Immersive;
				SetActive(settingImmersivePanel);
				SetFollowTarget(settingImmersivePanel.transform);
				break;
			case SettingMenuType.Sound:
				menuModeInfo.settingMenuType = SettingMenuType.Sound;
				SetActive(settingSoundPanel);
				SetFollowTarget(settingSoundPanel.transform);
				break;
#if false
			case SettingMenuType.Color:
				menuModeInfo.settingMenuType = SettingMenuType.Color;
				SetActive(settingColorPanel);
				SetFollowTarget(settingColorPanel.transform);
				break;
			case SettingMenuType.Language:
				menuModeInfo.settingMenuType = SettingMenuType.Language;
				SetActive(settingLanguagePanel);
				SetFollowTarget(settingLanguagePanel.transform);
				break;
#endif
			default: break;
		}
	}

	private void SetCountDownText()
	{
		questionPanelCountDownTextMeshPro.text = challengeModeInfo.count.ToString();
		SetActive(questionPanelCountDownImangeObject);

		if (challengeModeInfo.count > 0)
		{
			challengeModeInfo.count--;
			Invoke("SetCountDownText", 1f);
		}
		else
		{
			StartPlayMode();
		}
	}

	private void StartPlayMode()
	{
		challengeModeInfo.challengeModeState = ChallengeModeState.Playing;
		challengeModeInfo.currentNumberOfQuestion = 1;

		questionPanelCountDownTextMeshPro.text = "";
		questionPanelCountDownImangeObject.SetActive(false);

		ChallengeModeSetQuestion();

		SetActive(timerPanel);
		timerPanelController.ResetTimer();
	}

	private void SwitchObjectActive(GameMode gameMode)
	{
		AllObjectDisable();

		switch (gameMode)
		{
			case GameMode.MenuMode:
				MenuObjectActive();
				break;
			case GameMode.TutorialMode:
				TutorialObjectActive();
				break;
			case GameMode.ChallengeMode:
				ChallengeObjectActive();
				break;
			case GameMode.RecordMode:
				RecordObujectActive();
				break;
			case GameMode.SettingMode:
				SettingObjectActive();
				break;
			case GameMode.PrivacyPolicyMode:
				PrivacyPolicyObjectActive();
				break;
			default: break;
		}
	}

	private void MenuObjectActive()
	{
		SetActive(menuPanel);
		SetAbacus(true);
	}

	private void TutorialObjectActive()
	{
		SetAbacus(true);
		SetActive(tutorialBodyPanel);
	}

	private void ChallengeObjectActive()
	{
		SetAbacus(true);
		SetActive(tenkeyPanel);
		SetActive(questionPanel);
	}

	private void RecordObujectActive()
	{
		SetAbacus(true);
	}

	private void SettingObjectActive()
	{
	}

	private void PrivacyPolicyObjectActive()
	{
		SetActive(privacyPolicyPanel);
		SetAbacus(false);
	}

	private void AllObjectDisable()
	{
		timerPanel.SetActive(false);
		questionPanel.SetActive(false);
		completeDialog.SetActive(false);
		questionAbacusObject.SetActive(false);
		tenkeyBasicRead.SetActive(false);
		tenkeyPanel.SetActive(false);

		menuPanel.SetActive(false);

		tutorialPanel.SetActive(false);
		tutorialBodyPanel.SetActive(false);
		tutorialHand.SetActive(false);

		TutorialSetArrowAll(false);
		TutorialSetDigitLineAll(false);
		TutorialSetDigitArrowAll(false);

		challengeHomePanel.SetActive(false);
		challengeBasicPanel.SetActive(false);
		challengeAdditionPanel.SetActive(false);
		challengeSubtractionPanel.SetActive(false);
		challengeMultiplicationPanel.SetActive(false);
		challengeDivisionPanel.SetActive(false);
		challengeCustomPanel.SetActive(false);

		recordHomePanel.SetActive(false);
		recordBasicPanel.SetActive(false);
		recordAdditionPanel.SetActive(false);
		recordSubtractionPanel.SetActive(false);
		recordMultiplicationPanel.SetActive(false);
		recordDivitionPanel.SetActive(false);
		recordCustomPanel.SetActive(false);
		recordPanel.SetActive(false);

		settingHomePanel.SetActive(false);
		settingAbacusPanel.SetActive(false);
		settingControlPanel.SetActive(false);
		//settingColorPanel.SetActive(false);
		settingImmersivePanel.SetActive(false);
		settingSoundPanel.SetActive(false);
		//settingLanguagePanel.SetActive(false);

		privacyPolicyPanel.SetActive(false);

		dialogPanel.SetActive(false);
	}

	private void CheckGameMode()
    {
        switch (gameMode)
        {
			case GameMode.TutorialMode:
				CheckTutorialMode();
				break;
            case GameMode.ChallengeMode:
                CheckChallengeMode();
                break;
            default: break;
        }
    }

	private void CheckTutorialMode()
	{
        switch (tutorialModeInfo.tutorialModeState)
        {
            case TutorialModeState.Playing:
				PollingTutorialPlayingMode();
				break;
			case TutorialModeState.Waiting:
				PollingTutorialWaitingMode();
				break;
			default: break;
		}
	}

	private void PollingTutorialPlayingMode()
	{
		if (totalValue.value == tutorialModeInfo.answer)
		{
			CorrectEffect();

			TutorialSetNextSeq();
		}
	}

	private void PollingTutorialWaitingMode()
	{
	}

    private void CheckChallengeMode()
	{
        switch (challengeModeInfo.challengeModeState)
        {
			case ChallengeModeState.CountDown:
				PollingChallengeCountDownMode();
				break;
            case ChallengeModeState.Playing:
				PollingChallengePlayingMode();
				break;
			case ChallengeModeState.Waiting:
				PollingChallengeWaitingMode();
				break;
			case ChallengeModeState.Finish:
				PollingChallengeFinishMode();
				break;
			default: break;
		}
    }
	private void PollingChallengeCountDownMode()
	{
	}

	private void PollingChallengePlayingMode()
	{
		GetKeyPadValue();
		if (totalValue.value == challengeModeInfo.answer)
		{
			CorrectEffect();

			challengeModeInfo.challengeModeState = ChallengeModeState.Waiting;

			SetAnswer();

			if (challengeModeInfo.currentNumberOfQuestion < challengeModeInfo.challengeNumberOfQuestions)
			{
				Invoke("NextQuestion", 1f);
			}
			else
			{
				timerPanelController.StopTimer();
				Invoke("SetComplete", 1f);
			}
		}
	}

	private void SetAnswer()
	{
		SetQuesitonAbacus(false);
		questionTextMeshPro.font = LocalizationManager.Instance.GetLocalizedFont();
		questionTextMeshPro.text = LocalizationManager.Instance.GetText("challenge_answer_body") + " : " + challengeModeInfo.answer.ToString();
	}

	private void ClearAnswer()
	{
		SetQuesitonAbacus(true);
		questionTextMeshPro.text = "";
	}
	

	private void SetComplete()
	{
		timerPanel.SetActive(false);
		questionPanel.SetActive(false);
		SetActive(completeDialog);
		challengeModeInfo.completeTime = timerPanelController.timerText.text;
		completeDialogTime.text = challengeModeInfo.completeTime;

		challengeModeInfo.challengeModeState = ChallengeModeState.Finish;

		keyPadValue.ClearValue();
		keyPadValueBasicRead.ClearValue();
		UpdateProgressBar();

		CompleteEffect();

		SetRecord();
	}

	private void SetRecord()
	{
		int rank = 0;
		string highScore = "";
		OperationType operationType = OperationType.None;
		operationType = GetOperationType(challengeModeInfo.challengeType, challengeModeInfo.challengeBasicType, challengeModeInfo.challengeAdditionType);
		highScore =  DataManager.GetHighScore(operationType, challengeModeInfo.challengeDifficultyType, challengeModeInfo.challengeNumberOfQuestions);
		rank = DataManager.AddAndSaveRecord(operationType, challengeModeInfo.challengeDifficultyType, challengeModeInfo.challengeNumberOfQuestions, challengeModeInfo.completeTime);

		if (rank <= 1)
		{
			SetActive(completeDialogNewRecordObject);
			coimpleteDialogHighScoreTime.text = "";
		}
		else
		{
			completeDialogNewRecordObject.SetActive(false);
			coimpleteDialogHighScoreTime.font = LocalizationManager.Instance.GetLocalizedFont();
			coimpleteDialogHighScoreTime.text = LocalizationManager.Instance.GetText("challenge_highscore_body") + " : " + highScore;
		}
	}

	private OperationType GetOperationType(ChallengeType challengeType, ChallengeBasicType challengeBasicType, ChallengeAdditionType challengeAdditionType)
	{
		OperationType operationType = OperationType.None;

		switch (challengeType)
		{
			case ChallengeType.Basic:
				if (challengeBasicType == ChallengeBasicType.Read)
				{
					operationType = OperationType.Basic_Read;
				}
				else
				{
					operationType = OperationType.Basic_Write;
				}
				break;
			case ChallengeType.Addition:
				if (challengeAdditionType == ChallengeAdditionType.Normal)
				{
					operationType = OperationType.Addition_Normal;
				}
				else if (challengeAdditionType == ChallengeAdditionType.Chain)
				{
					operationType = OperationType.Addition_Flash;
				}
				else
				{
					operationType = OperationType.Addition_Chain;
				}
				break;
			case ChallengeType.Subtraction:
				operationType = OperationType.Subtraction;
				break;
			case ChallengeType.Multiplication:
				operationType = OperationType.Multiplication;
				break;
			case ChallengeType.Division:
				operationType = OperationType.Division;
				break;
			case ChallengeType.Custom:
				operationType = DataManager.GetCustomOperationType(challengeModeInfo.challengeCustomInfo);
				break;
			default: break;
		}
		return operationType;
	}

	private void PollingChallengeWaitingMode()
	{
	}

	private void NextQuestion()
	{
		challengeModeInfo.challengeModeState = ChallengeModeState.Playing;

		challengeModeInfo.currentNumberOfQuestion++;

		ChallengeModeSetQuestion();
	}

	private void CountDownEffect()
	{
		audioSources[(int)AudioType.CountDown].Stop();
		audioSources[(int)AudioType.CountDown].Play();
	}
	private void CorrectEffect()
	{
		audioSources[(int)AudioType.Correct].Stop();
		audioSources[(int)AudioType.Correct].Play();
	}

	private void CompleteEffect()
	{
		audioSources[(int)AudioType.Complete].Stop();
		audioSources[(int)AudioType.Complete].Play();
	}

	private void PollingChallengeFinishMode()
	{

	}

	private void ChallengeModeSetQuestion()
	{
		ArithmeticType arithmeticType = new ArithmeticType();

		switch (challengeModeInfo.challengeType)
		{
			case ChallengeType.Basic:
				arithmeticType = ArithmeticType.None;
				SetRandomNumbers(challengeModeInfo.challengeDifficultyType, arithmeticType);
				break;
			case ChallengeType.Addition:
				arithmeticType = ArithmeticType.Addition;
				SetRandomNumbers(challengeModeInfo.challengeDifficultyType, arithmeticType);
				break;
			case ChallengeType.Subtraction:
				arithmeticType = ArithmeticType.Subtraction;
				SetRandomNumbers(challengeModeInfo.challengeDifficultyType, arithmeticType);
				break;
			case ChallengeType.Multiplication:
				arithmeticType = ArithmeticType.Multiplication;
				SetRandomNumbers(challengeModeInfo.challengeDifficultyType, arithmeticType);
				break;
			case ChallengeType.Division:
				arithmeticType = ArithmeticType.Division;
				SetRandomNumbers(challengeModeInfo.challengeDifficultyType, arithmeticType);
				break;
			case ChallengeType.Custom:
				arithmeticType = GetRandomArithmeticType();
				SetRandomNumbers(challengeModeInfo.challengeDifficultyType, arithmeticType);
				break;
			default: break;
		}
		
		challengeModeInfo.arithmeticType = arithmeticType;
		SetQuestionPanel(arithmeticType);
		totalValue.SetTotalValue(0);
		keyPadValue.ClearValue();
		UpdateProgressBar();
	}

	private void SetQuestionPanel(ArithmeticType arithmeticType)
	{
		if (challengeModeInfo.challengeType == ChallengeType.Basic)
		{
			if (challengeModeInfo.challengeBasicType == ChallengeBasicType.Read)
			{
				keyPadValueBasicRead.ClearValue();
				ClearAnswer();
				questionAbacusTotalValue.SetTotalValue(challengeModeInfo.num1);
			}
			else
			{
				valueBasicWrite = "0";
				totalValue.SetTotalValue(0);
				questionTextMeshPro.text = challengeModeInfo.num2.ToString();
			}
		}
		else
		{
			questionTextMeshPro.text = challengeModeInfo.num1.ToString() + " " + GetArithmeticSymbol(arithmeticType) + " " + challengeModeInfo.num2.ToString();
		}
		questionNoTextMeshPro.font = LocalizationManager.Instance.GetLocalizedFont();
		questionNoTextMeshPro.text = LocalizationManager.Instance.GetText("challenge_question_body") + " " + challengeModeInfo.currentNumberOfQuestion.ToString();

	}

	private string GetArithmeticSymbol(ArithmeticType arithmeticType)
	{
		switch (arithmeticType)
		{
			case ArithmeticType.Addition:
				return "+";
			case ArithmeticType.Subtraction:
				return "-";
			case ArithmeticType.Multiplication:
				return "x";
			case ArithmeticType.Division:
				return "\u00F7"; //÷
			default: return "?";
		}
	}

	private ArithmeticType GetRandomArithmeticType()
	{
		int count = 0;
		ArithmeticType[] arithmeticTypes = new ArithmeticType[(int)ArithmeticType.Num];


		if (challengeModeInfo.challengeCustomInfo.add)
		{
			arithmeticTypes[count++] = ArithmeticType.Addition;
		}
		if (challengeModeInfo.challengeCustomInfo.sub)
		{
			arithmeticTypes[count++] = ArithmeticType.Subtraction;
		}
		if (challengeModeInfo.challengeCustomInfo.multi)
		{
			arithmeticTypes[count++] = ArithmeticType.Multiplication;
		}
		if (challengeModeInfo.challengeCustomInfo.divide)
		{
			arithmeticTypes[count++] = ArithmeticType.Division;
		}

		int random = UnityEngine.Random.Range(0, count);

		return arithmeticTypes[random];
	}

	private void SetRandomNumbers(ChallengeDifficultyType difficulty, ArithmeticType arithmeticType)
	{
		int result = 1;
		ArithmeticDigitRange digitRange = GetDigitRange(difficulty, arithmeticType);

		do {
			challengeModeInfo.num1 = GetRandomNumber(digitRange.num1.min, digitRange.num1.max);
			challengeModeInfo.num2 = GetRandomNumber(digitRange.num2.min, digitRange.num2.max);

			switch (arithmeticType)
			{
				case ArithmeticType.None:
					if (challengeModeInfo.challengeBasicType == ChallengeBasicType.Read)
					{
						result = challengeModeInfo.num1;
					}
					else
					{
						result = challengeModeInfo.num2;
					}
					break;
				case ArithmeticType.Addition:
					result = challengeModeInfo.num1 + challengeModeInfo.num2;
					break;
				case ArithmeticType.Subtraction:
					if (challengeModeInfo.num1 < challengeModeInfo.num2)
					{
						int tmp = challengeModeInfo.num1;
						challengeModeInfo.num2 = challengeModeInfo.num1;
						challengeModeInfo.num1 = tmp;
					}
					result = challengeModeInfo.num1 - challengeModeInfo.num2;
					break;
				case ArithmeticType.Multiplication:
					if (challengeModeInfo.num2 == 1)
					{
						challengeModeInfo.num2++;
					}
					result = challengeModeInfo.num1 * challengeModeInfo.num2;
					break;
				case ArithmeticType.Division:
				if (challengeModeInfo.num2 == 1)
					{
						challengeModeInfo.num2++;
					}
					challengeModeInfo.num1 -= challengeModeInfo.num1 % challengeModeInfo.num2;
					result = challengeModeInfo.num1 / challengeModeInfo.num2;
					break;
				default: break;
			}
		} while (!CheckResult(result));

		challengeModeInfo.answer = result;
	}

	private bool CheckResult(int num)
	{
		if ((num > 0) && (num < Math.Pow(10, abacusDigit - 1)))
		{
			return true;
		}
		else
		{
			return false;
		}
	}

	private int GetRandomNumber(int minDigit, int maxDigit)
	{
		int min = (int)Math.Pow(10, minDigit - 1);
		int max = (int)(Math.Pow(10, maxDigit) - 1);
		return UnityEngine.Random.Range(min, max + 1);
	}

	private ArithmeticDigitRange GetDigitRange(ChallengeDifficultyType difficulty, ArithmeticType arithmeticType)
	{
		ArithmeticDigitRange arithmeticDigitRange = new ArithmeticDigitRange();

		switch (arithmeticType)
		{
			case ArithmeticType.None:
				arithmeticDigitRange = GetBasicDigitRange(difficulty);
				break;
			case ArithmeticType.Addition:
				arithmeticDigitRange = GetAddDigitRange(difficulty);
				break;
			case ArithmeticType.Subtraction:
				arithmeticDigitRange = GetSubDigitRange(difficulty);
				break;
			case ArithmeticType.Multiplication:
				arithmeticDigitRange = GetMultiDigitRange(difficulty);
				break;
			case ArithmeticType.Division:
				arithmeticDigitRange = GetDivideDigitRange(difficulty);
				break;
			default: break;
		}
		return arithmeticDigitRange;
	}

	private ArithmeticDigitRange GetBasicDigitRange(ChallengeDifficultyType difficulty)
	{
		ArithmeticDigitRange arithmeticDigitRange = new ArithmeticDigitRange();
		switch (difficulty)
		{
			case ChallengeDifficultyType.Easy:
				arithmeticDigitRange = challengeModeInfo.digitRangeInfo.easy.basic;
				break;
			case ChallengeDifficultyType.Medium:
				arithmeticDigitRange = challengeModeInfo.digitRangeInfo.medium.basic;
				break;
			case ChallengeDifficultyType.Hard:
				arithmeticDigitRange = challengeModeInfo.digitRangeInfo.hard.basic;
				break;
			default: break;
		}
		return arithmeticDigitRange;
	}

	private ArithmeticDigitRange GetAddDigitRange(ChallengeDifficultyType difficulty)
	{
		ArithmeticDigitRange arithmeticDigitRange = new ArithmeticDigitRange();
		switch (difficulty)
		{
			case ChallengeDifficultyType.Easy:
				arithmeticDigitRange = challengeModeInfo.digitRangeInfo.easy.add;
				break;
			case ChallengeDifficultyType.Medium:
				arithmeticDigitRange = challengeModeInfo.digitRangeInfo.medium.add;
				break;
			case ChallengeDifficultyType.Hard:
				arithmeticDigitRange = challengeModeInfo.digitRangeInfo.hard.add;
				break;
			default: break;
		}
		return arithmeticDigitRange;
	}

	private ArithmeticDigitRange GetSubDigitRange(ChallengeDifficultyType difficulty)
	{
		ArithmeticDigitRange arithmeticDigitRange = new ArithmeticDigitRange();
		switch (difficulty)
		{
			case ChallengeDifficultyType.Easy:
				arithmeticDigitRange = challengeModeInfo.digitRangeInfo.easy.sub;
				break;
			case ChallengeDifficultyType.Medium:
				arithmeticDigitRange = challengeModeInfo.digitRangeInfo.medium.sub;
				break;
			case ChallengeDifficultyType.Hard:
				arithmeticDigitRange = challengeModeInfo.digitRangeInfo.hard.sub;
				break;
			default: break;
		}
		return arithmeticDigitRange;
	}

	private ArithmeticDigitRange GetMultiDigitRange(ChallengeDifficultyType difficulty)
	{
		ArithmeticDigitRange arithmeticDigitRange = new ArithmeticDigitRange();
		switch (difficulty)
		{
			case ChallengeDifficultyType.Easy:
				arithmeticDigitRange = challengeModeInfo.digitRangeInfo.easy.multi;
				break;
			case ChallengeDifficultyType.Medium:
				arithmeticDigitRange = challengeModeInfo.digitRangeInfo.medium.multi;
				break;
			case ChallengeDifficultyType.Hard:
				arithmeticDigitRange = challengeModeInfo.digitRangeInfo.hard.multi;
				break;
			default: break;
		}
		return arithmeticDigitRange;
	}

	private ArithmeticDigitRange GetDivideDigitRange(ChallengeDifficultyType difficulty)
	{
		ArithmeticDigitRange arithmeticDigitRange = new ArithmeticDigitRange();
		switch (difficulty)
		{
			case ChallengeDifficultyType.Easy:
				arithmeticDigitRange = challengeModeInfo.digitRangeInfo.easy.divide;
				break;
			case ChallengeDifficultyType.Medium:
				arithmeticDigitRange = challengeModeInfo.digitRangeInfo.medium.divide;
				break;
			case ChallengeDifficultyType.Hard:
				arithmeticDigitRange = challengeModeInfo.digitRangeInfo.hard.divide;
				break;
			default: break;
		}
		return arithmeticDigitRange;
	}

	private void GetKeyPadValue()
	{
		int value;

		if (challengeModeInfo.arithmeticType == ArithmeticType.None)
		{
			if (challengeModeInfo.challengeBasicType == ChallengeBasicType.Read)
			{
				value = keyPadValueBasicRead.GetValue();
			}
			else
			{
				return;
			}
		}
		else
		{
			value = keyPadValue.GetValue();
		}

		if (_keyPadValue != value)
		{
			_keyPadValue = value;
			totalValue.SetTotalValue(_keyPadValue);
		}
	}

	private void UpdateProgressBar()
	{
		float xPos;
		float progress = (float)challengeModeInfo.currentNumberOfQuestion / (float)challengeModeInfo.challengeNumberOfQuestions;

		SetActive(questionPanelProgressActive.gameObject);
		SetActive(questionPanelProgressInactive.gameObject);

		questionPanelProgressActive.sizeDelta = new Vector2(PROGRESS_BAR_WIDTH * progress, questionPanelProgressActive.sizeDelta.y);
		questionPanelProgressInactive.sizeDelta = new Vector2(PROGRESS_BAR_WIDTH * (1 - progress), questionPanelProgressInactive.sizeDelta.y);

		xPos = ((PROGRESS_BAR_WIDTH / 2) * progress) - (PROGRESS_BAR_WIDTH / 2);
		questionPanelProgressActive.anchoredPosition = new Vector2(xPos, questionPanelProgressActive.anchoredPosition.y);

		xPos = (PROGRESS_BAR_WIDTH / 2) * progress;
		questionPanelProgressInactive.anchoredPosition = new Vector2(xPos, questionPanelProgressInactive.anchoredPosition.y);
	}
}
