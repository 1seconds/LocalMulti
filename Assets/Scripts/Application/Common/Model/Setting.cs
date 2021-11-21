using System.Collections.Generic;

public class SettingPref {
    public bool isDev = false;
    public bool termsAgreed = false;
}

public class Setting {
    public string language;
    public bool effectSound = true;
    public SettingPref pref;

    public Setting() {
        language = StringBundleService.GetLanguageCode();
        pref = new SettingPref();
    }
}