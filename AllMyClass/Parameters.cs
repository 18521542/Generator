using System;

namespace AllMyClass
{
    public class Parameters
    {
        //idea: Split some string like this: a:R 
        //a is name, R is type
        public String name;
        public String Type;
        public String DefaultValue = " 0 ";
        public bool isResult = false;
        public Parameters(String content)
        {
            String[] spitContent = content.Split(':');
            name = spitContent[0];
            Type = spitContent[1];

            ChangeType(ref Type);
        }

        public void ChangeType(ref String type)
        {
            switch (type)
            {
                case "R":
                    type = "float";
                    break;
                case "Z":
                    type = "int";
                    break;
                case "N":
                    type = "int";
                    break;
                case "B":
                    type = "bool";
                    DefaultValue = "false";
                    break;
                case "char*":
                    type = "String";
                    this.DefaultValue = "\"\"";
                    break;
                default:
                    type = "String";
                    break;
            }
        }
    }
}
