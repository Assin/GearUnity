using System.Collections;

public abstract class AbstractCommand {
	public delegate void OnCommandCompleteDelegate(AbstractCommand command);
	public delegate void OnCommandErrorDelegate(AbstractCommand command);
	
	public OnCommandCompleteDelegate OnCommandComplete;
	public OnCommandErrorDelegate OnCommandError;

	public float delayInvokeComplete = 0f;
	/// <summary>
	/// 开始执行命令
	/// </summary>
	public abstract void Execute();

	protected virtual void InvokeComplete(){
		if(delayInvokeComplete == 0f)
		{
			if (OnCommandComplete != null) {
				OnCommandComplete(this);
			}
		}else{
			EnterFrameTimer.SetTimeOut((uint)(delayInvokeComplete * 1000), delegate() {
				if (OnCommandComplete != null) {
					OnCommandComplete(this);
				}
			});
		}
	}

	protected virtual void InvokeError(){
		if (OnCommandError != null) {
			OnCommandError(this);
		}
	}
}
