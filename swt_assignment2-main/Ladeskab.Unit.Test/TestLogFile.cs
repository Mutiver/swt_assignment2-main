namespace Ladeskab.Unit.Test
{
    [TestFixture]
    public class TestLogFile
    {
        private LogFile _uut;
        [SetUp]
        public void Setup()
        {
            _uut = new LogFile();
        }

        [Test]
        [TestCase("Testing LogFile WriteToLog metode", TestName = "Testing LogFile WriteToLog metode")]
        public void Test_Write_To_LogFile(string message)
        {

            // Arrange
            message = "Test message";

            // Act
            _uut.WriteToLogFile(message);

            // Assert - GetFilesCount returner antallet af filer (lavet for holde files private i klassen)
            Assert.That(_uut.GetFilesCount(), Is.EqualTo(1));
            // Assert - GetFiles returner files
            Assert.That(_uut.GetFiles()[0], Is.EqualTo(message));
        }

        [Test]
        [TestCase(TestName = "Testing LogFil showLog metode")]
        public void ShowLog_WritesMessagesToConsole()
        {
            // Arrange
            _uut.WriteToLogFile("Test message 1");
            _uut.WriteToLogFile("Test message 2");

            // Act
            using (StringWriter stringWriter = new StringWriter())
            {
                Console.SetOut(stringWriter);
                _uut.ShowLog();
                string output = stringWriter.ToString().Trim();

                // Assert
                Assert.That(output, Is.EqualTo("Test message 1\nTest message 2"));
            }
        }
    }
}
