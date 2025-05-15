using NTDLS.WinFormsHelpers;
using SecureChat.Client.Forms;

namespace SecureChat.Client.Helpers
{
    /// <summary>
    /// Progress form used for multi-threaded progress reporting.
    /// </summary>
    public class ThemedProgressForm : IDisposable
    {
        /// <summary>
        /// Delegate used to initialize the form.
        /// </summary>
        /// <param name="form"></param>
        public delegate void InitializeForm(Form form);

        private const string LockObject = "FormProgress.Singleton.LockObject";

        private readonly FormThemedProgress _form;

        /// <summary>
        /// Delegate used for OnCancel event.
        /// </summary>
        public delegate void EventOnCancel(object sender, OnCancelInfo e);

        /// <summary>
        /// Cancel event parameter.
        /// </summary>
        public class OnCancelInfo
        {
            /// <summary>
            /// Set to true to cancel the cancel operation.
            /// </summary>
            public bool Cancel = false;
        }

        #region ~Ctor.

        /// <summary>
        /// Creates a new instance of the FormProgress which is used for multi-threaded progress reporting.
        /// </summary>
        public ThemedProgressForm(InitializeForm? initializeForm = null)
        {
            _form = new FormThemedProgress();

            initializeForm?.Invoke(_form);
        }

        /// <summary>
        /// Creates a new instance of the FormProgress which is used for multi-threaded progress reporting.
        /// </summary>
        public ThemedProgressForm(string title, InitializeForm? initializeForm = null)
        {
            _form = new FormThemedProgress();

            initializeForm?.Invoke(_form);

            _form.SetTitleText(title);
        }

        /// <summary>
        /// Creates a new instance of the FormProgress which is used for multi-threaded progress reporting.
        /// </summary>
        public ThemedProgressForm(string title, string header, InitializeForm? initializeForm = null)
        {
            _form = new FormThemedProgress();

            initializeForm?.Invoke(_form);

            _form.SetTitleText(title);
            _form.SetHeaderText(header);
        }

        /// <summary>
        /// Creates a new instance of the FormProgress which is used for multi-threaded progress reporting.
        /// </summary>
        public ThemedProgressForm(string title, string header, string body, InitializeForm? initializeForm = null)
        {
            _form = new FormThemedProgress();

            initializeForm?.Invoke(_form);

            _form.SetTitleText(title);
            _form.SetHeaderText(header);
            _form.SetBodyText(body);
        }

        #endregion

        #region Execution Helpers.

        /// <summary>
        /// Worker thread delegate.
        /// </summary>
        public delegate void NoParamWorkerWithVoid();

        /// <summary>
        /// Worker thread delegate.
        /// </summary>
        public delegate T NoParamWorkerWithResult<T>();

        /// <summary>
        /// Worker thread delegate.
        /// </summary>
        public delegate void WorkerWithVoid(ThemedProgressForm sender);

        /// <summary>
        /// Worker thread delegate.
        /// </summary>
        public delegate T WorkerWithResult<T>(ThemedProgressForm sender);

        /// <summary>
        /// Worker thread delegate.
        /// </summary>
        public delegate Task AsyncNoParamWorkerWithVoid();

        /// <summary>
        /// Worker thread delegate.
        /// </summary>
        public delegate Task<T> AsyncNoParamWorkerWithResult<T>();

        /// <summary>
        /// Worker thread delegate.
        /// </summary>
        public delegate Task AsyncWorkerWithVoid(ThemedProgressForm sender);

        /// <summary>
        /// Worker thread delegate.
        /// </summary>
        public delegate Task<T> AsyncWorkerWithResult<T>(ThemedProgressForm sender);

        /// <summary>
        /// Executes a workload in a separate thread while showing a progress form.
        /// Does all the work for you. Loads the form, waits on it, runs the worker, and closes the form when complete.
        /// </summary>
        public T? Execute<T>(NoParamWorkerWithResult<T> worker)
        {
            T? result = default;

            new Thread(() =>
            {
                WaitForVisible();
                result = worker();
                Close();
            }).Start();

            ShowDialog();

            return result;
        }

        /// <summary>
        /// Executes a workload in a separate thread while showing a progress form.
        /// Does all the work for you. Loads the form, waits on it, runs the worker, and closes the form when complete.
        /// </summary>
        public void Execute(NoParamWorkerWithVoid worker)
        {
            new Thread(() =>
            {
                WaitForVisible();
                worker();
                Close();
            }).Start();

            ShowDialog();
        }

