using System;
using System.Collections;
using System.Collections.Generic;

public class CompositeCommand : AbstractCommand{
	public delegate void OnCommandItemCompleteDelegate(AbstractCommand command);

	private Queue<AbstractCommand> _commands = new Queue<AbstractCommand>();
	private List<AbstractCommand> _executingCommands = new List<AbstractCommand>();
	private List<AbstractCommand> _executedCommands = new List<AbstractCommand>();
	private CompositeCommandMode _mode = CompositeCommandMode.SEQUENCE;
	private AbstractCommand _currentCommand = null;

	/// <summary>
	/// 在SEQUENCE模式下，完成一个子命令就会调用一下这个回调
	/// </summary>
	public OnCommandItemCompleteDelegate OnCommandItemComplete;
	public bool _isPlaying = false;
	public int _totalCount = 0;

	public CompositeCommandMode Mode {
		get { 
			return _mode; 
		}
		set {
			if (!_isPlaying) {
				_mode = value;
			}
		}
	}

	public bool IsPlaying {
		get { 
			return _isPlaying;
		}
	}
	/// <summary>
	/// 总命令的数量
	/// </summary>
	/// <value>The total count.</value>
	public int TotalCount{
		get{
			return _totalCount;
		}
	}
	/// <summary>
	/// 未执行的命令的数量
	/// </summary>
	/// <value>The left count.</value>
	public int LeftCount {
		get { 
			return _commands.Count;
		}
	}
	/// <summary>
	/// 执行过的命令的数量
	/// </summary>
	/// <value>The executed count.</value>
	public int ExecutedCount{
		get{
			return _executedCommands.Count;
		}
	}

	public AbstractCommand CurrentCommand {
		get { 
			return _currentCommand;
		}
	}

	public CompositeCommand(CompositeCommandMode mode){
		Mode = mode;
	}

	public virtual void AddCommand(AbstractCommand command){
		if (_commands == null) {
			_commands = new Queue<AbstractCommand>();
		}
		_commands.Enqueue(command);
	}

	/// <summary>
	/// 开始执行队列命令
	/// </summary>
	public override void Execute(){
		if (_isPlaying == true) {
			return;
		}
		if (_executingCommands == null) {
			_executingCommands = new List<AbstractCommand>();
		}
		if (_executedCommands == null) {
			_executedCommands = new List<AbstractCommand>();
		}
		_totalCount = _commands.Count;

		if (_commands != null && _commands.Count > 0) {
			_isPlaying = true;
			switch (_mode) {
			case CompositeCommandMode.SEQUENCE:
				ExecuteNextCommand();
				break;
			case CompositeCommandMode.PARALLEL:
				ExecuteCommandsInParallel();
				break;
			}
		} else {
			_isPlaying = false;
			InvokeComplete();
		}
	}

	protected virtual void ExecuteNextCommand(){
		if (_commands.Count > 0) {
			//取出最前面的命令
			AbstractCommand command = _commands.Dequeue();
			if (command != null) {
				ExecuteCommandItem(command);
			}
		} else {
			_isPlaying = false;
			InvokeComplete();
		}
	}

	protected virtual void ExecuteCommandItem(AbstractCommand command){
		_currentCommand = command;
		//放入执行中的命令中
		_executingCommands.Add(command);
		AddDelegateToCommand(command);
		command.Execute();
	}

	protected virtual void ExecuteCommandsInParallel(){
		while (_commands.Count > 0) {
			AbstractCommand command = _commands.Dequeue();
			if (command != null) {
				AddDelegateToCommand(command);
				command.Execute();
				//立即加入执行完成的列表中
				_executedCommands.Add(command);
			}
		}
		_isPlaying = false;
		InvokeComplete();
	}

	protected virtual void AddDelegateToCommand(AbstractCommand command){
		command.OnCommandComplete = OnExecutingCommandItemComplete;
		command.OnCommandError = OnExecutingCommandItemError;
	}

	protected virtual void RemoveDelegateToCommand(AbstractCommand command){
		command.OnCommandComplete = null;
		command.OnCommandError = null;
	}

	protected virtual void OnExecutingCommandItemComplete(AbstractCommand command){
		//移除回调函数
		RemoveDelegateToCommand (command);
		switch (_mode) {
		case CompositeCommandMode.SEQUENCE:
			//如果执行中的包含的话 就移除这个
			if (_executingCommands.Contains(command)) {
				_executingCommands.Remove(command);
			}
			_executedCommands.Add(command);
			//调用下完成单个命令的回调，让外部可以得知完成了一个，队列中完成的那个命令
			if(OnCommandItemComplete != null)
			{
				OnCommandItemComplete(command);
			}
			//继续执行下一个
			ExecuteNextCommand();
			break;
		}
	}

	protected virtual void OnExecutingCommandItemError(AbstractCommand command){
	}

	public void ClearAllCommand(){
		_isPlaying = false;
		_totalCount = 0;
		if(_commands != null)
			_commands.Clear();
	}
}
