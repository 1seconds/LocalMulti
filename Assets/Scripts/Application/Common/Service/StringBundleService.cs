using UnityEngine;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

public class StringBundleService : Singleton<StringBundleService> {
    private Dictionary<string, Dictionary<string, string>> bundleSet;
    
    public class KoreanJosa {
        public int no { get; set; }
        public int js { get; set; }
        public int je { get; set; }
        public string withJong { get; set; }
        public string withoutJong { get; set; }
    }
    
    // properties
    //-------------------------------------------------------------------------
    public ServiceType type => ServiceType.StringBundle;
    public HashSet<string> supportLangs { get; private set; }
    public int mlRevision { get; private set; }
    public bool ready;

    public string language {
        get {
            if (Service.setting.value == null) {
                return "ko";
            }
            return Service.setting.value.language; 
        }
    }

    // apis
    //-------------------------------------------------------------------------
    public async Task<bool> Initialize(ServiceStatePresenter presenter) {
        string [] langs = {"en", "ko", "ja", "zh", "zh-hant", "ru", "id"};

        this.supportLangs = new HashSet<string>(langs);
        this.bundleSet = new Dictionary<string, Dictionary<string, string>>();
        LoadFromBundle("StringBundles/StringBundle");
        LoadFromCache();
        ready = true;
        return true;
    }

    private string GetValue(Dictionary<string, string> from, string key) {
        if (!from.ContainsKey(key)) {
            return "";
        }

        return from[key].Replace("\n", "\\n");
    }

    private bool LoadFromCache() {
        var text = PersistenceUtil.LoadTextFile(URL.instance.localLastSb);
        if (string.IsNullOrEmpty(text)) {
            return false;
        }

        try {
            var data = JsonConvert.DeserializeObject<StringBundleSet>(text);
            mlRevision = data.mlVersion;
            ReadBundles(data.bundles);
        } catch (Exception e) {
            Debug.LogError(e);
        }

        return false;
    }

    private void ReadBundles(List<StringBundle> bundles) {
        foreach (var bd in bundles) {
            AddString(bd.key, "ko", bd.ko);
            AddString(bd.key, "en", bd.en);
            AddString(bd.key, "ja", bd.ja);
            AddString(bd.key, "zh", bd.zh);
            AddString(bd.key, "ru", bd.ru);
            AddString(bd.key, "zh-hant", bd.zhHant);
            AddString(bd.key, "id", bd.id);
        }
    }

    public bool IsContainKey(string key) {
        if (bundleSet.ContainsKey(key.ToLower())){
            return true;
        }

        return false;
    }

    public void RefreshRule(string ml, int mlVersion) {
        if (string.IsNullOrEmpty(ml)) {
            return;
        }

        try {
            var data = new StringBundleSet();
            data.mlVersion = mlVersion;
            data.bundles = JsonConvert.DeserializeObject<List<StringBundle>>(ml);
            
            this.mlRevision = mlVersion;
            ReadBundles(data.bundles);

            var text = JsonConvert.SerializeObject(data);
            PersistenceUtil.SaveTextFile(URL.instance.localLastSb, text);
        } catch (Exception e) {
            Debug.LogError(e.ToString());
        }
    }

    public string Get(string key, params object[] values) {
        return Format(Get(key), values);
    }

    public List<KoreanJosa> ParseJosa(string text) {
        var res = new List<KoreanJosa>();
        var from = 0;
        while (true) {
            var start = text.IndexOf('{', from);
            if (start < 0) {
                break;
            }
            var end = text.IndexOf('}', start);
            if (end < 0) {
                break;
            }

            int no = 0;
            from = end + 1;

            try {
                no = Int32.Parse(text.Substring(start + 1, end - start - 1));
            } catch (Exception) {
                continue;
            }

            var js = text.IndexOf('[', end);
            if (js != end + 1) {
                continue;
            }

            var jc = text.IndexOf('|', js);
            if (jc < 0) {
                continue;
            }
            
            var je = text.IndexOf(']', js);
            if (je < 0) {
                continue;
            }

            from = je;
            var josa = new KoreanJosa();
            josa.no = no;
            josa.js = js;
            josa.je = je;
            josa.withJong = text.Substring(js + 1, jc - js - 1);
            josa.withoutJong = text.Substring(jc + 1, je - jc - 1);
            res.Add(josa);
        }

        return res;
    }

    public string PreprocessJosa(string text, params object[] values) {
        var res = ParseJosa(text);
        var sb = new StringBuilder();
        var from = 0;
        foreach (var josa in res) {
            sb.Append(text.Substring(from, josa.js - from));
            if (HasJong(values[josa.no].ToString())) {
                sb.Append(josa.withJong);
            } else {
                sb.Append(josa.withoutJong);
            }

            from = josa.je + 1;
        }
        
        sb.Append(text.Substring(from));
        return sb.ToString();
    }

    public string Format(string format, params object[] values) {
        try {
            if (language == "ko") {
                format = PreprocessJosa(format, values);
            }
            return string.Format(format, values);
        } catch (Exception e) {
            Debug.LogError(e.ToString() + ": " + format);
        }

        return "";
    }