        /// <summary>
        /// Executes a workload in a separate thread while showing a progress form.
        /// Does all the work for you. Loads the form, waits on it, runs the worker, and closes the form when complete.
        /// </summary>
        public T? Execute<T>(WorkerWithResult<T> worker)
        {
            T? result = default;

            new Thread(() =>
            {
                WaitForVisible();
                result = worker(this);
                Close();
            }).Start();

            ShowDialog();

            return result;
        }

        /// <summary>
        /// Executes a workload in a separate thread while showing a progress form.
        /// Does all the work for you. Loads the form, waits on it, runs the worker, and closes the form when complete.
        /// </summary>
        public void Execute(WorkerWithVoid worker)
        {
            new Thread(() =>
            {
                WaitForVisible();
                worker(this);
                Close();
            }).Start();

            ShowDialog();
        }


        //----------

        /// <summary>
        /// Executes a workload in a separate thread while showing a progress form.
        /// Does all the work for you. Loads the form, waits on it, runs the worker, and closes the form when complete.
        /// </summary>
        public async Task<T?> ExecuteAsync<T>(AsyncNoParamWorkerWithResult<T> worker)
        {
            var workerTask = Task.Run(async () =>
            {
                WaitForVisible();
                var workerResult = await worker();
                Close();
                return workerResult;
            });

            ShowDialog();

            return await workerTask;
        }

        /// <summary>
        /// Executes a workload in a separate thread while showing a progress form.
        /// Does all the work for you. Loads the form, waits on it, runs the worker, and closes the form when complete.
        /// </summary>
        public async Task ExecuteAsync(AsyncNoParamWorkerWithVoid worker)
        {
            var workerTask = Task.Run(async () =>
            {
                WaitForVisible();
                await worker();
                Close();
            });

            ShowDialog();

            await workerTask;
        }

        /// <summary>
        /// Executes a workload in a separate thread while showing a progress form.
        /// Does all the work for you. Loads the form, waits on it, runs the worker, and closes the form when complete.
        /// </summary>
        public async Task<T?> ExecuteAsync<T>(AsyncWorkerWithResult<T> worker)
        {
            var workerTask = Task.Run(async () =>
            {
                WaitForVisible();
                var workerResult = await worker(this);
                Close();
                return workerResult;
            });

            ShowDialog();

            return await workerTask;
        }

        /// <summary>
        /// Executes a workload in a separate thread while showing a progress form.
        /// Does all the work for you. Loads the form, waits on it, runs the worker, and closes the form when complete.
        /// </summary>
        public async Task ExecuteAsync(AsyncWorkerWithVoid worker)
        {
            var workerTask = Task.Run(async () =>
            {
                WaitForVisible();
                await worker(this);
                Close();
            });

            ShowDialog();

            await workerTask;
        }

        #endregion

        #region MessageBox.

        /// <summary>
        /// Invokes the form to show a message box.
        /// </summary>
        public DialogResult MessageBox(string message, string title, MessageBoxButtons buttons, MessageBoxIcon icon)
        {
            if (_form.InvokeRequired)
            {
                return _form.Invoke(new Func<DialogResult>(() => _form.InvokeMessageBox(message, title, buttons, icon)));
            }
            else
            {
                return System.Windows.Forms.MessageBox.Show(_form, message, title, buttons, icon);
            }
        }

        /// <summary>
        /// Invokes the form to show a message box.
        /// </summary>
        public DialogResult MessageBox(string message, string title, MessageBoxButtons buttons)
        {
            if (_form.InvokeRequired)
            {
                return _form.Invoke(new Func<DialogResult>(() => _form.InvokeMessageBox(message, title, buttons)));
            }
            else
            {
                return System.Windows.Forms.MessageBox.Show(_form, message, title, buttons);
            }
        }

        /// <summary>
        /// Invokes the form to show a message box.
        /// </summary>
        public DialogResult MessageBox(string message, string title)
        {
            if (_form.InvokeRequired)
            {
                return _form.Invoke(new Func<DialogResult>(() => _form.InvokeMessageBox(message, title)));
            }
            else
            {
                return System.Windows.Forms.MessageBox.Show(_form, message, title);
            }
        }

        #endregion

        /// <summary>
        /// Used by the user to set proprietary state information;
        /// </summary>
        public object? UserData { get; set; } = null;

        /// <summary>
        /// Indicates whether the form has been shown or not.
        /// </summary>
        public bool HasBeenShown { get => _form.HasBeenShown; }

