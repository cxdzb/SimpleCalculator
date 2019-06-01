using System.ComponentModel;

namespace SimpleCalculator
{
    class Infix:INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private string formula="";
        private string input = "";
        private string current = "";
        public string Formula
        {
            get
            {
                return formula;
            }
            set
            {
                formula = value;
                if (PropertyChanged != null)
                    PropertyChanged.Invoke(this, new PropertyChangedEventArgs("Formula"));
            }
        }
        public string Input
        {
            get
            {
                return input;
            }
            set
            {
                if (value == "." && input == "")
                    value = "0.";
                input = value;
                if (PropertyChanged != null)
                    PropertyChanged.Invoke(this, new PropertyChangedEventArgs("Input"));
            }
        }
        public string Current
        {
            get
            {
                return current;
            }
            set
            {
                current = value;
                if (PropertyChanged != null)
                    PropertyChanged.Invoke(this, new PropertyChangedEventArgs("Current"));
            }
        }
    }
}
