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
            data.AccemblePacket(packet); //패킷 조립
            if (!data.IsPacketCompleted) //패킷이 잘린 부분이 없이 조립이 되어있지 않으면 함수 종료
                return;
            byte[] AccembledPacket = data.Packet; //조립한 패킷을 가져옴
            //todo : 패킷 처리
        }
    }
    public class RecvData
    {
        byte[] data;
        int index = 0;
        /// <summary>
        /// 패킷이 잘린 부분 없이 조립이 되어있는지 여부
        /// </summary>
        public bool IsPacketCompleted
        {
            get 
            {
                if (data == null)
                    return false;
                if (data.Length < 5)
                    return false;
                byte[] dataSize = new byte[4];
                Buffer.BlockCopy(data, 1, dataSize, 0, 4);
                int size = BitConverter.ToInt32(dataSize, 0);
                return data.Length == 5 + size;
            }
        }
        /// <summary>
        /// 조립된 패킷
        /// 잘린 부분이 있으면 data를 null로 만들어 주지 않음
        /// 잘린 부분이 없으면 data 바이트 배열을 다시 사용하기 위해 data에 null을 넣어줌
        /// </summary>
        public byte[] Packet 
        {
            get 
            {
                if (!IsPacketCompleted)
                    return null;
                byte[] CopiedPacket = new byte[data.Length];
                Buffer.BlockCopy(data, 0, CopiedPacket, 0, data.Length);
                data = null;
                index = 0;
                return CopiedPacket;
            }
        }
        /// <summary>
        /// 패킷을 조립하는 함수
        /// </summary>
        /// <param name="packet">Recieve로 받아온 패킷</param>
        public void AccemblePacket(byte[] packet)
        {
            //data가 null일 경우 패킷의 길이만큼 배열의 길이를 넣어줌
            if (data == null)
                data = new byte[packet.Length];
            //data가 null이 아닐 경우 즉, 패킷이 잘려서 온 경우
            else
            {
                byte[] temp = new byte[data.Length];
                Buffer.BlockCopy(data, 0, temp, 0, data.Length);
                data = new byte[packet.Length + temp.Length];
                Buffer.BlockCopy(temp, 0, data, 0, temp.Length);
            }
            Buffer.BlockCopy(packet, 0, data, index, packet.Length);
            index += packet.Length;
        }
    }
}
