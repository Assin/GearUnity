public class MessageDecodeBase : MessageBase{
	virtual protected void PackageMessageInfo(){

	}
	protected int ReadUnsignedShort(){
		return this._byteArray.ReadUnsignedShort();
	}

}