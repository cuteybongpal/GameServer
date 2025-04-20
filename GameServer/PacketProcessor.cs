using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace GameServer
{
    internal class PacketProcessor
    {
        Dictionary<Socket, RecvData> packets = new Dictionary<Socket, RecvData>();
        public void ProcessPacket(byte[] packet, Socket sender)
        {
            //패킷 딕셔너리에 패킷을 보낸 소켓을 키값으로 가지는 RecvData클래스가 있는지 확인 없으면 새로 추가
            if (!packets.ContainsKey(sender))
                packets.Add(sender, new RecvData());

            RecvData data = packets[sender];
            data.AddPacket(packet);

            while (data.Packets.Count > 0)
            {
                Packet _packet = new Packet(data.Packets.Dequeue());
                int dataSize = BitConverter.ToInt32(_packet.DataSize, 0);
                if ((HeaderType)_packet.PacketHeader == HeaderType.Chatting)
                {
                    string stringData = "";
                    for (int i = 0; i < dataSize; i += 2)
                    {
                        byte[] charData = new byte[2];
                        Buffer.BlockCopy(_packet.Data, i, charData, 0, sizeof(char));
                        stringData += BitConverter.ToChar(charData); 
                    }
                    Console.WriteLine(stringData);
                }
            }
        }
    }
    public class RecvData
    {
        public Queue<byte[]> Packets = new Queue<byte[]>();
        byte[] packet = new byte[0];

        /// <summary>
        /// 패킷을 넣어준다. 패킷이 잘려서 오거나 합쳐져서 왔을 경우 분리 혹은 합쳐준다.
        /// </summary>
        /// <param name="packet"></param>
        public void AddPacket(byte[] packet)
        {
            byte[] _packet = new byte[this.packet.Length + packet.Length];
            int index = 0;
            Buffer.BlockCopy(this.packet, 0, _packet, index, this.packet.Length);
            index += this.packet.Length;
            Buffer.BlockCopy(packet, 0, _packet, index, packet.Length);
            packet = _packet;

            if (packet.Length < 5)
                return;

            while (true)
            {
                if (packet.Length < 5)
                    return;

                byte[] size = new byte[4];
                Buffer.BlockCopy(packet, 1, size, 0, 4);

                int dataSize = BitConverter.ToInt32(size, 0);
                if (packet.Length < dataSize + 5)
                    return;
                byte[] p = new byte[dataSize + 5];
                Buffer.BlockCopy(packet, 0, p, 0, dataSize + 5);
                Packets.Enqueue(p);

                byte[] temp = new byte[packet.Length - dataSize - 5];
                Buffer.BlockCopy(packet, dataSize + 5, temp, 0, temp.Length);
                packet = temp;
            }
        }
    }
}