        /// <summary>
        /// Indicates whether a cancel operation has been started.
        /// </summary>
        public bool IsCancelPending { get => _form.IsCancelPending; }

        /// <summary>
        /// Shows a new progress form and returns the result when its closed.
        /// </summary>
        public DialogResult ShowDialog(string titleText)
        {
            lock (LockObject)
            {
                _form.SetTitleText(titleText);
            }

            return _form.ShowDialog();
        }

        /// <summary>
        /// Shows a new progress form and returns the result when its closed.
        /// </summary>
        public DialogResult ShowDialog()
        {
            return _form.ShowDialog();
        }

        /// <summary>
        /// Shows a new progress form and returns the result when its closed.
        /// </summary>
        public DialogResult ShowDialog(string titleText, string headerText, string bodyText)
        {
            lock (LockObject)
            {
                _form.SetTitleText(titleText);
                _form.SetHeaderText(headerText);
                _form.SetBodyText(bodyText);
            }

            return _form.ShowDialog();
        }

        /// <summary>
        /// Shows a new progress form and returns the result when its closed.
        /// </summary>
        public DialogResult ShowDialog(string headerText, string bodyText)
        {
            lock (LockObject)
            {
                _form.SetHeaderText(headerText);
                _form.SetBodyText(bodyText);
            }

            return _form.ShowDialog();
        }

        /// <summary>
        /// Shows a new progress form and returns the result when its closed.
        /// </summary>
        public DialogResult ShowDialog(string headerText, EventOnCancel onCancel)
        {
            lock (LockObject)
            {
                _form.SetHeaderText(headerText);
                _form.OnCancel += onCancel;
                _form.SetCanCancel(true);
            }

            return _form.ShowDialog();
        }

        /// <summary>
        /// Shows a new progress form and returns the result when its closed.
        /// </summary>
        public DialogResult ShowDialog(string headerText, string bodyText, EventOnCancel onCancel)
        {
            lock (LockObject)
            {
                _form.OnCancel += onCancel;
                _form.SetHeaderText(headerText);
                _form.SetBodyText(bodyText);
                _form.SetCanCancel(true);
            }

            return _form.ShowDialog();
        }

        /// <summary>
        /// Waits for the form to become visible. This is typically done from within the thread that will be controlling the form.
        /// </summary>
        public void WaitForVisible()
        {
            while (true)
            {
                lock (LockObject)
                {
                    if (_form != null && _form.HasBeenShown == true)
                    {
                        break;
                    }
                }
                Thread.Sleep(10);
            }
        }

        /// <summary>
        /// Closes the form with the given dialog result in a thread safe manner.
        /// </summary>
        public void Close(DialogResult result)
            => _form.Close(result);

        /// <summary>
        /// Closes the form in a thread safe manner.
        /// </summary>
        public void Close()
            => _form.Close();

        /// <summary>
        /// Sets the header label text in a thread safe manner (this is not the title).
        /// </summary>
        public void SetHeaderText(string text)
            => _form.SetHeaderText(text);

        /// <summary>
        /// Sets the body label text in a thread safe manner (this is not the title).
        /// </summary>
        public void SetBodyText(string text)
            => _form.SetBodyText(text);

        /// <summary>
        /// Sets the form title text in a thread safe manner.
        /// </summary>
        public void SetTitleText(string text)
            => _form.SetTitleText(text);

        /// <summary>
        /// Sets the progress bar minimum value in a thread safe manner.
        /// </summary>
        public void SetProgressMinimum(int value)
            => _form.SetProgressMinimum(value);

        /// <summary>
        /// Sets the progress bar maximum value in a thread safe manner.
        /// </summary>
        public void SetProgressMaximum(int value)
            => _form.SetProgressMaximum(value);

        /// <summary>
        /// Increments the progress bar value in a thread safe manner.
        /// </summary>
        public void IncrementProgressValue()
            => _form.IncrementProgressValue();

        /// <summary>
        /// Sets the progress bar value in a thread safe manner.
        /// </summary>
        public void SetProgressValue(int value)
            => _form.SetProgressValue(value);

        /// <summary>
        /// Sets the progress bar style in a thread safe manner.
        /// </summary>
        public void SeProgressStyle(ProgressBarStyle value)
            => _form.SeProgressStyle(value);

        /// <summary>
        /// Enables or disabled cancelation support in a thread safe manner.
        /// </summary>
        public void SetCanCancel(bool value)
            => _form.SetCanCancel(value);

        public void Dispose()
        {
            _form.Dispose();
        }
    }
}