    public string Get(string key) {
        if (key == null) {
            return "";
        }
        
        key = key.ToLower();
        if (bundleSet.ContainsKey(key) == false ){
            return key;
        }

        var container = bundleSet[key];
        if (container.ContainsKey(language)) {
            return container[language];
        }

        // default language1
        if (container.ContainsKey("en")) {
            return container["en"];
        }

        // default language2
        if (container.ContainsKey("ko")) {
#if UNITY_EDITOR            
            return "T@" + container["ko"];
#else
            return container["ko"];
#endif
        }
        
        return key;
    }

    public string GetWithEeGa(string key) {
        return GetWithEeGaValue(Get(key));
    }

    public string GetWithEeGaValue(string value) {
        if (language != "ko") {
            return value;
        }

        if (HasJong(value)) {
            return value + Get("korean.ee");
        }

        return value + Get("korean.ga");
    }


    public string GetWithEulReulValue(string value) {
        if (language != "ko") {
            return value;
        }

        if (HasJong(value)) {
            return value + Get("korean.eul");
        }

        return value + Get("korean.reul");
    }

    public string GetWithRoEuroValue(string value) {
        if (language != "ko") {
            return value;
        }

        int jong = GetJong(value);
        if (jong <= 0 || jong == 8) {
            return value + Get("korean.ro");
        }

        return value + Get("korean.euro");
    }

    public string GetWithEulReul(string key) {
        return GetWithEulReulValue(Get(key));
    }

    public string GetWithRoEuro(string key) {
        return GetWithRoEuroValue(Get(key));
    }

    public bool HasJong(string text) {
        return GetJong(text) > 0;
    }

    private int GetJong(string text) {
        if (text.Length > 0) {
            char last = text[text.Length - 1];
            int hangul = last - 0xAC00;
            return (hangul % 0x001C);
        }

        return 0;
    }

    // private methods
    //-------------------------------------------------------------------------
    private void AddString(string key, string lang, string value) {
        if (value == null) {
            return;
        }

        if (key == null) {
            Debug.LogError("null key:" + value);
            return;
        }

        value = value.Replace("\\n","\n");
        key = key.ToLower();
        if (value.Length == 0) {
            return;
        }

        if (bundleSet.ContainsKey(key) == false) {
            bundleSet.Add(key, new Dictionary<string, string>());
        }

        supportLangs.Add(lang);        
        var container = bundleSet[key];
        if (container.ContainsKey(lang) == false) {
            container.Add(lang, value);
        }
        else {
            container[lang] = value;
            //Debug.LogError(key + ", " + variable);
        }
    }

    private void LoadFromBundle(string path) {
        TextAsset textAsset = (TextAsset) Resources.Load(path, typeof(TextAsset));
        if (textAsset == null) {
            Debug.LogError("Can not load string bundle.");
            return;
        }

        LoadTsv(textAsset.text);
    }

    private void LoadTsv(string text) {
        var parser = new CsvParser();
        parser.Parse(text, "\t", StringSplitOptions.None);
        for (int index = 0; index < parser.Count; index++) {
            var row = parser.GetRow(index);
            var key = row.NextString();
            var ko = row.NextString();
            var en = row.NextString();
            var ja = row.NextString();
            var zh = row.NextString();
            var ru = row.NextString();
            var zhHant = row.NextString();
            var id = row.NextString();

            AddString(key, "ko", ko);
            AddString(key, "en", en);
            AddString(key, "ja", ja);
            AddString(key, "zh", zh);
            AddString(key, "ru", ru);
            AddString(key, "zh-hant", zhHant);
            AddString(key, "id", id);
        }
    }

    public static string GetLanguageCode() {
        return ToLanguageCode(Application.systemLanguage);
    }

    public string GetTextFor(string lang) {
        switch (lang) {
            case "af": return "Afrikaans";
            case "ar": return "Arabic";
            case "eu": return "Basque";
            case "be": return "Belarusian";
            case "bg": return "Bulgarian";
            case "ca": return "Catalan";
            case "zh": return "Chinese(Simplified)";
            case "zh-hans": return "Chinese(Simplified)";
            case "zh-hant": return "Chinese(Traditional)";
            case "cs": return "Czech";
            case "da": return "Danish";
            case "nl": return "Dutch";
            case "en": return "English";
            case "et": return "Estonian";
            case "fo": return "Faroese";
            case "fi": return "Finnish";
            case "fr": return "French";
            case "de": return "German";
            case "el": return "Greek";
            case "he": return "Hebrew";
            case "is": return "Icelandic";
            case "id": return "Indonesian";
            case "ja": return "Japanese";
            case "ko": return "Korean";
            case "lv": return "Latvian";
            case "lt": return "Lithuanian";
            case "no": return "Norwegian";
            case "pl": return "Polish";
            case "pt": return "Portuguese";
            case "ro": return "Romanian";
            case "ru": return "Russian";
            case "hr": return "SerboCroatian";
            case "sk": return "Slovak";
            case "sl": return "Slovenian";
            case "es": return "Spanish";
            case "sv": return "Swedish";
            case "th": return "Thai";
            case "tr": return "Turkish";
            case "uk": return "Ukrainian";
            case "vi": return "Vietnamese";
            case "hu": return "Hungarian";
        }   
        
        return "Unknown";
    }

