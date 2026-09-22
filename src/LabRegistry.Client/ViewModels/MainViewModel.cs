using LabRegistry.Client.Models;
using LabRegistry.Client.Services;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net.Http;
using System.Runtime.CompilerServices;

namespace LabRegistry.Client.ViewModels;

public class MainViewModel : INotifyPropertyChanged
{
    private readonly InspectionObjectsApiClient _apiClient;

    private string _searchText = string.Empty;
    private string? _selectedProductType;
    private string? _selectedProductResult;

    private InspectionObjectViewModel? _selectedObject;

    private const int FirstPage = 1;
    private int _currentPage = 1;
    private const int PageSize = 10;
    private int _totalPages;

    private string _newName = string.Empty;
    private string _newVersion = string.Empty;
    private string? _newProductType;
    private string _newComment = string.Empty;

    private DateTime? _receiptDate = DateTime.Today;
    private TimeSpan _receiptTime = new(12, 0, 0);

    private string? _editProductResult;
    private string _editComment = string.Empty;

    private bool _isLoading;
    private string? _errorMessage;
    private string? _successMessage;

    public MainViewModel(InspectionObjectsApiClient apiClient)
    {
        _apiClient = apiClient;

        LoadCommand = new AsyncRelayCommand(
            LoadFirstPageAsync,
            () => !IsLoading);

        CreateCommand = new AsyncRelayCommand(
            CreateAsync,
            () => !IsLoading);

        UpdateCommand = new AsyncRelayCommand(
            UpdateAsync,
            () => !IsLoading && SelectedObject is not null);

        ClearFiltersCommand = new AsyncRelayCommand(
            ClearFiltersAsync,
            () => !IsLoading);

        PreviousPageCommand = new AsyncRelayCommand(
            PreviousPageAsync,
            () => !IsLoading && CanGoPreviousPage);

        NextPageCommand = new AsyncRelayCommand(
            NextPageAsync,
            () => !IsLoading && CanGoNextPage);

        GetProductTypes =
        [
            "Все",
            "ПО (Программное обеспечение)",
            "ПАК (Программно-аппаратный комплекс)"
        ];

        CreateProductTypes =
        [
            "ПО (Программное обеспечение)",
            "ПАК (Программно-аппаратный комплекс)"
        ];

        GetProductResults =
        [
            "Все",
            "В работе",
            "Соответствует",
            "Не соответствует"
        ];

        UpdateProductResults =
        [
            "В работе",
            "Соответствует",
            "Не соответствует"
        ];

        TimeOptions = [.. Enumerable
            .Range(0, 48)
            .Select(i => TimeSpan.FromMinutes(i * 30))];

        _selectedProductType = "Все";
        _selectedProductResult = "Все";

        _newProductType = "ПО (Программное обеспечение)";
    }

    public ObservableCollection<InspectionObjectViewModel> Objects { get; } = [];

    public IReadOnlyList<string> GetProductTypes { get; }

    public IReadOnlyList<string> CreateProductTypes { get; }

    public IReadOnlyList<string> GetProductResults { get; }

    public IReadOnlyList<string> UpdateProductResults { get; }

    public IReadOnlyList<TimeSpan> TimeOptions { get; }

    public AsyncRelayCommand LoadCommand { get; }

    public AsyncRelayCommand CreateCommand { get; }

    public AsyncRelayCommand UpdateCommand { get; }

    public AsyncRelayCommand ClearFiltersCommand { get; }

    public AsyncRelayCommand PreviousPageCommand { get; }

    public AsyncRelayCommand NextPageCommand { get; }

    public string SearchText
    {
        get => _searchText;
        set
        {
            if (_searchText == value)
            {
                return;
            }

            _searchText = value;
            OnPropertyChanged();
        }
    }

    public string? SelectedProductType
    {
        get => _selectedProductType;
        set
        {
            if (_selectedProductType == value)
            {
                return;
            }

            _selectedProductType = value;
            OnPropertyChanged();
        }
    }

    public string? SelectedProductResult
    {
        get => _selectedProductResult;
        set
        {
            if (_selectedProductResult == value)
            {
                return;
            }

            _selectedProductResult = value;
            OnPropertyChanged();
        }
    }

