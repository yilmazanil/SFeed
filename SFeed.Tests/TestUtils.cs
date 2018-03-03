using System;
using System.Text;

namespace SFeed.Tests
{
    public static class TestUtils
    {
       public static string GenerateLoremIpsumText()
        {
            int minWordInSentence = 5;
            int maxWordInSentence = 15;
            int maxParagraphCount = 5;
            int minSentenceCountInParagraph = 1;
            int maxSentenceCountInParagraph = 5;

            var words = new[]{"lorem", "ipsum", "dolor", "sit", "amet", "consectetuer",
            "adipiscing", "elit", "sed", "diam", "nonummy", "nibh", "euismod",
            "tincidunt", "ut", "laoreet", "dolore", "magna", "aliquam", "erat"};

            var rand = new Random();
            var numOfSentences = rand.Next(minSentenceCountInParagraph, maxSentenceCountInParagraph);
            var numOfWords = rand.Next(minWordInSentence, maxWordInSentence);
            var numOfParagraphs = rand.Next(1,maxParagraphCount);


            var result = new StringBuilder();

            for (int p = 0; p < numOfParagraphs; p++)
            {
                result.Append("<p>");
                for (int s = 0; s < numOfSentences; s++)
                {
                    for (int w = 0; w < numOfWords; w++)
                    {
                        if (w > 0) {
                            result.Append(" ");
                        }
                        var randomWordIndex = rand.Next(words.Length - 1);
                        result.Append(words[randomWordIndex]);
                    }
                    result.Append(". ");
                }
                result.Append("</p>");
            }

            return result.ToString();
        }
    }
}
