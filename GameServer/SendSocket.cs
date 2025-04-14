using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace GameServer
{
    internal class SendSocket
    {
        //패킷의 크기가 1024바이트 이상이면 세그먼트로 분리해서 보내야 하기 때문에 필요한 변수임
        const int MaxPacketLength = 1024;
        public SendSocket()
        {
            
        }
        //패킷을 세그먼트로 분리해 만들어주는 함수
        public void SendPacket(byte[] data, Socket target)
        {

            int segmentCount = data.Length / MaxPacketLength;
            int dataSize = data.Length;
            if (segmentCount == 0)
            {
                ArraySegment<byte> segment = new ArraySegment<byte>(data, 0, data.Length);
                GameServer.SendPacketJobProcessor.Push(async () =>
                {
                    await SendAsync(segment, target);
                });
                return;
            }
            for (int i = 0; i < segmentCount; i++)
            {
                dataSize -= MaxPacketLength;
                ArraySegment<byte> segment;
                if (dataSize < MaxPacketLength)
                    segment = new ArraySegment<byte>(data, MaxPacketLength * i, dataSize);
                else
                    segment = new ArraySegment<byte>(data, MaxPacketLength * i, dataSize);
                GameServer.SendPacketJobProcessor.Push(async () =>
                {
                    await SendAsync(segment, target);
                });
            }
        }

        async Task SendAsync(ArraySegment<byte> packet, Socket target)
        {
            await target.SendAsync(packet);
        }
    }
}
