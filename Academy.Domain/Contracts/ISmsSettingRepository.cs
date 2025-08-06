using Academy.Domain.Contracts.Generics;
using Academy.Domain.Models.SmsSetting;

namespace Academy.Domain.Contracts;

public interface ISmsSettingRepository:IEfRepository<SmsSetting,short>
{
    
}