    public InspectionObjectViewModel? SelectedObject
    {
        get => _selectedObject;
        set
        {
            if (_selectedObject == value)
            {
                return;
            }

            _selectedObject = value;

            EditProductResult = value?.ProductResult;
            EditComment = value?.Comment ?? string.Empty;

            OnPropertyChanged();
            UpdateCommand.RaiseCanExecuteChanged();
        }
    }

    public int CurrentPage
    {
        get => _currentPage;
        set
        {
            if (_currentPage == value)
            {
                return;
            }

            _currentPage = value;

            OnPropertyChanged();
            OnPropertyChanged(nameof(CanGoPreviousPage));
            OnPropertyChanged(nameof(CanGoNextPage));

            PreviousPageCommand.RaiseCanExecuteChanged();
            NextPageCommand.RaiseCanExecuteChanged();
        }
    }

    public int TotalPages
    {
        get => _totalPages;
        set
        {
            if (_totalPages == value)
            {
                return;
            }

            _totalPages = value;

            OnPropertyChanged();
            OnPropertyChanged(nameof(CanGoPreviousPage));
            OnPropertyChanged(nameof(CanGoNextPage));

            PreviousPageCommand.RaiseCanExecuteChanged();
            NextPageCommand.RaiseCanExecuteChanged();
        }
    }

    public bool CanGoPreviousPage =>
        CurrentPage > 1;

    public bool CanGoNextPage =>
        CurrentPage < TotalPages;

    public string NewName
    {
        get => _newName;
        set
        {
            if (_newName == value)
            {
                return;
            }

            _newName = value;
            OnPropertyChanged();
        }
    }

    public string NewVersion
    {
        get => _newVersion;
        set
        {
            if (_newVersion == value)
            {
                return;
            }

            _newVersion = value;
            OnPropertyChanged();
        }
    }

    public string? NewProductType
    {
        get => _newProductType;
        set
        {
            if (_newProductType == value)
            {
                return;
            }

            _newProductType = value;
            OnPropertyChanged();
        }
    }

    public string NewComment
    {
        get => _newComment;
        set
        {
            if (_newComment == value)
            {
                return;
            }

            _newComment = value;
            OnPropertyChanged();
        }
    }

    public DateTime? ReceiptDate
    {
        get => _receiptDate;
        set
        {
            if (_receiptDate == value)
            {
                return;
            }

            _receiptDate = value;
            OnPropertyChanged();
        }
    }

    public TimeSpan ReceiptTime
    {
        get => _receiptTime;
        set
        {
            if (_receiptTime == value)
            {
                return;
            }

            _receiptTime = value;
            OnPropertyChanged();
        }
    }

    public string? EditProductResult
    {
        get => _editProductResult;
        set
        {
            if (_editProductResult == value)
            {
                return;
            }

            _editProductResult = value;
            OnPropertyChanged();
        }
    }

    public string EditComment
    {
        get => _editComment;
        set
        {
            if (_editComment == value)
            {
                return;
            }

            _editComment = value;
            OnPropertyChanged();
        }
    }

    public bool IsLoading
    {
        get => _isLoading;
        private set
        {
            if (_isLoading == value)
            {
                return;
            }

            _isLoading = value;
            OnPropertyChanged();

            LoadCommand.RaiseCanExecuteChanged();
            CreateCommand.RaiseCanExecuteChanged();
            UpdateCommand.RaiseCanExecuteChanged();
            ClearFiltersCommand.RaiseCanExecuteChanged();
            PreviousPageCommand.RaiseCanExecuteChanged();
            NextPageCommand.RaiseCanExecuteChanged();
        }
    }

    public string? ErrorMessage
    {
        get => _errorMessage;
        private set
        {
            if (_errorMessage == value)
            {
                return;
            }

            _errorMessage = value;
            OnPropertyChanged();

            OnPropertyChanged(nameof(HasError));
        }
    }

    public bool HasError => !string.IsNullOrEmpty(ErrorMessage);

    public string? SuccessMessage
    {
        get => _successMessage;
        private set
        {
            if (_successMessage == value)
            {
                return;
            }

            _successMessage = value;
            OnPropertyChanged();

            OnPropertyChanged(nameof(HasSuccess));
        }
    }

    public bool HasSuccess => !string.IsNullOrEmpty(SuccessMessage);

    public async Task InitializeAsync()
    {
        await LoadFirstPageAsync();
    }

    private async Task LoadFirstPageAsync()
    {
        await LoadPageAsync(FirstPage);
    }

