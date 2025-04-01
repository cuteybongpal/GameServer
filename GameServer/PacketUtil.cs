using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Linq.Expressions;
using System.Numerics;
using System.Runtime.Intrinsics;
using System.Text;
using System.Threading.Tasks;

namespace GameServer
{
    /// <summary>
    /// 패킷을 만들어주는 클래스
    /// 패킷 구조 :
    /// 0 : 패킷 데이터 타입
    /// 1~4 : 데이터의 크기(int 가 4byte이기 때문이다.)
    /// 나머지 : 데이터가 직접 들어감
    /// </summary>
    internal static class PacketUtil
    {
        public enum HeaderType
        {
            H_int,
            H_Vector3,
            H_Vector2,
            H_float,
            H_string,
        }
        const short SizeByteOffSet = 4;

        //패킷을 만들어서 반환해준다.
        public static Packet CreatePacket(HeaderType headerType, object[] args)
        {
            return new Packet(headerType, args);
        }
        public static HeaderType GetHeaderTypeByPacket(Packet packet)
        {
            return (HeaderType)packet.PacketHeader;
        }
        public static T GetData<T>(Packet packet)
        {
            int index = 0;
            byte[] size = new byte[SizeByteOffSet];

            Buffer.BlockCopy(packet.Data, index, size, 0, SizeByteOffSet);
            int dataSize = BitConverter.ToInt32(size, 0);
            index += 4;

            byte[] binaryData = packet.Data;
            byte[] data;

            switch ((HeaderType)packet.PacketHeader)
            {
                case HeaderType.H_Vector3:
                    {
                        data = new byte[sizeof(float)];

                        Vector3 vector3 = new Vector3();
                        Buffer.BlockCopy(binaryData, index, data, 0, sizeof(float));
                        float floatData = BitConverter.ToSingle(data, 0);
                        vector3.X = floatData;
                        index += sizeof(float);

                        Buffer.BlockCopy(binaryData, index, data, 0, sizeof(float));
                        floatData = BitConverter.ToSingle(data, 0);
                        vector3.Y = floatData;
                        index += sizeof(float);
                        Buffer.BlockCopy(binaryData, index, data, 0, sizeof(float));
                        floatData = BitConverter.ToSingle(data, 0);
                        vector3.Z = floatData;

                        return (T)(object)vector3;
                    }
                case HeaderType.H_Vector2:
                    {
                        data = new byte[sizeof(float)];
                        Vector2 vector2 = new Vector2();

                        Buffer.BlockCopy(binaryData, index, data, 0, sizeof(float));
                        float floatData = BitConverter.ToSingle(data, 0);
                        vector2.X = floatData;
                        index += sizeof(float);

                        Buffer.BlockCopy(binaryData, index, data, 0, sizeof(float));
                        floatData = BitConverter.ToSingle(data, 0);
                        vector2.Y = floatData;

                        return (T)(object)vector2;
                    }
                case HeaderType.H_int:
                    {
                        data = new byte[dataSize];
                        Buffer.BlockCopy(binaryData, index, data, 0, sizeof(int));
                        int intData = BitConverter.ToInt32(data, 0);
                        return (T)(object)intData;
                    }
                case HeaderType.H_float:
                    {
                        data = new byte[dataSize];
                        Buffer.BlockCopy(binaryData, index, data, 0, sizeof(float));
                        float floatData = BitConverter.ToSingle(data, 0);
                        return (T)(object)floatData;
                    }
                case HeaderType.H_string:
                    {
                        data = new byte[sizeof(char)];
                        string stringData = "";
                        for (int i = 0; i < data.Length / 2; i++)
                        {
                            Buffer.BlockCopy(binaryData, index, data, 0, sizeof(char));
                            char charData = BitConverter.ToChar(data, 0);
                            stringData += charData;
                            index += 2;
                        }
                        return (T)(object)stringData;
                    }
                default:
                    return default(T);
            }

        }
        public static Packet GetPacketByByteArray(byte[] bytePacket)
        {
            Packet packet = new Packet(bytePacket);
            return packet;
        }
        public struct Packet
        {
            //패킷 헤더 어떤 내용의 패킷인지 알려줌
            public byte PacketHeader;
            //데이터의 크기
            //크기가 4인 이유는 int가 32비트이기 때문임
            public byte[] DataSize = new byte[4];

