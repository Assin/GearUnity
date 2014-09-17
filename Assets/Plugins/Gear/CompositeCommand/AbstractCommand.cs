using System.Collections;

public abstract class AbstractCommand {
	public delegate void OnCommandCompleteDelegate(AbstractCommand command);
	public delegate void OnCommandErrorDelegate(AbstractCommand command);
	
	public event OnCommandCompleteDelegate OnCommandComplete;
	public event OnCommandErrorDelegate OnCommandError;

	/// <summary>
	/// 开始执行命令
	/// </summary>
	public abstract void Execute();

	protected virtual void InvokeComplete(){
		if (OnCommandComplete != null) {
			OnCommandComplete(this);
		}
	}

	protected virtual void InvokeError(){
		if (OnCommandError != null) {
			OnCommandError(this);
		}
	}
}
