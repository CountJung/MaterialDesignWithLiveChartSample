using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

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

    //task queue concept test, using keyword, or call Dispose
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
                        //SyncUI(task);
                        if (task == null) return; //dispose end
                    }
                }
                if (task == null)
                    wh.WaitOne();         // No more tasks - wait for a signal
            }
        }
        //ui thread Sync test
        public static void SyncUI(Action? action)
        {
            if (action == null) return;
            if (Application.Current.Dispatcher.CheckAccess())
                action?.Invoke();
            else
                Application.Current.Dispatcher.BeginInvoke(action);
        }
    }

    //Test Concept
    public class SequenceTask
    {
        class TaskWorker
        {
            readonly EventWaitHandle waitHandle = new AutoResetEvent(false);
            readonly Func<Task> functionTask;
            readonly Task worker;
            public TaskWorker(Func<Task> act)
            {
                functionTask = act;
                worker = Task.Run(Work);
            }

            private async Task Work()
            {
                while (true)
                {
                    if (waitHandle.WaitOne(1))
                        break;
                    await functionTask.Invoke();
                }
            }
            public void Close()
            {
                waitHandle.Set();
                worker.Wait();
                waitHandle.Close();
            }
        }

        readonly object locker = new();
        readonly Dictionary<string, TaskWorker> SequenceDic = new();

        public void AddSequence(string seqName, Func<Task> act)
        {
            if (act is null) return;
            if (SequenceDic.ContainsKey(seqName))
                return;
            lock (locker) SequenceDic.Add(seqName, new TaskWorker(act));
        }

        public void RemoveSequence(string seqName)
        {
            if (SequenceDic.ContainsKey(seqName))
            {
                SequenceDic[seqName].Close();
                lock (locker) SequenceDic.Remove(seqName);
            }
        }
        public void ClearSequence()
        {
            foreach (var seq in SequenceDic.Values)
                seq.Close();
            lock (locker) SequenceDic.Clear();
        }
    }

    // blocking task queue, call Dispose for end
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
