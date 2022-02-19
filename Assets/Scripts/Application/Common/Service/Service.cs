using UnityEngine;
using System.Collections;
using System.Threading.Tasks;
using Newtonsoft.Json;

public enum Phase {
    Local = 0,
    Development,
    Test,
    Staging,
    Production
}

public delegate void OnNativeMessage(NativeResMessage message);

public class Service : SingletonGameObject<Service> {
    // client version set
    public static Phase phase { get; set; }
    public static Config config { get; set; }
    public static bool quickServiceInit { get; set; }
    
    // messages
    //-------------------------------------------------------------------------
    public static event OnNativeMessage onNativeMessage;

    // properties
    //-------------------------------------------------------------------------
    public static bool ready { get; set; }
    public static Version version { get; set; }

    // Services
    //-------------------------------------------------------------------------
    public static SceneController scene { get; set; }
    public static GoPooler goPooler => GoPooler.instance;
    public static StringBundleService sb => StringBundleService.instance;
    public static SettingService setting => SettingService.instance;
    public static SoundService sound => SoundService.instance;
    public static TimeService time => TimeService.instance;
    public static RuleService rule => RuleService.instance;
    public static LobbySceneService lobby => LobbySceneService.instance;

    public static StageService stage => StageService.instance;
    public static UnitService unit => UnitService.instance;

    // Callbacks
    //-------------------------------------------------------------------------
    private void Awake() {
        Application.logMessageReceived += HandleException;
        version = new Version(Application.version);
        DontDestroyOnLoad(gameObject);
    }

    private void InitLogHandler() {
        Debug.Log("SERVICE STARTED!");
    }
    
    public static void Initialize() {
        instance.InitLogHandler();
    }

    public static void OpenURL(string url) {
#if UNITY_WEBGL
        webGL.OpenURL(url);
#else
        Application.OpenURL(url);
#endif
    }

    private void HandleException(string condition, string stackTrace, LogType type) {
        if (type == LogType.Exception) {
            var text = condition + "\n" + stackTrace;
            if (text.Length > 512) {
                text = text.Substring(0, 512);
            }
            
            Debug.LogError(text);
        }
    }
    
    // APIs
    //-------------------------------------------------------------------------
    public static Coroutine Run(IEnumerator iterationResult) {
		return instance.StartCoroutine(iterationResult);
    }

    public static void Stop(Coroutine coroutine) {
        if (instance != null && coroutine != null) {
    		instance.StopCoroutine(coroutine);
        }
    }
}

public static class AsyncRunner { 
    public static async void RunAsync(this Task task) {
        await task;
    }
}
