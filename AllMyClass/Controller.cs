using System;
using System.Collections.Generic;

namespace AllMyClass
{
    public class Controller
    {
        public void HandleFirstLine(String firstLine, ref List<Parameters> parameters,ref Name Header )
        {
            
            String BiggestContent = firstLine.Replace(" ", string.Empty);
            char openBracket = '(';
            char closeBracket = ')';
            char separateChar = ',';
            String[] content = BiggestContent.Split(openBracket, separateChar, closeBracket);

            String headerContent = content[0];
            int index = 0;
            foreach (String piece in content)
            {
                if (index == 0)
                {
                    headerContent = piece;
                }
                else
                {
                    parameters.Add(new Parameters(piece));
                }
                index++;
            }
            Header = new Name(headerContent);

            //set the last parameter is result
            parameters[parameters.Count - 1].isResult = true;
        }

        public void HandleThirdLine(String ThirdLine, ref List<Condition> conditions)
        {
            String BiggestContent = ThirdLine.Replace("post", string.Empty);
            BiggestContent = BiggestContent.Replace(" ", string.Empty);

            if (!BiggestContent.Contains("("))
            {
                BiggestContent = "(" + BiggestContent + ")";
            }
                

            if (BiggestContent.Contains("||"))
            {
                String[] BigCons = BiggestContent.Split(new[] { "||" }, StringSplitOptions.None);
                foreach (String conditionContent in BigCons)
                {
                    conditions.Add(new Condition(conditionContent));
                }
                return;
            }
            else
            {
                conditions.Add(new Condition(BiggestContent));
            }

        }

        public void HandleSecondLine(String secondLine, ref List<Condition> conditions)
        {
            String BiggestContent = secondLine.Replace("pre", string.Empty);
            BiggestContent = BiggestContent.Replace(" ", string.Empty);

            if (BiggestContent == "")
                return;

            if (!BiggestContent.Contains("("))
            {
                BiggestContent = "(" + BiggestContent + ")";
            }

            if (BiggestContent.Contains("||"))
            {
                String[] BigCons = BiggestContent.Split(new[] { "||" }, StringSplitOptions.None);
                foreach (String conditionContent in BigCons)
                {
                    conditions.Add(new Condition(conditionContent));
                }
                return;
            }
            else
            {
                conditions.Add(new Condition(BiggestContent));
            }
        }
    }
}
