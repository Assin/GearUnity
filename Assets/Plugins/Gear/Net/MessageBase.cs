using System;

public class MessageBase : IMessageBase{
	protected int _messageID;
	protected string _messageType;
	protected int _messageLength;
	protected int _compressedLength;
	protected ByteArray _byteArray = new ByteArray();

	public int messageID {
		get {
			return _messageID;
		}
	}

	public ByteArray byteArray {
		get {
			return _byteArray;
		}
		set {
			_byteArray = value;
		}
	}

	public override string ToString(){
		return string.Format("[MessageBase: _messageID={0}, _messageType={1}, _messageLength={2}, _compressedLength={3}]", _messageID, _messageType, _messageLength, _compressedLength);
	}
	
}