    private async Task LoadPageAsync(int page)
    {
        await ExecuteSafelyAsync(async () =>
        {
            var productType =
                SelectedProductType == "Все"
                    ? null
                    : SelectedProductType;

            var productResult =
                SelectedProductResult == "Все"
                    ? null
                    : SelectedProductResult;

            var result = await _apiClient.GetListAsync(
                SearchText,
                productType,
                productResult,
                page,
                PageSize);

            Objects.Clear();

            foreach (var item in result.InspectionObjects)
            {
                Objects.Add(
                    new InspectionObjectViewModel(item));
            }

            CurrentPage = result.Page;
            TotalPages = result.TotalPages;

            SelectedObject = null;
        });
    }

    private async Task CreateAsync()
    {
        if (string.IsNullOrWhiteSpace(NewName))
        {
            ErrorMessage = "Введите наименование объекта.";
            return;
        }

        if (string.IsNullOrWhiteSpace(NewVersion))
        {
            ErrorMessage = "Введите версию объекта.";
            return;
        }

        if (string.IsNullOrWhiteSpace(NewProductType))
        {
            ErrorMessage = "Выберите тип объекта.";
            return;
        }

        if (ReceiptDate is null)
        {
            ErrorMessage = "Выберите дату поступления.";
            return;
        }

        var receiptDateTime = ReceiptDate.Value.Date + ReceiptTime;
        var localDateTime = DateTime.SpecifyKind(receiptDateTime, DateTimeKind.Local);

        if (localDateTime > DateTime.Now)
        {
            ErrorMessage = "Дата и время поступления не могут быть в будущем.";
            return;
        }

        await ExecuteSafelyAsync(async () =>
        {
            var request = new CreateInspectionObjectRequest
            {
                Name = NewName.Trim(),
                Version = NewVersion.Trim(),
                ProductType = NewProductType,
                RecieptDate = new DateTimeOffset(localDateTime).ToUniversalTime(),
                Comment = string.IsNullOrWhiteSpace(NewComment)
                    ? null
                    : NewComment.Trim()
            };

            await _apiClient.CreateAsync(request);

            NewName = string.Empty;
            NewVersion = string.Empty;
            NewComment = string.Empty;
            NewProductType = "ПО (Программное обеспечение)";
            ReceiptDate = DateTime.Today;
            ReceiptTime = new TimeSpan(12, 0, 0);

            SuccessMessage = "Объект успешно добавлен.";

            await LoadFirstPageAsync();
        });
    }

    private async Task UpdateAsync()
    {
        if (SelectedObject is null)
        {
            return;
        }

        if (string.IsNullOrWhiteSpace(EditProductResult))
        {
            ErrorMessage = "Выберите результат проверки.";
            return;
        }

        await ExecuteSafelyAsync(async () =>
        {
            var request = new UpdateInspectionObjectRequest
            {
                ProductResult = EditProductResult,
                Comment = string.IsNullOrWhiteSpace(EditComment)
                    ? null
                    : EditComment.Trim()
            };

            await _apiClient.UpdateAsync(SelectedObject.Id, request);

            SelectedObject.ProductResult = EditProductResult;
            SelectedObject.Comment = request.Comment;
            SuccessMessage = "Результат проверки обновлён.";
        });
    }

    private async Task ClearFiltersAsync()
    {
        SearchText = string.Empty;
        SelectedProductType = "Все";
        SelectedProductResult = "Все";

        await LoadFirstPageAsync();
    }

    private async Task PreviousPageAsync()
    {
        if (!CanGoPreviousPage)
        {
            return;
        }

        await LoadPageAsync(CurrentPage - 1);
    }

    private async Task NextPageAsync()
    {
        if (!CanGoNextPage)
        {
            return;
        }

        await LoadPageAsync(CurrentPage + 1);
    }

    private async Task ExecuteSafelyAsync(Func<Task> action)
    {
        try
        {
            ErrorMessage = null;
            SuccessMessage = null;

            IsLoading = true;

            await action();
        }
        catch (ApiException ex)
        {
            ErrorMessage = ex.Message;
        }
        catch (HttpRequestException)
        {
            ErrorMessage = "Не удалось подключиться к серверу. ";
        }
        catch (TaskCanceledException)
        {
            ErrorMessage = "Запрос был отменён или сервер не отвечает.";
        }
        catch (Exception)
        {
            ErrorMessage = "Произошла непредвиденная ошибка.";
        }
        finally
        {
            IsLoading = false;
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}