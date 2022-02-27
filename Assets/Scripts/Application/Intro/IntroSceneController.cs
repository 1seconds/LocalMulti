using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroSceneController : SceneControllerBase, ServiceStatePresenter {
    [SerializeField] private GameObject mainAnchor;
    [SerializeField] private GameObject alertAnchor;
    [SerializeField] private GameObject waitingAnchor;
    [SerializeField] private GameObject pushAnchor;
    [SerializeField] private IntroLogo logo;

    public override SceneType sceneType => SceneType.Intro;
    public override GameObject defaultUI => mainAnchor;

    private static bool logoDisplayed;

    public async void Start() {
        InitController();        
        Service.Initialize();
        ReadyPhase();
        
        await Ready();
        
        if (!logoDisplayed) {
            await logo.Display();
            logoDisplayed = true;
        }

        if (await Boot() == false) {
            SwitchScene(0).RunAsync();
            return;
        }

        TextLocale.RefreshAll();

        Service.ready = true;
        Service.quickServiceInit = false;
        SwitchScene(SceneType.Lobby).RunAsync();
    }

    private void ReadyPhase() {
        PersistenceUtil.CreateFolder(URL.instance.localRoot);
    }

    public void ShowServiceState(string key) {
    }

    private async Task Ready() {
        await Service.setting.Initialize(this);
        await Service.sb.Initialize(this);
    }
    
    private async Task<bool> Boot() {
        List<IService> services = new List<IService>();
        services.Add(Service.rule);
        services.Add(Service.unit);
        services.Add(Service.stage);
        services.Add(Service.skill);
        
        return await InitializeServices(services);
    }
    
    private async Task<bool> InitializeServices(List<IService> services) {
        foreach (IService service in services) {
            bool result = await service.Initialize(this);
            if (result == false) {
                Debug.LogError("Init services fail");
                return false;
            }
        }

        return true;
    }
}
