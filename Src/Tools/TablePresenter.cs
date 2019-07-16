using System;
using System.Collections;
using System.Text;

namespace Gravityzero.Console.Utility.Tools
{
    public class TablePresenter
    {
        private const short RequredStringLength = 14;
        private const string Border = "-------------------";
        private IEnumerable data;
        private string [] columns;
        public TablePresenter(string [] columns, IEnumerable data)
        {
            this.columns = columns;
            this.data = data;
        }

        public string Render()
        {
            StringBuilder border = new StringBuilder();
            foreach (string columnName in columns)
                border.Append(Border);

            StringBuilder table = new StringBuilder();

            table.AppendLine(border.ToString());

            table.Append("|");
            foreach (string columnName in columns)
            {
                table.Append(PrepareString(columnName)).Append("|");
            }
                
            table.AppendLine();                
            table.AppendLine(border.ToString());

            foreach(var @object in data)
            {
                table.Append("|");
                foreach (string columnName in columns)
                {
                    try
                    {
                        object value = @object.GetType().GetProperty(columnName).GetValue(@object);
                        if (value == null)
                        {
                            table.Append(PrepareString("+|empty|+")).Append("|");
                            continue;
                        }
                        string columnValue = value.ToString();
                        if (!string.IsNullOrEmpty(columnName))
                            table.Append(PrepareString(columnValue)).Append("|");
                        else
                            table.Append(PrepareString("+|empty|+")).Append("|");
                    }
                    catch(Exception ex)
                    {
                        table.Append(PrepareString("+|error|+")).Append("|");
                    }
                }
                table.AppendLine();
                table.AppendLine(border.ToString());
            }
            return table.ToString();
        }
        private string PrepareString(string value)
        {
            if (string.IsNullOrEmpty(value))
                return "                  ";
            string temp = value.PadLeft(value.Length + 1);
            if (temp.Length > RequredStringLength)
            {
                temp = string.Concat(temp.Substring(0, RequredStringLength), "...");
            }
            return  temp.PadRight(RequredStringLength + 4);
        }
    }
}