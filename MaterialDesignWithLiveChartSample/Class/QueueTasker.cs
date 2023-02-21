using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MaterialDesignWithLiveChartSample.Class
{
    // C# Threading origin example
    class ProducerConsumerQueue : IDisposable
    {
        EventWaitHandle _wh = new AutoResetEvent(false);
        Thread _worker;
        readonly object _locker = new object();
        Queue<string> _tasks = new Queue<string>();

        public ProducerConsumerQueue()
        {
            _worker = new Thread(Work);
            _worker.Start();
        }

        public void EnqueueTask(string task)
        {
            lock (_locker) _tasks.Enqueue(task);
            _wh.Set();
        }

        public void Dispose()
        {
            EnqueueTask(null!);     // Signal the consumer to exit.
            _worker.Join();         // Wait for the consumer's thread to finish.
            _wh.Close();            // Release any OS resources.
        }

        void Work()
        {
            while (true)
            {
                string? task = null;
                lock (_locker)
                    if (_tasks.Count > 0)
                    {
                        task = _tasks.Dequeue();
                        if (task == null) return;
                    }
                if (task != null)
                {
                    Console.WriteLine("Performing task: " + task);
                    Thread.Sleep(1000);  // simulate work...
                }
                else
                    _wh.WaitOne();         // No more tasks - wait for a signal
            }
        }
    }

    //keyword using for easily dispose
    public class QueueTasker : IDisposable
    {
        readonly EventWaitHandle wh = new AutoResetEvent(false);
        readonly Task worker;
        readonly object locker = new();
        readonly Queue<Action> tasks = new();
        Action? task = null;

        public QueueTasker()
        {
            worker = Task.Run(Work);
        }

        public void EnqueueTask(Action? task)
        {
            lock (locker) tasks.Enqueue(task!);
            wh.Set();
        }

        public void Dispose()
        {
            EnqueueTask(null);     // Signal the consumer to exit.
            worker.Wait();         // Wait for the consumer's task to finish.
            wh.Close();            // Release any OS resources.
            GC.SuppressFinalize(this);
        }

        void Work()
        {
            while (true)
            {
                task = null;
                lock (locker)
                {
                    if (tasks.Count > 0)
                    {
                        task = tasks.Dequeue();
                        task?.Invoke();
                        if (task == null) return; //dispose end
                    }
                }
                if (task == null)
                    wh.WaitOne();         // No more tasks - wait for a signal
            }
        }
    }
    public class SequenceTasker
    {
        readonly EventWaitHandle wh = new AutoResetEvent(false);
        readonly Task worker;
        readonly object locker = new();
        readonly Dictionary<string, Func<Task>> tasks = new();

        public SequenceTasker()
        {
            worker = Task.Run(Work);
        }

        public void AddSequence(string seqName, Func<Task> task)
        {
            lock (locker) tasks.TryAdd(seqName, task);
        }
        public void RemoveSequence(string seqName)
        {
            lock (locker) tasks.Remove(seqName);
        }
        public void ClearSequence()
        {
            lock (locker) tasks.Clear();
        }
        //call for end Work() Method
        public void Close()
        {
            ClearSequence();
            wh.Set();
            worker.Wait();         // Wait for the consumer's task to finish.
            wh.Close();            // Release any OS resources.
        }

        async Task Work()
        {
            while (true)
            {
                if (wh.WaitOne(1))
                    break;

                foreach (Func<Task> task in tasks.Values)
                {
                    await task.Invoke();
                }
            }
        }
    }

    public class PCQueue : IDisposable
    {
        class WorkItem
        {
            public readonly TaskCompletionSource<object> TaskSource;
            public readonly Action Action;
            public readonly CancellationToken? CancelToken;

            public WorkItem(
              TaskCompletionSource<object> taskSource,
              Action action,
              CancellationToken? cancelToken)
            {
                TaskSource = taskSource;
                Action = action;
                CancelToken = cancelToken;
            }
        }

        readonly BlockingCollection<WorkItem> taskQ = new();
        public int RemainTaskCount { get; private set; }
        public PCQueue(int workerCount)
        {
            // Create and start a separate Task for each consumer:
            for (int i = 0; i < workerCount; i++)
                Task.Factory.StartNew(Consume);
        }

        public void Dispose()
        {
            //call this method for end consume method - blocking collection signal set
            taskQ.CompleteAdding();
            GC.SuppressFinalize(this);
        }

        public Task EnqueueTask(Action action)
        {
            return EnqueueTask(action, null);
        }

        public Task EnqueueTask(Action action, CancellationToken? cancelToken)
        {
            var tcs = new TaskCompletionSource<object>();
            taskQ.Add(new WorkItem(tcs, action, cancelToken));
            return tcs.Task;
        }

        void Consume()
        {
            foreach (WorkItem workItem in taskQ.GetConsumingEnumerable())
            {
                RemainTaskCount = taskQ.Count;
                if (workItem.CancelToken.HasValue &&
                   workItem.CancelToken.Value.IsCancellationRequested)
                {
                    workItem.TaskSource.SetCanceled();
                }
                else
                {
                    try
                    {
                        workItem.Action();
                        workItem.TaskSource.SetResult(true);   // Indicate completion
                    }
                    catch (OperationCanceledException ex)
                    {
                        if (ex.CancellationToken == workItem.CancelToken)
                            workItem.TaskSource.SetCanceled();
                        else
                            workItem.TaskSource.SetException(ex);
                    }
                    catch (Exception ex)
                    {
                        workItem.TaskSource.SetException(ex);
                    }
                }
            }
        }
    }
}
