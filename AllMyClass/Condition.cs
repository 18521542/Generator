using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace AllMyClass
{
    public class Condition
    {
        //Condition include: two bracket and content 
        public String content;
        public List<Condition> SubCondition;
        int numberOfSubContent;
        
        //input : some string like: " (content) "
        //output : we get the content
        public Condition(String input)
        {
            try
            {
                numberOfSubContent = 0;
                SubCondition = new List<Condition>();
                SpitToGetContent(input);
                HandleContent();
            }
            catch(Exception e)
            {
                content = "Empty";
            }
            
        }
        public Condition(String input, String charNeedToTest)
        {
            try
            {
                numberOfSubContent = 0;
                SubCondition = new List<Condition>();
                SpitToGetContent(input);
                HandleContent(charNeedToTest);
            }
            catch (Exception e)
            {
                content = "Empty";
            }

        }

        
        private void SpitToGetContent(String input)
        {
            int depth = 0;
            int startIndex = 0;

            int currentindex = 0;
            foreach(char piece in input)
            {
                if(piece == '(')
                {
                    depth++;
                }
                else if (piece == ')')
                {
                    depth--;
                    if (depth == 0)
                    {
                        content = input.Substring(startIndex+1, currentindex-1);
                        return;
                    }
                }
                currentindex++;
            }
        }
        
        // a content can have some sub-content inside it
        // like: content = (sub-content1)... (sub-content2)...etc..
        public void HandleContent()
        {
            if (!content.Contains("&&")) { return; }

            String[] SubContent_Contents = content.Split(new[] { "&&" }, StringSplitOptions.None);
            foreach(String SubContent_Content in SubContent_Contents)
            {
                SubCondition.Add(new Condition(SubContent_Content));
                numberOfSubContent++;
            }
        }

        //extend
        public void HandleContent(String stringNeedToTest)
        {
            if (!content.Contains(stringNeedToTest)) { return; }

            String[] SubContent_Contents = content.Split(new[] { stringNeedToTest }, StringSplitOptions.None);
            foreach (String SubContent_Content in SubContent_Contents)
            {
                SubCondition.Add(new Condition(SubContent_Content));
                numberOfSubContent++;
            }
        }

        //debug
        public void Print(ref RichTextBox rtb)
        {
            rtb.Text += content + "\n";
            foreach(Condition cons in SubCondition)
            {
                rtb.Text += cons.content + "\n";
            }
             
        }
    }
}
