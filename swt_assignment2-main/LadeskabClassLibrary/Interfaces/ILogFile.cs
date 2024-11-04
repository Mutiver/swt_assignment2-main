namespace Ladeskab.Interfaces
{
    public interface ILogFile
    {
        public void WriteToLogFile(string message);
        public void ShowLog();

        public int GetFilesCount();
        public List<string> GetFiles();
    }
}
