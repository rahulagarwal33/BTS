using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Net;

namespace SC
{
    public class TCPMessage
    {
        public StateObject stateObject;
        // Received data string.
        public int msgLen = 0;
        public int state = 0;
        public List<byte> bytes = new List<byte>();
        public delegate void MessageCompleteHandler(StateObject st, byte[] msg);
        public event MessageCompleteHandler MessageComplete;
        public TCPMessage(StateObject stOb)
        {
            stateObject = stOb;
        }
        static byte[] AddRange(byte[] target, byte[] itemArr, int startIndex, int endIndex)
        {
            byte[] result = new byte[target.Length + endIndex - startIndex];
            target.CopyTo(result, 0);
            Array.Copy(itemArr, startIndex, result, target.Length, endIndex - startIndex);
            return result;
        }
        public static TCPMessage wrap(byte[] data)
        {
            int len = data.Length;
            TCPMessage msg = new TCPMessage(null);
            msg.msgLen = len;
            byte[] lenBytes = BitConverter.GetBytes(IPAddress.HostToNetworkOrder(len));
            msg.bytes.AddRange(lenBytes);
            msg.bytes.AddRange(data);
            return msg;
        }
        public void addBytes(byte[] arrBytes, int startIndex, int endIndex)
        {
            if (endIndex <= startIndex)
                return;
            switch (state)
            {
                case 0:
                    {
                        if (bytes.Count + (endIndex - startIndex) >= sizeof(int))
                        {
                            int lenRequired = sizeof(int) - bytes.Count;
                            //bytes = AddRange(bytes, arrBytes, startIndex, startIndex + lenRequired);
                            bytes.AddRange(arrBytes.Skip(startIndex).Take(lenRequired));
                            if (BitConverter.IsLittleEndian)
                                bytes.Reverse();
                                //Array.Reverse(bytes.ToArray());
                            msgLen = BitConverter.ToInt32(bytes.ToArray(), 0);
                            state = 1;
                            addBytes(arrBytes, startIndex + lenRequired, endIndex);
                        }
                        else
                        {
                            bytes.AddRange(arrBytes.Skip(startIndex).Take(endIndex - startIndex));
                            //bytes = AddRange(bytes, arrBytes, startIndex, endIndex - startIndex);
                        }
                        break;
                    }
                case 1:
                    {
                        if (bytes.Count + (endIndex - startIndex) >= msgLen + sizeof(int))
                        {
                            int lenRequired = msgLen - (bytes.Count - sizeof(int));
                            //message complete
                            //bytes = AddRange(bytes, arrBytes, startIndex, startIndex + lenRequired);
                            bytes.AddRange(arrBytes.Skip(startIndex).Take(lenRequired));
                            byte[] msg = new byte[msgLen];
                            Array.Copy(bytes.ToArray(), sizeof(int), msg, 0, msgLen);
                            MessageComplete(stateObject, msg);
                            state = 0;
                            bytes.Clear();
                            msgLen = 0;
                            addBytes(arrBytes, startIndex + lenRequired, endIndex);
                        }
                        else
                        {
                            bytes.AddRange(arrBytes.Skip(startIndex).Take(endIndex - startIndex));
                            //bytes = AddRange(bytes, arrBytes, startIndex, endIndex - startIndex);
                        }
                        break;
                    }
            }
        }
    }
}
