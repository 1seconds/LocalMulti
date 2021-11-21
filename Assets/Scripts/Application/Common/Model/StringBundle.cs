using System.Collections.Generic;

public class StringBundle {
    public string key;
    public string ko;
    public string en;
    public string ja;
    public string ru;
    public string zh;
    public string zhHant;
    public string id;
}

public class StringBundleSet {
    public int mlVersion;
    public List<StringBundle> bundles;
}