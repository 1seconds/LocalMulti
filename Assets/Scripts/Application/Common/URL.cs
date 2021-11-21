using UnityEngine;

public class URL : Singleton<URL> {
    // local path
    //-------------------------------------------------------------------------
    public string localRoot => Application.persistentDataPath + "/" + Service.phase;
    public string localSetting => Application.persistentDataPath + "/setting";
    public string localNotice => Application.persistentDataPath + "/notice";
    public string localRule => localRoot + "/rule";
    public string localLastSb => localRoot + "/sb";
}