using ArtSharingApp.Backend.Models;
using ArtSharingApp.Backend.Service.ServiceInterface;
using ArtSharingApp.Backend.DataAccess.Repository.RepositoryInterface;
using ArtSharingApp.Backend.Exceptions;
using ArtSharingApp.Backend.DTO;
using AutoMapper;

namespace ArtSharingApp.Backend.Service;

public class ChatService : IChatService
{
    private readonly IChatRepository _chatRepository;
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    public ChatService(IChatRepository chatRepository, IUserRepository userRepository, IMapper mapper)
    {
        _chatRepository = chatRepository;
        _userRepository = userRepository;
        _mapper = mapper;
    }

    public async Task<ChatMessageResponseDTO> SendMessageAsync(int senderId, int receiverId, string message)
    {
        if (receiverId == senderId)
            throw new BadRequestException("Cannot send messages to yourself.");
        if (string.IsNullOrWhiteSpace(message))
            throw new BadRequestException("Message cannot be empty.");

        var sender = await _userRepository.GetByIdAsync(senderId);
        if (sender == null)
            throw new NotFoundException("Sender not found.");

        var receiver = await _userRepository.GetByIdAsync(receiverId);
        if (receiver == null)
            throw new NotFoundException("Receiver not found.");

        var chatMessage = new ChatMessage
        {
            SenderId = senderId,
            ReceiverId = receiverId,
            Message = message,
            SentAt = DateTime.UtcNow,
            IsRead = false
        };
        await _chatRepository.AddAsync(chatMessage);
        await _chatRepository.SaveAsync();
        return _mapper.Map<ChatMessageResponseDTO>(chatMessage);
    }

    public async Task MarkAsReadAsync(int messageId, int userId)
    {
        var message = await _chatRepository.GetByIdAsync(messageId);
        if (message != null && message.ReceiverId == userId && !message.IsRead)
        {
            message.IsRead = true;
            _chatRepository.Update(message);
            await _chatRepository.SaveAsync();
        }
    }

    public async Task<IEnumerable<ChatMessageResponseDTO>> GetChatHistoryAsync(int userId, int otherUserId, int skip,
        int take)
    {
        var messages = await _chatRepository.GetChatHistoryAsync(userId, otherUserId, skip, take);
        return _mapper.Map<IEnumerable<ChatMessageResponseDTO>>(messages);
    }

    /// <summary>
    /// Returns all users that the logged-in user has had conversations with.
    /// This includes users who have sent messages to the logged-in user or vice versa.
    /// The result is ordered by the last message date, with the most recent conversations first.
    /// </summary>
    /// <param name="userId">
    /// The ID of the user for whom to retrieve conversations.
    /// This should be the ID of the currently logged-in user.
    /// </param>
    /// <param name="skip">
    /// The number of conversations to skip for pagination purposes.
    /// </param>
    /// <param name="take">
    /// The number of conversations to take for pagination purposes.
    /// </param>
    /// <returns>
    /// An enumerable collection of <see cref="UserConversationDTO"/> objects representing the users the provided user has chatted with.
    /// </returns>
    public async Task<IEnumerable<UserConversationDTO>?> GetConversationsAsync(int userId, int skip, int take)
    {
        var conversations = await _chatRepository.GetConversationsAsync(userId);
        var mapped = _mapper.Map<IEnumerable<UserConversationDTO>>(conversations, opt =>
            opt.Items["CurrentUserId"] = userId);
        return mapped.OrderByDescending(c => c.LastMessageDateTime).Skip(skip).Take(take);
    }
}