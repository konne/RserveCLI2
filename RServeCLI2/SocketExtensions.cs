//-----------------------------------------------------------------------
// Original work Copyright (c) 2015, Atif Aziz
// All rights reserved.
//-----------------------------------------------------------------------

namespace RserveCLI2
{
    using System;
    using System.Net;
    using System.Net.Sockets;
    using System.Threading.Tasks;

    static class SocketExtensions
    {
        public static Task<int> SendAsync(this Socket socket, byte[] buffer) =>
            SendAsync(socket, buffer, buffer.Length);

        public static Task<int> SendAsync(this Socket socket, byte[] buffer, int size) =>
            SendAsync(socket, buffer, 0, size);

        public static Task<int> SendAsync(this Socket socket, byte[] buffer, int offset, int size) =>
            SendAsync(socket, buffer, offset, size, SocketFlags.None);

        public static Task<int> SendAsync(this Socket socket,
            byte[] buffer, int offset, int size, SocketFlags socketFlags)
        {
            if (socket == null) throw new ArgumentNullException(nameof(socket));

            var tcs = new TaskCompletionSource<int>();
            socket.BeginSend(buffer, offset, size, socketFlags, ar =>
            {
                try { tcs.TrySetResult(socket.EndReceive(ar)); }
                catch (Exception e) { tcs.TrySetException(e);  }
            }, state: null);
            return tcs.Task;
        }

        public static Task<int> ReceiveAsync(this Socket socket, byte[] buffer) =>
            socket.ReceiveAsync(buffer, buffer.Length);

        public static Task<int> ReceiveAsync(this Socket socket, byte[] buffer, int size) =>
            socket.ReceiveAsync(buffer, 0, size, SocketFlags.None);

        public static Task<int> ReceiveAsync(this Socket socket,
            byte[] buffer, int offset, int size, SocketFlags socketFlags)
        {
            if (socket == null) throw new ArgumentNullException(nameof(socket));

            var tcs = new TaskCompletionSource<int>();
            socket.BeginReceive(buffer, offset, size, socketFlags, ar =>
            {
                try { tcs.TrySetResult(socket.EndReceive(ar)); }
                catch (Exception e) { tcs.TrySetException(e);  }
            }, state: null);
            return tcs.Task;
        }

        public static Task ConnectAsync(this Socket socket, EndPoint endPoint)
        {
            if (socket == null) throw new ArgumentNullException(nameof(socket));

            var tcs = new TaskCompletionSource<int>();
            socket.BeginConnect(endPoint, iar =>
            {
                try { socket.EndConnect(iar); tcs.TrySetResult(0); }
                catch (Exception e) { tcs.TrySetException(e); }
            }, state: null);
            return tcs.Task;
        }
    }
}