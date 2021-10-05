/*
The MIT License (MIT)
Copyright (c) 2017 Yusuke Sakurai / @keroxp.
Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:
The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.
THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.
*/
using System;
namespace StatusManager.Hexat
{
    public interface ISecureValue<T>
    {
        T Value { get; set; }
    }

    public static class SecureValues
    {
        public static ISecureValue<int> Int(int v = 0)
        {
            return new SecureInt(v);
        }

        public static ISecureValue<uint> UInt(uint v = 0)
        {
            return new SecureUInt(v);
        }

        public static ISecureValue<float> Float(float v = 0)
        {
            return new SecureFloat(v);
        }
    }
    public static class SecureValue
    {
        public static object New<T>(float v=0)
        {
            Type type=typeof(T);
            if(type==typeof(int))
                return new SecureInt((int)v);
            else if(type==typeof(uint))
                return new SecureUInt((uint)v);
            else if(type==typeof(float))
                return new SecureFloat(v);
            return null;

        }
    }


    [Serializable]
    internal class SecureInt : ISecureValue<int>
    {
        private int _value;
        private readonly int seed;
        public int Value
        {
            get { return _value ^ seed; }
            set { _value = value ^ seed; }
        }

        public SecureInt(int value = 0)
        {
            var rnd = new Random();
            seed = rnd.Next() << 32 | rnd.Next();
            Value = value;
        }
    }

    [Serializable]
    internal class SecureUInt : ISecureValue<uint>
    {
        private uint _value;
        private readonly uint seed;

        public SecureUInt(uint value = 0)
        {
            var rnd = new Random();
            seed = (uint)(rnd.Next() << 32 | rnd.Next());
            Value = value;
        }      
        public uint Value
        {
            get { return _value ^ seed; }
            set { _value = value ^ seed; }
        }
    }

    [Serializable]
    internal class SecureFloat : ISecureValue<float>
    {
        private readonly byte[] _bytes;
        private byte[] _buffer;
        private readonly byte seed;
        public SecureFloat(float v = 0)
        {
            _buffer = new byte[4];
            _bytes = new byte[4];
            var rnd = new Random();
            seed = (byte)(rnd.Next() << 32 | rnd.Next());
            Value = v;
        }

        public float Value
        {
            get
            {
                _bytes.CopyTo(_buffer,0);
                Xor(ref _buffer);
                return BitConverter.ToSingle(_buffer,0);
            }
            set
            {
                _buffer = BitConverter.GetBytes(value);
                Xor(ref _buffer);
                _buffer.CopyTo(_bytes,0);
            }
        }

        private void Xor(ref byte[] arr)
        {
            arr[0] ^= seed;
            arr[1] ^= seed;
            arr[2] ^= seed;
            arr[3] ^= seed;
        }
    }
}