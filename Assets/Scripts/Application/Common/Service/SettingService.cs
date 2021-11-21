using System.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;

public delegate void SettingEvent(Setting value);

public class SettingService : Singleton<SettingService>, IService {
    public event SettingEvent onSettingUpdate;

    public ServiceType type => ServiceType.Setting;
    public Setting value { get; internal set; }

    public async Task<bool> Initialize(ServiceStatePresenter presenter) {
        Load();
        return true;
    }

    public void Sync() {
        Save();
        onSettingUpdate?.Invoke(value);
    }

    private void Load() {
        value = LocalSerializer.instance.Load<Setting>(URL.instance.localSetting);
        if (value == null) {
            value = new Setting();
        }

        if (value.pref == null) {
            value.pref = new SettingPref();
        }
    }

    private void Save() {
        LocalSerializer.instance.Save(URL.instance.localSetting, value);
    }
}