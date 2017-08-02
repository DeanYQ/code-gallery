using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
 
namespace ConsoleApplication17
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("start...");
 
            var task = DoTask();
 
            Console.WriteLine("end...");
 
            Console.WriteLine("get result start...");
 
            Console.WriteLine(task.Result.ToString(​));
 
            Console.WriteLine("get result end...");
 
        }
 
        [AsyncStateMachine(typeof(​DoActionAsyncStateMachine))]
        private static Task<int> DoTask()
        {
            var stateMachine = new DoActionAsyncStateMachine();
            stateMachine.builder = AsyncTaskMethodBuilder<int>.Create();
            stateMachine.state = -1;
            stateMachine.builder.Start(ref stateMachine);
            return stateMachine.builder.Task;
        }
    }
 
    public class DoActionAsyncStateMachine : IAsyncStateMachine
    {
        public int state;
        public AsyncTaskMethodBuilder<int> builder;
        private TaskAwaiter<int> awaiter;
        private Task<int> task;
 
        public void MoveNext()
        {
            int result;
            int _state = state;
 
            try
            {
                TaskAwaiter<int> _awaiter;
                if (_state ​!= 0)
                {
                    this.task = Task.Run((Func<int>)DoSome);
                    _awaiter = this.task.GetAwaiter();
                    if (!_awaiter.IsCompleted)
                    {
                        this.state = _state = 0;
                        this.awaiter = _awaiter;
                        DoActionAsyncStateMachine _stateMachine = this;
                        this.builder.AwaitUnsafeOnCompleted<​TaskAwaiter<int>, DoActionAsyncStateMachine>(ref _awaiter, ref _stateMachine);
                        return;
                    }
                }
                else
                {
                    _awaiter = this.awaiter;
                    this.awaiter = new TaskAwaiter<int>();
                    this.state = _state = -1;
                }
 
                _awaiter.GetResult();
                awaiter = new TaskAwaiter<int>();
                result = this.task.Result;
            }
            catch (Exception ex)
            {
                this.state = -2;
                this.builder.SetException(ex);
                return;
            }
 
            this.state = -2;
            this.builder.SetResult(result);
        }
 
        public void SetStateMachine(IAsyncStateMachine stateMachine)
        {
 
        }
 
        public int DoSome()
        {
            var delta = 0;
            var i = 0;
            while (i < 10)
	            {
	                i++;
	                Thread.Sleep(300);
	                delta += i;
	            }
	            return delta;
	        }
	    }
	}