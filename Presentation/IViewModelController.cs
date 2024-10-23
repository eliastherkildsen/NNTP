using NNTP_NEWS_CLIENT.Presentation.ViewModel;

namespace NNTP_NEWS_CLIENT.Presentation;

public interface IViewModelController
{
    void RegistryViewModel(ViewModelBase viewModel);
    void UnRegistryViewModel(Type viewModelType);
    void SetCurrentViewModel(Type viewModelType);
    Dictionary<Type, ViewModelBase> GetAllViewModels();
}