using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace Lesson17
{
    class Program
    {
        static void Main(string[] args)
        {
            var parh = Environment.CurrentDirectory + "\\File1.txt";
            // "\n" переход на новую строку при выводе данных
            // @ перед строкой, дословная запись string при выводе данных 
            var _newFile = File.Create(parh);
            _newFile.Dispose();


            if (File.Exists(parh))
            {
                using (var stream = File.OpenWrite(parh)) //при использование using переменная существует только в рамках фигурных скобок
                {
                    using (var _writer = new BinaryWriter(stream, Encoding.Default, false))
                    {
                        _writer.Write("this is my text");
                        for (int i = 0; i < 10; i++)
                        {
                            _writer.Write("\n" + i);
                        }
                    }
                }
            }

            var _readStream = File.OpenRead(parh);
            var _reader = new BinaryReader(_readStream);
            var _firstLine = _reader.ReadString();
            string _secondLine = string.Empty;

            while (_reader.PeekChar() != -1)
            {
                _secondLine += _reader.ReadString();
            }

            _reader.Dispose();
            _readStream.Dispose();

            var _myStruct = new TestStruct { _value = 10, _longValue = 100, _sum = "sum + sum" };

            _readStream = File.Create(Environment.CurrentDirectory + "\\File2.txt");
            var _formatter = new BinaryFormatter();

            _formatter.Serialize(_readStream, _myStruct);
            
            _readStream.Dispose();

            using (var _stream = File.OpenRead(Environment.CurrentDirectory + "\\File2.txt"))
            {
                var _newStruct = (TestStruct)_formatter.Deserialize(_stream);
            }
        }

        [Serializable]
        public struct TestStruct
        {
            public int _value;
            public long _longValue;
            public string _sum; 
        }
    }
}
