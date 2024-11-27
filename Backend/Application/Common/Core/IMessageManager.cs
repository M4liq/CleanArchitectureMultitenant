using Domain.Common.Base;
using Domain.Common.Interfaces;

namespace Application.Common.Core;

public interface IMessageManager
{
    Task<List<string>> GetMessagesForAsync<T>() where T : IDomainMessage, new();
    Task<K> GetMessagesForAsync<T, K>() where T : IDomainMessage, new() where K : BaseResponse, new();
}