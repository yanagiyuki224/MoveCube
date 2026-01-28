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
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    public override void Close()
    {
        SoundManager.Instance.seVolume = SEslider.value;
        SoundManager.Instance.bgmVolume = BGMslider.value;
        SoundManager.Instance.UpdateVolume();
        base.Close();
    }
}
