using ArtSharingApp.Backend.DTO;

namespace ArtSharingApp.Backend.Service.ServiceInterface
{
    public interface IChatService
    {
        Task<ChatMessageResponseDTO> SendMessageAsync(int senderId, int receiverId, string message);
        Task MarkAsReadAsync(int messageId, int userId);
        Task<IEnumerable<ChatMessageResponseDTO>> GetChatHistoryAsync(int userId, int otherUserId, int skip, int take);
        Task<IEnumerable<UserConversationDTO>?> GetConversationsAsync(int userId, int skip, int take);
    }
}