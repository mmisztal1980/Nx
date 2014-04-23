using NUnit.Framework;
using System.Collections.Generic;
using TechTalk.SpecFlow;

namespace Nx.Domain.UnitTests
{
    [Binding]
    public class AnswersSteps
    {
        private int screen;
        private List<int> values = new List<int>();

        [Given(@"I have entered (.*) into the calculator")]
        public void GivenIHaveEnteredIntoTheCalculator(int p0)
        {
            values.Add(p0);
        }

        [When(@"I press add")]
        public void WhenIPressAdd()
        {
            values.ForEach(v => screen += v);
        }

        [Then(@"the result should be (.*) on the screen")]
        public void ThenTheResultShouldBeOnTheScreen(int p0)
        {
            Assert.AreEqual(screen, p0);
        }
    }
}