    public static string ToLanguageCode(SystemLanguage language) {
        switch (language) {
            case SystemLanguage.Afrikaans: return "af";
            case SystemLanguage.Arabic: return "ar";
            case SystemLanguage.Basque: return "eu";
            case SystemLanguage.Belarusian: return "be";
            case SystemLanguage.Bulgarian: return "bg";
            case SystemLanguage.Catalan: return "ca";
            case SystemLanguage.Chinese: return "zh";
            case SystemLanguage.ChineseSimplified: return "zh-hans";
            case SystemLanguage.ChineseTraditional: return "zh-hant";
            case SystemLanguage.Czech: return "cs";
            case SystemLanguage.Danish: return "da";
            case SystemLanguage.Dutch: return "nl";
            case SystemLanguage.English: return "en";
            case SystemLanguage.Estonian: return "et";
            case SystemLanguage.Faroese: return "fo";
            case SystemLanguage.Finnish: return "fi";
            case SystemLanguage.French: return "fr";
            case SystemLanguage.German: return "de";
            case SystemLanguage.Greek: return "el";
            case SystemLanguage.Hebrew: return "he";
            case SystemLanguage.Icelandic: return "is";
            case SystemLanguage.Indonesian: return "id";
            case SystemLanguage.Japanese: return "ja";
            case SystemLanguage.Korean: return "ko";
            case SystemLanguage.Latvian: return "lv";
            case SystemLanguage.Lithuanian: return "lt";
            case SystemLanguage.Norwegian: return "no";
            case SystemLanguage.Polish: return "pl";
            case SystemLanguage.Portuguese: return "pt";
            case SystemLanguage.Romanian: return "ro";
            case SystemLanguage.Russian: return "ru";
            case SystemLanguage.SerboCroatian: return "hr";
            case SystemLanguage.Slovak: return "sk";
            case SystemLanguage.Slovenian: return "sl";
            case SystemLanguage.Spanish: return "es";
            case SystemLanguage.Swedish: return "sv";
            case SystemLanguage.Thai: return "th";
            case SystemLanguage.Turkish: return "tr";
            case SystemLanguage.Ukrainian: return "uk";
            case SystemLanguage.Vietnamese: return "vi";
            case SystemLanguage.Hungarian: return "hu";
            case SystemLanguage.Unknown: return "en";
        }

        return "en";
    }
    
    public static SystemLanguage ToSystemLanguage(string language) {
        switch (language) {
            case "af": return SystemLanguage.Afrikaans;
            case "ar": return SystemLanguage.Arabic;
            case "eu": return SystemLanguage.Basque;
            case "be": return SystemLanguage.Belarusian;
            case "bg": return SystemLanguage.Bulgarian;
            case "ca": return SystemLanguage.Catalan;
            case "zh": return SystemLanguage.Chinese;
            case "zh-hans": return SystemLanguage.ChineseSimplified;
            case "zh-hant": return SystemLanguage.ChineseTraditional;
            case "cs": return SystemLanguage.Czech;
            case "da": return SystemLanguage.Danish;
            case "nl": return SystemLanguage.Dutch;
            case "en": return SystemLanguage.English;
            case "et": return SystemLanguage.Estonian;
            case "fo": return SystemLanguage.Faroese;
            case "fi": return SystemLanguage.Finnish;
            case "fr": return SystemLanguage.French;
            case "de": return SystemLanguage.German;
            case "el": return SystemLanguage.Greek;
            case "he": return SystemLanguage.Hebrew;
            case "is": return SystemLanguage.Icelandic;
            case "id": return SystemLanguage.Indonesian;
            case "ja": return SystemLanguage.Japanese;
            case "ko": return SystemLanguage.Korean;
            case "lv": return SystemLanguage.Latvian;
            case "lt": return SystemLanguage.Lithuanian;
            case "no": return SystemLanguage.Norwegian;
            case "pl": return SystemLanguage.Polish;
            case "pt": return SystemLanguage.Portuguese;
            case "ro": return SystemLanguage.Romanian;
            case "ru": return SystemLanguage.Russian;
            case "hr": return SystemLanguage.SerboCroatian;
            case "sk": return SystemLanguage.Slovak;
            case "sl": return SystemLanguage.Slovenian;
            case "es": return SystemLanguage.Spanish;
            case "sv": return SystemLanguage.Swedish;
            case "th": return SystemLanguage.Thai;
            case "tr": return SystemLanguage.Turkish;
            case "uk": return SystemLanguage.Ukrainian;
            case "vi": return SystemLanguage.Vietnamese;
            case "hu": return SystemLanguage.Hungarian;
        }

        return SystemLanguage.English;
    }    
}
