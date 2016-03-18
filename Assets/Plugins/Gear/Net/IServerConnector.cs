using System;

public interface IServerConnector{
	void Init();
	string name{get;}
	void Request(IMessageBase messageBase);
	void Response(IMessageBase messageBase);
	void Close();

}