            public byte[] Data;
            //패킷에 들어갈 데이터 길이를 구함
            //바이트 크기로(int데이터 : 4byte, 4반환함)
            //string : 문자열 크기 * 2
            public int GetLength(HeaderType type, object[] args)
            {
                int count = 0;
                switch (type)
                {
                    case HeaderType.H_int:
                        count += sizeof(int);
                        break;
                    case HeaderType.H_Vector2:
                        count += sizeof(float) * 2;
                        break;
                    case HeaderType.H_Vector3:
                        count += sizeof(float) * 3;
                        break;
                    case HeaderType.H_float:
                        count += sizeof(float);
                        break;
                    case HeaderType.H_string:
                        string d = args[0] as string;
                        foreach (char c in d)
                            count += sizeof(char);
                        break;
                }
                return count;
            }
            /// <summary>
            /// 
            /// </summary>
            /// <param name="type">헤더 타입</param>
            /// <param name="args">매개 변수들(헤더 타입이 Vector3일 경우float 변수가 3개가 들어가고, Vector2일 경우 float 변수가 2개가 들어가는 형식임)</param>
            /// <returns></returns>
            byte[] DataToByteArray(HeaderType type, object[] args)
            {
                byte[] data = null;
                switch (type)
                {
                    case HeaderType.H_int:
                        data = BitConverter.GetBytes((int)args[0]);
                        break;
                    case HeaderType.H_float:
                        data = BitConverter.GetBytes((float)args[0]);
                        break;
                    case HeaderType.H_string:
                        string s = (string)args[0];
                        data = Encoding.Unicode.GetBytes(s);
                        break;
                    case HeaderType.H_Vector2:
                        data = new byte[sizeof(float) * 2];
                        for (int i = 0; i < 2; i++)
                        {
                            //비트컨버터로 float를 byte배열로 바꾼 것을 data에 다가 sizeof(float) * i번째에서 sizeof(float)만큼 값을 넣어준다. 
                            Buffer.BlockCopy(BitConverter.GetBytes((float)args[i]), 0
                                , data, sizeof(float) * i, sizeof(float));
                        }
                        break;
                    case HeaderType.H_Vector3:
                        data = new byte[sizeof(float) * 3];
                        for (int i = 0; i < 3; i++)
                        {
                            Buffer.BlockCopy(BitConverter.GetBytes((float)args[i]),
                                0, data, sizeof(float) * i, sizeof(float));
                        }
                        break;
                    default:
                        break;
                }
                return data;
            }

            public byte[] PacketToArray()
            {
                byte[] packet = new byte[1 +  sizeof(int) + Data.Length];
                int index = 1;
                packet[0] = PacketHeader;
                Buffer.BlockCopy(DataSize, 0, packet, index, sizeof(int));
                index += sizeof(int);
                Buffer.BlockCopy(Data, 0, packet, index, Data.Length);

                return packet;
            }
            public Packet(HeaderType type, object[] args)
            {
                this.PacketHeader = (byte)type;
                this.DataSize = BitConverter.GetBytes(GetLength(type, args));
                this.Data = DataToByteArray(type, args);
            }
            public Packet(byte[] bytePacket)
            {
                this.PacketHeader = (byte)bytePacket[0];
                int index = 1;

                Buffer.BlockCopy(bytePacket, index, this.DataSize, 0, sizeof(int));
                index += sizeof(int);
                Data = new byte[BitConverter.ToInt32(DataSize,0)];
                Buffer.BlockCopy(bytePacket, index, Data, 0, Data.Length);
                
            }
        }
    }
}
