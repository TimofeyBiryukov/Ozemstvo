using System.ComponentModel;

namespace OzemstvoWPF.Models
{
    public class BrowserProperty : INotifyPropertyChanged
    {
        virtual public string? Id { get; set; } = null;
        public string Name { get; set; } = string.Empty;
        public string Path { get; set; } = string.Empty;
        public bool UserDefined { get; set; } = true;
        public bool Default { get; set; } = false;

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void SetDefault(bool _default)
        {
            Default = _default;
            OnPropertyChanged(nameof(Default));
        }
    }
}
