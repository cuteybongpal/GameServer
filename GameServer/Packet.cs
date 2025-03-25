using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Numerics;
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
    internal static class Packet
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
        public static byte[] CreatePacket(HeaderType type, object[] args)
        {
            int dataSize = GetLength(type, args);
            byte[] packet = new byte[dataSize + SizeByteOffSet + 1];//데이터와 데이터 크기, 헤더 타입까지 해서 이렇게 함
            int index = 0;
            packet[index] = (byte)type;
            index++;
            byte[] size = BitConverter.GetBytes(dataSize);
            for (int i = 0; i < size.Length; i++)
            {
                packet[index] = size[i];
                index++;
            }
            byte[] datas = DataToByteArray(type, args);
            for (int i = 0; i< datas.Length; i++)
            {
                packet[index] = datas[i];
                index++;
            }
            return packet;
        }
        //패킷에 들어갈 데이터 길이를 구함
        static int GetLength(HeaderType type, object[] args )
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
        static byte[] DataToByteArray(HeaderType type, object[] args)
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
                            , data, sizeof(float)*i, sizeof(float));
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

        public static HeaderType GetHeaderTypeByPacket(byte[] packet)
        {
            return (HeaderType)packet[0];
        }
        public static T GetData<T>(HeaderType header, byte[] data)
        {
            int index = 1;
            byte[] size = new byte[SizeByteOffSet];
            Buffer.BlockCopy(data, index, size, 0, SizeByteOffSet);
            int dataSize = BitConverter.ToInt32(size, 0);
            index += 4;

            switch (header)
            {
                case HeaderType.H_Vector3:
                    Vector3 vector3 = new Vector3();
                    byte[] binaryFloatData = new byte[dataSize];
                    Buffer.BlockCopy(data, index, binaryFloatData, 0, sizeof(float) * 3);
                    float floatData = BitConverter.ToSingle(binaryFloatData, 0);
                    vector3.X = floatData;
                    floatData = BitConverter.ToSingle(binaryFloatData, 0);
                    vector3.Y = floatData;
                    floatData = BitConverter.ToSingle(binaryFloatData, sizeof(float));
                    vector3.Z = floatData;
                    return (T)(object)vector3;
                case HeaderType.H_Vector2:
                    Vector2 vector2 = new Vector2();
                    byte[] binaryFloat = new byte[dataSize];
                    Buffer.BlockCopy(data, index, binaryFloat, 0, sizeof(float) * 3);
                    float floats = BitConverter.ToSingle(binaryFloat, 0);
                    vector2.X = floats;
                    floats = BitConverter.ToSingle(binaryFloat, sizeof(float));
                    vector2.Y = floats;
                    return (T)(object)vector2;
                case HeaderType.H_int:
                    byte[] binary = new byte[dataSize];
                    Buffer.BlockCopy(data, index, binary, 0, sizeof(int));
                    int intData = BitConverter.ToInt32(binary, 0);
                    return (T)(object)intData;
                case HeaderType.H_float:
                    byte[] b = new byte[dataSize];
                    Buffer.BlockCopy(data, index, b, 0, sizeof(float));
                    float ff = BitConverter.ToSingle(b, 0);
                    return (T)(object)ff;
                case HeaderType.H_string:
                    byte[] st = new byte[dataSize];
                    //Buffer.BlockCopy
                    break;
                default:
                    return default(T);
            }
        }
    }
}
