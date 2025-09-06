using ArtSharingApp.Backend.Models;
using ArtSharingApp.Backend.Service.ServiceInterface;
using ArtSharingApp.Backend.DataAccess.Repository.RepositoryInterface;
using ArtSharingApp.Backend.Exceptions;
using ArtSharingApp.Backend.DTO;
using AutoMapper;

namespace ArtSharingApp.Backend.Service;

/// <summary>
/// Provides business logic for managing chat messages and conversations between users.
/// </summary>
public class ChatService : IChatService
{
    private readonly IChatRepository _chatRepository;
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    /// <summary>
    /// Initializes a new instance of the <see cref="ChatService"/> class.
    /// </summary>
    /// <param name="chatRepository">Repository for chat message data access.</param>
    /// <param name="userRepository">Repository for user data access.</param>
    /// <param name="mapper">AutoMapper instance for DTO mapping.</param>
    public ChatService(IChatRepository chatRepository, IUserRepository userRepository, IMapper mapper)
    {
        _chatRepository = chatRepository;
        _userRepository = userRepository;
        _mapper = mapper;
    }

    /// <summary>
    /// Sends a chat message from one user to another and persists it.
    /// </summary>
    /// <param name="senderId">The ID of the user sending the message.</param>
    /// <param name="receiverId">The ID of the user receiving the message.</param>
    /// <param name="message">The message content.</param>
    /// <returns>
    /// The sent message as <see cref="ChatMessageResponseDTO"/>.
    /// </returns>
    /// <exception cref="BadRequestException">Thrown if the message is empty or sent to self.</exception>
    /// <exception cref="NotFoundException">Thrown if sender or receiver is not found.</exception>
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

        var chatMessage = ChatMessage.Create(senderId, receiverId, message);

        await _chatRepository.AddAsync(chatMessage);
        await _chatRepository.SaveAsync();
        return _mapper.Map<ChatMessageResponseDTO>(chatMessage);
    }

    /// <summary>
    /// Marks a chat message as read by the receiver.
    /// </summary>
    /// <param name="messageId">The ID of the message to mark as read.</param>
    /// <param name="userId">The ID of the user marking the message as read.</param>
    public async Task MarkAsReadAsync(int messageId, int userId)
    {
        var message = await _chatRepository.GetByIdAsync(messageId);
        if (message != null && message.ReceiverId == userId && !message.IsRead)
        {
            message.MarkAsRead();
            _chatRepository.Update(message);
            await _chatRepository.SaveAsync();
        }
    }

    /// <summary>
    /// Retrieves the chat history between two users.
    /// </summary>
    /// <param name="userId">
    /// The ID of one user.
    /// </param>
    /// <param name="otherUserId">
    /// The ID of the other user.
    /// </param>
    /// <param name="skip">
    /// The number of messages to skip for pagination.
    /// </param>
    /// <param name="take">
    /// The number of messages to take for pagination.
    /// </param>
    /// <returns>
    /// An enumerable collection of <see cref="ChatMessageResponseDTO"/> representing the chat history between the two users.
    /// </returns>
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