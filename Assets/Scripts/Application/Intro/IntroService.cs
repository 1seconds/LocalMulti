using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

public class IntroService : Singleton<IntroService> {
	private Deserializer deserializer;
	private Serializer serializer;

	public IntroService() {
		var namingConvention = new CamelCaseNamingConvention();
		deserializer = new Deserializer(namingConvention: namingConvention, ignoreUnmatched: true);
		serializer = new Serializer(namingConvention: namingConvention);
	}
}
