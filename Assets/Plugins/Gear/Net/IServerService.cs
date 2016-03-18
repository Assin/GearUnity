public interface IServerService{
	void Init();
	void AddConnector(IServerInfoVO serverInfoVO);
	void StartupConnectorByName(string name);
	void CloseConnectorByName(string name);
	IServerConnector GetConnectorByName(string name);
	void Request(IMessageBase messageBase, string serverName = "");
	void Request(IMessageBase messageBase, string toURL, string serverName);
	void Response(IMessageBase messageBase, string serverName);

}