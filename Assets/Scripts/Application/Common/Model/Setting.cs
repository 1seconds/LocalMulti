using System.Collections.Generic;

public class SettingPref {
    public bool isDev = false;
    public bool termsAgreed = false;
}

public class Setting {
    public string language;
    public bool effectSound = true;
    public SettingPref pref;

    public int unitId = 0;

    public Setting() {
        language = StringBundleService.GetLanguageCode();
        pref = new SettingPref();
    }

    public int GetUnitId() {
        unitId += 1;
        return unitId;
    }
}