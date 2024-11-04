using Ladeskab.Interfaces;

namespace Ladeskab
{
    public class LogFile : ILogFile
    {
        private List<string> files;

        public LogFile() { files = new List<string>(); }
        public void WriteToLogFile(string message)
        {
            files.Add(message);
        }

        public void ShowLog()
        {
            foreach (var message in files)
            {
                Console.WriteLine(message.ToString());
            }
        }

        public int GetFilesCount() { return files.Count; }
        public List<string> GetFiles() { return files; }
    }
}
