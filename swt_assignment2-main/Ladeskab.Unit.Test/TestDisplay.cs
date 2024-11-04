namespace Ladeskab.Unit.Test
{
    [TestFixture]
    public class TestDisplay
    {
        private Display _uut;
        [SetUp]
        public void Setup()
        {
            _uut = new Display();
        }

        [Test]
        [TestCase("testing", TestName = "print testing string")]

        public void Write_Message(string message)
        {
            //Arrange
            var stringWriter = new StringWriter();
            Console.SetOut(stringWriter);

            //Act
            _uut.DisplayMessage(message);

            //Assert
            var output = stringWriter.ToString().Trim();
            Assert.That(message, Is.EqualTo(output));


        }
    }
}
