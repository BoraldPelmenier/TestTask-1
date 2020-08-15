using System;
using System.IO;


namespace TestTask
{
    public class ReadOnlyStream : IReadOnlyStream
    {
        public StreamReader LocalStream { get; set; }
        private bool disposed = false;       
        /// <summary>
        /// Конструктор класса. 
        /// Т.к. происходит прямая работа с файлом, необходимо 
        /// обеспечить ГАРАНТИРОВАННОЕ закрытие файла после окончания работы с таковым!
        /// </summary>
        /// <param name="fileFullPath">Полный путь до файла для чтения</param>
        public ReadOnlyStream(string fileFullPath)
        {            
            // TODO : Заменить на создание реального стрима для чтения файла!
            LocalStream = new StreamReader(fileFullPath);        
        }
                
        /// <summary>
        /// Флаг окончания файла.
        /// </summary>
        public bool IsEof
        {
            // TODO : Заполнять данный флаг при достижении конца файла/стрима при чтении
            get
            {
                return LocalStream.EndOfStream;
            }
            
        }

        /// <summary>
        /// Ф-ция чтения следующего символа из потока.
        /// Если произведена попытка прочитать символ после достижения конца файла, метод 
        /// должен бросать соответствующее исключение
        /// </summary>
        /// <returns>Считанный символ.</returns>
        public char ReadNextChar()
        {
            // TODO : Необходимо считать очередной символ из _localStream         
            if (IsEof)            
                throw new EndOfStreamException();
            
            var letter = (char)LocalStream.Read();
            return letter;       
        }

        /// <summary>
        /// Сбрасывает текущую позицию потока на начало.
        /// </summary>
        public void ResetPositionToStart()
        {
            if (LocalStream != null)
                LocalStream.BaseStream.Position = 0;                       
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }  
        
        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
                if (disposing)
                    LocalStream.Dispose();

            disposed = true;                                  
        }
    }
}
