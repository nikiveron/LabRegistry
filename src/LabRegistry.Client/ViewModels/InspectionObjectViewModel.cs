using System.ComponentModel;
using System.Runtime.CompilerServices;
using LabRegistry.Client.Models;

namespace LabRegistry.Client.ViewModels;

public class InspectionObjectViewModel(InspectionObjectModel model) : INotifyPropertyChanged
{
    private string _productResult = model.ProductResult;
    private string? _comment = model.Comment;

    public Guid Id { get; } = model.Id;
    public string Name { get; } = model.Name;
    public string Version { get; } = model.Version;
    public string ProductType { get; } = model.ProductType;
    public DateTimeOffset ReceiptDate { get; } = model.ReceiptDate.ToLocalTime();

    public string ProductResult
    {
        get => _productResult;
        set
        {
            if (_productResult == value)
            {
                return;
            }

            _productResult = value;
            OnPropertyChanged();
        }
    }

    public string? Comment
    {
        get => _comment;
        set
        {
            if (_comment == value)
            {
                return;
            }

            _comment = value;
            OnPropertyChanged();
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    private void OnPropertyChanged(
        [CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(
            this,
            new PropertyChangedEventArgs(propertyName));
    }
}