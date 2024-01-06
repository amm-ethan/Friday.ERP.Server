using Friday.ERP.Core.Data.Entities.SystemManagement;

namespace Friday.ERP.Core.IRepositories.Entities.SystemManagement;

public interface ISettingRepository
{
    Task<Setting?> GetSetting(bool trackChanges);
}