using System.Net;
using System.Net.Sockets;

public class GSocket{
	public string ip = "";
	public int port = 0;
	private IPAddress ipAddress;
	private Socket socket;
	public void Init(){
		ipAddress = IPAddress.Parse(ip);
		socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
	}


}