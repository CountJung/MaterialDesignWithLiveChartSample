using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MaterialDesignWithLiveChartSample.Class
{
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
                if(task == null)
                    wh.WaitOne();         // No more tasks - wait for a signal
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
                RemainTaskCount=taskQ.Count;
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
