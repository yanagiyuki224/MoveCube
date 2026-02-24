using UnityEngine;
using UnityEngine.UI;
using DialogSystem;
public class SettingOption : DialogOption
{
}
public class SettingUI : DialogBase<SettingOption>
{
    public new static string prefabName = "Setting_Canvas";
    public new static GameObject prefab;
    public Slider SEslider;
    public Slider BGMslider;
    public float initialSEvolume;
    public float initialBGMvolume;
    public bool isChangeVolume;
    public static SettingUI SettingShow(SettingOption option)
    {
        if (prefab == null)
        {
            prefab = Resources.Load(prefabName) as GameObject;
        }

        GameObject obj = Instantiate(prefab);
        SettingUI Dlog = obj.GetComponent<SettingUI>();
        Dlog.UpdateContent(option);
        return Dlog;
    }
    public override void UpdateContent(SettingOption opt)
    {
        base.UpdateContent(opt);
    }
    private void Start()
    {
        SEslider.value = SoundManager.Instance.seVolume;
        BGMslider.value = SoundManager.Instance.bgmVolume;

        UpdateInitialVolume();

        SEslider.onValueChanged.AddListener(OnSEVolumeChanged);
        BGMslider.onValueChanged.AddListener(OnBGMVolumeChanged);
    }
    // SEのスライダーが動いた時だけ呼ばれる
    private void OnSEVolumeChanged(float value)
    {
        SoundManager.Instance.seVolume = value;
        SoundManager.Instance.UpdateVolume();
        Debug.Log($"SE Volume changed to: {value}");
        isChangeVolume=true;
        // ここで保存処理や音量反映を行う
    }

    // BGMのスライダーが動いた時だけ呼ばれる
    private void OnBGMVolumeChanged(float value)
    {
        SoundManager.Instance.bgmVolume = value;
        SoundManager.Instance.UpdateVolume();
        Debug.Log($"BGM Volume changed to: {value}");
        isChangeVolume=true;
    }
    public void DecisionSoundVolume()
    {
        isChangeVolume = false;
        UpdateInitialVolume();
    }
    void UpdateInitialVolume()
    {
        initialBGMvolume = SoundManager.Instance.bgmVolume;
        initialSEvolume = SoundManager.Instance.seVolume;
    }
    public override void Close()
    {
        if (isChangeVolume)
        {
            ResetVolume();
        }
        base.Close();
    }
    public void ResetVolume()
    {
        SoundManager.Instance.bgmVolume=initialBGMvolume;
        SoundManager.Instance.seVolume=initialSEvolume;
        SoundManager.Instance.UpdateVolume();

        SEslider.value = SoundManager.Instance.seVolume;
        BGMslider.value = SoundManager.Instance.bgmVolume;
    }
}
