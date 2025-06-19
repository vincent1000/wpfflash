using System;
using System.Collections.Generic;
using System.IO.Pipes;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp3
{
    public class NamePipeClient
    {
        private const string TAG = nameof(NamePipeClient);
        private const int _maxBytesNum = 10;
        private const int _maxConnectInterval = 3000;
        private string _pipeName = "MuteAlertPipe";
        private CancellationTokenSource? _cts;
        public event Action<char[], int>? DataReceived;
        private NamedPipeClientStream? _pipeClient;
        public async Task StartListening()
        {
            _cts = new CancellationTokenSource();

            while (!_cts.IsCancellationRequested)
            {
                _pipeClient = new NamedPipeClientStream(
                    ".",
                    _pipeName,
                    PipeDirection.InOut,
                    PipeOptions.Asynchronous);

                try
                {
                    //Logger.Instance.Debug(TAG, $"Before connecting NamePipe Server: {_pipeName}");
                    await _pipeClient.ConnectAsync(_cts.Token);
                    //Logger.Instance.Debug(TAG, "Connect NamePipe Server Successfully");

                    using (var reader = new StreamReader(_pipeClient))
                    {
                        char[] buffer = new char[_maxBytesNum];

                        while (!_cts.IsCancellationRequested)
                        {
                            int bytesRead = await reader.ReadAsync(buffer, 0, buffer.Length);
                            if (bytesRead == 0)
                            {
                                //Logger.Instance.Debug(TAG, $"NamePipe Server: {_pipeName} Disconnect");
                                break;
                            }
                            DataReceived?.Invoke(buffer, bytesRead);
                        }
                    }
                }
                catch (OperationCanceledException)
                {
                    break;
                }
                catch (Exception ex)
                {
                    //Logger.Instance.Error(TAG, $"NamePipe Error: {ex.Message}");
                    if (!_cts.IsCancellationRequested)
                    {
                        await Task.Delay(_maxConnectInterval, _cts.Token);
                    }
                }
                finally
                {
                    if (_pipeClient != null)
                    {
                        _pipeClient.Dispose();
                        _pipeClient = null;
                    }
                }
            }
        }

        public void StopListening()
        {
            _cts?.Cancel();
            _pipeClient?.Close();
            _pipeClient?.Dispose();
        }
        public async Task<bool> SendMessageAsync(byte[] message)
        {
            if (_pipeClient == null || !_pipeClient.IsConnected)
            {
                //Logger.Instance.Error(TAG, "Pipe client is not connected");
                return false;
            }

            try
            {
                await _pipeClient.WriteAsync(message, 0, message.Length).ConfigureAwait(false);
                await _pipeClient.FlushAsync().ConfigureAwait(false);
                return true;
            }
            catch (Exception ex)
            {
                //Logger.Instance.Error(TAG, $"Pipe Send Err: {ex.Message}");
                return false;
            }
        }
    }
}
